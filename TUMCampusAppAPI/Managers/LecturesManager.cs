using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.UserDatas;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.Managers
{
    public class LecturesManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static LecturesManager INSTANCE;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
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
        public List<TUMOnlineLecture> getLectures()
        {
            return dB.Query<TUMOnlineLecture>("SELECT * FROM TUMOnlineLecture");
        }        
        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<TUMOnlineLecture>();
        }

        /// <summary>
        /// Trys to download the information for the given lecture if it is not cached.
        /// </summary>
        /// <param name="stp_sp_nr">The lectures stp_sp nr.</param>
        /// <returns>Returns the found lecture information or null if none found.</returns>
        public async Task<List<TUMOnlineLectureInformation>> searchForLectureInformationAsync(string stp_sp_nr)
        {
            List<TUMOnlineLectureInformation> list = null;
            XmlDocument doc = await getLectureInformationDocumentAsync(stp_sp_nr);
            if (doc == null || doc.SelectSingleNode("/error") != null)
            {
                return list;
            }
            list = new List<TUMOnlineLectureInformation>();
            foreach (var element in doc.SelectNodes("/rowset/row"))
            {
                list.Add(new TUMOnlineLectureInformation(element));
            }
            return list;
        }

        /// <summary>
        /// Trys to download your personal lectures if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all lectures.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadLecturesAsync(bool force)
        {
            if (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            if ((force || SyncManager.INSTANCE.needSync(this, CacheManager.VALIDITY_FIFE_DAYS)) && DeviceInfo.isConnectedToInternet())
            {
                XmlDocument doc = await getPersonalLecturesDocumentAsync();
                if (doc == null || doc.SelectSingleNode("/error") != null)
                {
                    return;
                }
                dB.DropTable<TUMOnlineLecture>();
                dB.CreateTable<TUMOnlineLecture>();
                foreach (var element in doc.SelectNodes("/rowset/row"))
                {
                    dB.Insert(new TUMOnlineLecture(element));
                }
                SyncManager.INSTANCE.replaceIntoDb(new Syncs.Sync(this));
            }
        }

        /// <summary>
        /// Downloads lectures for the given query if it is necessary and caches them.
        /// </summary>
        /// <param name="query">The search query. Min 4 chars!</param>
        /// <returns>Returns the downloaded lectures.</returns>
        public async Task<List<TUMOnlineLecture>> searchForLecturesAsync(string query)
        {
            List<TUMOnlineLecture> list = null;
            XmlDocument doc = await getQueryedLecturesDocumentAsync(query);
            if (doc == null || doc.SelectSingleNode("/error") != null)
            {
                return list;
            }
            list = new List<TUMOnlineLecture>();
            foreach (var element in doc.SelectNodes("/rowset/row"))
            {
                list.Add(new TUMOnlineLecture(element));
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
