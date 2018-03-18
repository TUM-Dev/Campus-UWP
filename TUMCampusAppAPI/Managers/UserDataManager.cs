using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
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
            dB.CreateTable<UserDataTable>();
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
            List<UserDataTable> list = dB.Query<UserDataTable>(true, "SELECT * FROM UserDataTable WHERE id = ?", DeviceInfo.INSTANCE.Id);
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
            List<UserDataTable> list = dB.Query<UserDataTable>(true, "SELECT * FROM UserDataTable WHERE id = ?", DeviceInfo.INSTANCE.Id);
            if (list == null || list.Count <= 0)
            {
                update(new UserDataTable(pos));
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
        /// <returns>Returns the last selected canteen id or 'mensa-garching' if none exists.</returns>
        public string getLastSelectedCanteenId()
        {
            List<UserDataTable> list = dB.Query<UserDataTable>(true, "SELECT * FROM UserDataTable WHERE id = ?", DeviceInfo.INSTANCE.Id);
            if (list == null || list.Count <= 0)
            {
                return "mensa-garching";
            }
            string canteen_id = list[0].lastSelectedCanteenId;
            if(canteen_id == null)
            {
                return "mensa-garching";
            }
            return canteen_id;
        }

        /// <summary>
        /// Saves the given id as the last selected canteen id.
        /// </summary>
        /// <param name="canteen_id">Saves the given canteen_id as the last selected canteen id.</param>
        public void setLastSelectedCanteenId(string canteen_id)
        {
            if(dB.Execute("UPDATE UserDataTable SET lastSelectedCanteenId = ? WHERE id = ?", canteen_id, DeviceInfo.INSTANCE.Id) <= 0)
            {
                update(new UserDataTable() { lastSelectedCanteenId = canteen_id });
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    await LocationManager.INSTANCE.getCurrentLocationAsync();
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
