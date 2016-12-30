using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TUMCampusApp.classes.managers;
using TUMCampusApp.pages.setup;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            wifiOnly_tgls.IsOn = UserDataManager.INSTANCE.onlyUpdateWhileConnectedToWifi();
        }

        private void initTUMonlineControls()
        {
            hideWizardOnStartup_tgls.IsOn = UserDataManager.INSTANCE.shouldHideWizardOnStartup();
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

        #endregion

        private void wifiOnly_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            UserDataManager.INSTANCE.setOnlyUpdateWhileConnectedToWifi(wifiOnly_tgls.IsOn);
        }

        private void hideWizardOnStartup_tgls_Toggled(object sender, RoutedEventArgs e)
        {
            UserDataManager.INSTANCE.setShouldHideWizardOnStartup(hideWizardOnStartup_tgls.IsOn);
        }
    }
}
