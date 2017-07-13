using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Syncs;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using TUMCampusAppAPI.UserDatas;
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
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 20/01/2017 Created [Fabian Sauter]
        /// </history>
        public CalendarManager()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the next calendar entry.
        /// </summary>
        /// <returns>Returns the next calendar entry</returns>
        public TUMOnlineCalendarEntry getNextEntry()
        {
            List<TUMOnlineCalendarEntry> list = getEntries();
            if(list == null || list.Count <= 0)
            {
                return null;
            }
            TUMOnlineCalendarEntry entry = null;
            foreach(TUMOnlineCalendarEntry e in list)
            {
                if(entry == null)
                {
                    if(e != null && e.dTEnd.CompareTo(DateTime.Now) > 0)
                    {
                        entry = e;
                    }
                    continue;
                }
                if(e != null && e.dTEnd.CompareTo(DateTime.Now) > 0 && e.dTEnd.CompareTo(entry.dTEnd) < 0)
                {
                    entry = e;
                }
            }
            return entry;
        }

        /// <summary>
        /// Renturns all calendar entries, the db contains.
        /// Also converts dTStrat and dTEnd from universal time to local time.
        /// </summary>
        /// <returns>Renturns all calendar entries</returns>
        public List<TUMOnlineCalendarEntry> getEntries()
        {
            lock (thisLock)
            {
                List<TUMOnlineCalendarEntry> list = dB.Query<TUMOnlineCalendarEntry>("SELECT * FROM TUMOnlineCalendarEntry");
                for(int i = 0; i < list.Count; i++)
                {
                    list[i].dTStrat = list[i].dTStrat.ToLocalTime();
                    list[i].dTEnd = list[i].dTEnd.ToLocalTime();
                }
                return list;
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<TUMOnlineCalendarEntry>();
            syncCalendar();
        }
        
        /// <summary>
        /// Creates a new Task and starts syncing the calendar in the background
        /// </summary>
        public void syncCalendar()
        {
            syncCalendar(false);
        }

        /// <summary>
        /// Creates a new Task and starts syncing the calendar in the background
        /// </summary>
        /// <param name="force">Force sync calendar</param>
        public void syncCalendar(bool force)
        {
            if (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            Task.Factory.StartNew(() => {
                lock (thisLock)
                {
                    Task.WaitAny(syncCalendarTaskAsync(true));
                }
            });
        }

        /// <summary>
        /// Deletes all calendars created by this app
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Refreshes the whole calendar if needed or force == true.
        /// </summary>
        /// <param name="force">Force sync calandar.</param>
        /// <returns></returns>
        public async Task syncCalendarTaskAsync(bool force)
        {
            if (!DeviceInfo.isConnectedToInternet() || (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi()))
            {
                return;
            }
            long time = SyncManager.GetCurrentUnixTimestampMillis();
            List<TUMOnlineCalendarEntry> list = null;
            if (force || SyncManager.INSTANCE.needSync(this, CacheManager.VALIDITY_ONE_DAY).NEEDS_SYNC)
            {
                XmlDocument doc = null;
                try
                {
                    doc = await getCalendarEntriesDocumentAsync();
                }
                catch (BaseTUMOnlineException e)
                {
                    SyncManager.INSTANCE.replaceIntoDb(new Sync(this, e));
                    return;
                }
                catch (Exception e)
                {
                    SyncManager.INSTANCE.replaceIntoDb(new Sync(this, e));
                }
                if (doc == null)
                {
                    Logger.Error("Unable to sync Calendar! Unable to request a documet.");
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("News", SyncResult.STATUS_ERROR_UNKNOWN, "Unable to sync Calendar! Unable to request a documet."));
                    return;
                }
                list = parseToList(doc);

                if (force)
                {
                    dB.DropTable<TUMOnlineCalendarEntry>();
                    dB.CreateTable<TUMOnlineCalendarEntry>();
                }
                dB.InsertOrReplaceAll(list);

                SyncManager.INSTANCE.replaceIntoDb(new Sync(this));
            }
            else
            {
                list = getEntries();
            }

            if (!Util.getSettingBoolean(Const.DISABLE_CALENDAR_INTEGRATION))
            {
                await insterInCalendarAsync(list);
            }
            Logger.Info("Finished syncing calendar in: " + (SyncManager.GetCurrentUnixTimestampMillis() - time) + " ms");
        }
        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Parses a xml document into a list of TUMOnlineCalendarEntries.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns>Returns a list of TUMOnlineCalendarEntry</returns>
        private List<TUMOnlineCalendarEntry> parseToList(XmlDocument doc)
        {
            List<TUMOnlineCalendarEntry> list = new List<TUMOnlineCalendarEntry>();
            foreach (var element in doc.SelectNodes("/events/event"))
            {
                addEntryToList(list, new TUMOnlineCalendarEntry(element));
            }
            return list;
        }

        /// <summary>
        /// Adds a given TUMOnlineCalendarEntry to the given list. Checks bevor adding whether the enty is valid.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entry"></param>
        private void addEntryToList(List<TUMOnlineCalendarEntry> list, TUMOnlineCalendarEntry entry)
        {
            for(var i = 0; i < list.Count; i++)
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
        /// <param name="list"></param>
        /// <returns>Returns an asynchronous Task</returns>
        private async Task insterInCalendarAsync(List<TUMOnlineCalendarEntry> list)
        {
            // 1. get access to appointmentstore 
            AppointmentStore aS = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);

            // 2. delete the calendar if one exists
            await deleteCalendarAsync();

            // 3. request a new one
            AppointmentCalendar calendar = null;
            calendar = await aS.CreateAppointmentCalendarAsync(Const.CALENDAR_NAME);

            // 4. insert appointments
            if(calendar != null)
            {
                calendar.DisplayColor = Windows.UI.Color.FromArgb(0,101,189,1);
                foreach (TUMOnlineCalendarEntry entry in list)
                {
                    await calendar.SaveAppointmentAsync(entry.getAppointment());
                }
            }
            Logger.Info("Finished loading calendar.");
        }

        /// <summary>
        /// Creates a TUMOnlineRequest to request the personal calendar
        /// </summary>
        /// <returns>Returns the personal calendar in form of a xml document</returns>
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
