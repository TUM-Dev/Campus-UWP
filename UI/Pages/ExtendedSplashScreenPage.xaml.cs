using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes;
using UI.Classes;
using UI_Context.Classes;
using Windows.ApplicationModel.Activation;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Pages
{
    public sealed partial class ExtendedSplashScreenPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private readonly IActivatedEventArgs ACTIVATION_ARGS;
        private readonly Frame ROOT_FRAME;
        private SplashScreenImageScale curImageScale = SplashScreenImageScale.TINY;
        private double deviceScaleFactor;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public ExtendedSplashScreenPage(IActivatedEventArgs args, Frame rootFrame)
        {
            ACTIVATION_ARGS = args;
            ROOT_FRAME = rootFrame;
            InitializeComponent();

            SetupSplashScreen();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void SetImageScale()
        {
            if (ROOT_FRAME.ActualWidth == 0 || ROOT_FRAME.ActualHeight == 0)
            {
                return;
            }

            if (ROOT_FRAME.ActualWidth >= 3000 || ROOT_FRAME.ActualHeight >= 3000)
            {
                if (curImageScale != SplashScreenImageScale.HUGE)
                {
                    curImageScale = SplashScreenImageScale.HUGE;
                }
            }
            else if (ROOT_FRAME.ActualWidth >= 2000 || ROOT_FRAME.ActualHeight >= 2000)
            {
                if (curImageScale != SplashScreenImageScale.LARGE)
                {
                    curImageScale = SplashScreenImageScale.LARGE;
                }
            }
            else if (ROOT_FRAME.ActualWidth >= 1000 || ROOT_FRAME.ActualHeight >= 1000)
            {
                if (curImageScale != SplashScreenImageScale.MEDIUM)
                {
                    curImageScale = SplashScreenImageScale.MEDIUM;
                }
            }
            else if (ROOT_FRAME.ActualWidth >= 800 || ROOT_FRAME.ActualHeight >= 800)
            {
                if (curImageScale != SplashScreenImageScale.SMALL)
                {
                    curImageScale = SplashScreenImageScale.SMALL;
                }
            }
            else if (curImageScale != SplashScreenImageScale.TINY)
            {
                curImageScale = SplashScreenImageScale.TINY;
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void PositionLogoImage()
        {
            logo_img.SetValue(Canvas.LeftProperty, ACTIVATION_ARGS.SplashScreen.ImageLocation.X);
            logo_img.SetValue(Canvas.TopProperty, ACTIVATION_ARGS.SplashScreen.ImageLocation.Y);

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                logo_img.Height = ACTIVATION_ARGS.SplashScreen.ImageLocation.Height / deviceScaleFactor;
                logo_img.Width = ACTIVATION_ARGS.SplashScreen.ImageLocation.Width / deviceScaleFactor;
            }
            else
            {
                logo_img.Height = ACTIVATION_ARGS.SplashScreen.ImageLocation.Height;
                logo_img.Width = ACTIVATION_ARGS.SplashScreen.ImageLocation.Width;
            }
        }

        private void SetupSplashScreen()
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            deviceScaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            SetImageScale();
            if (!(ACTIVATION_ARGS.SplashScreen is null))
            {
                PositionLogoImage();
                if (ACTIVATION_ARGS.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    ACTIVATION_ARGS.SplashScreen.Dismissed += SPLASH_SCREEN_Dismissed;
                    return;
                }
            }

            if (ACTIVATION_ARGS.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Loaded += ExtendedSplashScreenPage_Loaded;
            }
        }

        private async Task LoadAppAsync()
        {
            // Setup listening for theme changes:
            ThemeUtils.SetupThemeListener();

            // Setup listener for window activated changes:
            UiUtils.SetupWindowActivatedListener();

            // Setup window:
            UiUtils.SetupWindow(Application.Current);

            // Perform app update tasks if necessary:
            await AppUpdateHelper.OnAppStartAsync();

            // Register background tasks:
            Logger.Info("Registering background tasks...");
            // TODO: Add background tasks here
            Logger.Info("Finished registering background tasks.");

            // Init all db managers to force event subscriptions:
            await InitDBManagersAsync();

            // Show initial start dialog:
            /*if (!Settings.getSettingBoolean(SettingsConsts.HIDE_INITIAL_START_DIALOG_ALPHA))
            {
                InitialStartDialog initialStartDialog = new InitialStartDialog();
                await UiUtils.ShowDialogAsync(initialStartDialog);
            }

            // Show what's new dialog:
            if (!Settings.getSettingBoolean(SettingsConsts.HIDE_WHATS_NEW_DIALOG))
            {
                WhatsNewDialog whatsNewDialog = new WhatsNewDialog();
                await UiUtils.ShowDialogAsync(whatsNewDialog);
            }*/

            EvaluateActivationArgs();
        }

        private void PerformInitialStartSetup()
        {
            Storage.Classes.Settings.SetSetting(SettingsConsts.INITIALLY_STARTED, true);
        }

        private void EvaluateActivationArgs()
        {
            // Initially started?
            if (Storage.Classes.Settings.GetSettingBoolean(SettingsConsts.INITIALLY_STARTED))
            {
                if (ACTIVATION_ARGS is ProtocolActivatedEventArgs protocolActivationArgs)
                {
                    Logger.Info("App activated by protocol activation with: " + protocolActivationArgs.Uri.ToString());
                    ROOT_FRAME.Navigate(typeof(MainPage));
                }
                else if (ACTIVATION_ARGS is ToastNotificationActivatedEventArgs toastActivationArgs)
                {
                    Logger.Info("App activated by toast with: " + toastActivationArgs.Argument);
                    ROOT_FRAME.Navigate(typeof(MainPage));
                }
                else if (ACTIVATION_ARGS is LaunchActivatedEventArgs launchActivationArgs)
                {
                    // If launched with arguments (not a normal primary tile/applist launch)
                    if (launchActivationArgs.Arguments.Length > 0)
                    {
                        Logger.Debug(launchActivationArgs.Arguments);
                        // TODO: Handle arguments for cases = launching from secondary Tile, so we navigate to the correct page
                        //throw new NotImplementedException();
                    }

                    // If we're currently not on a page, navigate to the main page
                    ROOT_FRAME.Navigate(typeof(MainPage));
                }
            }
            else
            {
                PerformInitialStartSetup();
                ROOT_FRAME.Navigate(typeof(SetupPage), typeof(MainPage));
            }
        }

        /// <summary>
        /// Inits all DB managers to force event subscriptions.
        /// </summary>
        private async Task InitDBManagersAsync()
        {
            // await AbstractDBManager.exportDBAsync();
            await Task.Run(() =>
            {
                // TODO init DB manager
            });
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void SPLASH_SCREEN_Dismissed(SplashScreen sender, object args)
        {
            await SharedUtils.CallDispatcherAsync(async () => await LoadAppAsync());
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            SetImageScale();
            PositionLogoImage();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetImageScale();
            PositionLogoImage();
        }

        private void ExtendedSplashScreenPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ExtendedSplashScreenPage_Loaded;
            EvaluateActivationArgs();
        }

        #endregion
    }
}
