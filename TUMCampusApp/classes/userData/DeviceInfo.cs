using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

namespace TUMCampusApp.classes.userData
{
    class DeviceInfo
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static DeviceInfo INSTANCE;

        public string Id { get; private set; }
        public string Model { get; private set; }
        public string Manufracturer { get; private set; }
        public string Name { get; private set; }
        public static string OSName { get; set; }

        public MainPage mainPage;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/12/2016  Created [Fabian Sauter]
        /// </history>
        public DeviceInfo()
        {
            Id = getId();
            var deviceInformation = new EasClientDeviceInformation();
            Model = deviceInformation.SystemProductName;
            Manufracturer = deviceInformation.SystemManufacturer;
            Name = deviceInformation.FriendlyName;
            OSName = deviceInformation.OperatingSystem;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private static string getId()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
            {
                var token = HardwareIdentification.GetPackageSpecificToken(null);
                var hardwareId = token.Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                byte[] bytes = new byte[hardwareId.Length];
                dataReader.ReadBytes(bytes);

                return BitConverter.ToString(bytes).Replace("-", "");
            }

            throw new Exception("NO API FOR DEVICE ID PRESENT!");
        }

        /// <summary>
        /// Checks if the device is connected to the internet
        /// </summary>
        /// <history>
        /// 30/12/2016  Created [Fabian Sauter]
        /// </history>
        public static bool isConnectedToInternet()
        {
            ConnectionProfile cP = NetworkInformation.GetInternetConnectionProfile();
            return cP != null && cP.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

        /// <summary>
        /// Checks if the device is connected to a wifi network and has access to the internet
        /// </summary>
        /// <history>
        /// 30/12/2016  Created [Fabian Sauter]
        /// </history>
        public static bool isConnectedToWifi()
        {
            ConnectionProfile cP = NetworkInformation.GetInternetConnectionProfile();
            return cP != null && cP.IsWlanConnectionProfile && cP.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

        /// <summary>
        /// Checks if the device is connected to a celular network and has access to the internet
        /// </summary>
        /// <history>
        /// 30/12/2016  Created [Fabian Sauter]
        /// </history>
        public static bool isConnectedToCelular()
        {
            ConnectionProfile cP = NetworkInformation.GetInternetConnectionProfile();
            return cP != null && cP.IsWwanConnectionProfile && cP.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

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


        #endregion
    }
}
