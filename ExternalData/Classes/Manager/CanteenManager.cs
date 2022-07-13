using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes.Contexts;
using Storage.Classes.Models.Canteens;
using Windows.Data.Json;

namespace ExternalData.Classes.Manager
{
    public class CanteenManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly Uri LANGUAGES_URI = new Uri("https://tum-dev.github.io/eat-api/enums/languages.json");
        private static readonly string BASE_URI_PREFIX = "https://tum-dev.github.io/eat-api/";
        private static readonly string CANTEENS_URI_POSTDIX = "/enums/canteens.json";
        private static readonly TimeSpan MAX_TIME_IN_CACHE = TimeSpan.FromDays(7);

        private const string JSON_LOCATION = "location";
        private const string JSON_LOCATION_ADDRESS = "address";
        private const string JSON_LOCATION_LATITUDE = "latitude";
        private const string JSON_LOCATION_LONGITUDE = "longitude";
        private const string JSON_CANTEEN_ID = "canteen_id";
        private const string JSON_CANTEEN_NAME = "name";

        private const string JSON_CANTEEN_LANGUAGE_NAME = "name";
        private const string JSON_CANTEEN_LANGUAGE_BASE_URL = "base_url";
        private const string JSON_CANTEEN_LANGUAGE_FLAG = "flag";
        private const string JSON_CANTEEN_LANGUAGE_LABEL = "label";


        private Task<IEnumerable<Canteen>> updateCanteensTask;
        private Task<IEnumerable<Language>> updateLanguagesTask;

        public static readonly CanteenManager INSTANCE = new CanteenManager();
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void MigrateActiveLanguage(IEnumerable<Language> languages)
        {
            Language activeLang = CanteensDbContext.GetActiveLanguage();
            if (activeLang is null)
            {
                bool foundEnglish = false;
                foreach (Language lang in languages)
                {
                    if (string.Equals(lang.Name, "EN"))
                    {
                        lang.Active = true;
                        foundEnglish = true;
                        break;
                    }
                }

                if (!foundEnglish)
                {
                    Logger.Warn("English not found as language for setting it as default language for canteens in result.");
                    languages.First().Active = true;
                }
            }
            else
            {
                bool foundActiveLanguage = false;
                foreach (Language lang in languages)
                {
                    if (string.Equals(lang.Name, activeLang.Name))
                    {
                        lang.Active = true;
                        foundActiveLanguage = true;
                        break;
                    }
                }

                if (!foundActiveLanguage)
                {
                    Logger.Warn($"Active language {activeLang.Name} not found as language for setting it as default language for canteens in result.");
                    languages.First().Active = true;
                }
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<IEnumerable<Canteen>> UpdateCanteensAsync(bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateCanteensTask is null) && !updateCanteensTask.IsCompleted)
            {
                return await updateCanteensTask.ConfAwaitFalse();
            }

            updateCanteensTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(CANTEENS_URI_POSTDIX))
                {
                    Logger.Info("No need to fetch canteens. Cache is still valid.");
                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        return ctx.Canteens.Include(ctx.GetIncludePaths(typeof(Canteen))).ToList();
                    }
                }
                IEnumerable<Canteen> canteens = await DownloadCanteensAsync();
                if (!(canteens is null))
                {
                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        ctx.RemoveRange(ctx.Canteens);
                        ctx.AddRange(canteens);
                    }
                    CacheDbContext.UpdateCacheEntry(CANTEENS_URI_POSTDIX, DateTime.Now.Add(MAX_TIME_IN_CACHE));
                    return canteens;
                }
                Logger.Info("Failed to retrieve canteens. Returning from DB.");
                using (CanteensDbContext ctx = new CanteensDbContext())
                {
                    return ctx.Canteens.Include(ctx.GetIncludePaths(typeof(Canteen))).ToList();
                }
            });
            return await updateCanteensTask.ConfAwaitFalse();
        }

        public async Task<IEnumerable<Language>> UpdateLanguagesAsync(bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateLanguagesTask is null) && !updateLanguagesTask.IsCompleted)
            {
                return await updateLanguagesTask.ConfAwaitFalse();
            }

            updateLanguagesTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(LANGUAGES_URI.ToString()))
                {
                    Logger.Info("No need to fetch canteen languages. Cache is still valid.");
                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        return ctx.Languages.Include(ctx.GetIncludePaths(typeof(Language))).ToList();
                    }
                }
                IEnumerable<Language> languages = await DownloadLanguagesAsync();
                if (!(languages is null))
                {
                    // Keep currently selected language or select english per default:
                    MigrateActiveLanguage(languages);

                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        ctx.RemoveRange(ctx.Languages);
                        ctx.AddRange(languages);
                    }
                    CacheDbContext.UpdateCacheEntry(LANGUAGES_URI.ToString(), DateTime.Now.Add(MAX_TIME_IN_CACHE));
                    return languages;
                }
                Logger.Info("Failed to retrieve canteen languages. Returning from DB.");
                using (CanteensDbContext ctx = new CanteensDbContext())
                {
                    return ctx.Languages.Include(ctx.GetIncludePaths(typeof(Language))).ToList();
                }
            });
            return await updateLanguagesTask.ConfAwaitFalse();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<IEnumerable<Canteen>> DownloadCanteensAsync()
        {
            Logger.Info("Downloading canteen...");

            // Get current language:
            Language lang = CanteensDbContext.GetActiveLanguage();
            if (lang is null)
            {
                Logger.Error("Failed to download canteens. No language available.");
                return null;
            }

            Logger.Debug($"Canteen language: {lang.Name}");

            string uri = BASE_URI_PREFIX + lang.BaseUrl + CANTEENS_URI_POSTDIX;

            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(uri);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to download canteens string.", e);
                    return null;
                }
            }

            JsonArray json;
            try
            {
                json = JsonArray.Parse(jsonString);
                IEnumerable<Canteen> canteens = json.Select(x => LoadCanteenFromJson(x.GetObject()));
                Logger.Info("Successfully downloaded " + canteens.Count() + " canteens.");
                return canteens;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error("Failed to parse downloaded canteens.", e);
                return null;
            }
        }

        private async Task<IEnumerable<Language>> DownloadLanguagesAsync()
        {
            Logger.Info("Downloading canteen languages...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(LANGUAGES_URI);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to download canteen languages string.", e);
                    return null;
                }
            }

            JsonArray json;
            try
            {
                json = JsonArray.Parse(jsonString);
                IEnumerable<Language> languages = json.Select(x => LoadLanguageFromJson(x.GetObject()));
                Logger.Info("Successfully downloaded " + languages.Count() + " canteen languages.");
                return languages;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error("Failed to parse downloaded canteen languages.", e);
                return null;
            }
        }

        private Canteen LoadCanteenFromJson(JsonObject json)
        {
            return new Canteen()
            {
                Id = json.GetNamedString(JSON_CANTEEN_ID),
                Name = json.GetNamedString(JSON_CANTEEN_NAME),
                Location = LoadLocationFromJson(json.GetNamedObject(JSON_LOCATION))
            };
        }

        private Language LoadLanguageFromJson(JsonObject json)
        {
            return new Language()
            {
                Name = json.GetNamedString(JSON_CANTEEN_LANGUAGE_NAME),
                BaseUrl = json.GetNamedString(JSON_CANTEEN_LANGUAGE_BASE_URL),
                Flag = json.GetNamedString(JSON_CANTEEN_LANGUAGE_FLAG),
                Label = json.GetNamedString(JSON_CANTEEN_LANGUAGE_LABEL),
                Active = false
            };
        }

        private Location LoadLocationFromJson(JsonObject json)
        {
            return new Location()
            {
                Address = json.GetNamedString(JSON_LOCATION_ADDRESS),
                Latitude = json.GetNamedNumber(JSON_LOCATION_LATITUDE),
                Longitude = json.GetNamedNumber(JSON_LOCATION_LONGITUDE)
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