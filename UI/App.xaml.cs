using System;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes;
using UI.Pages;
using UI_Context.Classes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App: Application
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool isRunning;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public App()
        {
            isRunning = false;

            // Set requested theme:
            ElementTheme theme = ThemeUtils.LoadRequestedTheme();
            RequestedTheme = ThemeUtils.GetActualTheme(theme);

            InitializeComponent();
            Suspending += OnSuspending;
            Resuming += App_Resuming;
            UnhandledException += App_UnhandledException;
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
        /// Sets the log level for the logger class.
        /// </summary>
        private static void InitLogger()
        {
            object o = Settings.getSetting(SettingsConsts.LOG_LEVEL);
            if (o is int)
            {
                Logger.logLevel = (LogLevel)o;
            }
            else
            {
                Settings.setSetting(SettingsConsts.LOG_LEVEL, (int)LogLevel.INFO);
                Logger.logLevel = LogLevel.INFO;
            }
            Logger.logLevel = LogLevel.DEBUG;
            Settings.setSetting(SettingsConsts.LOG_LEVEL, (int)LogLevel.DEBUG);
        }

        private void OnActivatedOrLaunched(IActivatedEventArgs args)
        {
            // Sets the log level:
            InitLogger();

            // Override resources to increase the UI performance on mobile devices:
            if (DeviceFamilyHelper.GetDeviceFamilyType() == DeviceFamilyType.Mobile)
            {
                ThemeUtils.OverrideThemeResources();
            }

            isRunning = true;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page:
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            ExtendedSplashScreenPage extendedSplashScreen = new ExtendedSplashScreenPage(args, rootFrame);
            rootFrame.Content = extendedSplashScreen;

            Window.Current.Activate();
        }

        #endregion

        #region --Misc Methods (Protected)--
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            BackgroundTaskDeferral deferral = args.TaskInstance.GetDeferral();
            // Background activation through toast
            deferral.Complete();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            OnActivatedOrLaunched(args);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            OnActivatedOrLaunched(args);
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            isRunning = false;

            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void App_Resuming(object sender, object e)
        {
            isRunning = true;
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Logger.Error("Unhanded exception: ", e.Exception);
        }

        #endregion
    }
}
