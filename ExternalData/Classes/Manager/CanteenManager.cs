using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExternalData.Classes.CampusBackend;
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
        private static readonly Uri LABELS_URI = new Uri("https://tum-dev.github.io/eat-api/enums/labels.json");
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

        private const string JSON_LABEL_ENUM_NAME = "enum_name";
        private const string JSON_LABEL_TEXT = "text";
        private const string JSON_LABEL_ABBREVIATION = "abbreviation";

        private const string JSON_COUNT = "count";
        private const string JSON_MAX_COUNT = "maxCount";
        private const string JSON_PERCENT = "percent";
        private const string JSON_TIMESTAMP = "timestamp";


        private Task<IEnumerable<Canteen>> updateCanteensTask;
        private Task<IEnumerable<Language>> updateLanguagesTask;
        private Task<IEnumerable<Label>> updateLabelsTask;

        private readonly Dictionary<string, Label> LABEL_CACHE = new Dictionary<string, Label>();

        public static readonly CanteenManager INSTANCE = new CanteenManager();
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void MigrateActiveLanguage(List<Language> languages)
        {
            Language activeLang = CanteensDbContext.GetActiveLanguage();
            if (activeLang is null)
            {
                bool foundEnglish = false;
                for (int i = 0; i < languages.Count(); i++)
                {
                    if (string.Equals(languages[i].Name, "EN"))
                    {
                        languages[i].Active = true;
                        foundEnglish = true;
                        break;
                    }
                }
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
                // Get current language:
                Language lang = CanteensDbContext.GetActiveLanguage();
                if (lang is null)
                {
                    Logger.Error("Failed to download canteens. No language available.");
                    return new List<Canteen>();
                }
                Logger.Debug($"Canteen language: {lang.Name}");

                if (!force && CacheDbContext.IsCacheEntryValid(CANTEENS_URI_POSTDIX + '_' + lang.BaseUrl))
                {
                    Logger.Info("No need to fetch canteens. Cache is still valid.");
                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        return ctx.Canteens.Include(ctx.GetIncludePaths(typeof(Canteen))).ToList();
                    }
                }
                IEnumerable<Canteen> canteens = await DownloadCanteensAsync(lang);
                if (!(canteens is null))
                {
                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        ctx.RemoveRange(ctx.Canteens);
                        ctx.AddRange(canteens);
                    }
                    CacheDbContext.UpdateCacheEntry(CANTEENS_URI_POSTDIX + '_' + lang.BaseUrl, DateTime.Now.Add(MAX_TIME_IN_CACHE));
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
                    List<Language> langList = languages.ToList();
                    MigrateActiveLanguage(langList);

                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        ctx.RemoveRange(ctx.Languages);
                        ctx.AddRange(langList);
                    }
                    CacheDbContext.UpdateCacheEntry(LANGUAGES_URI.ToString(), DateTime.Now.Add(MAX_TIME_IN_CACHE));
                    return (IEnumerable<Language>)langList;
                }
                Logger.Info("Failed to retrieve canteen languages. Returning from DB.");
                using (CanteensDbContext ctx = new CanteensDbContext())
                {
                    return ctx.Languages.Include(ctx.GetIncludePaths(typeof(Language))).ToList();
                }
            });
            return await updateLanguagesTask.ConfAwaitFalse();
        }

        public async Task<IEnumerable<Label>> UpdateLabelsAsync(bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateLabelsTask is null) && !updateLabelsTask.IsCompleted)
            {
                return await updateLabelsTask.ConfAwaitFalse();
            }

            updateLabelsTask = Task.Run(async () =>
            {
                IEnumerable<Label> labels = null;
                if (!force && CacheDbContext.IsCacheEntryValid(LABELS_URI.ToString()))
                {
                    Logger.Info("No need to fetch labels. Cache is still valid.");
                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        labels = ctx.Labels.Include(ctx.GetIncludePaths(typeof(Label))).ToList();
                    }
                    if (LABEL_CACHE.Count <= 0)
                    {
                        UpdateLabelCache(labels);
                    }
                }
                labels = await DownloadLabelsAsync();
                if (!(labels is null))
                {
                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        ctx.RemoveRange(ctx.Labels);
                        ctx.RemoveRange(ctx.LabelTranslations);
                        ctx.AddRange(labels);
                    }
                    CacheDbContext.UpdateCacheEntry(LABELS_URI.ToString(), DateTime.Now.Add(MAX_TIME_IN_CACHE));
                    UpdateLabelCache(labels);
                    return labels;
                }
                Logger.Info("Failed to retrieve labels. Returning from DB.");
                using (CanteensDbContext ctx = new CanteensDbContext())
                {
                    labels = ctx.Labels.Include(ctx.GetIncludePaths(typeof(Label))).ToList();
                }
                if (LABEL_CACHE.Count <= 0)
                {
                    UpdateLabelCache(labels);
                }
                return labels;
            });
            return await updateLabelsTask.ConfAwaitFalse();
        }

        public Label LookupLabel(string labelStr)
        {
            if (LABEL_CACHE.ContainsKey(labelStr))
            {
                return LABEL_CACHE[labelStr];
            }
            return null;
        }

        public async Task<CanteenHeadCount> GetHeadCountAsync(string canteenId)
        {
            Debug.Assert(!string.IsNullOrEmpty(canteenId));

            Logger.Info($"Requesting canteen head count for '{canteenId}' ...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(new Uri($"https://api.tum.app/v1/canteen/headCount/{canteenId}"));
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error($"Requesting canteen head count failed for '{canteenId}'.", e);
                    return null;
                }
            }

            JsonObject json;
            try
            {
                json = JsonObject.Parse(jsonString);
                CanteenHeadCount headCount = ParseCanteenHeadCountResult(json);
                if (headCount is null)
                {
                    Logger.Error($"Failed to parse canteen head count result for '{canteenId}'. No valid head cound found in response JSON: {jsonString}");
                }
                else
                {
                    Logger.Info($"Found valid canteen head count for: {canteenId}");
                    Logger.Debug($"Canteen head count is {headCount.count} for: {canteenId}");
                }
                return headCount;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error($"Failed to parse canteen head count result for '{canteenId}'.", e);
                return null;
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        private static CanteenHeadCount ParseCanteenHeadCountResult(JsonObject json)
        {
            return new CanteenHeadCount
            {
                count = (int)json.GetNamedNumber(JSON_COUNT),
                maxCount = (int)json.GetNamedNumber(JSON_MAX_COUNT),
                percent = json.GetNamedNumber(JSON_PERCENT),
                timestamp = DateTime.Parse(json.GetNamedString(JSON_TIMESTAMP))
            };
        }

        private async Task<IEnumerable<Canteen>> DownloadCanteensAsync(Language lang)
        {
            Logger.Info("Downloading canteen...");
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

        private async Task<IEnumerable<Label>> DownloadLabelsAsync()
        {
            Logger.Info("Downloading labels...");

            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(LABELS_URI.ToString());
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to download labels string.", e);
                    return null;
                }
            }

            JsonArray json;
            try
            {
                json = JsonArray.Parse(jsonString);
                IEnumerable<Label> labels = json.Select(x => LoadLablesFromJson(x.GetObject()));
                Logger.Info("Successfully downloaded " + labels.Count() + " labels.");
                return labels;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error("Failed to parse downloaded labels.", e);
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

        private Label LoadLablesFromJson(JsonObject json)
        {
            JsonObject text = json.GetNamedObject(JSON_LABEL_TEXT);
            List<LabelTranslation> translations = new List<LabelTranslation>();
            foreach (string lang in text.Keys)
            {
                translations.Add(new LabelTranslation
                {
                    Text = text.GetNamedString(lang),
                    Lang = lang
                });
            }

            return new Label()
            {
                EnumName = json.GetNamedString(JSON_LABEL_ENUM_NAME),
                Abbreviation = json.GetNamedString(JSON_LABEL_ABBREVIATION),
                Translations = translations
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

        private void UpdateLabelCache(IEnumerable<Label> labels)
        {
            LABEL_CACHE.Clear();
            foreach (Label label in labels)
            {
                LABEL_CACHE.Add(label.EnumName, label);
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}