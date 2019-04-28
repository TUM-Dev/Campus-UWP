using System;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using TUMCampusApp.Pages.Setup;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TUMCampusAppAPI;
using static TUMCampusApp.Classes.UiUtils;
using Windows.UI.Popups;
using Data_Manager;
using Logging;

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
        private string arguments;

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
            this.arguments = args.Arguments;
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
            Logger.Info("Started loading App...");
            long time = SyncDBManager.GetCurrentUnixTimestampMillis();

            await invokeTbxAsync("Gathering device infos...");
            DeviceInfo.INSTANCE = new DeviceInfo();
            await incProgressAsync();

            await invokeTbxAsync("Loading cache manager...");
            CacheDBManager.INSTANCE = new CacheDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading study room manager...");
            StudyRoomDBManager.INSTANCE = new StudyRoomDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading canteen manager...");
            CanteenDBManager.INSTANCE = new CanteenDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading canteen menu manager...");
            CanteenDishDBManager.INSTANCE = new CanteenDishDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading grades manager...");
            GradesDBManager.INSTANCE = new GradesDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading location manager...");
            LocationManager.INSTANCE = new LocationManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading sync manager...");
            SyncDBManager.INSTANCE = new SyncDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading user data manager...");
            UserDataDBManager.INSTANCE = new UserDataDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading TumManager...");
            TumManager.INSTANCE = new TumManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading tuition fee manager...");
            TuitionFeeDBManager.INSTANCE = new TuitionFeeDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading my lectures manager...");
            LecturesDBManager.INSTANCE = new LecturesDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading my TUM calendar manager...");
            CalendarDBManager.INSTANCE = new CalendarDBManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading news manager...");
            NewsDBManager.INSTANCE = new NewsDBManager();
            await incProgressAsync();


            await invokeTbxAsync("Initializing cache manager...");
            CacheDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing study room manager...");
            StudyRoomDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing canteen manager...");
            CanteenDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing canteen menu manager...");
            CanteenDishDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing grades manager...");
            GradesDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing location manager...");
            LocationManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing sync manager...");
            SyncDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing device position...");

            if (initialInit)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    MessageDialog message = new MessageDialog(GetLocalizedString("PrivacyPolicylocation_Text").Replace("\\n", "\n"));
                    await message.ShowAsync();
                });
            }
            UserDataDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing tuition fee manager...");
            TuitionFeeDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing my lectures manager...");
            LecturesDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing my TUM calendar manager...");
            CalendarDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing news manager...");
            NewsDBManager.INSTANCE.initManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing TumManager...");
            TumManager.INSTANCE.initManager();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => splashProgressBar.Value = 100.0);

            Logger.Info("Finished loading App in: " + (SyncDBManager.GetCurrentUnixTimestampMillis() - time) + " ms");
        }

        /// <summary>
        /// Invokes the status text box and sets the given string as its text.
        /// This method should only be called in a separate task.
        /// </summary>
        /// <param name="s">The text that should be set.</param>
        /// <returns></returns>
        private async Task invokeTbxAsync(string s)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => task_tbx.Text = s);
        }

        /// <summary>
        /// Increments the progress bar.
        /// This method should only be called in a separate task.
        /// </summary>
        /// <returns></returns>
        private async Task incProgressAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => splashProgressBar.Value += INC_PROGRESS_STEP);
        }

        /// <summary>
        /// Positions all screen elements, inits the app and dismisses the extended splash screen.
        /// This method should only be called in a separate task.
        /// </summary>
        /// <returns></returns>
        private async Task initAppTaskAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => positionElements());
            bool initialStart = !Settings.getSettingBoolean(SettingsConsts.INITIALLY_STARTED);
            await initAppAsync(initialStart);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => DismissExtendedSplashAsync());
            Settings.setSetting(SettingsConsts.INITIALLY_STARTED, true);
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
            if (tileID != null && tileID.Equals(Consts.TILE_ID_CANTEEN))
            {

                Window.Current.Content = new MainPage2(typeof(CanteensPage2), arguments);
            }
            else
            {
                Frame f = new Frame();
                bool connectedToInternet = DeviceInfo.isConnectedToInternet();
                if (!Settings.getSettingBoolean(SettingsConsts.HIDE_WIZARD_ON_STARTUP))
                {
                    task_tbx.Text = "Validating TUM Online Token...";
                    bool wifiOnly = Settings.getSettingBoolean(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING);
                    if ((!wifiOnly && connectedToInternet) || (wifiOnly && DeviceInfo.isConnectedToWifi() && connectedToInternet))
                    {
                        if (TumManager.getToken() == null || TumManager.getToken() == "")
                        {
                            f.Navigate(typeof(SetupPageStep1));
                        }
                        else if (connectedToInternet && !await isTokenConfirmedWithTimeoutAsync())
                        {
                            f.Navigate(typeof(SetupPageStep2));
                        }
                        else
                        {
                            Settings.setSetting(SettingsConsts.TUM_ONLINE_ENABLED, true);
                            f.Navigate(typeof(MainPage2));
                        }
                    }
                    else
                    {
                        string token = TumManager.getToken();
                        Settings.setSetting(SettingsConsts.TUM_ONLINE_ENABLED, (token != null && token != ""));
                        f.Navigate(typeof(MainPage2));
                    }
                }
                else
                {
                    if (TumManager.getToken() == null || TumManager.getToken() == "")
                    {
                        Settings.setSetting(SettingsConsts.TUM_ONLINE_ENABLED, false);
                        f.Navigate(typeof(MainPage2));
                    }
                    else if (connectedToInternet && !await isTokenConfirmedWithTimeoutAsync())
                    {
                        f.Navigate(typeof(SetupPageStep2));
                    }
                    else
                    {
                        Settings.setSetting(SettingsConsts.TUM_ONLINE_ENABLED, true);
                        f.Navigate(typeof(MainPage2));
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
            Task.Run(() => initAppTaskAsync());
        }

        private void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            positionElements();
        }

        #endregion
    }
}
