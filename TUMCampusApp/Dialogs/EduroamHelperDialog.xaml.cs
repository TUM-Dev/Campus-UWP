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
            showStatus("Requesting wireless adapter access...");
            WiFiAccessStatus status = await helper.requestAccessAsync();
            if(status == WiFiAccessStatus.Allowed)
            {
                showStatus("Loading wireless adapters...");
                WiFiAdapter adapter = await helper.loadAdapterAsync();
                if(adapter != null)
                {
                    showStatus("Searching for eduroam network...");
                    await helper.startSearchingAsync();
                    helper.EduroamNetworkFound += Helper_EduroamNetworkFound;
                    await helper.startSearchingAsync();
                }
                else
                {
                    // No adapter:
                    showStatus("ERROR no wireless adapter found!");
                    enableButtons();
                }
            }
            else
            {
                // Access denied:
                showStatus("ERROR access to wireless adapter denied!");
                enableButtons();
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
            showStatus("Installing certificate...");

            showStatus("Finished certificate installation.");
        }

        private void showStatus(string msg)
        {
            status_tbx.Text = msg;
            status_tbx.Visibility = Visibility.Visible;
        }

        private void disableButtons()
        {
            setup_btn.IsEnabled = false;
            setup_prgr.Visibility = Visibility.Visible;

            connetToEduroam_btn.IsEnabled = false;
            connetToEduroam_prgr.Visibility = Visibility.Visible;

            installCert_btn.IsEnabled = false;
            installCert_prgr.Visibility = Visibility.Visible;
        }

        private void enableButtons()
        {
            setup_btn.IsEnabled = true;
            setup_prgr.Visibility = Visibility.Collapsed;

            connetToEduroam_btn.IsEnabled = true;
            connetToEduroam_prgr.Visibility = Visibility.Collapsed;

            installCert_btn.IsEnabled = true;
            installCert_prgr.Visibility = Visibility.Collapsed;
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
            disableButtons();

            await installCertAsync();
            await connectToEduroamAsync();
        }

        private async void installCert_btn_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();

            await installCertAsync();
        }

        private async void connetToEduroam_btn_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();

            await connectToEduroamAsync();
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            helper.EduroamNetworkFound -= Helper_EduroamNetworkFound;
            helper.stopSearching();
        }

        private async void Helper_EduroamNetworkFound(WiFiAdapter adapter, Classes.Events.EduroamNetworkFoundEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                showStatus("Connecting to eduroam...");
                PasswordCredential passwordCredential = new PasswordCredential
                {
                    Password = password_pwbx.Password,
                    UserName = userName_tbx.Text
                };
                Task.Run(async () =>
                {
                    WiFiConnectionResult result = await helper.connectAsync(args.NETWORK, WiFiReconnectionKind.Automatic, passwordCredential);
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        showStatus("Connecting to eduroam finished with: " + result.ConnectionStatus.ToString());
                        enableButtons();
                    });
                });
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
