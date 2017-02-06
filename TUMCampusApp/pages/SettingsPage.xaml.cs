using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Classes.Managers;
using TUMCampusApp.Pages.Setup;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        private async Task<StorageFile> getTargetPathAsync()
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Logs", new List<string>() { ".zip" });
            savePicker.SuggestedFileName = "Logs";
            return await savePicker.PickSaveFileAsync(); ;
        }

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
            disableExampleWidget_tgls.IsOn = Utillities.getSettingBoolean(Const.DISABLE_EXAMPLE_WIDGET);
            disableCanteenWidget_tgls.IsOn = Utillities.getSettingBoolean(Const.DISABLE_CANTEEN_WIDGET);
            disableCalendarWidget_tgls.IsOn = Utillities.getSettingBoolean(Const.DISABLE_CALENDAR_WIDGET);
            disableTuitionFeeWidget_tgls.IsOn = Utillities.getSettingBoolean(Const.DISABLE_TUITION_FEE_WIDGET);
        }

        private void initServices()
        {
            disableCalendar_tgls.IsOn = Utillities.getSettingBoolean(Const.DISABLE_CALENDAR_INTEGRATION);
        }

        private void initGeneralControls()
        {
            wifiOnly_tgls.IsOn = Utillities.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING);
        }

        private void initTUMonlineControls()
        {
            hideWizardOnStartup_tgls.IsOn = Utillities.getSettingBoolean(Const.HIDE_WIZARD_ON_STARTUP);
        }

        private void resetApp()
        {
            Task.Factory.StartNew(async () =>
            {
                await CalendarManager.INSTANCE.deleteCalendarAsync();
            });
            Utillities.setSetting(Const.ONLY_USE_WIFI_FOR_UPDATING, false);
            Utillities.setSetting(Const.HIDE_WIZARD_ON_STARTUP, false);
            Utillities.setSetting(Const.DISABLE_EXAMPLE_WIDGET, false);
            Utillities.setSetting(Const.DISABLE_CANTEEN_WIDGET, false);
            Utillities.setSetting(Const.DISABLE_CALENDAR_WIDGET, false);
            Utillities.setSetting(Const.DISABLE_TUITION_FEE_WIDGET, false);
            Utillities.setSetting(Const.DISABLE_CALENDAR_INTEGRATION, false);
            Utillities.setSetting(Const.ACCESS_TOKEN, null);

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

        private async Task exportLogs()
        {
            StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Logs");
            if (folder != null)
            {
                StorageFile target = await getTargetPathAsync();
                if (target == null)
                {
                    return;
                }
                await Task.Factory.StartNew(async () =>
                 {
                     try
                     {
                         IStorageItem file = await ApplicationData.Current.LocalFolder.GetFileAsync("Logs.zip");
                         if (file != null)
                         {
                             await file.DeleteAsync();
                         }
                         ZipFile.CreateFromDirectory(folder.Path, ApplicationData.Current.LocalFolder.Path + @"\Logs.zip", CompressionLevel.Optimal, false);
                         file = await ApplicationData.Current.LocalFolder.GetFileAsync("Logs.zip");
                         if (file != null && file is StorageFile)
                         {
                             StorageFile f = file as StorageFile;
                             await f.CopyAndReplaceAsync(target);
                         }
                         Logger.Info("Exported logs successfully.");
                     }
                     catch (Exception e)
                     {
                         Logger.Error("Error during exporting loggs", e);
                     }
                 });
            }
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
            Utillities.setSetting(Const.DISABLE_CALENDAR_INTEGRATION, disableCalendar_tgls.IsOn);
        }

        private async void exportLogs_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            await exportLogs();
        }

        private void showWizard_btn_Click(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(SetupPageStep1));
        }

        private void wifiOnly_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Utillities.setSetting(Const.ONLY_USE_WIFI_FOR_UPDATING, wifiOnly_tgls.IsOn);
        }

        private void hideWizardOnStartup_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Utillities.setSetting(Const.HIDE_WIZARD_ON_STARTUP, hideWizardOnStartup_tgls.IsOn);
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
            Utillities.setSetting(Const.DISABLE_EXAMPLE_WIDGET, disableExampleWidget_tgls.IsOn);
        }

        private void diableCanteenWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Utillities.setSetting(Const.DISABLE_CANTEEN_WIDGET, disableCanteenWidget_tgls.IsOn);
        }

        private void disableCalendarWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Utillities.setSetting(Const.DISABLE_CALENDAR_WIDGET, disableCalendar_tgls.IsOn);
        }

        private void disableTuitionFeeWidget_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            Utillities.setSetting(Const.DISABLE_TUITION_FEE_WIDGET, disableTuitionFeeWidget_tgls.IsOn);
        }

        private async void feedback_stckp_TappedAsync(object sender, TappedRoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private async void contributeGithub_stckp_TappedAsync(object sender, TappedRoutedEventArgs e)
        {
            await Utillities.launchBrowser(new Uri("https://github.com/COM8/UWP-TUM-Campus-App"));
        }

        private async void reportBug_stckp_TappedAsync(object sender, TappedRoutedEventArgs e)
        {
            await Utillities.launchBrowser(new Uri("https://github.com/COM8/UWP-TUM-Campus-App/issues"));
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

        #endregion
    }
}
