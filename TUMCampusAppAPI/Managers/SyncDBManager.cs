using Logging;
using SQLite;
using System;
using System.Collections.Generic;
using TUMCampusAppAPI.DBTables;

namespace TUMCampusAppAPI.Managers
{
    public class SyncDBManager : AbstractTumDBManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static SyncDBManager INSTANCE;
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public SyncDBManager()
        {
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the current unix time in ms.
        /// </summary>
        /// <returns>Returns the current unix time in ms.</returns>
        public static long GetCurrentUnixTimestampMillis()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
        }

        /// <summary>
        /// Converts the given ms from unix time to a DateTime obj and returns it.
        /// </summary>
        /// <param name="millis">Unix time in ms.</param>
        /// <returns>Returns the DateTime obj equal to the given ms.</returns>
        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            return UnixEpoch.AddMilliseconds(millis);
        }

        /// <summary>
        /// Returns the current unix time in s.
        /// </summary>
        /// <returns>Returns the current unix time in s.</returns>
        public static long GetCurrentUnixTimestampSeconds()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// Converts the given seconds from unix time to a DateTime obj and returns it.
        /// </summary>
        /// <param name="seconds">Unix time in seconds.</param>
        /// <returns>Returns the DateTime obj equal to the given seconds.</returns>
        public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
        {
            return UnixEpoch.AddSeconds(seconds);
        }

        /// <summary>
        /// Returns the sync status for the given object.
        /// </summary>
        public SyncResult getSyncStatus(object obj)
        {
            return getSyncStatus(obj.GetType().Name);
        }

        /// <summary>
        /// Returns the sync status for the given object name.
        /// </summary>
        public SyncResult getSyncStatus(string id)
        {
            return needSync(id, -1);
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Checks if a new sync is needed or if the data is still up to date.
        /// Also returns if the last sync was successful.
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public SyncResult needSync(object obj, int seconds)
        {
            return needSync(obj.GetType().Name, seconds);
        }

        /// <summary>
        /// Checks if a new sync is needed or if the data is still up to date.
        /// Also returns if the last sync was successful.
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public SyncResult needSync(string id, int seconds)
        {
            try
            {
                List<SyncTable> list = dB.Query<SyncTable>(true, "SELECT * FROM " + DBTableConsts.SYNC_TABLE + " WHERE id LIKE ?;", id);
                if (list == null || list.Count <= 0)
                {
                    return new SyncResult(-1, SyncResult.STATUS_NOT_FOUND, true, null);
                }

                if (seconds < 0)
                {
                    return new SyncResult(list[0], false);
                }

                if (list[0].lastSync + seconds < GetCurrentUnixTimestampSeconds() || list[0].status < 0)
                {
                    return new SyncResult(list[0], true);
                }
                Logger.Info("Sync not required! - " + id);
                return new SyncResult(list[0], false);
            }
            catch (SQLiteException e)
            {
                Logger.Error("Unable to execute a query for checking if the given object needs to sync again: ", e);
                return new SyncResult(-1, SyncResult.STATUS_ERROR_SQL, true, e.Message);
            }
        }

        public void Update(SyncTable table)
        {
            dB.InsertOrReplace(table);
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--
        protected override void DropTables()
        {
            dB.DropTable<SyncTable>();
        }

        protected override void CreateTables()
        {
            dB.CreateTable<SyncTable>();
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
