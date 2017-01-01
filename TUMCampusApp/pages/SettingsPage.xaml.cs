using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.classes;
using TUMCampusApp.classes.managers;
using TUMCampusApp.pages.setup;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.pages
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
            Utillities.setSetting(Const.ONLY_USE_WIFI_FOR_UPDATING, false);
            Utillities.setSetting(Const.HIDE_WIZARD_ON_STARTUP, false);
            Utillities.setSetting(Const.ACCESS_TOKEN, null);

            deleteCache();
        }

        private void deleteCache()
        {
            AbstractManager.resetDB();
            AbstractManager.deleteDB();

            SplashScreenPage extendedSplash = new SplashScreenPage();
            Window.Current.Content = extendedSplash;
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
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
        #endregion

    }
}
