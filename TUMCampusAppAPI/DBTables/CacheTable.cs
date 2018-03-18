using SQLite;
using System;

namespace TUMCampusAppAPI
{
    public class CacheTable
    {//--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [Unique, PrimaryKey]
        public string url { get; set; }
        public byte[] data { get; set; }
        public int validity { get; set; }
        public string max_age { get; set; }
        public int type { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public CacheTable()
        {

        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/01/2017  Created [Fabian Sauter]
        /// </history>
        public CacheTable(string url, byte[] data, int validity, int max_age, int type)
        {
            this.url = url;
            this.data = data;
            this.validity = validity;
            DateTime date = DateTime.Now.AddSeconds(max_age);
            this.max_age = date.Year.ToString() + '-' + date.Month.ToString() + '-' + date.Day.ToString() + ' ' + date.Hour.ToString() + ':' + date.Minute.ToString() + ':' + date.Second.ToString();
            this.type = type;
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
