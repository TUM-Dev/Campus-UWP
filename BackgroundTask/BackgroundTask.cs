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
        private BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            _deferral = taskInstance.GetDeferral();
            Logger.Info("Started background task.");
            long time = SyncManager.GetCurrentUnixTimestampMillis();

            await RefreshData();

            Logger.Info("Finished background task in " + (SyncManager.GetCurrentUnixTimestampMillis() - time) + " ms.");
            _deferral.Complete();
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Reloads the data for the app
        /// </summary>
        /// <returns></returns>
        private async Task RefreshData()
        {
            if (Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            if(CacheManager.INSTANCE == null)
            {
                CacheManager.INSTANCE = new CacheManager();
                SyncManager.INSTANCE = new SyncManager();
                CanteenManager.INSTANCE = new CanteenManager();
                CanteenMenueManager.INSTANCE = new CanteenMenueManager();
                CalendarManager.INSTANCE = new CalendarManager();
                TuitionFeeManager.INSTANCE = new TuitionFeeManager();
                LocationManager.INSTANCE = new LocationManager();
                UserDataManager.INSTANCE = new UserDataManager();
                TumManager.INSTANCE = new TumManager();
            }
            
            await CacheManager.INSTANCE.InitManagerAsync();
            await SyncManager.INSTANCE.InitManagerAsync();
            await CalendarManager.INSTANCE.InitManagerAsync();
            await LocationManager.INSTANCE.InitManagerAsync();
            await UserDataManager.INSTANCE.InitManagerAsync();
            await CanteenManager.INSTANCE.InitManagerAsync();
            await CanteenMenueManager.INSTANCE.InitManagerAsync();
            await TuitionFeeManager.INSTANCE.InitManagerAsync();
            await TumManager.INSTANCE.InitManagerAsync();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _cancelRequested = true;
            _cancelReason = reason;

            Logger.Error("Background " + sender.Task.Name + " Cancel Requested...\nReason=" + reason.ToString());
        }

        #endregion
    }
}
