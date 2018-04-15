using System;
using TUMCampusApp.Pages;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.Media.SpeechRecognition;
using System.Linq;
using TUMCampusApp.Classes.Helpers;
using TUMCampusAppAPI;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;
using Microsoft.HockeyApp;
using TUMCampusApp.Classes;
using Data_Manager;
using Logging;

namespace TUMCampusApp
{
    sealed partial class App : Application
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 17/03/2018 Created [Fabian Sauter]
        /// </history>
        public App()
        {
            //Crash reports capturing:
#if !DEBUG
            if (!Settings.getSettingBoolean(SettingsConsts.DISABLE_CRASH_REPORTING))
            {
                HockeyClient.Current.Configure("24b423fcb785439994ef1c96b818e72e");
            }
#endif

            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += App_UnhandledException;

            // Perform App update tasks if necessary:
            AppUpdateHandler.onAppStart();
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
        private void initLogLevel()
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
        }

        /// <summary>
        /// Dyes the TitleBar on the PC or the StatusBar on mobile.
        /// </summary>
        private void dyeStatusBar()
        {
            //PC customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.BackgroundColor = ((SolidColorBrush)Current.Resources["TUM_blue"]).Color;
                }
            }

            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {

                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundColor = ((SolidColorBrush)Current.Resources["TUM_blue"]).Color;
                    statusBar.BackgroundOpacity = 1;
                }
            }
        }

        /// <summary>
        /// Returns the semantic interpretation of a speech result.
        /// Returns null if there is no interpretation for that key.
        /// </summary>
        /// <param name="interpretationKey">The interpretation key.</param>
        /// <param name="speechRecognitionResult">The speech recognition result to get the semantic interpretation from.</param>
        /// <returns></returns>
        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

        #endregion

        #region --Misc Methods (Protected)--
        /// <summary>
        /// Gets called during a normal App start
        /// </summary>
        /// <param name="e">Details about the start.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Sets the log level:
            initLogLevel();

            dyeStatusBar();
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                SplashScreenPage extendedSplash = new SplashScreenPage(args);
                Window.Current.Content = extendedSplash;
            }
            else
            {
                if (args.TileId != null && args.TileId.Equals(Consts.TILE_ID_CANTEEN))
                {

                    Window.Current.Content = new MainPage2(typeof(CanteensPage2), args.Arguments);
                }
            }

            Window.Current.Activate();

            // Init Cortana integration
            try
            {
                // Install the main VCD.
                StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"TUMCampusAppCommands.xml");
                //await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile); // Disabled because broken
            }
            catch (Exception ex)
            {
                Logger.Error("Installing Voice Commands Failed!", ex);
            }

            if (!Settings.getSettingBoolean(SettingsConsts.DISABLE_BACKGROUND_TASKS))
            {
                MyBackgroundTaskHelper.RegisterBackgroundTask();
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.VoiceCommand)
            {
                var commandArgs = args as VoiceCommandActivatedEventArgs;
                SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;

                string voiceCommandName = speechRecognitionResult.RulePath[0];
                string textSpoken = speechRecognitionResult.Text;

                switch (voiceCommandName)
                {
                    case "showMenusForDate":
                        string date = this.SemanticInterpretation("date", speechRecognitionResult);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // ToDo: Save App state
            deferral.Complete();
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error("Unhandled exception:", e.Exception);
        }

        #endregion
    }
}
