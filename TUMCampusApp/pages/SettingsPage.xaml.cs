using System;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Helpers;
using TUMCampusAppAPI.Managers;
using TUMCampusApp.Pages.Setup;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using TUMCampusAppAPI;
using TUMCampusApp.Classes;
using Windows.UI.Core;
using TUMCampusApp.Dialogs;
using Data_Manager;

namespace TUMCampusApp.Pages
{
    public sealed partial class SettingsPage : Page, INamedPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------SettingsConstsructor:----------------------------------------------------------------\\
        #region --SettingsConstsructors--
        /// <summary>
        /// Basic SettingsConstsructor
        /// </summary>
        /// <history>
        /// 24/12/2016  Created [Fabian Sauter]
        /// </history>
        public SettingsPage()
        {
            this.InitializeComponent();
            initControls();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UIUtils.getLocalizedString("SettingsPageName_text");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Inits all controls.
        /// </summary>
        private void initControls()
        {
            initGeneralControls();
            initTUMonlineControls();
            initWidgetControls();
            initServices();
            initMiscControls();
            initAboutAndLinks();
        }

        private void initMiscControls()
        {
            disableCrashReporting_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.DISABLE_CRASH_REPORTING);
        }

        /// <summary>
        /// Inits all about and links controls.
        /// </summary>
        private void initAboutAndLinks()
        {
            if (!Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                feedback_stckp.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Inits all widget controls.
        /// </summary>
        private void initWidgetControls()
        {
            disableExampleWidget_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.DISABLE_EXAMPLE_WIDGET);
            disableCanteenWidget_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.DISABLE_CANTEEN_WIDGET);
            disableCalendarWidget_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.DISABLE_CALENDAR_WIDGET);
            disableTuitionFeeWidget_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.DISABLE_TUITION_FEE_WIDGET);
            disableNewsWidget_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.DISABLE_NEWS_WIDGET);
        }

        /// <summary>
        /// Inits all service controls.
        /// </summary>
        private void initServices()
        {
            disableCalendar_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.DISABLE_CALENDAR_INTEGRATION);
            disableBackgroundTask_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.DISABLE_BACKGROUND_TASKS);
        }

        /// <summary>
        /// Inits all general controls.
        /// </summary>
        private void initGeneralControls()
        {
            wifiOnly_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING);
        }

        /// <summary>
        /// Inits all tum online controls.
        /// </summary>
        private void initTUMonlineControls()
        {
            hideWizardOnStartup_tgls.IsOn = Settings.getSettingBoolean(SettingsConsts.HIDE_WIZARD_ON_STARTUP);
        }

        /// <summary>
        /// Resets the app to its default settings and deletes the current db.
        /// </summary>
        private async Task resetAppAsync()
        {
            pleaseWait_grid.Visibility = Visibility.Visible;
            await CalendarManager.INSTANCE.deleteCalendarAsync();
            Settings.setSetting(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING, false);
            Settings.setSetting(SettingsConsts.HIDE_WIZARD_ON_STARTUP, false);
            Settings.setSetting(SettingsConsts.DISABLE_EXAMPLE_WIDGET, false);
            Settings.setSetting(SettingsConsts.DISABLE_CANTEEN_WIDGET, false);
            Settings.setSetting(SettingsConsts.DISABLE_CALENDAR_WIDGET, false);
            Settings.setSetting(SettingsConsts.DISABLE_TUITION_FEE_WIDGET, false);
            Settings.setSetting(SettingsConsts.DISABLE_CRASH_REPORTING, false);
            Settings.setSetting(SettingsConsts.DISABLE_NEWS_WIDGET, false);
            Settings.setSetting(SettingsConsts.DISABLE_CALENDAR_INTEGRATION, false);
            Settings.setSetting(SettingsConsts.INITIALLY_STARTED, false);
            Settings.setSetting(SettingsConsts.ACCESS_TOKEN, null);
            Settings.setSetting(SettingsConsts.LAST_BACKGROUND_TASK_ACTION, null);
            Settings.setSetting(SettingsConsts.FACULTY_INDEX, null);
            Settings.setSetting(SettingsConsts.USER_ID, null);

            await deleteCacheAsync();
            pleaseWait_grid.Visibility = Visibility.Collapsed;
            Logger.Info("Finished reseting the app.");
        }

        /// <summary>
        /// Resets the apps cache (db).
        /// </summary>
        private async Task deleteCacheAsync()
        {
            pleaseWait_grid.Visibility = Visibility.Visible;
            await CacheManager.INSTANCE.clearCacheAsync();
            AbstractManager.resetDB();
            AbstractManager.deleteDB();

            SplashScreenPage extendedSplash = new SplashScreenPage();
            Window.Current.Content = extendedSplash;
            pleaseWait_grid.Visibility = Visibility.Collapsed;
            Logger.Info("Finished deleting the app cache.");
        }

        /// <summary>
        /// Shows the size of the "Logs" folder.
        /// </summary>
        private void showLogSize()
        {
            logSize_tblck.Text = UIUtils.getLocalizedString("SettingsPageLogSizeCalculating_Text");
            Task.Factory.StartNew(async () =>
            {
                long size = await Logger.getLogFolderSizeAsync();
                string text = "~ ";
                if (size >= 1024)
                {
                    size /= 1024;
                    text += size + " MB";
                }
                else
                {
                    text += size + " KB";
                }
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    logSize_tblck.Text = text;
                });
            });
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void disableCalendar_tgls_ToggledAsync(object sender, RoutedEventArgs e)
        {
            if (disableCalendar_tgls.IsOn)
            {
                await CalendarManager.INSTANCE.deleteCalendarAsync();
            }
            else
            {
                CalendarManager.INSTANCE.syncCalendar(true);
            }
            Settings.setSetting(SettingsConsts.DISABLE_CALENDAR_INTEGRATION, disableCalendar_tgls.IsOn);
        }

        private async void exportLogs_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Logger.exportLogs();
        }

        private void showWizard_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame f)
            {
                f.Navigate(typeof(SetupPageStep1));
            }
        }

        private void wifiOnly_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING, wifiOnly_tgls.IsOn);
        }

        private void hideWizardOnStartup_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.HIDE_WIZARD_ON_STARTUP, hideWizardOnStartup_tgls.IsOn);
        }

        private async void deleteCache_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(UIUtils.getLocalizedString("SettingsPageDeleteCache_Text"));
            dialog.Commands.Add(new UICommand { Label = UIUtils.getLocalizedString("MessageBoxNo_Text"), Id = 0 });
            dialog.Commands.Add(new UICommand { Label = UIUtils.getLocalizedString("MessageBoxYes_Text"), Id = 1 });
            IUICommand command = await dialog.ShowAsync();
            if ((int)command.Id == 1)
            {
                await deleteCacheAsync();
            }
        }

        private async void resetApp_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(UIUtils.getLocalizedString("SettingsPageResetApp_Text"));
            dialog.Commands.Add(new UICommand { Label = UIUtils.getLocalizedString("MessageBoxNo_Text"), Id = 0 });
            dialog.Commands.Add(new UICommand { Label = UIUtils.getLocalizedString("MessageBoxYes_Text"), Id = 1 });
            IUICommand command = await dialog.ShowAsync();
            if ((int)command.Id == 1)
            {
                await resetAppAsync();
            }
        }

        private async void openLogFolder_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Logger.openLogFolderAsync();
        }

        private void diableExampleWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.DISABLE_EXAMPLE_WIDGET, disableExampleWidget_tgls.IsOn);
        }

        private void diableCanteenWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.DISABLE_CANTEEN_WIDGET, disableCanteenWidget_tgls.IsOn);
        }

        private void disableCalendarWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.DISABLE_CALENDAR_WIDGET, disableCalendar_tgls.IsOn);
        }

        private void disableTuitionFeeWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.DISABLE_TUITION_FEE_WIDGET, disableTuitionFeeWidget_tgls.IsOn);
        }

        private void disableNewsWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.DISABLE_NEWS_WIDGET, disableNewsWidget_tgls.IsOn);
        }

        private async void feedback_stckp_TappedAsync(object sender, TappedRoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private async void contributeGithub_stckp_TappedAsync(object sender, TappedRoutedEventArgs e)
        {
            await Util.launchBrowser(new Uri("https://github.com/COM8/UWP-TUM-Campus-App"));
        }

        private async void license_stckp_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Util.launchBrowser(new Uri("https://github.com/COM8/UWP-TUM-Campus-App/blob/master/LICENSE"));
        }

        private async void reportBug_stckp_TappedAsync(object sender, TappedRoutedEventArgs e)
        {
            await Util.launchBrowser(new Uri("https://github.com/COM8/UWP-TUM-Campus-App/issues"));
        }

        private async void deleteLogs_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(UIUtils.getLocalizedString("SettingsPageDeleteLoggs_Text"));
            dialog.Commands.Add(new UICommand { Label = UIUtils.getLocalizedString("MessageBoxNo_Text"), Id = 0 });
            dialog.Commands.Add(new UICommand { Label = UIUtils.getLocalizedString("MessageBoxYes_Text"), Id = 1 });
            IUICommand command = await dialog.ShowAsync();
            if ((int)command.Id == 1)
            {
                await Logger.deleteLogsAsync();
            }
            showLogSize();
        }

        private void disableBackgroundTask_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.DISABLE_BACKGROUND_TASKS, disableBackgroundTask_tgls.IsOn);
            if (disableBackgroundTask_tgls.IsOn)
            {
                MyBackgroundTaskHelper.RemoveBackgroundTask();
            }
            else
            {
                MyBackgroundTaskHelper.RegisterBackgroundTask();
            }
        }

        private async void privacyPolicy_stckp_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Util.launchBrowser(new Uri("https://github.com/COM8/UWP-TUM-Campus-App/blob/master/PRIVACY_POLICY.md"));
        }

        private void showToken_btn_Click(object sender, RoutedEventArgs e)
        {
            if (tumonlineToken_tbx.Visibility == Visibility.Visible)
            {
                tumonlineToken_tbx.Text = "";
                tumonlineToken_tbx.Visibility = Visibility.Collapsed;
                showToken_btn.Content = UIUtils.getLocalizedString("SettingsPageShowToken_Text");
            }
            else
            {
                string token = TumManager.getToken();
                tumonlineToken_tbx.Text = token == null ? UIUtils.getLocalizedString("SettingsPageNoToken_Text") : token;
                tumonlineToken_tbx.Visibility = Visibility.Visible;
                showToken_btn.Content = UIUtils.getLocalizedString("SettingsPageShowToken_Text");
            }
        }

        private void disableCrashReporting_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.DISABLE_CRASH_REPORTING, disableCrashReporting_tgls.IsOn);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            showLogSize();
        }

        private async void setupEduroam_btn_Click(object sender, RoutedEventArgs e)
        {
            EduroamHelperDialog dialog = new EduroamHelperDialog();
            await dialog.ShowAsync();
        }

        #endregion
    }
}
