using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;

namespace TUMCampusAppAPI.Managers
{
    public class CacheManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CacheManager INSTANCE;
        public static readonly int CACHE_TYP_DATA = 0;

        /**
         * Validity's for entries in seconds
         */
        public static readonly int VALIDITY_DO_NOT_CACHE = 0;
        public static readonly int VALIDITY_THREE_HOURS = 10800;
        public static readonly int VALIDITY_ONE_DAY = 86400;
        public static readonly int VALIDITY_TWO_DAYS = 2 * 86400;
        public static readonly int VALIDITY_FIFE_DAYS = 5 * 86400;
        public static readonly int VALIDITY_TEN_DAYS = 10 * 86400;
        public static readonly int VALIDITY_ONE_MONTH = 30 * 86400;


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
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
            dB.CreateTable<CacheTable>();
            dB.Execute("DELETE FROM " + DBTableConsts.CACHE_TABLE + " WHERE datetime() > max_age;");

            // CacheTable images 30 days:
            ImageCache.Instance.CacheDuration = new TimeSpan(30, 0, 0, 0, 0);
        }

        public async Task clearCacheAsync()
        {
            try
            {
                await ImageCache.Instance.ClearAsync();
                Logger.Info("Cleared the image cache!");
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred during deletion of a cached file.", e);
            }
            dB.DeleteAll<CacheTable>();
        }

        /// <summary>
        /// Checks if the given url is cached
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Returns null if it is not cached or the cached string</returns>
        public string isCached(string url)
        {
            List<CacheTable> list = dB.Query<CacheTable>(true, "SELECT * FROM " + DBTableConsts.CACHE_TABLE + " WHERE datetime() < max_age AND url LIKE ?;", url);
            if(list == null || list.Count <= 0)
            {
                return null;
            }
            return decodeString(list[0].data);
        }

        /// <summary>
        /// Caches the given CacheTable object
        /// </summary>
        /// <param name="c"></param>
        public void cache(CacheTable c)
        {
            dB.InsertOrReplace(c);
        }

        /// <summary>
        /// Downloads and caches the image from the given url.
        /// </summary>
        /// <param name="url">The image url.</param>
        public async Task cacheImageAsync(Uri url)
        {
            await ImageCache.Instance.PreCacheAsync(url, false, false);
        }

        /// <summary>
        /// Decodes a given bytes[] to a string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>Returns the decoded string</returns>
        public static string decodeString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }


        /// <summary>
        /// Encodes the given string into a byte[]
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns the encode byte[]</returns>
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
