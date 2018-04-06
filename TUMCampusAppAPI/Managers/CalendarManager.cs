using Data_Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.TUMOnline;
using Windows.ApplicationModel.Appointments;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.Managers
{
    public class CalendarManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CalendarManager INSTANCE;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 20/01/2017 Created [Fabian Sauter]
        /// </history>
        public CalendarManager()
        {
            this.refreshingTask = null;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the next calendar entry.
        /// </summary>
        /// <returns>Returns the next calendar entry.</returns>
        public TUMOnlineCalendarTable getNextEntry()
        {
            List<TUMOnlineCalendarTable> list = getEntries();
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            TUMOnlineCalendarTable entry = null;
            foreach (TUMOnlineCalendarTable e in list)
            {
                if (entry == null)
                {
                    if (e != null && e.dTEnd.CompareTo(DateTime.Now) > 0)
                    {
                        entry = e;
                    }
                    continue;
                }
                if (e != null && e.dTEnd.CompareTo(DateTime.Now) > 0 && e.dTEnd.CompareTo(entry.dTEnd) < 0)
                {
                    entry = e;
                }
            }
            return entry;
        }

        /// <summary>
        /// Returns all calendar entries, the db contains.
        /// </summary>
        /// <returns>Returns all calendar entries.</returns>
        public List<TUMOnlineCalendarTable> getEntries()
        {
            waitForSyncToFinish();
            List<TUMOnlineCalendarTable> list = dB.Query<TUMOnlineCalendarTable>(true, "SELECT * FROM " + DBTableConsts.TUM_ONLINE_CALENDAR_TABLE + ";");
            for (int i = 0; i < list.Count; i++)
            {
                list[i].dTStrat = list[i].dTStrat;
                list[i].dTEnd = list[i].dTEnd;
            }
            return list;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<TUMOnlineCalendarTable>();
            syncCalendar();
        }

        /// <summary>
        /// Creates a new Task and starts syncing the calendar in the background.
        /// </summary>
        public void syncCalendar()
        {
            syncCalendar(false);
        }

        /// <summary>
        /// Creates a new Task and starts syncing the calendar in the background.
        /// </summary>
        /// <param name="force">Force sync calendar.</param>
        /// <returns>Returns the sync task or null if not syncing.</returns>
        public Task syncCalendar(bool force)
        {
            if (!force && Settings.getSettingBoolean(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return null;
            }
            return Task.Run(() =>
            {
                waitForSyncToFinish();

                REFRESHING_TASK_SEMA.Wait();
                refreshingTask = syncCalendarTaskAsync(force);
                REFRESHING_TASK_SEMA.Release();
            });
        }

        /// <summary>
        /// Deletes all calendars created by this app.
        /// </summary>
        public async Task deleteCalendarAsync()
        {
            AppointmentStore aS = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);
            IReadOnlyList<AppointmentCalendar> cal = await aS.FindAppointmentCalendarsAsync();
            foreach (AppointmentCalendar c in cal)
            {
                await c.DeleteAsync();
            }
            Logger.Info("Deleted all existing calendars.");
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Refreshes the whole calendar if needed or force == true.
        /// </summary>
        /// <param name="force">Force sync calendar.</param>
        /// <returns></returns>
        private async Task syncCalendarTaskAsync(bool force)
        {
            if (!DeviceInfo.isConnectedToInternet() || (!force && Settings.getSettingBoolean(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi()))
            {
                return;
            }
            if (force || SyncManager.INSTANCE.needSync(DBTableConsts.TUM_ONLINE_CALENDAR_TABLE, CacheManager.VALIDITY_ONE_DAY).NEEDS_SYNC)
            {
                long time = SyncManager.GetCurrentUnixTimestampMillis();
                List<TUMOnlineCalendarTable> list = null;
                XmlDocument doc = null;
                try
                {
                    doc = await getCalendarEntriesDocumentAsync();
                }
                catch (Exception e)
                {
                    Logger.Error("Unable to sync Calendar! Unable to request a document.");
                    SyncManager.INSTANCE.replaceIntoDb(new SyncTable(this, e));
                    return;
                }

                if (doc == null)
                {
                    Logger.Error("Unable to sync Calendar! Unable to request a document.");
                    SyncManager.INSTANCE.replaceIntoDb(new SyncTable("News", SyncResult.STATUS_ERROR_UNKNOWN, "Unable to sync Calendar! Unable to request a document."));
                    return;
                }
                list = parseToList(doc);

                if (force)
                {
                    dB.DropTable<TUMOnlineCalendarTable>();
                    dB.CreateTable<TUMOnlineCalendarTable>();
                }

                // Do not use insertAll, because insertAll is unable to replace entries => Exception:
                for (int i = 0; i < list.Count; i++)
                {
                    dB.InsertOrReplace(list[i]);
                }

                if (!Settings.getSettingBoolean(SettingsConsts.DISABLE_CALENDAR_INTEGRATION))
                {
                    await insterInCalendarAsync(list);
                }
                SyncManager.INSTANCE.replaceIntoDb(new SyncTable(DBTableConsts.TUM_ONLINE_CALENDAR_TABLE));
                Logger.Info("Finished syncing calendar in: " + (SyncManager.GetCurrentUnixTimestampMillis() - time) + " ms");
            }
        }

        /// <summary>
        /// Parses a xml document into a list of TUMOnlineCalendarEntries.
        /// </summary>
        /// <param name="doc">The XmlDocument that should get parsed.</param>
        /// <returns>Returns a list of TUMOnlineCalendarTable.</returns>
        private List<TUMOnlineCalendarTable> parseToList(XmlDocument doc)
        {
            List<TUMOnlineCalendarTable> list = new List<TUMOnlineCalendarTable>();
            foreach (var element in doc.SelectNodes("/events/event"))
            {
                try
                {
                    addEntryToList(list, new TUMOnlineCalendarTable(element));
                }
                catch (Exception e)
                {
                    Logger.Error("Error during parsing calendar entry (" + (element == null ? "NULL" : element.GetXml() + ')'), e);
                }
            }
            return list;
        }

        /// <summary>
        /// Adds a given TUMOnlineCalendarTable to the given list. Checks before adding whether the entity is valid.
        /// </summary>
        private void addEntryToList(List<TUMOnlineCalendarTable> list, TUMOnlineCalendarTable entry)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (entry.status.Equals("CANCEL"))
                {
                    return;
                }
                if (list[i].Equals(entry))
                {
                    list[i].location += ",\n" + entry.location;
                    return;
                }
            }
            list.Add(entry);
        }


        /// <summary>
        /// Resets the calendar, creates a new one and inserts all given TUMOnlineCalendarEntries into it.
        /// </summary>
        /// <param name="list">A list with all TUMOnlineCalendarTable entries that should get added to the calendar.</param>
        private async Task insterInCalendarAsync(List<TUMOnlineCalendarTable> list)
        {
            // 1. Get access to appointmentstore:
            AppointmentStore aS = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);

            // 2. Delete the calendar if one exists:
            await deleteCalendarAsync();

            // 3. Request a new one:
            AppointmentCalendar calendar = null;
            calendar = await aS.CreateAppointmentCalendarAsync(Const.CALENDAR_NAME);

            // 4. Insert appointments:
            if (calendar != null)
            {
                calendar.DisplayColor = Windows.UI.Color.FromArgb(0, 101, 189, 1);
                foreach (TUMOnlineCalendarTable entry in list)
                {
                    await calendar.SaveAppointmentAsync(entry.getAppointment());
                }
            }
            Logger.Info("Finished loading calendar.");
        }

        /// <summary>
        /// Creates a TUMOnlineRequest to request the personal calendar.
        /// </summary>
        /// <returns>Returns the personal calendar in form of a xml document.</returns>
        private async Task<XmlDocument> getCalendarEntriesDocumentAsync()
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.CALENDAR);
            req.addToken();
            req.addParameter(Const.P_MONTH_AHEAD, "2");
            req.addParameter(Const.P_MONTH_BACK, "5");
            return await req.doRequestDocumentAsync();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
