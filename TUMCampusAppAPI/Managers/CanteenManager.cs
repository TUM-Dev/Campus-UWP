using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using TUMCampusAppAPI.DBTables;
using Windows.Devices.Geolocation;

namespace TUMCampusAppAPI.Managers
{
    public class CanteenManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CanteenManager INSTANCE;
        private static readonly int TIME_TO_SYNC = 604800; // 1 week

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteenManager()
        {
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Parses the given JsonObject into a Canteen object
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Returns the parsed Canteen</returns>
        private CanteenTable getFromJson(JsonObject json)
        {
            CanteenTable c = new CanteenTable()
            {
                canteen_id = json.GetNamedString(Const.JSON_CANTEEN_ID),
                name = json.GetNamedString(Const.JSON_NAME)
            };

            JsonObject location = json.GetNamedObject(Const.JSON_LOCATION);
            if (location != null)
            {
                c.address = location.GetNamedString(Const.JSON_ADDRESS);
                c.latitude = location.GetNamedNumber(Const.JSON_LATITUDE);
                c.longitude = location.GetNamedNumber(Const.JSON_LONGITUDE);
            }
            return c;
        }

        /// <summary>
        /// Returns all Canteens from the db
        /// </summary>
        /// <returns>Returns all Canteens from the db.</returns>
        public List<CanteenTable> getCanteens()
        {
            return dB.Query<CanteenTable>("SELECT * FROM CanteenTable;");
        }

        /// <summary>
        /// Returns all canteens from the db with the distance attribute set to the current distance.
        /// </summary>
        /// <returns>Returns all Canteens from the db.</returns>
        public async Task<List<CanteenTable>> getCanteensWithDistanceAsync()
        {
            List<CanteenTable> list = getCanteens();
            Geopoint pos = UserDataManager.INSTANCE.getLastKnownDevicePosition();
            if (pos == null)
            {
                pos = await LocationManager.INSTANCE.getCurrentLocationAsync();
            }
            if (pos == null)
            {
                foreach (CanteenTable c in list)
                {
                    c.distance = -1F;
                }
                return list;
            }
            else
            {
                foreach (CanteenTable c in list)
                {
                    c.distance = (float)LocationManager.INSTANCE.calcDistance(c.latitude, c.longitude, pos.Position.Latitude, pos.Position.Longitude);
                }
            }
            return list;
        }

        /// <summary>
        /// Tries to download and return the canteen associated by the given id
        /// </summary>
        /// <param name="canteen_id">Canteen id</param>
        /// <returns>Returns the canteen associated by the given id</returns>
        public async Task<CanteenTable> getCanteenByIdAsync(string canteen_id)
        {
            await downloadCanteensAsync(false);
            List<CanteenTable> list = dB.Query<CanteenTable>("SELECT * FROM CanteenTable WHERE canteen_id = ?;", canteen_id);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// Returns the CanteenTable that matches the given canteen_id.
        /// </summary>
        public CanteenTable getCanteen(string canteen_id)
        {
            List<CanteenTable> list = dB.Query<CanteenTable>("SELECT * FROM CanteenTable WHERE canteen_id = ?;", canteen_id);
            if(list.Count >= 1)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// Adds the given dish types to the favorite canteens dish types.
        /// </summary>
        /// <param name="canteen_id">The canteen id.</param>
        /// <param name="dish_types">All dish types.</param>
        public void setFavoriteCanteenDishTypes(string canteen_id, List<string> dish_types)
        {
            dB.Execute("UPDATE CanteenTable SET favorite = ? WHERE canteen_id = ?;", 1, canteen_id);
            List<FavoriteCanteenDishTypeTable> fav = new List<FavoriteCanteenDishTypeTable>();
            foreach (string dish_type in dish_types)
            {
                dB.InsertOrReplace(new FavoriteCanteenDishTypeTable()
                {
                    id = FavoriteCanteenDishTypeTable.generateId(canteen_id, dish_type),
                    canteen_id = canteen_id,
                    dish_type = dish_type
                });
            }
        }

        /// <summary>
        /// Returns all CanteenTables, that are tagged as favorite.
        /// </summary>
        public List<CanteenTable> getFavoriteCanteens()
        {
            return dB.Query<CanteenTable>("SELECT canteen_id FROM CanteenTable WHERE favorite = 1;");
        }

        /// <summary>
        /// Returns all dish types for the given canteen_id.
        /// </summary>
        /// <param name="canteen_id">The canteen id you want all dish types for.</param>
        public List<FavoriteCanteenDishTypeTable> getDishTypesForFavoriteCanteen(string canteen_id)
        {
            return dB.Query<FavoriteCanteenDishTypeTable>("SELECT * FROM FavoriteCanteenDishTypeTable WHERE canteen_id = ?;", canteen_id);
        }

        /// <summary>
        /// Removes all entries from the FavoriteCanteenDishTypeTable where the canteen_id matches.
        /// Also updates CanteenTable and sets favorite to 0 where the canteen_id matches.
        /// </summary>
        /// <param name="canteen_id">The canteen id you want to unfavorite.</param>
        public void unfavoriteCanteen(string canteen_id)
        {
            dB.Execute("UPDATE CanteenTable SET favorite = ? WHERE canteen_id = ?;", 0, canteen_id);
            dB.Execute("DELETE FROM FavoriteCanteenDishTypeTable WHERE canteen_id = ?;", canteen_id);
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Downloads the canteens if necessary or if force = true.
        /// </summary>
        /// <param name="force">Forces to download all canteens.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadCanteensAsync(bool force)
        {
            if (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            try
            {
                if (!force && !SyncManager.INSTANCE.needSync(this, TIME_TO_SYNC).NEEDS_SYNC)
                {
                    return;
                }
                Uri url = new Uri(Const.CANTEENS_URL);
                JsonArray jsonArr = await NetUtils.downloadJsonArrayAsync(url);
                if (jsonArr == null)
                {
                    return;
                }

                List<CanteenTable> list = new List<CanteenTable>();
                foreach (JsonValue val in jsonArr)
                {
                    list.Add(getFromJson(val.GetObject()));
                }
                dB.DeleteAll<CanteenTable>();
                dB.InsertAll(list);
                SyncManager.INSTANCE.replaceIntoDb(new SyncTable(this));
            }
            catch (Exception e)
            {
                Logger.Error("Unable to download canteens.", e);
            }
        }

        public async override Task InitManagerAsync()
        {
            dB.CreateTable<CanteenTable>();
            dB.CreateTable<FavoriteCanteenDishTypeTable>();
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
