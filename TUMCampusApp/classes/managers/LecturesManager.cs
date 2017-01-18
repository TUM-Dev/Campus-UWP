using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes.tum;
using TUMCampusApp.classes.userData;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.classes.managers
{
    class LecturesManager : AbstractManager
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
        public async Task<XmlDocument> getPersonalLecturesDocumentAsync()
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.LECTURES_PERSONAL);
            req.addToken();
            return await req.doRequestDocumentAsync();
        }

        public async Task<XmlDocument> getQueryedLecturesDocumentAsync(string query)
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.LECTURES_SEARCH);
            req.addToken();
            req.addParameter(Const.P_SEARCH, query);
            req.setValidity(CacheManager.VALIDITY_TEN_DAYS);
            return await req.doRequestDocumentAsync();
        }

        public async Task<XmlDocument> getLectureInformationDocumentAsync(string stp_sp_nr)
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.LECTURES_DETAILS);
            req.addToken();
            req.addParameter(Const.P_LV_NR, stp_sp_nr);
            req.setValidity(CacheManager.VALIDITY_TEN_DAYS);
            return await req.doRequestDocumentAsync();
        }

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

        public async Task downloadLecturesAsync(bool force)
        {
            if (!force && Utillities.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
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
                SyncManager.INSTANCE.replaceIntoDb(new sync.Sync(this));
            }
        }

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
