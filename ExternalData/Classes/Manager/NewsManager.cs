using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes.Contexts;
using Storage.Classes.Models.News;
using Windows.Data.Json;

namespace ExternalData.Classes.Manager
{
    public class NewsManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly Uri NEWS_SOURCES_URI = new Uri("https://api.tum.app/v1/news/sources");
        private static readonly TimeSpan MAX_TIME_IN_CACHE = TimeSpan.FromDays(1);

        private const string JSON_NEWS_SOURCE_ID = "source";
        private const string JSON_NEWS_SOURCE_TITLE = "title";
        private const string JSON_NEWS_SOURCE_ICON = "icon";

        private Task<List<NewsSource>> updateTask;

        public static readonly NewsManager INSTANCE = new NewsManager();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<List<NewsSource>> UpdateNewsSourcesAsync(bool force)
        {
            // Wait for the old update to finish first:
            if (updateTask is null || updateTask.IsCompleted)
            {
                updateTask = Task.Run(async () =>
                {
                    List<NewsSource> newsSources;
                    if (!force && CacheDbContext.IsCacheEntryValid(NEWS_SOURCES_URI.ToString()))
                    {
                        Logger.Info("No need to fetch news sources. Cache is still valid.");
                        using (NewsDbContext ctx = new NewsDbContext())
                        {
                            newsSources = ctx.NewsSources.ToList();
                        }
                    }
                    else
                    {
                        newsSources = await DownloadNewsSourcesAsync();
                        if (!(newsSources is null))
                        {
                            using (NewsDbContext ctx = new NewsDbContext())
                            {
                                ctx.RemoveRange(ctx.NewsSources);
                                ctx.AddRange(newsSources);
                            }
                            CacheDbContext.UpdateCacheEntry(NEWS_SOURCES_URI.ToString(), DateTime.Now.Add(MAX_TIME_IN_CACHE));
                            return newsSources;
                        }
                    }
                    return new List<NewsSource>();
                });
            }
            return await updateTask.ConfAwaitFalse();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<List<NewsSource>> DownloadNewsSourcesAsync()
        {
            Logger.Info("Downloading news sources...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(NEWS_SOURCES_URI);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to download news sources string.", e);
                    return null;
                }
            }

            JsonArray json;
            try
            {
                List<NewsSource> newsSources = new List<NewsSource>();
                json = JsonArray.Parse(jsonString);
                foreach (IJsonValue newsSource in json)
                {
                    newsSources.Add(LoadNewsSourceFromJson(newsSource.GetObject()));
                }
                Logger.Info("Successfully downloaded " + newsSources.Count() + " news sources.");
                return newsSources;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error("Failed to parse downloaded news sources.", e);
                return null;
            }
        }

        private NewsSource LoadNewsSourceFromJson(JsonObject json)
        {
            return new NewsSource
            {
                Id = json.GetNamedString(JSON_NEWS_SOURCE_ID),
                Title = json.GetNamedString(JSON_NEWS_SOURCE_TITLE),
                Icon = json.GetNamedString(JSON_NEWS_SOURCE_ICON)
            };
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
