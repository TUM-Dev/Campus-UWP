﻿using SQLite;
using System;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline.Exceptions;

namespace TUMCampusAppAPI
{
    [Table(DBTableConsts.SYNC_TABLE)]
    public class SyncTable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public String id { get; set; }
        public long lastSync { get; set; }
        public int status { get; set; }
        public string errorMessage { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic empty Constructor
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public SyncTable()
        {

        }

        public SyncTable(Object obj, Exception e) : this(obj.GetType().Name)
        {
            if(e is InvalidTokenTUMOnlineException)
            {
                this.errorMessage = "Your token is either unknown or not activated yet.";
                this.status = SyncResult.STATUS_ERROR_TUM_ONLINE;
            }
            else if(e is NoAccessTUMOnlineException)
            {
                this.errorMessage = "You didn't give the token the required rights for accessing your TUM calendar.";
                this.status = SyncResult.STATUS_ERROR_TUM_ONLINE;
            }
            else
            {
                this.status = SyncResult.STATUS_ERROR_UNKNOWN;
                this.errorMessage = "An unknown error occurred. Please try again.\n\n" + e.ToString();
            }
        }

        public SyncTable(Object obj) : this(obj.GetType().Name)
        {
        }

        public SyncTable(string id) : this(id, SyncResult.STATUS_OK, null)
        {
        }

        public SyncTable(Object obj, int status) : this(obj.GetType().Name, status)
        {
        }

        public SyncTable(string id, int status) : this(id, status, null)
        {
        }

        public SyncTable(Object obj, int status, string errorMessage) : this(obj.GetType().Name, status, errorMessage)
        {
        }

        public SyncTable(string id, int status, string errorMessage)
        {
            this.id = id;
            this.lastSync = SyncManager.GetCurrentUnixTimestampSeconds();
            this.status = status;
            this.errorMessage = errorMessage;
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
