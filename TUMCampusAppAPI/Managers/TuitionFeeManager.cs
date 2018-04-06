using Data_Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.TUMOnline;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.Managers
{
    public class TuitionFeeManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static TuitionFeeManager INSTANCE;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/01/2017 Created [Fabian Sauter]
        /// </history>
        public TuitionFeeManager()
        {
            dB.CreateTable<TUMTuitionFeeTable>();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Downloads your personal tuition fees.
        /// </summary>
        /// <returns>Returns the downloaded tuition fees in form of a XmlDocument.</returns>
        private async Task<XmlDocument> getFeeStatusAsync()
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.TUITION_FEE_STATUS);
            req.addToken();
            return await req.doRequestDocumentAsync();
        }

        /// <summary>
        /// Searches in the local db for all tuition fees and returns them.
        /// </summary>
        /// <returns>Returns all found tuition fees.</returns>
        public List<TUMTuitionFeeTable> getFees()
        {
            waitForSyncToFinish();
            return dB.Query<TUMTuitionFeeTable>(true, "SELECT * FROM " + DBTableConsts.TUM_ONLINE_TUITION_FEE_TABLE + " WHERE money != 0;");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
        }

        /// <summary>
        /// Tries to download your tuition fees if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all tuition fees.</param>
        /// <returns>Returns the syncing task or null if did not sync.</returns>
        public Task downloadFees(bool force)
        {
            if (!force && Settings.getSettingBoolean(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return null;
            }

            waitForSyncToFinish();
            REFRESHING_TASK_SEMA.Wait();
            refreshingTask = Task.Run(async () =>
            {
                if ((force || SyncManager.INSTANCE.needSync(DBTableConsts.TUM_ONLINE_TUITION_FEE_TABLE, CacheManager.VALIDITY_ONE_DAY).NEEDS_SYNC) && DeviceInfo.isConnectedToInternet())
                {
                    XmlDocument doc = await getFeeStatusAsync();
                    dB.DropTable<TUMTuitionFeeTable>();
                    dB.CreateTable<TUMTuitionFeeTable>();
                    foreach (var element in doc.SelectNodes("/rowset/row"))
                    {
                        dB.InsertOrReplace(new TUMTuitionFeeTable(element));
                    }
                    SyncManager.INSTANCE.replaceIntoDb(new SyncTable(this));
                }
                return;
            });
            REFRESHING_TASK_SEMA.Release();

            return refreshingTask;
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
