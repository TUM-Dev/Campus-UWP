
namespace TUMCampusAppAPI.Syncs
{
    public class SyncResult
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly int STATUS_NOT_FOUND = 1;
        public static readonly int STATUS_OK = 0;
        public static readonly int STATUS_ERROR_UNKNOWN = -1;
        public static readonly int STATUS_ERROR_TUM_ONLINE = -2;
        public static readonly int STATUS_ERROR_SQL = -3;

        public readonly long LAST_SYNC;
        public readonly int STATUS;
        public readonly bool NEEDS_SYNC;
        public readonly string ERROR_MESSAGE;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/05/2017 Created [Fabian Sauter]
        /// </history>
        public SyncResult()
        {
            this.STATUS = STATUS_ERROR_UNKNOWN;
            this.NEEDS_SYNC = true;
        }

        public SyncResult(long lastSync, int status, bool needsSync, string errorMessage)
        {
            this.LAST_SYNC = lastSync;
            this.STATUS = status;
            this.NEEDS_SYNC = needsSync;
            this.ERROR_MESSAGE = errorMessage;
        }

        public SyncResult(Sync sync, bool needsSync)
        {
            this.LAST_SYNC = sync.lastSync;
            this.STATUS = sync.status;
            this.ERROR_MESSAGE = sync.errorMessage;
            this.NEEDS_SYNC = needsSync;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


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
