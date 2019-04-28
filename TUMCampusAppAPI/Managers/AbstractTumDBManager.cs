using Shared.Classes.SQLite;
using System.Threading;
using System.Threading.Tasks;

namespace TUMCampusAppAPI.Managers
{
    public abstract class AbstractTumDBManager : AbstractDBManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        protected Task refreshingTask;
        protected readonly SemaphoreSlim REFRESHING_TASK_SEMA = new SemaphoreSlim(1);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 25/08/2018 Created [Fabian Sauter]
        /// </history>
        public AbstractTumDBManager()
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
            return SyncDBManager.INSTANCE.getSyncStatus(this);
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Initializes the manager for a background task.
        /// </summary>
        /// <returns></returns>
        public virtual void initManagerForBackgroundTask()
        {

        }

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
