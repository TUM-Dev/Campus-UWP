using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using TUMCampusAppAPI.Canteens;
using TUMCampusAppAPI.Syncs;
using TUMCampusAppAPI.UserDatas;

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
        private Canteen getFromJson(JsonObject json)
        {
            Canteen c = new Canteen()
            {
                canteen_id = json.GetNamedString(Const.JSON_CANTEEN_ID),
                name = json.GetNamedString(Const.JSON_NAME)
            };

            JsonObject location = json.GetNamedObject(Const.JSON_LOCATION);
            if(location != null)
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
        /// <returns>Returns all Canteens from the db as a list</returns>
        public List<Canteen> getCanteens()
        {
            return dB.Query<Canteen>("SELECT * FROM Canteen;");
        }

        /// <summary>
        /// Tries to download and return the canteen associated by the given id
        /// </summary>
        /// <param name="canteen_id">Canteen id</param>
        /// <returns>Returns the canteen associated by the given id</returns>
        public async Task<Canteen> getCanteenByIdAsync(string canteen_id)
        {
            await downloadCanteensAsync(false);
            List<Canteen> list = dB.Query<Canteen>("SELECT * FROM Canteen WHERE canteen_id = ?;", canteen_id);
            if(list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
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

                List<Canteen> list = new List<Canteen>();
                foreach (JsonValue val in jsonArr)
                {
                    list.Add(getFromJson(val.GetObject()));
                }
                dB.DeleteAll<Canteen>();
                dB.InsertAll(list);
                SyncManager.INSTANCE.replaceIntoDb(new Sync(this));
            }
            catch (Exception e)
            {
                Logger.Error("Unable to download canteens.", e);
            }
        }

        public async override Task InitManagerAsync()
        {
            dB.CreateTable<Canteen>();
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
