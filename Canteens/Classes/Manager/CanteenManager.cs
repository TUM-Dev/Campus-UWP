using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes.Contexts.Canteens;
using Storage.Classes.Models.Canteens;
using Windows.Data.Json;

namespace Canteens.Classes.Manager
{
    public class CanteenManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly Uri CANTEENS_URI = new Uri("https://tum-dev.github.io/eat-api/canteens.json");

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
        public async Task<IEnumerable<Canteen>> UpdateAsync()
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                return await updateTask.ConfAwaitFalse();
            }

            updateTask = Task.Run(async () =>
            {
                IEnumerable<Canteen> canteens = await DownloadCanteensAsync();
                if (!(canteens is null))
                {
                    using (CanteensDbContext ctx = new CanteensDbContext())
                    {
                        ctx.RemoveRange(ctx.Canteens);
                        ctx.AddRange(canteens);
                        ctx.SaveChanges();
                    }
                }
                return canteens;
            });
            return await updateTask.ConfAwaitFalse();
        }

        public async Task<IEnumerable<Canteen>> LoadCanteensAsync()
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                return await updateTask.ConfAwaitFalse();
            }

            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                return ctx.Canteens;
            }
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
                    Logger.Error("Failed to download canteens string.", e);
                    return null;
                }
            }

            Logger.Debug("Downloaded canteens JSON string: " + jsonString);
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
                Logger.Error("Failed to parse downloaded canteens.", e);
                return null;
            }
        }

        private Canteen LoadCanteenFromJson(JsonObject json)
        {
            return new Canteen()
            {
                CanteenId = json.GetNamedString(JSON_CANTEEN_ID),
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
