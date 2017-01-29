using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Caches;

namespace TUMCampusApp.Classes.Managers
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
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<Cache>();
            // Delete all entries that are too old and delete corresponding image files
            foreach (Cache c in dB.Query<Cache>("SELECT data FROM Cache WHERE datetime() > max_age AND type = ?", CACHE_TYP_IMAGE))
            {
                File.Delete(c.url);
            }
            dB.Execute("DELETE FROM Cache WHERE datetime() > max_age");
        }

        public string isCached(string url)
        {
            List<Cache> list = dB.Query<Cache>("SELECT * FROM Cache WHERE datetime() < max_age AND url LIKE ?", url);
            if(list == null || list.Count <= 0)
            {
                return null;
            }
            return decodeString(list[0].data);
        }

        public void cache(Cache c)
        {
            dB.InsertOrReplace(c);
        }

        public static string decodeString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] encodeString(string s)
        {
            return Encoding.UTF8.GetBytes(s);
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
