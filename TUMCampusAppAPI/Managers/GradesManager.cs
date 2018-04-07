using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
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
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 04/03/2017 Created [Fabian Sauter]
        /// </history>
        public GradesManager()
        {
        }

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
            waitForSyncToFinish();

            List<TUMOnlineGradeTable> list = dB.Query<TUMOnlineGradeTable>(true, "SELECT * FROM " + DBTableConsts.TUM_ONLINE_GRADE_TABLE + ";");
            List<TUMOnlineGradeSemester> semester = new List<TUMOnlineGradeSemester>();
            bool match = false;
            foreach (TUMOnlineGradeTable g in list)
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
            dB.CreateTable<TUMOnlineGradeTable>();
        }

        /// <summary>
        /// Tries to download all grades and caches them into the local DB.
        /// </summary>
        /// <param name="force">Force download and ignore cache.</param>
        /// <returns>Returns the syncing task or null if did not sync.</returns>
        public Task downloadGrades(bool force)
        {
            if (force || SyncManager.INSTANCE.needSync(DBTableConsts.TUM_ONLINE_GRADE_TABLE, Consts.VALIDITY_ONE_DAY).NEEDS_SYNC)
            {
                waitForSyncToFinish();
                REFRESHING_TASK_SEMA.Wait();
                refreshingTask = Task.Run(async () =>
                {
                    Logger.Info("Started downloading grades...");
                    XmlDocument doc = await getGradesDocumentAsync();
                    if (doc == null || doc.SelectSingleNode("/error") != null)
                    {
                        return;
                    }
                    dB.DropTable<TUMOnlineGradeTable>();
                    dB.CreateTable<TUMOnlineGradeTable>();
                    foreach (var element in doc.SelectNodes("/rowset/row"))
                    {
                        dB.InsertOrReplace(new TUMOnlineGradeTable(element));
                    }
                    SyncManager.INSTANCE.replaceIntoDb(new SyncTable(DBTableConsts.TUM_ONLINE_GRADE_TABLE));
                    Logger.Info("Finished downloading grades.");
                });
                REFRESHING_TASK_SEMA.Release();

                return refreshingTask;
            }
            return null;
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
