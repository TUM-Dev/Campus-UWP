using System;
using TUMCampusApp.Pages;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.SpeechRecognition;
using System.Linq;
using TUMCampusApp.Classes.Helpers;
using TUMCampusAppAPI;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Microsoft.HockeyApp;

namespace TUMCampusApp
{
    /// <summary>
    /// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
        /// und daher das logische Äquivalent von main() bzw. WinMain().
        /// </summary>
        public App()
        {
            //Crash reports capturing
#if !DEBUG
            if (!Util.getSettingBoolean(Const.DISABLE_CRASH_REPORTING))
            {
                HockeyClient.Current.Configure("24b423fcb785439994ef1c96b818e72e");
            }
#endif

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Deletes the unused "Cache" folder and its contents, which was used in versions bellow v.1.0.4 for storing cached images.
        /// </summary>
        /// <returns></returns>
        private async Task deleteCacheFolderAsync()
        {
            try
            {
                StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Cache");
                if (folder != null)
                {
                    await folder.DeleteAsync();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Anwendung durch den Endbenutzer normal gestartet wird. Weitere Einstiegspunkte
        /// werden z. B. verwendet, wenn die Anwendung gestartet wird, um eine bestimmte Datei zu öffnen.
        /// </summary>
        /// <param name="e">Details über Startanforderung und -prozess.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            dyeStatusBar();
            await deleteCacheFolderAsync();
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                SplashScreenPage extendedSplash = new SplashScreenPage(args);
                Window.Current.Content = extendedSplash;
            }

            Window.Current.Activate();

            // Init Cortana integration
            try
            {
                // Install the main VCD.
                StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"TUMCampusAppCommands.xml");
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);
            }
            catch (Exception ex)
            {
                Logger.Error("Installing Voice Commands Failed!", ex);
            }

            if (!Util.getSettingBoolean(Const.DISABLE_BACKGROUND_TASKS))
            {
                MyBackgroundTaskHelper.RegisterBackgroundTask();
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Navigation auf eine bestimmte Seite fehlschlägt
        /// </summary>
        /// <param name="sender">Der Rahmen, bei dem die Navigation fehlgeschlagen ist</param>
        /// <param name="e">Details über den Navigationsfehler</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ausführung der Anwendung angehalten wird.  Der Anwendungszustand wird gespeichert,
        /// ohne zu wissen, ob die Anwendung beendet oder fortgesetzt wird und die Speicherinhalte dabei
        /// unbeschädigt bleiben.
        /// </summary>
        /// <param name="sender">Die Quelle der Anhalteanforderung.</param>
        /// <param name="e">Details zur Anhalteanforderung.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Anwendungszustand speichern und alle Hintergrundaktivitäten beenden
            deferral.Complete();
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
    }
}
