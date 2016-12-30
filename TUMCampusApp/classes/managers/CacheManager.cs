using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMCampusApp.classes.managers
{
    class CacheManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CacheManager INSTANCE;
        public static readonly int CACHE_TYP_DATA = 0;
        public static readonly int CACHE_TYP_IMAGE = 1;

        /**
         * Validity's for entries in seconds
         */
        public static readonly int VALIDITY_DO_NOT_CACHE = 0;
        public static readonly int VALIDITY_ONE_DAY = 86400;
        public static readonly int VALIDITY_TWO_DAYS = 2 * 86400;
        public static readonly int VALIDITY_FIFE_DAYS = 5 * 86400;
        public static readonly int VALIDITY_TEN_DAYS = 10 * 86400;
        public static readonly int VALIDITY_ONE_MONTH = 30 * 86400;


        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public CacheManager()
        {
            dB.CreateTable<cache.Cache>();

            // Delete all entries that are too old and delete corresponding image files
            foreach (cache.Cache c in dB.Query<cache.Cache>("SELECT data FROM cache WHERE datetime() > max_age AND type = ?", 1))
            {
                File.Delete(c.url);
            }
            dB.Execute("DELETE FROM Cache WHERE datetime() > max_age");
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
