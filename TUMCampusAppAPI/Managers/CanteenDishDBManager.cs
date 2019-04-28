using Data_Manager;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
using Windows.Data.Json;

namespace TUMCampusAppAPI.Managers
{
    public class CanteenDishDBManager : AbstractTumDBManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CanteenDishDBManager INSTANCE;

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
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteenDishDBManager()
        {
        }

        static CanteenDishDBManager()
        {
            foreach (var pair in INGREDIENTS_EMOJI_ADDITIONALS_LOOKUP)
            {
                INGREDIENTS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
            }

            foreach (var pair in INGREDIENTS_EMOJI_ALLERGENS_LOOKUP)
            {
                INGREDIENTS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
            }

            foreach (var pair in INGREDIENTS_EMOJI_MISC_LOOKUP)
            {
                INGREDIENTS_EMOJI_ALL_LOOKUP[pair.Key] = pair.Value;
            }
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the date of the first next dish for the given canteen_id.
        /// </summary>
        public DateTime getFirstNextDate(string canteen_id)
        {
            DateTime time = DateTime.MaxValue;
            DateTime dateToday = DateTime.Now;
            if (dateToday.Hour >= 16) // If it's after 16 o' clock show the menus for the next day
            {
                dateToday = dateToday.AddDays(1);
            }

            foreach (DateTime dT in getDishDates(canteen_id))
            {
                if (dT.Date.CompareTo(time.Date) < 0 && dT.Date.CompareTo(dateToday.Date) >= 0)
                {
                    time = dT;
                }
            }
            return time;
        }

        /// <summary>
        /// Returns all dates where a dish exist for the given canteen.
        /// </summary>
        /// <param name="canteen_id">The id of the canteen you want the dates for.</param>
        public List<DateTime> getDishDates(string canteen_id)
        {
            List<DateTime> dates = new List<DateTime>();

            List<CanteenDishTable> dishes = dB.Query<CanteenDishTable>(true, "SELECT DISTINCT date FROM " + DBTableConsts.CANTEEN_DISH_TABLE + " WHERE canteen_id = ? ORDER BY date;", canteen_id);
            foreach (CanteenDishTable m in dishes)
            {
                dates.Add(m.date);
            }
            return dates;
        }

        /// <summary>
        /// Returns all dishes contained in the db that match the given canteen_id.
        /// </summary>
        /// <param name="canteen_id">Canteen id</param>
        public List<CanteenDishTable> getDishes(string canteen_id)
        {
            List<CanteenDishTable> list = dB.Query<CanteenDishTable>(true, "SELECT * FROM " + DBTableConsts.CANTEEN_DISH_TABLE + " WHERE canteen_id = ?;", canteen_id);
            return list;
        }

        /// <summary>
        /// Returns all menus that match the given canteen_id, menu_type, name and date from the db.
        /// </summary>
        /// <param name="canteen_id">The canteen id e.g. 'mensa-martinsried'.</param>
        /// <param name="dish_type">The canteen dish type e.g. 'Tagesgericht'.</param>
        /// <param name="contains">Whether the given dish_type, contains or equals the given dish type.</param>
        /// <param name="date">The dish date.</param>
        /// <returns>Returns all menus that match the given canteen id, type name and date from the db.</returns>
        public List<CanteenDishTable> getDishesForType(string canteen_id, string dish_type, bool contains, DateTime date)
        {
            List<CanteenDishTable> list;
            if (contains)
            {
                list = dB.Query<CanteenDishTable>(true, "SELECT * FROM " + DBTableConsts.CANTEEN_DISH_TABLE + " WHERE canteen_id = ? AND dish_type LIKE '%" + dish_type + "%';", canteen_id);
            }
            else
            {
                list = dB.Query<CanteenDishTable>(true, "SELECT * FROM " + DBTableConsts.CANTEEN_DISH_TABLE + " WHERE canteen_id = ? AND dish_type = ?;", canteen_id, dish_type);
            }
            return list.Where(d => d.date.Date.Equals(date.Date)).ToList();
        }

        /// <summary>
        /// Returns all dishes for the given date and canteen_id.
        /// </summary>
        /// <param name="canteen_id">The canteen id.</param>
        /// <param name="date">The dish date.</param>
        /// <returns>Returns a list of all dishes, that match the given date and canteen_id.</returns>
        public List<CanteenDishTable> getDishes(string canteen_id, DateTime date)
        {
            List<CanteenDishTable> list = dB.Query<CanteenDishTable>(true, "SELECT * FROM " + DBTableConsts.CANTEEN_DISH_TABLE + " WHERE canteen_id = ? ORDER BY dish_type DESC;", canteen_id);
            return list.Where(d => d.date.Date.Equals(date.Date)).ToList();
        }

        /// <summary>
        /// Returns a list of all dish types.
        /// </summary>
        /// <returns>A list of CanteenDishTable objects, only populated with the dish_type attribute.</returns>
        public List<CanteenDishTable> getAllDishTypes()
        {
            return dB.Query<CanteenDishTable>(true, "SELECT DISTINCT dish_type FROM " + DBTableConsts.CANTEEN_DISH_TABLE + ";");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Launches the web browser and googles for images with the given dish.
        /// </summary>
        /// <param name="dish">The dish which should be used for googling.</param>
        public async Task googleDishString(CanteenDishTable dish)
        {
            await Util.launchBrowser(generateSearchString(dish));
        }

        /// <summary>
        /// Converts a list of string ingredients to a single string with emoji ingredients separated by ", ".
        /// </summary>
        /// <param name="ingredients">A list of ingredients.</param>
        /// <returns>All given ingredient strings as a single emoji ingredient string.</returns>
        public string ingredientsToEmojiString(IEnumerable<string> ingredients)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string ingredient in ingredients)
            {
                sb.Append(replaceWithEmoji(ingredient));

                // If not last append with ", ":
                if (ingredient != ingredients.Last())
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Downloads the menus if necessary or if force == true.
        /// </summary>
        /// <param name="force">Forces to download all menus.</param>
        /// <returns>Returns the syncing task or null if did not sync.</returns>
        public Task downloadCanteenDishes(bool force)
        {
            if (!force && Settings.getSettingBoolean(SettingsConsts.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return null;
            }

            waitForSyncToFinish();
            REFRESHING_TASK_SEMA.Wait();
            refreshingTask = Task.Run(async () =>
            {
                try
                {
                    if (!force && !SyncDBManager.INSTANCE.needSync(DBTableConsts.CANTEEN_DISH_TABLE, Consts.VALIDITY_ONE_DAY).NEEDS_SYNC)
                    {
                        return;
                    }
                    Logger.Info("Started downloading canteen dishes...");
                    Uri url = new Uri(Consts.MENUS_URL);
                    JsonArray json = await NetUtils.downloadJsonArrayAsync(url);

                    if (json != null && json.Count > 0)
                    {
                        List<CanteenDishTable> menus = new List<CanteenDishTable>();
                        foreach (JsonValue canteen in json)
                        {
                            JsonObject obj = canteen.GetObject();
                            string canteen_id = obj.GetNamedString("canteen_id");
                            if (!string.IsNullOrEmpty(canteen_id))
                            {
                                foreach (JsonValue dish in obj.GetNamedArray("dishes"))
                                {
                                    CanteenDishTable m = new CanteenDishTable(dish.GetObject(), canteen_id);
                                    m.nameEmojis = m.name;
                                    string ingredientsEmojiString = ingredientsToEmojiString(m.ingredients.Split(new char[] { ' ' }));
                                    if (ingredientsEmojiString.Length > 0)
                                    {
                                        m.nameEmojis += " (" + ingredientsEmojiString + ')';
                                    }
                                    menus.Add(m);
                                }
                            }
                        }

                        dB.DeleteAll<CanteenDishTable>();
                        dB.InsertAll(menus);
                    }
                    SyncDBManager.INSTANCE.Update(new SyncTable(DBTableConsts.CANTEEN_DISH_TABLE));
                    Logger.Info("Finished downloading canteen dishes.");
                }
                catch (Exception e)
                {
                    Logger.Error("Unable to download Canteen Menus", e);
                }
            });
            REFRESHING_TASK_SEMA.Release();

            return refreshingTask;
        }
        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Replaces the given ingredient string with the right emoji.
        /// E.g. "MSC" => "🎣"
        /// </summary>
        /// <param name="s">The ingredient string.</param>
        /// <returns>Returns the emoji for the given string.</returns>
        private string replaceWithEmoji(string s)
        {
            if (INGREDIENTS_EMOJI_ALL_LOOKUP.ContainsKey(s))
            {
                return INGREDIENTS_EMOJI_ALL_LOOKUP[s];
            }
            else
            {
                return s;
            }
        }

        /// <summary>
        /// Generates a url based an the given string for googling images.
        /// </summary>
        /// <param name="dish">The dish for googling.</param>
        /// <returns>Returns a Google Images url.</returns>
        private Uri generateSearchString(CanteenDishTable dish)
        {
            string name = dish.name.Replace(' ', '+');

            string result = Consts.GOOGLE_IMAGES_SEARCH_STRING_START + name + "&oq=" + name;
            return new Uri(result);
        }

        #endregion

        #region --Misc Methods (Protected)--
        protected override void DropTables()
        {
            dB.DropTable<CanteenDishTable>();
        }

        protected override void CreateTables()
        {
            dB.CreateTable<CanteenDishTable>();
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--



        #endregion
    }
}
