using System;
using TUMCampusApp.Classes;
using TUMCampusAppAPI;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
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
            Util.setSetting(Const.TUMO_ENABLED, false);
            Util.setSetting(Const.HIDE_WIZARD_ON_STARTUP, true);
            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
        }

        private async void next_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            if(!await TumManager.INSTANCE.isTokenConfirmedAsync()){
                MessageDialog message = new MessageDialog(Utillities.getLocalizedString("ActivateTokenFirst_Text"));
                message.Title = Utillities.getLocalizedString("Error_Text");
                await message.ShowAsync();
                return;
            }
            Util.setSetting(Const.TUMO_ENABLED, true);
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
