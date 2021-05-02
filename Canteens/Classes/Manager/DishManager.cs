using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes.Contexts;
using Storage.Classes.Models.Canteens;
using Windows.Data.Json;

namespace Canteens.Classes.Manager
{
    public class DishManager
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
        private const string JSON_DISH_INGREDIENTS = "ingredients";
        private const string JSON_DISH_DATE = "date";
        private const string JSON_DISH_TYPE = "dish_type";

        private Task updateTask;

        public static readonly DishManager INSTANCE = new DishManager();

        public static readonly Dictionary<string, string> INGREDIENTS_EMOJI_ADDITIONALS_LOOKUP = new Dictionary<string, string>()
        {
            { "1", "🎨" },
            { "2", "🥫" },
            { "3", "⚗" },
            { "4", "🔬" },
            { "5", "🔶" },
            { "6", "⬛" },
            { "7", "🐝" },
            { "8", "🔷" },
            { "9", "🍬" },
            { "10", "💊" },
            { "11", "🍡" },
            { "13", "🍫" },
            { "14", "🍮" },
            { "99", "🍷" }
        };

        public static readonly Dictionary<string, string> INGREDIENTS_EMOJI_ALLERGENS_LOOKUP = new Dictionary<string, string>()
        {
            { "F", "🌽" },
            { "V", "🥕" },
            { "S", "🐖" },
            { "R", "🐄" },
            { "K", "🐂" },
            { "G", "🐔" },
            { "W", "🐗" },
            { "L", "🐑" },
            { "Kn", "Kn" },
            { "Ei", "🥚" },
            { "En", "🥜" },
            { "Fi", "🐟" },
            { "Gl", "🌾" },
            { "GlW", "GlW" },
            { "GlR", "GlR" },
            { "GlG", "GlG" },
            { "GlH", "GlH" },
            { "GlD", "GlD" },
            { "Kr", "🦀" },
            { "Lu", "Lu" },
            { "Mi", "🥛" },
            { "Sc", "🥥" },
            { "ScM", "ScM" },
            { "ScH", "🌰" },
            { "ScW", "ScW" },
            { "ScC", "ScC" },
            { "ScP", "ScP" },
            { "Se", "Se" },
            { "Sf", "Sf" },
            { "Sl", "Sl" },
            { "So", "So" },
            { "Sw", "🔻" },
            { "Wt", "🐙" }
        };

        public static readonly Dictionary<string, string> INGREDIENTS_EMOJI_MISC_LOOKUP = new Dictionary<string, string>()
        {
            { "GQB", "GQB" },
            { "MSC", "🎣" },
        };

        public static readonly Dictionary<string, string> INGREDIENTS_EMOJI_ALL_LOOKUP = new Dictionary<string, string>();
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        static DishManager()
        {
            foreach (KeyValuePair<string, string> pair in INGREDIENTS_EMOJI_ADDITIONALS_LOOKUP)
            {
                INGREDIENTS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, string> pair in INGREDIENTS_EMOJI_ALLERGENS_LOOKUP)
            {
                INGREDIENTS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, string> pair in INGREDIENTS_EMOJI_MISC_LOOKUP)
            {
                INGREDIENTS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
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
                    if (!force && CacheDbContext.IsCacheEntryUpToDate(DISHES_URI.ToString(), MAX_TIME_IN_CACHE))
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
                        CacheDbContext.UpdateCacheEntry(DISHES_URI.ToString(), DateTime.Now);
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

            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                return ctx.Dishes.Where(d => string.Equals(d.CanteenId, canteenId) && d.Date.Date.CompareTo(date.Date) == 0).Include(ctx.GetIncludePaths(typeof(Dish))).ToList();
            }
        }

        /// <summary>
        /// Returns the next date a dish for the given <paramref name="canteenId"/> was found after the given <paramref name="date"/>.
        /// In case no date was found. <see cref="DateTime.MaxValue"/> will be returned.
        /// </summary>
        public DateTime GetNextDate(string canteenId, DateTime date)
        {
            Dish dish = null;
            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                dish = ctx.Dishes.Where(d => d.Date > date && string.Equals(d.CanteenId, canteenId)).OrderBy(d => d.Date).FirstOrDefault();
            }
            return dish is null ? DateTime.MaxValue : dish.Date;
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
                dish = ctx.Dishes.Where(d => d.Date < date && string.Equals(d.CanteenId, canteenId)).OrderByDescending(d => d.Date).FirstOrDefault();
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
                    Logger.Error("Failed to download dishes string.", e);
                    return null;
                }
            }

            Logger.Debug("Downloaded dishes JSON string: " + jsonString);
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
                Logger.Error("Failed to parse downloaded dishes.", e);
                return null;
            }
        }

        private Dish LoadDishFromJson(JsonObject json, string canteenId)
        {
            JsonObject prices = json.GetNamedObject(JSON_DISH_PRICES);
            return new Dish()
            {
                CanteenId = canteenId,
                Date = DateTime.ParseExact(json.GetNamedString(JSON_DISH_DATE), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Ingredients = LoadIngredientsFromJson(json.GetNamedArray(JSON_DISH_INGREDIENTS)),
                Name = json.GetNamedString(JSON_DISH_NAME),
                PriceGuests = LoadPriceFromJson(prices.GetNamedObject(JSON_DISH_PRICE_GUESTS)),
                PriceStaff = LoadPriceFromJson(prices.GetNamedObject(JSON_DISH_PRICE_STAFF)),
                PriceStudents = LoadPriceFromJson(prices.GetNamedObject(JSON_DISH_PRICE_STUDENTS)),
                Type = json.GetNamedString(JSON_DISH_TYPE)
            };
        }

        private Price LoadPriceFromJson(JsonObject json)
        {
            string basePrice = LoadJsonStringSave(json.GetNamedValue(JSON_DISH_PRICE_BASE));
            return basePrice is null || basePrice == "N/A"
                ? null
                : new Price()
                {
                    BasePrice = basePrice,
                    PerUnit = LoadJsonStringSave(json.GetNamedValue(JSON_DISH_PRICE_PER_UNIT)),
                    Unit = LoadJsonStringSave(json.GetNamedValue(JSON_DISH_PRICE_UNIT))
                };
        }

        private List<string> LoadIngredientsFromJson(JsonArray ingredients)
        {
            return ingredients.Select(x => x.GetString()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        }

        private string LoadJsonStringSave(JsonValue val)
        {
            switch (val.ValueType)
            {
                case JsonValueType.String:
                    return val.GetString();

                case JsonValueType.Null:
                    return null;

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
