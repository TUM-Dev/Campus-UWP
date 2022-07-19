using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class DishManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly Uri DISHES_URI = new Uri("https://tum-dev.github.io/eat-api/all_ref.json");
        private static readonly TimeSpan MAX_TIME_IN_CACHE = TimeSpan.FromDays(1);

        private const string JSON_CANTEEN_ID = "canteen_id";
        private const string JSON_DISHES = "dishes";
        private const string JSON_DISH_NAME = "name";
        private const string JSON_DISH_PRICES = "prices";
        private const string JSON_DISH_PRICE_BASE = "base_price";
        private const string JSON_DISH_PRICE_PER_UNIT = "price_per_unit";
        private const string JSON_DISH_PRICE_UNIT = "unit";
        private const string JSON_DISH_PRICE_STUDENTS = "students";
        private const string JSON_DISH_PRICE_STAFF = "staff";
        private const string JSON_DISH_PRICE_GUESTS = "guests";
        private const string JSON_DISH_LABELS = "labels";
        private const string JSON_DISH_DATE = "date";
        private const string JSON_DISH_TYPE = "dish_type";

        private Task updateTask;

        public static readonly DishManager INSTANCE = new DishManager();

        public static readonly Dictionary<string, string> LABELS_EMOJI_ADDITIONALS_LOOKUP = new Dictionary<string, string>()
        {
            { "DYESTUFF", "🎨" },
            { "PRESERVATIVES", "🥫" },
            { "ANTIOXIDANTS", "⚗" },
            { "FLAVOR_ENHANCER", "🔬" },
            { "SULPHURS", "🔻" },
            { "SULFITES", "🔺" },
            { "WAXED", "🐝" },
            { "PHOSPATES", "🔷" },
            { "SWEETENERS", "🍬" },
            { "PHENYLALANINE", "💊" },
            { "COCOA_CONTAINING_GREASE", "🍫" },
            { "GELATIN", "🍮" },
            { "ALCOHOL", "🍷" }
        };

        public static readonly Dictionary<string, string> LABELS_EMOJI_ALLERGENS_LOOKUP = new Dictionary<string, string>()
        {
            { "VEGETARIAN", "🌽" },
            { "VEGAN", "🥕" },
            { "MEAT", "🍖" },
            { "PORK", "🐖" },
            { "BEEF", "🐄" },
            { "VEAL", "🐂" },
            { "POULTRY", "🐔" },
            { "WILD_MEAT", "🐗" },
            { "LAMB", "🐑" },
            { "GARLIC", "🧄" },
            { "CHICKEN_EGGS", "🥚" },
            { "PEANUTS", "🥜" },
            { "FISH", "🐟" },
            { "CEREAL", "🌾" },
            { "GLUTEN", "🌿" },
            { "WHEAT", "GlW" },
            { "RYE", "GlR" },
            { "BARLEY", "GlG" },
            { "OAT", "GlH" },
            { "SPELT", "GlD" },
            { "HYBRIDS", "GlHy" },
            { "SHELLFISH", "🦀" },
            { "LUPIN", "Lu" },
            { "LACTOSE", "La" },
            { "MILK", "🥛" },
            { "SHELL_FRUITS", "🥥" },
            { "ALMONDS", "ScM" },
            { "HAZELNUTS", "🌰" },
            { "MACADAMIA", "ScMa" },
            { "PECAN", "ScP" },
            { "WALNUTS", "ScW" },
            { "CASHEWS", "ScC" },
            { "PISTACHIOES", "ScP" },
            { "SESAME", "Se" },
            { "MUSTARD", "Sf" },
            { "CELERY", "Sl" },
            { "SOY", "So" },
            { "MOLLUSCS", "🐙" }
        };

        public static readonly Dictionary<string, string> LABELS_EMOJI_MISC_LOOKUP = new Dictionary<string, string>()
        {
            { "BAVARIA", "GQB" },
            { "MSC", "🎣" },
        };

        public static readonly Dictionary<string, string> LABELS_EMOJI_ALL_LOOKUP = new Dictionary<string, string>();
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        static DishManager()
        {
            foreach (KeyValuePair<string, string> pair in LABELS_EMOJI_ADDITIONALS_LOOKUP)
            {
                LABELS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, string> pair in LABELS_EMOJI_ALLERGENS_LOOKUP)
            {
                LABELS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, string> pair in LABELS_EMOJI_MISC_LOOKUP)
            {
                LABELS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
            }
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task UpdateAsync(bool force)
        {
            // Wait for the old update to finish first:
            if (updateTask is null || updateTask.IsCompleted)
            {
                updateTask = Task.Run(async () =>
                {
                    if (!force && CacheDbContext.IsCacheEntryValid(DISHES_URI.ToString()))
                    {
                        Logger.Info("No need to fetch dishes. Cache is still valid.");
                        return;
                    }
                    IEnumerable<Dish> dishes = await DownloadDishesAsync();
                    if (!(dishes is null))
                    {
                        using (CanteensDbContext ctx = new CanteensDbContext())
                        {
                            ctx.RemoveRange(ctx.Dishes);
                            ctx.AddRange(dishes);
                        }
                        CacheDbContext.UpdateCacheEntry(DISHES_URI.ToString(), DateTime.Now.Add(MAX_TIME_IN_CACHE));
                    }
                });
            }
            await updateTask.ConfAwaitFalse();
        }

        public async Task<IEnumerable<Dish>> LoadDishesAsync(string canteenId, DateTime date)
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                await updateTask.ConfAwaitFalse();
            }

            List<Dish> dishes;
            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                dishes = ctx.Dishes.Where(d => string.Equals(d.CanteenId, canteenId) && d.Date.Date.CompareTo(date.Date) == 0).Include(ctx.GetIncludePaths(typeof(Dish))).ToList();
            }

            // Sort dishes, so side dishes are at the end:
            dishes.Sort((a, b) =>
            {
                if (a.IsSideDish && b.IsSideDish)
                {
                    return 0;
                }
                if (a.IsSideDish)
                {
                    return 1;
                }

                if (b.IsSideDish)
                {
                    return -1;
                }
                return 0;
            });
            return dishes;
        }

        /// <summary>
        /// Returns the next date a dish for the given <paramref name="canteenId"/> was found after the given <paramref name="date"/>.
        /// In case no date was found. <see cref="DateTime.MaxValue"/> will be returned.
        /// </summary>
        public DateTime GetNextDate(string canteenId, DateTime date)
        {
            DateTime dish = DateTime.MaxValue;
            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                dish = ctx.Dishes.Where(d => d.Date.Date > date.Date && string.Equals(d.CanteenId, canteenId)).Select(d => d.Date).OrderBy(d => d).FirstOrDefault();
            }
            return dish == DateTime.MinValue ? DateTime.MaxValue : dish.Date;
        }

        /// <summary>
        /// Returns the previous date a dish for the given <paramref name="canteenId"/> was found after the given <paramref name="date"/>.
        /// In case no date was found. <see cref="DateTime.MinValue"/> will be returned.
        /// </summary>
        public DateTime GetPrevDate(string canteenId, DateTime date)
        {
            Dish dish = null;
            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                dish = ctx.Dishes.Where(d => d.Date.Date < date.Date && string.Equals(d.CanteenId, canteenId)).OrderByDescending(d => d.Date).FirstOrDefault();
            }
            return dish is null ? DateTime.MinValue : dish.Date;
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<IEnumerable<Dish>> DownloadDishesAsync()
        {
            Logger.Info("Downloading dishes...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(DISHES_URI);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to download dishes string.", e);
                    return null;
                }
            }

            JsonArray json;
            try
            {
                List<Dish> dishes = new List<Dish>();
                json = JsonArray.Parse(jsonString);
                foreach (IJsonValue canteen in json)
                {
                    JsonObject obj = canteen.GetObject();
                    string canteenId = obj.GetNamedString(JSON_CANTEEN_ID);
                    JsonArray dishesJson = obj.GetNamedArray(JSON_DISHES);
                    foreach (IJsonValue dish in dishesJson)
                    {
                        dishes.Add(LoadDishFromJson(dish.GetObject(), canteenId));
                    }
                }
                Logger.Info("Successfully downloaded " + dishes.Count() + " dishes.");
                return dishes;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error("Failed to parse downloaded dishes.", e);
                return null;
            }
        }

        private Dish LoadDishFromJson(JsonObject json, string canteenId)
        {
            JsonObject prices = json.GetNamedObject(JSON_DISH_PRICES);
            string type = json.GetNamedString(JSON_DISH_TYPE);
            return new Dish()
            {
                CanteenId = canteenId,
                Date = DateTime.ParseExact(json.GetNamedString(JSON_DISH_DATE), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Labels = LoadLabelsFromJson(json),
                Name = json.GetNamedString(JSON_DISH_NAME),
                PriceGuests = LoadPriceFromJson(prices.GetNamedValue(JSON_DISH_PRICE_GUESTS)),
                PriceStaff = LoadPriceFromJson(prices.GetNamedValue(JSON_DISH_PRICE_STAFF)),
                PriceStudents = LoadPriceFromJson(prices.GetNamedValue(JSON_DISH_PRICE_STUDENTS)),
                Type = type,
                IsSideDish = string.Equals(type, "Beilagen")
            };
        }

        private Price LoadPriceFromJson(JsonValue value)
        {
            if (value.ValueType != JsonValueType.Object)
            {
                return null;
            }
            JsonObject obj = value.GetObject();
            string basePrice = LoadJsonStringSave(obj.GetNamedValue(JSON_DISH_PRICE_BASE));
            return new Price()
            {
                BasePrice = basePrice,
                PerUnit = LoadJsonStringSave(obj.GetNamedValue(JSON_DISH_PRICE_PER_UNIT)),
                Unit = LoadJsonStringSave(obj.GetNamedValue(JSON_DISH_PRICE_UNIT))
            };
        }

        private List<string> LoadLabelsFromJson(JsonObject json)
        {
            if (json.ContainsKey(JSON_DISH_LABELS))
            {
                JsonArray labels = json.GetNamedArray(JSON_DISH_LABELS);
                return labels.Select(x => x.GetString()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            }
            return new List<string>();
        }

        private string LoadJsonStringSave(JsonValue val)
        {
            switch (val.ValueType)
            {
                case JsonValueType.String:
                    return val.GetString();

                case JsonValueType.Null:
                    return null;

                case JsonValueType.Number:
                    return val.GetNumber().ToString("n2");

                default:
                    return val.Stringify();
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
