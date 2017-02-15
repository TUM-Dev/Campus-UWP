using SQLite.Net.Attributes;
using TUMCampusAppAPI.Managers;
using Windows.Devices.Geolocation;

namespace TUMCampusAppAPI.UserDatas
{
    public class UserData
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public string id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public long unixTimeSeconds { get; set; }
        public int lastSelectedCanteenId { get; set; }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/12/2016  Created [Fabian Sauter]
        /// </history>
        public UserData(Geopoint lastKnownDevicePosition)
        {
            this.lat = lastKnownDevicePosition.Position.Latitude;
            this.lng = lastKnownDevicePosition.Position.Longitude;
            this.id = DeviceInfo.INSTANCE.Id;
            this.unixTimeSeconds = SyncManager.GetCurrentUnixTimestampSeconds();
        }

        /// <summary>
        /// Basic empty Constructor
        /// </summary>
        /// <history>
        /// 15/12/2016  Created [Fabian Sauter]
        /// </history>
        public UserData()
        {
            this.id = DeviceInfo.INSTANCE.Id;
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


        #endregion
    }
}
