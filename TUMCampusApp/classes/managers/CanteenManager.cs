using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using TUMCampusApp.classes;
using System.Diagnostics;
using TUMCampusApp.classes.canteen;
using TUMCampusApp.classes.sync;
using TUMCampusApp.classes.userData;

namespace TUMCampusApp.classes.managers
{
    class CanteenManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CanteenManager INSTANCE;
        private static readonly int TIME_TO_SYNC = 604800; // 1 week

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteenManager()
        {
            dB.CreateTable<Canteen>();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private Canteen getFromJson(JsonObject json)
        {
            return new Canteen(int.Parse(json.GetNamedString(Const.JSON_ID)),
                json.GetNamedString(Const.JSON_NAME).Replace("\"", "\'"),
                json.GetNamedString(Const.JSON_ADDRESS).Replace("\"", "\'"),
                double.Parse(json.GetNamedString(Const.JSON_LATITUDE)),
                double.Parse(json.GetNamedString(Const.JSON_LONGITUDE)));
        }

        public List<Canteen> getCanteens()
        {
            return dB.Query<Canteen>("SELECT * FROM Canteen");
        }

        public async Task<Canteen> getCanteenByIdAsync(int id)
        {
            await downloadCanteensAsync(false);
            List<Canteen> list = dB.Query<Canteen>("SELECT * FROM Canteen WHERE id = ?", id);
            if(list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task downloadCanteensAsync(bool force)
        {
            if (!force && Utillities.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            try
            {
                if (!force && !SyncManager.INSTANCE.needSync(this, TIME_TO_SYNC))
                {
                    Logger.Info("Sync not required! - CanteenManager");
                    return;
                }
                Uri url = new Uri("https://tumcabe.in.tum.de/Api/mensen");
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
                Logger.Error("Unable to download Canteens", e);
            }
        }

        public async override Task InitManagerAsync()
        {
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
