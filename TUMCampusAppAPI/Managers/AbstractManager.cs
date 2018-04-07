using Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Thread_Save_Components.Classes.SQLite;
using TUMCampusAppAPI.DBTables;
using Windows.Storage;

namespace TUMCampusAppAPI.Managers
{
    public abstract class AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "data.db");
        protected static TSSQLiteConnection dB = new TSSQLiteConnection(DB_PATH);
        protected readonly Object thisLock = new Object();
        protected Task refreshingTask;
        protected readonly SemaphoreSlim REFRESHING_TASK_SEMA = new SemaphoreSlim(1);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public AbstractManager()
        {
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the status for the last sync of this object.
        /// </summary>
        /// <returns></returns>
        public SyncResult getSyncStatus()
        {
            return SyncManager.INSTANCE.getSyncStatus(this);
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Drops every table in the db
        /// </summary>
        public static void resetDB()
        {
            dB.DropTable<CacheTable>();
            dB.DropTable<CanteenTable>();
            dB.DropTable<CanteenDishTable>();
            dB.DropTable<FavoriteCanteenDishTypeTable>();
            dB.DropTable<SyncTable>();
            dB.DropTable<UserDataTable>();
            dB.DropTable<TUMOnlineLectureTable>();
            dB.DropTable<TUMTuitionFeeTable>();
            dB.DropTable<TUMOnlineLectureTable>();
            dB.DropTable<TUMOnlineCalendarTable>();
        }

        /// <summary>
        /// Deletes the whole db and recreates an empty one.
        /// Only for testing use resetDB() instead!
        /// </summary>
        public static void deleteDB()
        {
            try
            {
                dB.Close();
                File.Delete(DB_PATH);
            }
            catch (Exception e)
            {
                Logger.Error("Unable to close or delete the DB", e);
            }
            dB = new TSSQLiteConnection(DB_PATH);
        }

        /// <summary>
        /// Initializes the manager asynchronous
        /// </summary>
        /// <returns></returns>
        public abstract Task InitManagerAsync();

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--
        /// <summary>
        /// Waits for the sync task to finish.
        /// </summary>
        protected void waitForSyncToFinish()
        {
            REFRESHING_TASK_SEMA.Wait();

            if (refreshingTask != null)
            {
                Task.WaitAny(refreshingTask);
            }

            REFRESHING_TASK_SEMA.Release();
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
