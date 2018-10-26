using Data_Manager;
using Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.ApplicationModel.Background;

namespace TUMCampusApp.BackgroundTask
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private BackgroundTaskDeferral _deferral;
        volatile bool _cancelRequested = false;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            Logger.Info("[Background] Started background task.");
            long time = SyncDBManager.GetCurrentUnixTimestampMillis();

            await refreshData();

            Logger.Info("[Background] Finished background task in " + (SyncDBManager.GetCurrentUnixTimestampMillis() - time) + " ms.");
            _deferral.Complete();
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Reloads the data for the app
        /// </summary>
        /// <returns></returns>
        private async Task refreshData()
        {
            if (!DeviceInfo.isConnectedToInternet() || Settings.getSettingBoolean(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                Logger.Info("[Background] Canceling background task. Device not connected to a WIFI network.");
                return;
            }
            generalInit();

            byte lastState = Settings.getSettingByte(SettingsConsts.LAST_BACKGROUND_TASK_ACTION);
            switch (lastState)
            {
                case 0:
                    await syncCalendarEntriesAsync();
                    lastState++;
                    break;

                case 1:
                    await insertCalendarEntriesIntoCalendarAsync();
                    lastState++;
                    break;

                case 2:
                    await syncCanteensAndDishesAsync();
                    lastState++;
                    break;

                case 3:
                    await syncTuitionFeesAsync();
                    lastState++;
                    break;

                case 4:
                    await syncTuitionFeesAsync();
                    lastState = 0;
                    break;

                default:
                    lastState = 0;
                    break;
            }
            Settings.setSetting(SettingsConsts.LAST_BACKGROUND_TASK_ACTION, lastState);
            Logger.Info("[Background] Finished background task.");
        }

        private void generalInit()
        {
            Logger.Info("[Background] Started general init.");

            CacheDBManager.INSTANCE = new CacheDBManager();
            SyncDBManager.INSTANCE = new SyncDBManager();
            LocationManager.INSTANCE = new LocationManager();
            UserDataDBManager.INSTANCE = new UserDataDBManager();
            TumManager.INSTANCE = new TumManager();

            CacheDBManager.INSTANCE.initManager();
            SyncDBManager.INSTANCE.initManager();
            LocationManager.INSTANCE.initManager();
            UserDataDBManager.INSTANCE.initManager();
            TumManager.INSTANCE.initManager();

            Logger.Info("[Background] Finished general init.");
        }

        private async Task syncCalendarEntriesAsync()
        {
            Logger.Info("[Background] Started downloading calendar entries.");

            CalendarDBManager.INSTANCE = new CalendarDBManager();
            CalendarDBManager.INSTANCE.initManagerForBackgroundTask();

            Task t = CalendarDBManager.INSTANCE.syncCalendar(false, false);
            if (t != null)
            {
                await t;
            }

            Logger.Info("[Background] Finished downloading calendar entries.");
        }

        private async Task insertCalendarEntriesIntoCalendarAsync()
        {
            Logger.Info("[Background] Started inserting calendar entries into calendar.");

            CalendarDBManager.INSTANCE = new CalendarDBManager();
            CalendarDBManager.INSTANCE.initManagerForBackgroundTask();

            List<TUMOnlineCalendarTable> list = CalendarDBManager.INSTANCE.getEntries();
            if (list != null)
            {
                await CalendarDBManager.INSTANCE.insterInCalendarAsync(list);
            }

            Logger.Info("[Background] Finished inserting calendar entries into calendar.");
        }

        private async Task syncCanteensAndDishesAsync()
        {
            Logger.Info("[Background] Started syncing canteens and dishes.");

            CanteenDBManager.INSTANCE = new CanteenDBManager();
            CanteenDishDBManager.INSTANCE = new CanteenDishDBManager();

            CanteenDBManager.INSTANCE.initManager();
            CanteenDishDBManager.INSTANCE.initManager();

            Task t2 = CanteenDBManager.INSTANCE.downloadCanteens(false);
            if (t2 != null)
            {
                await t2;
            }
            Task t = CanteenDishDBManager.INSTANCE.downloadCanteenDishes(false);
            if (t != null)
            {
                await t;
            }

            Logger.Info("[Background] Finished syncing canteens and dishes.");
        }

        private async Task syncTuitionFeesAsync()
        {
            Logger.Info("[Background] Started syncing tuition fees.");

            TuitionFeeDBManager.INSTANCE = new TuitionFeeDBManager();

            TuitionFeeDBManager.INSTANCE.initManager();

            Task t = TuitionFeeDBManager.INSTANCE.downloadFees(false);
            if (t != null)
            {
                await t;
            }

            Logger.Info("[Background] Finished syncing tuition fees.");
        }

        private async Task syncNewsAsync()
        {
            Logger.Info("[Background] Started syncing news.");

            NewsDBManager.INSTANCE = new NewsDBManager();

            NewsDBManager.INSTANCE.initManager();

            Task t1 = NewsDBManager.INSTANCE.downloadNewsSources(false);
            Task t2 = NewsDBManager.INSTANCE.downloadNews(false);
            if (t1 != null)
            {
                await t1;
            }
            if (t2 != null)
            {
                await t2;
            }

            Logger.Info("[Background] Finished syncing news.");
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _cancelRequested = true;

            Logger.Error("[Background] " + sender.Task.Name + " Cancel Requested...\nReason=" + reason.ToString());
            Task.Delay(1000);
            _deferral.Complete();
        }

        #endregion
    }
}
