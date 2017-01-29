using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Tum;
using TUMCampusApp.Classes.UserDatas;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.Classes.Managers
{
    class TuitionFeeManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static TuitionFeeManager INSTANCE;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/01/2017 Created [Fabian Sauter]
        /// </history>
        public TuitionFeeManager()
        {
            dB.CreateTable<TUMTuitionFee>();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private async Task<XmlDocument> getFeeStatusAsync()
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.TUITION_FEE_STATUS);
            req.addToken();
            return await req.doRequestDocumentAsync();
        }

        public List<TUMTuitionFee> getFees()
        {
            return dB.Query<TUMTuitionFee>("SELECT * FROM TUMTuitionFee");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            //await downloadFeesAsync(false);
        }

        public async Task downloadFeesAsync(bool force)
        {
            if(!force && Utillities.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            if ((force || SyncManager.INSTANCE.needSync(this, CacheManager.VALIDITY_ONE_DAY)) && DeviceInfo.isConnectedToInternet())
            {
                XmlDocument doc = await getFeeStatusAsync();
                if (doc == null || doc.SelectSingleNode("/error") != null)
                {
                    return;
                }
                dB.DropTable<TUMTuitionFee>();
                dB.CreateTable<TUMTuitionFee>();
                foreach (var element in doc.SelectNodes("/rowset/row"))
                {
                    dB.Insert(new TUMTuitionFee(element));
                }
                SyncManager.INSTANCE.replaceIntoDb(new Syncs.Sync(this));
            }
            return;
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
