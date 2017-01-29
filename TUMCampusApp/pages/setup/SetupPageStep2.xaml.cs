using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TUMCampusApp.Classes;
using TUMCampusApp.Classes.Managers;
using TUMCampusApp.Classes.Tum;
using TUMCampusApp.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Pages.Setup
{
    public sealed partial class SetupPageStep2 : Page
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
        public SetupPageStep2()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void tumOnline_hbtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            bool success = await Windows.System.Launcher.LaunchUriAsync(new Uri("https://campus.tum.de/tumonline/webnav.ini"));
        }

        private void skip_btn_Click(object sender, RoutedEventArgs e)
        {
            Utillities.setSetting(Const.TUMO_ENABLED, false);
            Utillities.setSetting(Const.HIDE_WIZARD_ON_STARTUP, true);
            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
        }

        private async void next_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            if(!await TumManager.INSTANCE.isTokenConfirmedAsync()){
                MessageDialog message = new MessageDialog("Please activate the token first!");
                message.Title = "Error!";
                await message.ShowAsync();
                return;
            }
            Utillities.setSetting(Const.TUMO_ENABLED, true);
            Frame f = new Frame();
            f.Navigate(typeof(MainPage));
            Window.Current.Content = f;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                return;
            }
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        #endregion
    }
}
