using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes.userData;
using Windows.Devices.Geolocation;

namespace TUMCampusApp.classes.managers
{
    class UserDataManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static UserDataManager INSTANCE;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/12/2016  Created [Fabian Sauter]
        /// </history>
        public UserDataManager()
        {
            dB.CreateTable<UserData>();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public Geopoint getLastKnownDevicePosition()
        {
            List<UserData> list = dB.Query<UserData>("SELECT * FROM UserData WHERE id = ?", DeviceInfo.INSTANCE.Id);
            if(list == null || list.Count <= 0)
            {
                return null;
            }
            BasicGeoposition pos = new BasicGeoposition();
            pos.Latitude = list[0].lat;
            pos.Longitude = list[0].lng;
            return new Geopoint(pos);
        }

        public void setLastKnownDevicePosition(Geopoint pos)
        {
            dB.InsertOrReplace(new UserData(pos));
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task initManagerAsync()
        {
            await initLoaction();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task initLoaction()
        {
            await LocationManager.INSTANCE.getCurrentLocationAsync();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
