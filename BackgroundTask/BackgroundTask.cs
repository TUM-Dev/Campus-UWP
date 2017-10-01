using System.Threading.Tasks;
using TUMCampusAppAPI;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.UserDatas;
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
            long time = SyncManager.GetCurrentUnixTimestampMillis();

            await refreshData();

            Logger.Info("[Background] Finished background task in " + (SyncManager.GetCurrentUnixTimestampMillis() - time) + " ms.");
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
            if (!DeviceInfo.isConnectedToInternet() || Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                Logger.Info("[Background] Canceling background task. Device not connected to a WIFI network.");
                return;
            }
            await generalInit();

            byte lastState = Util.getSettingByte(Const.LAST_BACKGROUND_TASK_ACTION);
            switch (lastState)
            {
                case 0:
                    await refreshData1();
                    lastState++;
                    break;
                case 1:
                    await refreshData1();
                    lastState++;
                    break;
                case 2:
                    await refreshData1();
                    lastState = 0;
                    break;
                default:
                    lastState = 0;
                    break;
            }
            Util.setSetting(Const.LAST_BACKGROUND_TASK_ACTION, lastState);
            Logger.Info("[Background] Finished background task.");
        }

        private async Task generalInit()
        {
            Logger.Info("[Background] Started general init.");

            CacheManager.INSTANCE = new CacheManager();
            SyncManager.INSTANCE = new SyncManager();
            LocationManager.INSTANCE = new LocationManager();
            UserDataManager.INSTANCE = new UserDataManager();
            TumManager.INSTANCE = new TumManager();

            await CacheManager.INSTANCE.InitManagerAsync();
            await SyncManager.INSTANCE.InitManagerAsync();
            await LocationManager.INSTANCE.InitManagerAsync();
            await UserDataManager.INSTANCE.InitManagerAsync();
            await TumManager.INSTANCE.InitManagerAsync();

            Logger.Info("[Background] Finished general init.");
        }

        private async Task refreshData1()
        {
            Logger.Info("[Background] Started refreshing 1.");

            CalendarManager.INSTANCE = new CalendarManager();

            await CalendarManager.INSTANCE.InitManagerAsync();

            Logger.Info("[Background] Finished refreshing 1.");
        }

        private async Task refreshData2()
        {
            Logger.Info("[Background] Started refreshing 2.");

            CanteenManager.INSTANCE = new CanteenManager();
            CanteenMenueManager.INSTANCE = new CanteenMenueManager();

            await CanteenManager.INSTANCE.InitManagerAsync();
            await CanteenMenueManager.INSTANCE.InitManagerAsync();

            await CanteenManager.INSTANCE.downloadCanteensAsync(false);
            await CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(false);

            Logger.Info("[Background] Finished refreshing 2.");
        }

        private async Task refreshData3()
        {
            Logger.Info("[Background] Started refreshing 3.");

            TuitionFeeManager.INSTANCE = new TuitionFeeManager();

            await TuitionFeeManager.INSTANCE.InitManagerAsync();

            await TuitionFeeManager.INSTANCE.downloadFeesAsync(false);

            Logger.Info("[Background] Finished refreshing 3.");
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
            _deferral.Complete();
        }

        #endregion
    }
}
