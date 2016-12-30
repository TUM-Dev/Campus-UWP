using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMCampusApp.classes.cache
{
    class Cache
    {//--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [Unique]
        public string url { get; set; }
        public byte[] data { get; set; }
        public string validity { get; set; }
        public string max_age { get; set; }
        public int type { get; set; }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public Cache()
        {

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
