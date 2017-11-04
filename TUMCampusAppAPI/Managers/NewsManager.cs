using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Syncs;
using TUMCampusAppAPI.UserDatas;
using Windows.Data.Json;

namespace TUMCampusAppAPI.Managers
{
    public class NewsManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static NewsManager INSTANCE;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
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
        /// Returns the most current news id from the db.
        /// </summary>
        private string getLastNewsId()
        {
            string lastId = "";
            List<News.News> list = dB.Query<News.News>("SELECT id FROM News ORDER BY id DESC LIMIT 1");
            if (list != null && list.Count > 0)
            {
                lastId += list[0].id;
            }
            return lastId;
        }

        /// <summary>
        /// Returns all news sources from the db.
        /// </summary>
        /// <returns>A list of NewsSource elements.</returns>
        public List<News.NewsSource> getAllNewsSourcesFormDb()
        {
            return dB.Query<News.NewsSource>("SELECT * FROM NewsSource");
        }

        public List<News.News> getNewsWithImage()
        {
            return dB.Query<News.News>("SELECT * FROM News n WHERE n.imageUrl IS NOT NULL AND n.imageUrl != ''");
        }

        /// <summary>
        /// Returns all news from the db in descending order by date.
        /// Also only returns these, where the NewsSource is not disabled.
        /// </summary>
        /// <returns>A list of News elements.</returns>
        public List<News.News> getAllNewsFormDb()
        {
            return dB.Query<News.News>("SELECT n.* FROM News n JOIN NewsSource s ON n.src = s.src WHERE s.enabled = 1 ORDER BY date DESC");
        }

        /// <summary>
        /// Returns a list of news, their date matches todays date.
        /// Also the first tumMovie news, that is equal or bigger than todays date gets added to the list.
        /// </summary>
        /// <returns>Returns a list of News elements, max 10 entries.</returns>
        public List<News.News> getNewsForHomePage()
        {
            List<News.News> news = getAllNewsFormDb();
            List<News.News> result = new List<News.News>();
            DateTime tumMovieDate = DateTime.MaxValue;
            int tumMovieIndex = -1;
            DateTime yesterday = DateTime.Now.AddDays(-1);
            for (int i = 0; i < news.Count; i++)
            {
                if (result.Count >= 10 || news[i].date.CompareTo(yesterday) < 0)
                {
                    break;
                }

                if (news[i].src.Equals("2"))
                {
                    if (news[i].date.Date.CompareTo(DateTime.Now.Date) > 0 && news[i].date.CompareTo(tumMovieDate) < 0)
                    {
                        tumMovieIndex = i;
                        tumMovieDate = news[i].date;
                    }
                }
                else if (news[i].date.Date.CompareTo(DateTime.Now.Date) == 0)
                {
                    result.Add(news[i]);
                }
                else if (i < 3)
                {
                    result.Add(news[i]);
                }
            }
            if (tumMovieIndex >= 0)
            {
                result.Insert(0, news[tumMovieIndex]);
            }
            return result;
        }

        /// <summary>
        /// Returns the NewsSource for the given id.
        /// </summary>
        /// <param name="src">The src of the NewsSource.</param>
        /// <returns>Returns the NewsSource for the given id.</returns>
        public News.NewsSource getNewsSource(string src)
        {
            List<News.NewsSource> sources = dB.Query<News.NewsSource>("SELECT * FROM NewsSource WHERE src LIKE ?", src);
            return sources.Count > 0 ? sources[0] : null;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<News.News>();
            dB.CreateTable<News.NewsSource>();
        }

        /// <summary>
        /// Tries to download news if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all news.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadNewsAsync(bool force)
        {
            // Storage for news changed between version 1.0.3 and 1.0.4:
            if (!Util.getSettingBoolean(Const.V_1_0_4_CLEANED_NEWS))
            {
                force = true;
                Util.setSetting(Const.V_1_0_4_CLEANED_NEWS, true);
            }

            if (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            if ((force || SyncManager.INSTANCE.needSync("News", CacheManager.VALIDITY_THREE_HOURS).NEEDS_SYNC) && DeviceInfo.isConnectedToInternet())
            {
                if (force)
                {
                    clearCachedNewsImages();
                }
                try
                {
                    Uri url;
                    if (force)
                    {
                        url = new Uri(Const.NEWS_URL);
                    }
                    else
                    {
                        url = new Uri(Const.NEWS_URL + getLastNewsId());
                    }
                    JsonArray jsonArray = await NetUtils.downloadJsonArrayAsync(url);
                    if (jsonArray == null)
                    {
                        return;
                    }

                    cleanupDb();
                    List<News.News> news = new List<News.News>();
                    foreach (JsonValue val in jsonArray)
                    {
                        try
                        {

                            News.News n = new News.News(val.GetObject());
                            news.Add(n);
                            if (n.imageUrl != null)
                            {
                                Task t = CacheManager.INSTANCE.cacheImageAsync(new Uri(n.imageUrl));
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Caught an exception during parsing news!", e);
                        }
                    }
                    foreach (News.News n in news)
                    {
                        dB.InsertOrReplace(n);
                    }
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("News"));
                }
                catch (Exception e)
                {
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("News", SyncResult.STATUS_ERROR_UNKNOWN, e.ToString()));
                }
            }
        }

        /// <summary>
        /// Tries to download news sources if it is necessary and caches them into the local db.
        /// </summary>
        /// <param name="force">Forces to redownload all news sources.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadNewsSourcesAsync(bool force)
        {
            if (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            if ((force || SyncManager.INSTANCE.needSync("NewsSource", CacheManager.VALIDITY_ONE_MONTH).NEEDS_SYNC) && DeviceInfo.isConnectedToInternet())
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
                    replaceNewsSources(list);
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("NewsSource"));
                }
                catch (Exception e)
                {
                    SyncManager.INSTANCE.replaceIntoDb(new Sync("NewsSource", SyncResult.STATUS_ERROR_UNKNOWN, e.ToString()));
                }
            }
        }

        /// <summary>
        /// Updates the disabled status for a specific news source.
        /// </summary>
        /// <param name="id">The id of that specific news source.</param>
        /// <param name="enabled">Whether to enable it.</param>
        public void updateNewsSourceStatus(int id, bool enabled)
        {
            dB.Execute("UPDATE NewsSource SET enabled = ? WHERE id = ?", new object[] { enabled, id });
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Removes all old items (older than 3 months) from the db.
        /// </summary>
        private void cleanupDb()
        {
            List<News.News> all = getAllNewsFormDb();
            DateTime date = DateTime.Now.AddMonths(-3);
            foreach (var item in all)
            {
                if (item.date.CompareTo(date) < 0)
                {
                    dB.Execute("DELETE FROM News WHERE id = " + item.id);
                }
            }
        }

        /// <summary>
        /// Deletes all NewsSource entries from the NewsSource table and replaces them with the new ones.
        /// It keeps whether the NewsSource was enabled.
        /// </summary>
        /// <param name="list">A list of NewsSource objects.</param>
        private void replaceNewsSources(List<News.NewsSource> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                News.NewsSource old = getNewsSource(list[i].src);
                if (old != null)
                {
                    list[i].enabled = old.enabled;
                }
            }
            dB.DeleteAll<News.NewsSource>();
            dB.InsertAll(list);
        }

        /// <summary>
        /// Clears all cached news images.
        /// </summary>
        private void clearCachedNewsImages()
        {
            Task.Factory.StartNew(async () =>
            {
                List<Uri> uris = new List<Uri>();
                foreach (News.News n in getNewsWithImage())
                {
                    uris.Add(new Uri(n.imageUrl));
                }
                await ImageCache.Instance.RemoveAsync(uris);
            });
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
