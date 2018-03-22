using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Helpers;
using Windows.Devices.WiFi;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Dialogs
{
    public sealed partial class EduroamHelperDialog : ContentDialog
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private EduroamHelper helper;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 21/03/2018 Created [Fabian Sauter]
        /// </history>
        public EduroamHelperDialog()
        {
            this.helper = new EduroamHelper();
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private async Task connectToEduroamAsync()
        {
            WiFiAccessStatus status = await helper.requestAccessAsync();
            if(status == WiFiAccessStatus.Allowed)
            {
                WiFiAdapter adapter = await helper.loadAdapterAsync();
                if(adapter != null)
                {
                    await helper.startSearchingAsync();
                    helper.EduroamNetworkFound += Helper_EduroamNetworkFound;
                    await helper.startSearchingAsync();
                }
                else
                {
                    // No adapter
                }
            }
            else
            {
                // Access denied
            }
        }

        private void onPasswordOrUserNameChanged()
        {
            bool hasPasswordAndUserName = !string.IsNullOrWhiteSpace(userName_tbx.Text) && !string.IsNullOrWhiteSpace(password_pwbx.Password);
            setup_btn.IsEnabled = hasPasswordAndUserName;
            connetToEduroam_btn.IsEnabled = hasPasswordAndUserName;
        }

        private async Task installCertAsync()
        {

        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private async void setup_btn_Click(object sender, RoutedEventArgs e)
        {
            await installCertAsync();
            await connectToEduroamAsync();
        }

        private async void installCert_btn_Click(object sender, RoutedEventArgs e)
        {
            await installCertAsync();
        }

        private async void connetToEduroam_btn_Click(object sender, RoutedEventArgs e)
        {
            await connectToEduroamAsync();
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            //helper.EduroamNetworkFound -= Helper_EduroamNetworkFound;
            helper.stopSearching();
        }

        private async void Helper_EduroamNetworkFound(WiFiAdapter adapter, Classes.Events.EduroamNetworkFoundEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PasswordCredential passwordCredential = new PasswordCredential
                {
                    Password = password_pwbx.Password,
                    UserName = userName_tbx.Text
                };
                Task t = helper.connectAsync(args.NETWORK, WiFiReconnectionKind.Automatic, passwordCredential);
            });
        }

        private void userName_tbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            onPasswordOrUserNameChanged();
        }

        private void password_pwbx_PasswordChanged(object sender, RoutedEventArgs e)
        {
            onPasswordOrUserNameChanged();
        }

        #endregion
    }
}
