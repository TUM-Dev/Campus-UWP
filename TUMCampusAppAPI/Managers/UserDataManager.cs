using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.UserDatas;
using Windows.Devices.Geolocation;

namespace TUMCampusAppAPI.Managers
{
    public class UserDataManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static UserDataManager INSTANCE;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
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
        /// <summary>
        /// Returns the last known device position.
        /// </summary>
        /// <returns>Returns the last known device position.</returns>
        public Geopoint getLastKnownDevicePosition()
        {
            waitWhileLocked();
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

        /// <summary>
        /// Saves the given Geopoint as the last known device position.
        /// </summary>
        /// <param name="pos">The Geopoint that should get saved.</param>
        public void setLastKnownDevicePosition(Geopoint pos)
        {
            List<UserData> list = dB.Query<UserData>("SELECT * FROM UserData WHERE id = ?", DeviceInfo.INSTANCE.Id);
            if (list == null || list.Count <= 0)
            {
                dB.Insert(new UserData(pos));
            }
            else
            {
                list[0].lat = pos.Position.Latitude;
                list[0].lng = pos.Position.Longitude;
                list[0].unixTimeSeconds = SyncManager.GetCurrentUnixTimestampSeconds();
                dB.InsertOrReplace(list[0]);
            }
        }

        /// <summary>
        /// Returns the last selected canteen id.
        /// </summary>
        /// <returns>Returns the last selected canteen id or -1 if none exists.</returns>
        public int getLastSelectedCanteenId()
        {
            List<UserData> list = dB.Query<UserData>("SELECT * FROM UserData WHERE id = ?", DeviceInfo.INSTANCE.Id);
            if (list == null || list.Count <= 0)
            {
                return 422;
            }
            int id = list[0].lastSelectedCanteenId;
            if(id == 0)
            {
                return 422;
            }
            return id;
        }

        /// <summary>
        /// Saves the given id as the last selected canteen id.
        /// </summary>
        /// <param name="id">Saves the given id as the last selected canteen id.</param>
        public void setLastSelectedCanteenId(int id)
        {
            if(dB.Execute("UPDATE UserData SET lastSelectedCanteenId = " + id + " WHERE id = ?", DeviceInfo.INSTANCE.Id) <= 0)
            {
                dB.Insert(new UserData() { lastSelectedCanteenId = id });
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            Task t = null;
            t = Task.Factory.StartNew(() =>
            {
                try
                {
                    LocationManager.INSTANCE.getCurrentLocationAsync().Wait();
                }
                catch
                {

                }

            });
        }
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
