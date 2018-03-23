using System;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Events;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;
using Windows.Security.Cryptography.Certificates;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace TUMCampusApp.Classes.Helpers
{
    class EduroamHelper
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public const string EDUROAM_SSID = "eduroam";
        public const string EDUROAM_CERT_PATH = "ms-appx:///Assets/Resources/Telekom_root_cert_eduroam.cer";

        private WiFiAdapter adapter = null;
        private DispatcherTimer timer = null;
        private bool stopRequested;

        public delegate void EduroamNetworkFoundEventHandler(WiFiAdapter adapter, EduroamNetworkFoundEventArgs args);

        public event EduroamNetworkFoundEventHandler EduroamNetworkFound;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 21/03/2018 Created [Fabian Sauter]
        /// </history>
        public EduroamHelper()
        {
            this.stopRequested = false;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the eduroam certificate.
        /// </summary>
        public async Task<Certificate> getCertificateAsync()
        {
            StorageFile certFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(EDUROAM_CERT_PATH));
            IBuffer certBuffer = await FileIO.ReadBufferAsync(certFile);
            return new Certificate(certBuffer);
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Connects to the given wifi network with the given credentials.
        /// </summary>
        /// <param name="network">The wifi network you with to connect to.</param>
        /// <param name="wiFiReconnectionKind">The wifi reconnection type used for the given wifi network.</param>
        /// <param name="passwordCredential">The password and user name credentials.</param>
        /// <returns>Returns the connection result.</returns>
        public async Task<WiFiConnectionResult> connectAsync(WiFiAvailableNetwork network, WiFiReconnectionKind wiFiReconnectionKind, PasswordCredential passwordCredential)
        {
            return await adapter.ConnectAsync(network, wiFiReconnectionKind, passwordCredential);
        }

        /// <summary>
        /// Installs the eduroam certificate.
        /// </summary>
        /// <returns></returns>
        public async Task installCertificateAsync()
        {
            // Load certificate:
            Certificate cert = await getCertificateAsync();

            // Load certificate store:
            UserCertificateStore userStore = CertificateStores.GetUserStoreByName(StandardCertificateStoreNames.Personal);

            // Request add certificate:
            await userStore.RequestAddAsync(cert);
        }

        /// <summary>
        /// Requests access to the wifi adapter and returns the result.
        /// </summary>
        public async Task<WiFiAccessStatus> requestWifiAdapterAccessAsync()
        {
            return await WiFiAdapter.RequestAccessAsync();
        }

        /// <summary>
        /// Returns the first available wifi adapter.
        /// </summary>
        public async Task<WiFiAdapter> loadAdapterAsync()
        {
            DeviceInformationCollection adapterResult = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
            if (adapterResult.Count >= 1)
            {
                return await WiFiAdapter.FromIdAsync(adapterResult[0].Id);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Starts searching for the eduroam wifi network.
        /// </summary>
        /// <param name="adapter">The adapter for searching for the eduroam wifi network.</param>
        public async Task startSearchingAsync(WiFiAdapter adapter)
        {
            this.adapter = adapter;
            adapter.AvailableNetworksChanged += Adapter_AvailableNetworksChanged;
            await adapter.ScanAsync();

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 10)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Stops searching for the eduroam wifi network.
        /// </summary>
        public void stopSearching()
        {
            if (adapter != null)
            {
                adapter.AvailableNetworksChanged -= Adapter_AvailableNetworksChanged;
            }
            stopRequested = true;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Adapter_AvailableNetworksChanged(WiFiAdapter sender, object args)
        {
            foreach (WiFiAvailableNetwork n in sender.NetworkReport.AvailableNetworks)
            {
                if (string.Equals(n.Ssid, EDUROAM_SSID) && n.SecuritySettings.NetworkEncryptionType == NetworkEncryptionType.Ccmp && n.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.Rsna)
                {
                    stopSearching();
                    EduroamNetworkFound?.Invoke(sender, new EduroamNetworkFoundEventArgs(n));
                    return;
                }
            }
        }

        private async void Timer_Tick(object sender, object e)
        {
            if (stopRequested)
            {
                timer.Stop();
                return;
            }
            await adapter.ScanAsync();
        }

        #endregion
    }
}
