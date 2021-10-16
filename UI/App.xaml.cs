using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes.AppCenter;
using Storage.Classes;
using UI.Dialogs;
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
#pragma warning disable IDE0052 // Remove unread private members
        private bool isRunning;
#pragma warning restore IDE0052 // Remove unread private members

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
            Resuming += OnAppResuming;
            UnhandledException += OnAppUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
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
            object o = Settings.GetSetting(SettingsConsts.LOG_LEVEL);
            if (o is int)
            {
                Logger.logLevel = (LogLevel)o;
            }
            else
            {
                Settings.SetSetting(SettingsConsts.LOG_LEVEL, (int)LogLevel.INFO);
                Logger.logLevel = LogLevel.INFO;
            }
            Logger.logLevel = LogLevel.DEBUG;
            Settings.SetSetting(SettingsConsts.LOG_LEVEL, (int)LogLevel.DEBUG);
        }

        private void OnActivatedOrLaunched(IActivatedEventArgs args)
        {
            // Sets the log level:
            InitLogger();

            // Register a handler to show a dialog when we catch a crash:
            AppCenterCrashHelper.INSTANCE.OnTrackError += OnTrackError;

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

        private void OnAppResuming(object sender, object e)
        {
            isRunning = true;
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Logger.Error("Unhanded exception: ", e.Exception);
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        private void OnAppDomainUnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            if (e.ExceptionObject is Exception ex)
            {
                Logger.Error("Unhanded exception: ", ex);
            }
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            Logger.Error("Unhanded exception: ", e.Exception);
        }

        private static async Task OnTrackError(AppCenterCrashHelper sender, TrackErrorEventArgs args)
        {
            if (!Settings.GetSettingBoolean(SettingsConsts.ALWAYS_REPORT_CRASHES_WITHOUT_ASKING))
            {
                ReportCrashDialog dialog = new ReportCrashDialog(args);
                await UiUtils.ShowDialogAsync(dialog);
                if (!dialog.VIEW_MODEL.MODEL.Report)
                {
                    args.Cancel = true;
                }
                else if (dialog.AlwaysReport)
                {
                    Settings.SetSetting(SettingsConsts.ALWAYS_REPORT_CRASHES_WITHOUT_ASKING, true);
                }
            }
        }

        #endregion
    }
}
