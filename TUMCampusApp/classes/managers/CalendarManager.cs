using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes.sync;
using TUMCampusApp.classes.tum;
using Windows.ApplicationModel.Appointments;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.classes.managers
{
    class CalendarManager : AbstractManager
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
        public TUMOnlineCalendarEntry getNextEntry()
        {
            List<TUMOnlineCalendarEntry> list = dB.Query<TUMOnlineCalendarEntry>("SELECT * FROM TUMOnlineCalendarEntry");
            if(list == null || list.Count <= 0)
            {
                return null;
            }
            TUMOnlineCalendarEntry entry = null;
            foreach(TUMOnlineCalendarEntry e in list)
            {
                if(entry == null)
                {
                    if(e != null && e.dTStrat.CompareTo(DateTime.Now) > 0)
                    {
                        entry = e;
                    }
                    continue;
                }
                Debug.WriteLine(e.dTStrat);
                if(e != null && e.dTStrat.CompareTo(DateTime.Now) > 0 && e.dTStrat.CompareTo(entry.dTStrat) < 0)
                {
                    entry = e;
                }
            }
            return entry;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<TUMOnlineCalendarEntry>();
            syncCalendar();
        }
        
        public void syncCalendar()
        {
            Task.Factory.StartNew(() => syncCalendarTaskAsync());
        }

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
        private async Task syncCalendarTaskAsync()
        {
            long time = SyncManager.GetCurrentUnixTimestampMillis();
            if (!SyncManager.INSTANCE.needSync(this, CacheManager.VALIDITY_FIFE_DAYS))
            {
                return;
            }

            XmlDocument doc = await getCalendarEntriesDocumentAsync();
            if (doc == null)
            {
                Logger.Error("Unable to sync Calendar! Unable to request a documet.");
                return;
            }
            if(doc.SelectSingleNode("/error") != null)
            {
                Logger.Error("Unable to sync Calendar! " + doc.SelectSingleNode("/error").InnerText);
                return;
            }

            dB.DropTable<TUMOnlineCalendarEntry>();
            dB.CreateTable<TUMOnlineCalendarEntry>();
            List<TUMOnlineCalendarEntry> list = parseToList(doc);
            dB.InsertOrReplaceAll(list);

            if (Utillities.getSettingBoolean(Const.DISABLE_CALENDAR_INTEGRATION))
            {
                await insterInCalendarAsync(list);
            }
            SyncManager.INSTANCE.update(new Sync(this));
            Logger.Info("Finished syncing calendar in: " + (SyncManager.GetCurrentUnixTimestampMillis() - time) + " ms");
        }

        private List<TUMOnlineCalendarEntry> parseToList(XmlDocument doc)
        {
            List<TUMOnlineCalendarEntry> list = new List<TUMOnlineCalendarEntry>();
            foreach (var element in doc.SelectNodes("/events/event"))
            {
                addEtryToList(list, new TUMOnlineCalendarEntry(element));
            }
            return list;
        }

        private void addEtryToList(List<TUMOnlineCalendarEntry> list, TUMOnlineCalendarEntry entry)
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
