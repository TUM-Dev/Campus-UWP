using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Managers;

namespace TUMCampusApp.Classes.Syncs
{
    class Sync
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public String id { get; set; }
        public long lastSync { get; set; }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic empty Constructor
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public Sync()
        {

        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public Sync(Object obj)
        {
            this.id = obj.GetType().Name;
            this.lastSync = SyncManager.GetCurrentUnixTimestampSeconds();
        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/01/2017  Created [Fabian Sauter]
        /// </history>
        public Sync(string id)
        {
            this.id = id;
            this.lastSync = SyncManager.GetCurrentUnixTimestampSeconds();
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
