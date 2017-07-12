using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusAppAPI.TUMOnline;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.Managers
{
    public class GradesManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static GradesManager INSTANCE;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 04/03/2017 Created [Fabian Sauter]
        /// </history>


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Downloads all grades.
        /// </summary>
        /// <returns>Returns the downloaded grades in form of a XmlDocument.</returns>
        private async Task<XmlDocument> getGradesDocumentAsync()
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.EXAMS);
            req.addToken();
            return await req.doRequestDocumentAsync();
        }

        /// <summary>
        /// Searches in the local db for all grades and returns them.
        /// </summary>
        /// <returns>Returns all found grads as a list of TUMOnlineGradeSemester with their grades.</returns>
        public List<TUMOnlineGradeSemester> getGradesSemester()
        {
            List<TUMOnlineGrade> list = dB.Query<TUMOnlineGrade>("SELECT * FROM TUMOnlineGrade");
            List<TUMOnlineGradeSemester> semester = new List<TUMOnlineGradeSemester>();
            bool match = false;
            foreach (TUMOnlineGrade g in list)
            {
                match = false;
                foreach (TUMOnlineGradeSemester s in semester)
                {
                    if (s.tryAddGrade(g))
                    {
                        match = true;
                    }
                }
                if (!match)
                {
                    semester.Add(new TUMOnlineGradeSemester(g));
                }
            }
            return semester;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<TUMOnlineGrade>();
        }

        /// <summary>
        /// Trys to download all grades and caches them into the local db.
        /// </summary>
        /// <param name="force">Force download and ignore cache.</param>
        /// <returns></returns>
        public async Task downloadGradesAsync(bool force)
        {
            if (force || SyncManager.INSTANCE.needSync(this, CacheManager.VALIDITY_ONE_DAY).NEEDS_SYNC)
            {
                XmlDocument doc = await getGradesDocumentAsync();
                if (doc == null || doc.SelectSingleNode("/error") != null)
                {
                    return;
                }
                dB.DropTable<TUMOnlineGrade>();
                dB.CreateTable<TUMOnlineGrade>();
                foreach (var element in doc.SelectNodes("/rowset/row"))
                {
                    dB.Insert(new TUMOnlineGrade(element));
                }
                SyncManager.INSTANCE.replaceIntoDb(new Syncs.Sync(this));
            }
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
