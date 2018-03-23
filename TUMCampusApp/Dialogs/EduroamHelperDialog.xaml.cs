using System;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Classes.Helpers;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            showStatus(Utillities.getLocalizedString("EduroamHelperDialogRequestingAccess_Text"));
            WiFiAccessStatus status = await helper.requestAccessAsync();
            if (status == WiFiAccessStatus.Allowed)
            {
                showStatus(Utillities.getLocalizedString("EduroamHelperDialogLoadingAdapters_Text"));
                WiFiAdapter adapter = await helper.loadAdapterAsync();
                if (adapter != null)
                {
                    showStatus(Utillities.getLocalizedString("EduroamHelperDialogSearchingEduroam_Text"));
                    await helper.startSearchingAsync();
                    helper.EduroamNetworkFound += Helper_EduroamNetworkFound;
                    await helper.startSearchingAsync();
                }
                else
                {
                    // No adapter:
                    showStatus(Utillities.getLocalizedString("EduroamHelperDialogErrorNoAdapter_Text"));
                    enableButtons();
                }
            }
            else
            {
                // Access denied:
                showStatus(Utillities.getLocalizedString("EduroamHelperDialogErrorAdapterNoAccess_Text"));
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
            showStatus(Utillities.getLocalizedString("EduroamHelperDialogInstallingCert_Text"));

            await helper.installCertificateAsync();

            showStatus(Utillities.getLocalizedString("EduroamHelperDialogInstallingCertFinished_Text"));
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
            setup_prgr.IsActive = true;

            connetToEduroam_btn.IsEnabled = false;
            connetToEduroam_prgr.Visibility = Visibility.Visible;
            connetToEduroam_prgr.IsActive = true;

            installCert_btn.IsEnabled = false;
            installCert_prgr.Visibility = Visibility.Visible;
            installCert_prgr.IsActive = true;
        }

        private void enableButtons()
        {
            setup_btn.IsEnabled = true;
            setup_prgr.Visibility = Visibility.Collapsed;
            setup_prgr.IsActive = false;

            connetToEduroam_btn.IsEnabled = true;
            connetToEduroam_prgr.Visibility = Visibility.Collapsed;
            connetToEduroam_prgr.IsActive = false;

            installCert_btn.IsEnabled = true;
            installCert_prgr.Visibility = Visibility.Collapsed;
            installCert_prgr.IsActive = false;
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
                showStatus(Utillities.getLocalizedString("EduroamHelperDialogConnectingToEduroam_Text"));
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
                        showStatus(Utillities.getLocalizedString("EduroamHelperDialogConnectingToEduroamFinished_Text") + result.ConnectionStatus.ToString());
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
