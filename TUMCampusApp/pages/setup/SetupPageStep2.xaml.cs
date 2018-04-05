using System;
using System.Threading.Tasks;
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
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
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
        /// <summary>
        /// Disables all buttons and changes the next_tbx text.
        /// </summary>
        private void startValidatToken()
        {
            next_btn.IsEnabled = false;
            skip_btn.IsEnabled = false;
            startOverAgain_btn.IsEnabled = false;
            requestNewToken_btn.IsEnabled = false;
            validating_pgr.Visibility = Visibility.Visible;
            validating_pgr.IsActive = true;
            next_tbx.Text = UIUtils.getLocalizedString("SetupPage2ValidatingToken_Text");
        }

        /// <summary>
        /// Enables all buttons again and changes the next_tbx text.
        /// </summary>
        private void finishValidatingToken()
        {
            next_btn.IsEnabled = true;
            skip_btn.IsEnabled = true;
            startOverAgain_btn.IsEnabled = true;
            requestNewToken_btn.IsEnabled = true;
            validating_pgr.Visibility = Visibility.Visible;
            validating_pgr.IsActive = false;
            next_tbx.Text = UIUtils.getLocalizedString("Next_Text");
        }

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
            if (Window.Current.Content is Frame f)
            {
                f.Navigate(typeof(MainPage));
            }
        }

        private async void next_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            startValidatToken();
            if (!await TumManager.INSTANCE.isTokenConfirmedAsync()){
                MessageDialog message = new MessageDialog(UIUtils.getLocalizedString("ActivateTokenFirst_Text"));
                message.Title = UIUtils.getLocalizedString("Error_Text");
                await message.ShowAsync();
            }
            else
            {
                Util.setSetting(Const.TUMO_ENABLED, true);
                Frame f = new Frame();
                f.Navigate(typeof(MainPage));
                Window.Current.Content = f;
            }
            finishValidatingToken();
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

        private async void requestNewToken_btn_Click(object sender, RoutedEventArgs e)
        {
            requestNewToken_btn.IsEnabled = false;
            MessageDialog dialog = new MessageDialog(UIUtils.getLocalizedString("SetupPageRequestNewTokenMessageBox_Text"));
            dialog.Commands.Add(new UICommand { Label = UIUtils.getLocalizedString("MessageBoxYes_Text"), Id = 0 });
            dialog.Commands.Add(new UICommand { Label = UIUtils.getLocalizedString("MessageBoxNo_Text"), Id = 1 });
            IUICommand command = await dialog.ShowAsync();
            if ((int)command.Id == 0)
            {
                string result = await TumManager.INSTANCE.reqestNewTokenAsync(Util.getSettingString(Const.USER_ID));
                if (result == null)
                {
                    MessageDialog message = new MessageDialog(UIUtils.getLocalizedString("RequestNewTokenError_Text"));
                    message.Title = UIUtils.getLocalizedString("Error_Text");
                    await message.ShowAsync();
                }
                else if (result.Contains("Es wurde kein Benutzer zu diesen Benutzerdaten gefunden"))
                {
                    MessageDialog message = new MessageDialog(UIUtils.getLocalizedString("InvalidId_Text"));
                    message.Title = UIUtils.getLocalizedString("Error_Text");
                    await message.ShowAsync();
                    if (Window.Current.Content is Frame f)
                    {
                        f.Navigate(typeof(SetupPageStep2));
                    }
                    return;
                }
                else
                {
                    await Util.showMessageBoxAsync(UIUtils.getLocalizedString("SetupPageRequestedNewTokenSuccessMessageBox_Text"));
                }
            }
            requestNewToken_btn.IsEnabled = true;
        }

        #endregion

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            startValidatToken();
            if (await TumManager.INSTANCE.isTokenConfirmedAsync())
            {
                Util.setSetting(Const.TUMO_ENABLED, true);
                Frame f = new Frame();
                f.Navigate(typeof(MainPage));
                Window.Current.Content = f;
            }
            finishValidatingToken();
        }

        private void startOverAgain_btn_Click(object sender, RoutedEventArgs e)
        {
            Frame f = new Frame();
            f.Navigate(typeof(SetupPageStep1));
            Window.Current.Content = f;
        }
    }
}
