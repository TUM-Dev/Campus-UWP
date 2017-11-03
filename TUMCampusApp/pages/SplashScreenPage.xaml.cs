using System;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.UserDatas;
using TUMCampusApp.Pages.Setup;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TUMCampusAppAPI;
using static TUMCampusApp.Classes.Utillities;
using TUMCampusApp.Classes;
using Windows.UI.Popups;
using System.Threading;

namespace TUMCampusApp.Pages
{
    public sealed partial class SplashScreenPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static Rect splashImageRect;
        private SplashScreen splash;
        internal bool dismissed = false;
        private string tileID;
        private static readonly double INC_PROGRESS_STEP = 100 / 27;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 18/12/2016  Created [Fabian Sauter]
        /// </history>
        public SplashScreenPage(LaunchActivatedEventArgs args)
        {
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
            this.InitializeComponent();
            this.tileID = args.TileId;
            splash = args.SplashScreen;
            if (splash != null)
            {
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);
                positionElements();
            }
        }

        public SplashScreenPage()
        {
            this.InitializeComponent();
            this.tileID = null;
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
            DismissedEventHandler(null, null);
            positionElements();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Positions all elements on the screen.
        /// </summary>
        private void positionElements()
        {
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;
            }
            positionImage();
        }

        /// <summary>
        /// Positions the image on the screen.
        /// </summary>
        private void positionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            extendedSplashImage.Height = splashImageRect.Height;
            extendedSplashImage.Width = splashImageRect.Width;
        }

        /// <summary>
        /// Inits the app.
        /// This method should only be called in a separate task.
        /// </summary>
        /// <returns></returns>
        private async Task initAppAsync(bool initialInit)
        {
            Logger.Info("Started loading app...");
            long time = SyncManager.GetCurrentUnixTimestampMillis();

            await invokeTbxAsync("Gathering device infos...");
            DeviceInfo.INSTANCE = new DeviceInfo();
            await incProgressAsync();

            await invokeTbxAsync("Loading cache manager...");
            CacheManager.INSTANCE = new CacheManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading study room manager...");
            StudyRoomManager.INSTANCE = new StudyRoomManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading canteen manager...");
            CanteenManager.INSTANCE = new CanteenManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading canteen menu manager...");
            CanteenMenueManager.INSTANCE = new CanteenMenueManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading grades manager...");
            GradesManager.INSTANCE = new GradesManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading location manager...");
            LocationManager.INSTANCE = new LocationManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading sync manager...");
            SyncManager.INSTANCE = new SyncManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading user data manager...");
            UserDataManager.INSTANCE = new UserDataManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading TumManager...");
            TumManager.INSTANCE = new TumManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading tuition fee manager...");
            TuitionFeeManager.INSTANCE = new TuitionFeeManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading my lectures manager...");
            LecturesManager.INSTANCE = new LecturesManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading my TUM calendar manager...");
            CalendarManager.INSTANCE = new CalendarManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading news manager...");
            NewsManager.INSTANCE = new NewsManager();
            await incProgressAsync();


            await invokeTbxAsync("Initializing cache manager...");
            await CacheManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing study room manager...");
            await StudyRoomManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing canteen manager...");
            await CanteenManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing canteen menu manager...");
            await CanteenMenueManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing grades manager...");
            await GradesManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing location manager...");
            await LocationManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing sync manager...");
            await SyncManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing device position...");

            if (initialInit)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    MessageDialog message = new MessageDialog(Utillities.getLocalizedString("PrivacyPolicylocation_Text").Replace("\\n", "\n"));
                    await message.ShowAsync();
                });
            }
            await UserDataManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing tuition fee manager...");
            await TuitionFeeManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing my lectures manager...");
            await LecturesManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing my TUM calendar manager...");
            await CalendarManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing news manager...");
            await NewsManager.INSTANCE.InitManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing TumManager...");
            await TumManager.INSTANCE.InitManagerAsync();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                splashProgressBar.Value = 100.0;
            });

            Logger.Info("Finished loading app in: " + (SyncManager.GetCurrentUnixTimestampMillis() - time) + " ms");
        }

        /// <summary>
        /// Invokes the status text box and sets the given string as its text.
        /// This method should only be called in a separate task.
        /// </summary>
        /// <param name="s">The text that should be set.</param>
        /// <returns></returns>
        private async Task invokeTbxAsync(string s)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                task_tbx.Text = s;
            });
        }

        /// <summary>
        /// Increments the progress bar.
        /// This method should only be called in a separate task.
        /// </summary>
        /// <returns></returns>
        private async Task incProgressAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                splashProgressBar.Value += INC_PROGRESS_STEP;
            });
        }

        /// <summary>
        /// Positions all screen elements, inits the app and dismisses the extended splash screen.
        /// This method should only be called in a separate task.
        /// </summary>
        /// <returns></returns>
        private async Task initAppTaskAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 positionElements();
             });
            bool initialStart = !Util.getSettingBoolean(Const.INITIALLY_STARTED);
            Task.WaitAll(initAppAsync(initialStart));
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                DismissExtendedSplashAsync();
            });
            Util.setSetting(Const.INITIALLY_STARTED, true);
        }

        /// <summary>
        /// Checks if the current token is activated.
        /// It times out after 2000 ms.
        /// Returns true if a timeout occurred.
        /// </summary>
        /// <returns>Returns whether the token is activated or the request timed out.</returns>
        private async Task<bool> isTokenConfirmedWithTimeoutAsync()
        {
            Task<bool> t = TumManager.INSTANCE.isTokenConfirmedAsync();
            try
            {
                if (await Task.WhenAny(t, Task.Delay(2000)) == t)
                {
                    return t.Result;
                }
                else
                {
                    Logger.Warn("Is token confirmed timed out.");
                    return true;
                }
            }
            catch (AggregateException e)
            {
                Logger.Error("await isTokenConfirmedWithTimeoutAsync() has thrown an exception!", e);
                return true;
            }
        }

        /// <summary>
        /// Dismisses the extended splash screen and navigates the frame to the next page.
        /// </summary>
        private async void DismissExtendedSplashAsync()
        {
            if (tileID != null && tileID.Equals(Const.TILE_ID_CANTEEN))
            {
                Window.Current.Content = new MainPage(EnumPage.CanteensPage);
            }
            else
            {
                Frame f = new Frame();
                bool connectedToInternet = DeviceInfo.isConnectedToInternet();
                if (!Util.getSettingBoolean(Const.HIDE_WIZARD_ON_STARTUP))
                {
                    task_tbx.Text = "Validating TUM Online Token...";
                    bool wifiOnly = Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING);
                    if ((!wifiOnly && connectedToInternet) || (wifiOnly && DeviceInfo.isConnectedToWifi() && connectedToInternet))
                    {
                        if(TumManager.getToken() == null || TumManager.getToken() == "")
                        {
                            f.Navigate(typeof(SetupPageStep1));
                        }
                        else if (connectedToInternet && !await isTokenConfirmedWithTimeoutAsync())
                        {
                            f.Navigate(typeof(SetupPageStep2));
                        }
                        else
                        {
                            Util.setSetting(Const.TUMO_ENABLED, true);
                            f.Navigate(typeof(MainPage));
                        }
                    }
                    else
                    {
                        string token = TumManager.getToken();
                        Util.setSetting(Const.TUMO_ENABLED, (token != null && token != ""));
                        f.Navigate(typeof(MainPage));
                    }
                }
                else
                {
                    if(TumManager.getToken() == null || TumManager.getToken() == "")
                    {
                        Util.setSetting(Const.TUMO_ENABLED, false);
                        f.Navigate(typeof(MainPage));
                    }
                    else if (connectedToInternet && !await isTokenConfirmedWithTimeoutAsync())
                    {
                        f.Navigate(typeof(SetupPageStep2));
                    }
                    else
                    {
                        Util.setSetting(Const.TUMO_ENABLED, true);
                        f.Navigate(typeof(MainPage));
                    }
                }
                Window.Current.Content = f;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
            Task.Factory.StartNew(() => initAppTaskAsync());
        }

        private void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            positionElements();
        }

        #endregion
    }
}
