using Data_Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.TUMOnline;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.Managers
{
    public class LecturesManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static LecturesManager INSTANCE;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/01/2017 Created [Fabian Sauter]
        /// </history>
        public LecturesManager()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Downloads your personal lectures.
        /// </summary>
        /// <returns>Returns the downloaded lectures in form of a XmlDocument.</returns>
        private async Task<XmlDocument> getPersonalLecturesDocumentAsync()
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.LECTURES_PERSONAL);
            req.addToken();
            return await req.doRequestDocumentAsync();
        }

        /// <summary>
        /// Searches for lectures online and returns the result.
        /// </summary>
        /// <param name="query">The search query. Min 4 chars!</param>
        /// <returns>Returns the downloaded lectures in form of a XmlDocument.</returns>
        private async Task<XmlDocument> getQueryedLecturesDocumentAsync(string query)
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.LECTURES_SEARCH);
            req.addToken();
            req.addParameter(Const.P_SEARCH, query);
            req.setValidity(CacheManager.VALIDITY_TEN_DAYS);
            return await req.doRequestDocumentAsync();
        }

        /// <summary>
        /// Downloads information for the given lecture.
        /// </summary>
        /// <param name="stp_sp_nr">The lectures stp_sp nr.</param>
        /// <returns>Returns the downloaded lecture information in form of a XmlDocument.</returns>
        private async Task<XmlDocument> getLectureInformationDocumentAsync(string stp_sp_nr)
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.LECTURES_DETAILS);
            req.addToken();
            req.addParameter(Const.P_LV_NR, stp_sp_nr);
            req.setValidity(CacheManager.VALIDITY_TEN_DAYS);
            return await req.doRequestDocumentAsync();
        }

        /// <summary>
        /// Searches in the local db for all lectures and returns them.
        /// </summary>
        /// <returns>Returns all found lectures.</returns>
        public List<TUMOnlineLectureTable> getLectures()
        {
            waitForSyncToFinish();
            return dB.Query<TUMOnlineLectureTable>(true, "SELECT * FROM " + DBTableConsts.TUM_ONLINE_LECTURE_TABLE + ";");
        }
        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<TUMOnlineLectureTable>();
        }

        /// <summary>
        /// Tries to download the information for the given lecture if it is not cached.
        /// </summary>
        /// <param name="stp_sp_nr">The lectures stp_sp number.</param>
        /// <returns>Returns the found lecture information or null if none found.</returns>
        public async Task<List<TUMOnlineLectureInformationTable>> searchForLectureInformationAsync(string stp_sp_nr)
        {
            List<TUMOnlineLectureInformationTable> list = null;
            XmlDocument doc = await getLectureInformationDocumentAsync(stp_sp_nr);
            if (doc == null || doc.SelectSingleNode("/error") != null)
            {
                return list;
            }
            list = new List<TUMOnlineLectureInformationTable>();
            foreach (var element in doc.SelectNodes("/rowset/row"))
            {
                list.Add(new TUMOnlineLectureInformationTable(element));
            }
            return list;
        }

        /// <summary>
        /// Tries to download your personal lectures if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all lectures.</param>
        /// <returns>Returns the syncing task or null if did not sync.</returns>
        public Task downloadLectures(bool force)
        {
            if (!force && Settings.getSettingBoolean(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return null;
            }

            waitForSyncToFinish();
            REFRESHING_TASK_SEMA.Wait();
            refreshingTask = Task.Run(async () =>
            {
                if ((force || SyncManager.INSTANCE.needSync(DBTableConsts.TUM_ONLINE_LECTURE_TABLE, CacheManager.VALIDITY_FIFE_DAYS).NEEDS_SYNC) && DeviceInfo.isConnectedToInternet())
                {
                    XmlDocument doc = await getPersonalLecturesDocumentAsync();
                    if (doc == null || doc.SelectSingleNode("/error") != null)
                    {
                        return;
                    }
                    dB.DropTable<TUMOnlineLectureTable>();
                    dB.CreateTable<TUMOnlineLectureTable>();
                    foreach (var element in doc.SelectNodes("/rowset/row"))
                    {
                        dB.InsertOrReplace(new TUMOnlineLectureTable(element));
                    }
                    SyncManager.INSTANCE.replaceIntoDb(new SyncTable(DBTableConsts.TUM_ONLINE_LECTURE_TABLE));
                }
            });
            REFRESHING_TASK_SEMA.Release();

            return refreshingTask;
        }

        /// <summary>
        /// Downloads lectures for the given query if it is necessary and caches them.
        /// </summary>
        /// <param name="query">The search query. Min 4 chars!</param>
        /// <returns>Returns the downloaded lectures.</returns>
        public async Task<List<TUMOnlineLectureTable>> searchForLecturesAsync(string query)
        {
            List<TUMOnlineLectureTable> list = null;
            XmlDocument doc = await getQueryedLecturesDocumentAsync(query);
            if (doc == null || doc.SelectSingleNode("/error") != null)
            {
                return list;
            }
            list = new List<TUMOnlineLectureTable>();
            foreach (var element in doc.SelectNodes("/rowset/row"))
            {
                list.Add(new TUMOnlineLectureTable(element));
            }
            return list;
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
