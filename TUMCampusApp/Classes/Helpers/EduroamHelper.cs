using System;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Events;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;
using Windows.UI.Xaml;

namespace TUMCampusApp.Classes.Helpers
{
    class EduroamHelper
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private WiFiAdapter adapter = null;
        private DispatcherTimer timer = null;

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
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        public async Task connectAsync(WiFiAvailableNetwork network, WiFiReconnectionKind wiFiReconnectionKind, PasswordCredential passwordCredential)
        {
            await adapter.ConnectAsync(network, wiFiReconnectionKind, passwordCredential);
        }

        public async Task installCertificateAsync()
        {

        }

        public async Task<WiFiAccessStatus> requestAccessAsync()
        {
            return await WiFiAdapter.RequestAccessAsync();
        }

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

        public async Task startSearchingAsync()
        {
            WiFiAccessStatus access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                // No access:
            }
            else
            {
                DeviceInformationCollection adapterResult = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                if (adapterResult.Count >= 1)
                {
                    adapter = await WiFiAdapter.FromIdAsync(adapterResult[0].Id);
                    adapter.AvailableNetworksChanged += Adapter_AvailableNetworksChanged;
                    await adapter.ScanAsync();
                }
                else
                {
                    // No wifi adapter
                }
            }

            await adapter.ScanAsync();
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 10)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void stopSearching()
        {
            adapter.AvailableNetworksChanged -= Adapter_AvailableNetworksChanged;
            timer?.Stop();
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
                if (Equals(n.Ssid, "eduroam") && n.SecuritySettings.NetworkEncryptionType == Windows.Networking.Connectivity.NetworkEncryptionType.None)
                {
                    stopSearching();
                    EduroamNetworkFound?.Invoke(sender, new EduroamNetworkFoundEventArgs(n));
                }
            }
        }

        private async void Timer_Tick(object sender, object e)
        {
            await adapter.ScanAsync();
        }

        #endregion
    }
}
