using Data_Manager;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Classes.Helpers;
using TUMCampusAppAPI;
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

        private ObservableCollection<string> suggestions;

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
            this.suggestions = new ObservableCollection<string>();
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
        /// <summary>
        /// Updates the user names auto suggest box with its suggestions.
        /// </summary>
        private void updateSuggestions()
        {
            suggestions.Clear();
            if (!string.IsNullOrWhiteSpace(userName_asgbx.Text))
            {
                int index = userName_asgbx.Text.IndexOf('@');
                if (index > 0)
                {
                    string userName = userName_asgbx.Text.Substring(0, index);
                    foreach (string extension in Consts.EDUROAM_NAME_EXTENSIONS)
                    {
                        suggestions.Add(userName + extension);
                    }
                }
                else
                {
                    foreach (string extension in Consts.EDUROAM_NAME_EXTENSIONS)
                    {
                        suggestions.Add(userName_asgbx.Text + extension);
                    }
                }
            }
        }

        /// <summary>
        /// Starts connecting to the eduroam wifi network.
        /// </summary>
        private async Task connectToEduroamAsync()
        {
            Logger.Info("Starting setting up the eduroam connection...");
            showStatus(UIUtils.getLocalizedString("EduroamHelperDialogRequestingAccess_Text"));
            WiFiAccessStatus status = await helper.requestWifiAdapterAccessAsync();
            if (status == WiFiAccessStatus.Allowed)
            {
                showStatus(UIUtils.getLocalizedString("EduroamHelperDialogLoadingAdapters_Text"));
                WiFiAdapter adapter = await helper.loadAdapterAsync();
                if (adapter != null)
                {
                    showStatus(UIUtils.getLocalizedString("EduroamHelperDialogSearchingEduroam_Text"));
                    helper.EduroamNetworkFound += Helper_EduroamNetworkFound;
                    await helper.startSearchingAsync(adapter);
                }
                else
                {
                    // No adapter:
                    showStatus(UIUtils.getLocalizedString("EduroamHelperDialogErrorNoAdapter_Text"));
                    enableControl();
                }
            }
            else
            {
                // Access denied:
                showStatus(UIUtils.getLocalizedString("EduroamHelperDialogErrorAdapterNoAccess_Text"));
                enableControl();
            }
        }

        /// <summary>
        /// Enables or disables the setup_btn and connetToEduroam_btn based on the current user name and password.
        /// </summary>
        private void onPasswordOrUserNameChanged()
        {
            bool hasPasswordAndUserName = !string.IsNullOrWhiteSpace(userName_asgbx.Text) && !string.IsNullOrWhiteSpace(password_pwbx.Password);
            setup_btn.IsEnabled = hasPasswordAndUserName;
            connetToEduroam_btn.IsEnabled = hasPasswordAndUserName;
        }

        /// <summary>
        /// Installs the eduroam certificate.
        /// </summary>
        private async Task installCertAsync()
        {
            showStatus(UIUtils.getLocalizedString("EduroamHelperDialogInstallingCert_Text"));

            try
            {
                await helper.installCertificateAsync();
                showStatus(UIUtils.getLocalizedString("EduroamHelperDialogInstallingCertFinished_Text"));
                Logger.Info("Certificate installation finished!");
            }
            catch (Exception e)
            {
                showStatus(UIUtils.getLocalizedString("EduroamHelperDialogInstallingCertFailed_Text") + e.Message);
                Logger.Error("Certificate installation failed!", e);
            }

            enableControl();
        }

        /// <summary>
        /// Shows a given status message.
        /// </summary>
        /// <param name="msg">The status message that should get shown.</param>
        private void showStatus(string msg)
        {
            status_tbx.Text = msg;
            status_tbx.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Disables all controls.
        /// </summary>
        private void disableControls()
        {
            userName_asgbx.IsEnabled = false;
            password_pwbx.IsEnabled = false;

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

        /// <summary>
        /// Enables all controls.
        /// </summary>
        private void enableControl()
        {
            userName_asgbx.IsEnabled = true;
            password_pwbx.IsEnabled = true;

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
            disableControls();

            await installCertAsync();
            await connectToEduroamAsync();
        }

        private async void installCert_btn_Click(object sender, RoutedEventArgs e)
        {
            disableControls();

            await installCertAsync();
        }

        private async void connetToEduroam_btn_Click(object sender, RoutedEventArgs e)
        {
            disableControls();

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
                showStatus(UIUtils.getLocalizedString("EduroamHelperDialogConnectingToEduroam_Text"));
                PasswordCredential passwordCredential = new PasswordCredential
                {
                    Password = password_pwbx.Password,
                    UserName = userName_asgbx.Text
                };
                Task.Run(async () =>
                {
                    WiFiConnectionResult result = await helper.connectAsync(args.NETWORK, WiFiReconnectionKind.Automatic, passwordCredential);
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        showStatus(UIUtils.getLocalizedString("EduroamHelperDialogConnectingToEduroamFinished_Text") + result.ConnectionStatus.ToString());
                        enableControl();
                    });
                });
            });
        }

        private void userName_asgbx_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            onPasswordOrUserNameChanged();
            updateSuggestions();
        }

        private void password_pwbx_PasswordChanged(object sender, RoutedEventArgs e)
        {
            onPasswordOrUserNameChanged();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            string userId = Settings.getSettingString(SettingsConsts.USER_ID);
            if (userId != null)
            {
                userName_asgbx.Text = userId;
            }
        }

        #endregion
    }
}
