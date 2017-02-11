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

namespace TUMCampusApp.Pages
{
    public sealed partial class SettingsPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
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
        

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void initControls()
        {
            initGeneralControls();
            initTUMonlineControls();
            initWidgetControls();
            initServices();
            initAboutAndLinks();
        }

        private void initAboutAndLinks()
        {
            if (!Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                feedback_stckp.Visibility = Visibility.Collapsed;
            }
        }

        private void initWidgetControls()
        {
            disableExampleWidget_tgls.IsOn = Util.getSettingBoolean(Const.DISABLE_EXAMPLE_WIDGET);
            disableCanteenWidget_tgls.IsOn = Util.getSettingBoolean(Const.DISABLE_CANTEEN_WIDGET);
            disableCalendarWidget_tgls.IsOn = Util.getSettingBoolean(Const.DISABLE_CALENDAR_WIDGET);
            disableTuitionFeeWidget_tgls.IsOn = Util.getSettingBoolean(Const.DISABLE_TUITION_FEE_WIDGET);
        }

        private void initServices()
        {
            disableCalendar_tgls.IsOn = Util.getSettingBoolean(Const.DISABLE_CALENDAR_INTEGRATION);
            disableCalendar_tgls.IsOn = Util.getSettingBoolean(Const.DISABLE_BACKGROUND_TASKS);
        }

        private void initGeneralControls()
        {
            wifiOnly_tgls.IsOn = Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING);
        }

        private void initTUMonlineControls()
        {
            hideWizardOnStartup_tgls.IsOn = Util.getSettingBoolean(Const.HIDE_WIZARD_ON_STARTUP);
        }

        private void resetApp()
        {
            Task.Factory.StartNew(async () =>
            {
                await CalendarManager.INSTANCE.deleteCalendarAsync();
            });
            Util.setSetting(Const.ONLY_USE_WIFI_FOR_UPDATING, false);
            Util.setSetting(Const.HIDE_WIZARD_ON_STARTUP, false);
            Util.setSetting(Const.DISABLE_EXAMPLE_WIDGET, false);
            Util.setSetting(Const.DISABLE_CANTEEN_WIDGET, false);
            Util.setSetting(Const.DISABLE_CALENDAR_WIDGET, false);
            Util.setSetting(Const.DISABLE_TUITION_FEE_WIDGET, false);
            Util.setSetting(Const.DISABLE_CALENDAR_INTEGRATION, false);
            Util.setSetting(Const.ACCESS_TOKEN, null);

            deleteCache();
            Logger.Info("Finished reseting the app.");
        }

        private void deleteCache()
        {
            AbstractManager.resetDB();
            AbstractManager.deleteDB();

            SplashScreenPage extendedSplash = new SplashScreenPage();
            Window.Current.Content = extendedSplash;
            Logger.Info("Finished deleting the app cache.");
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
            Util.setSetting(Const.DISABLE_CALENDAR_INTEGRATION, disableCalendar_tgls.IsOn);
        }

        private async void exportLogs_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Logger.exportLogs();
        }

        private void showWizard_btn_Click(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(SetupPageStep1));
        }

        private void wifiOnly_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Util.setSetting(Const.ONLY_USE_WIFI_FOR_UPDATING, wifiOnly_tgls.IsOn);
        }

        private void hideWizardOnStartup_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Util.setSetting(Const.HIDE_WIZARD_ON_STARTUP, hideWizardOnStartup_tgls.IsOn);
        }

        private async void deleteCache_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog("Do you really want to delete the Apps cache?");
            dialog.Commands.Add(new UICommand { Label = "No!", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Yes!", Id = 1 });
            IUICommand command = await dialog.ShowAsync();
            if ((int)command.Id == 1)
            {
                deleteCache();
            }
        }

        private async void resetApp_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog("Do you really want to reset the App?");
            dialog.Commands.Add(new UICommand { Label = "No!", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Yes!", Id = 1 });
            IUICommand command = await dialog.ShowAsync();
            if ((int)command.Id == 1)
            {
                resetApp();
            }
        }

        private async void openLogFolder_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Logger.openLogFolderAsync();
        }

        private void diableExampleWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Util.setSetting(Const.DISABLE_EXAMPLE_WIDGET, disableExampleWidget_tgls.IsOn);
        }

        private void diableCanteenWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Util.setSetting(Const.DISABLE_CANTEEN_WIDGET, disableCanteenWidget_tgls.IsOn);
        }

        private void disableCalendarWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Util.setSetting(Const.DISABLE_CALENDAR_WIDGET, disableCalendar_tgls.IsOn);
        }

        private void disableTuitionFeeWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Util.setSetting(Const.DISABLE_TUITION_FEE_WIDGET, disableTuitionFeeWidget_tgls.IsOn);
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

        private async void reportBug_stckp_TappedAsync(object sender, TappedRoutedEventArgs e)
        {
            await Util.launchBrowser(new Uri("https://github.com/COM8/UWP-TUM-Campus-App/issues"));
        }

        private async void deleteLogs_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog("Do you really want to delete all logs?");
            dialog.Commands.Add(new UICommand { Label = "No!", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Yes!", Id = 1 });
            IUICommand command = await dialog.ShowAsync();
            if ((int)command.Id == 1)
            {
                await Logger.deleteLogsAsync();
            }
        }

        private void disableBackgroundTask_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Util.setSetting(Const.DISABLE_BACKGROUND_TASKS, disableBackgroundTask_tgls.IsOn);
            if (disableBackgroundTask_tgls.IsOn)
            {
                MyBackgroundTaskHelper.RemoveBackgroundTask();
            }
            else
            {
                MyBackgroundTaskHelper.RegisterBackgroundTask();
            }
        }

        #endregion
    }
}
