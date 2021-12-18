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
        private static readonly Uri CANTEENS_URI = new Uri("https://tum-dev.github.io/eat-api/enums/canteens.json");
        private static readonly TimeSpan MAX_TIME_IN_CACHE = TimeSpan.FromDays(7);

        private const string JSON_LOCATION = "location";
        private const string JSON_LOCATION_ADDRESS = "address";
        private const string JSON_LOCATION_LATITUDE = "latitude";
        private const string JSON_LOCATION_LONGITUDE = "longitude";
        private const string JSON_CANTEEN_ID = "canteen_id";
        private const string JSON_CANTEEN_NAME = "name";


        private Task<IEnumerable<Canteen>> updateTask;

        public static readonly CanteenManager INSTANCE = new CanteenManager();
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<IEnumerable<Canteen>> UpdateAsync(bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                return await updateTask.ConfAwaitFalse();
            }

            updateTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(CANTEENS_URI.ToString()))
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
                    CacheDbContext.UpdateCacheEntry(CANTEENS_URI.ToString(), DateTime.Now.Add(MAX_TIME_IN_CACHE));
                    return canteens;
                }
                Logger.Info("Failed to retrieve canteens. Returning from DB.");
                using (CanteensDbContext ctx = new CanteensDbContext())
                {
                    return ctx.Canteens.Include(ctx.GetIncludePaths(typeof(Canteen))).ToList();
                }
            });
            return await updateTask.ConfAwaitFalse();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<IEnumerable<Canteen>> DownloadCanteensAsync()
        {
            Logger.Info("Downloading canteen...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(CANTEENS_URI);
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

        private Canteen LoadCanteenFromJson(JsonObject json)
        {
            return new Canteen()
            {
                Id = json.GetNamedString(JSON_CANTEEN_ID),
                Name = json.GetNamedString(JSON_CANTEEN_NAME),
                Location = LoadLocationFromJson(json.GetNamedObject(JSON_LOCATION))
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
