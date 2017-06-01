using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Syncs;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.UserDatas;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.Managers
{
    public class NewsManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static NewsManager INSTANCE;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/01/2017 Created [Fabian Sauter]
        /// </history>
        public NewsManager()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Searches in the local db for all lectures and returns them.
        /// </summary>
        /// <returns>Returns all found lectures.</returns>
        public List<TUMOnlineLecture> getLectures()
        {
            return dB.Query<TUMOnlineLecture>("SELECT * FROM TUMOnlineLecture");
        }
        
        /// <summary>
        /// Returns the most current news id from the db.
        /// </summary>
        private string getLastNewsId()
        {
            string lastId = "";
            List<News.News> list = dB.Query<News.News>("SELECT id FROM News ORDER BY id DESC LIMIT 1");
            if(list != null && list.Count > 0)
            {
                lastId += list[0].id;
            }
            return lastId;
        }

        public List<News.News> getAllNewsFormDb()
        {
            return dB.Query<News.News>("SELECT * FROM News ORDER BY date DESC");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<News.News>();
        }

        /// <summary>
        /// Trys to download news if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all news.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadNewsAsync(bool force)
        {
            if (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            if ((force || SyncManager.INSTANCE.needSync(typeof(News.News), CacheManager.VALIDITY_ONE_DAY).NEEDS_SYNC) && DeviceInfo.isConnectedToInternet())
            {
                try
                {
                    JsonArray jsonArray = await NetUtils.downloadJsonArrayAsync(new Uri(Const.NEWS_URL + getLastNewsId()));
                    if (jsonArray == null)
                    {
                        return;
                    }

                    List<News.News> list = new List<News.News>();
                    foreach (JsonValue val in jsonArray)
                    {
                        try
                        {
                            list.Add(new News.News(val.GetObject()));
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Caught an exception during parsing news!", e);
                        }
                    }
                    cleanupDb();
                    dB.InsertOrReplaceAll(list);
                    SyncManager.INSTANCE.replaceIntoDb(new Sync(typeof(News.News)));
                }
                catch (Exception e)
                {
                    SyncManager.INSTANCE.replaceIntoDb(new Sync(typeof(News.News), SyncResult.STATUS_ERROR_UNKNOWN, e.ToString()));
                }
            }
        }

        /// <summary>
        /// Trys to download news sources if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all news sources.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadNewsSourcesAsync(bool force)
        {
            if (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            if ((force || SyncManager.INSTANCE.needSync(typeof(News.NewsSource), CacheManager.VALIDITY_ONE_MONTH).NEEDS_SYNC) && DeviceInfo.isConnectedToInternet())
            {
                try
                {
                    JsonArray jsonArray = await NetUtils.downloadJsonArrayAsync(new Uri(Const.NEWS_SOURCES_URL));
                    if (jsonArray == null)
                    {
                        return;
                    }

                    List<News.NewsSource> list = new List<News.NewsSource>();
                    foreach (JsonValue val in jsonArray)
                    {
                        try
                        {
                            list.Add(new News.NewsSource(val.GetObject()));
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Caught an exception during parsing news sources!", e);
                        }
                    }
                    dB.DeleteAll<News.NewsSource>();
                    dB.InsertAll(list);
                    SyncManager.INSTANCE.replaceIntoDb(new Sync(typeof(News.NewsSource)));
                }
                catch (Exception e)
                {
                    SyncManager.INSTANCE.replaceIntoDb(new Sync(typeof(News.NewsSource), SyncResult.STATUS_ERROR_UNKNOWN, e.ToString()));
                }
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Removes all old items (older than 3 months) from the db.
        /// </summary>
        private void cleanupDb()
        {
            dB.Execute("DELETE FROM News WHERE date < date('now','-3 month')");
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
