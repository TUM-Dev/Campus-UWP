using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Syncs;
using System.Text.RegularExpressions;
using TUMCampusAppAPI.UserDatas;
using System.Linq;

namespace TUMCampusAppAPI.Managers
{
    public class CanteenDishManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CanteenDishManager INSTANCE;
        private static readonly int TIME_TO_SYNC = 86400; // 1 day
        private static List<CanteenDishTable> menus = new List<CanteenDishTable>();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteenDishManager()
        {
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the first next date. Based on "Tagesgericht".
        /// </summary>
        /// <returns>Returns the first next date.</returns>
        public static DateTime getFirstNextDate(string canteen_id)
        {
            DateTime time = DateTime.MaxValue;
            DateTime dateToday = DateTime.Now;
            if (dateToday.Hour < 16) // If it's after 16 o' clock show the menus for the next day
            {
                dateToday = dateToday.AddDays(-2);
            }
            else
            {
                dateToday = dateToday.AddDays(-1);
            }

            foreach (CanteenDishTable m in dB.Query<CanteenDishTable>("SELECT * FROM CanteenDishTable WHERE dish_type LIKE '%Tagesgericht%' OR dish_type LIKE '%Beilage%'"))
            {
                if (m.canteen_id.Equals(canteen_id) && m.date.Date.CompareTo(time.Date) < 0 && m.date.Date.CompareTo(dateToday) >= 0)
                {
                    time = m.date;
                }
            }
            return time;
        }

        /// <summary>
        /// Returns all dates where a dish was found and the date is greater or equal to the current date.
        /// </summary>
        /// <param name="canteen_id">The id of the canteen you want the dates for.</param>
        public List<DateTime> getDishDates(string canteen_id)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime dateToday = DateTime.Now;
            if (dateToday.Hour < 16) // If it's after 16 o' clock show the menus for the next day
            {
                dateToday = dateToday.AddDays(-2);
            }
            else
            {
                dateToday = dateToday.AddDays(-1);
            }

            List<CanteenDishTable> x = getDishes(canteen_id);
            foreach (CanteenDishTable m in getDishes(canteen_id))
            {
                if (m.date.Date.CompareTo(dateToday) >= 0 && !dates.Contains(m.date))
                {
                    dates.Add(m.date);
                }
            }
            dates.Sort();
            return dates;
        }

        /// <summary>
        /// Returns all dishes contained in the db that match the given canteen_id.
        /// </summary>
        /// <param name="canteen_id">Canteen id</param>
        public static List<CanteenDishTable> getDishes(string canteen_id)
        {
            List<CanteenDishTable> list = dB.Query<CanteenDishTable>("SELECT * FROM CanteenDishTable WHERE canteen_id = ?", canteen_id);
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
                list = dB.Query<CanteenDishTable>("SELECT * FROM CanteenDishTable WHERE canteen_id = ? AND dish_type LIKE '%" + dish_type + "%';", canteen_id);
            }
            else
            {
                list = dB.Query<CanteenDishTable>("SELECT * FROM CanteenDishTable WHERE canteen_id = ? AND dish_type = ?;", canteen_id, dish_type);
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
            List<CanteenDishTable> list = dB.Query<CanteenDishTable>("SELECT * FROM CanteenDishTable WHERE canteen_id = ? ORDER BY dish_type DESC;", canteen_id);
            return list.Where(d => d.date.Date.Equals(date.Date)).ToList();
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
        /// Replaces the ingredients with emojis.
        /// </summary>
        /// <param name="s">Menu string.</param>
        /// <param name="withComma">Whether it should separate each emoji with a comma.</param>
        /// <returns>Returns the replaced dish string</returns>
        public string replaceDishStringWithEmojis(string s, bool withComma)
        {
            List<string> res = new List<string>();

            Regex reg1 = new Regex(@"\((\w{1,3},?)*\)");
            Regex reg2 = new Regex(@"\[(\w{1,3},?)*\]");
            s = replaceMatches(s, reg1.Matches(s), withComma);
            s = replaceMatches(s, reg2.Matches(s), withComma);
            if (s.EndsWith(", "))
            {
                s = s.Substring(0, s.Length - 2);
            }
            return s;
        }

        /// <summary>
        /// Downloads the menus if necessary or if force == true.
        /// </summary>
        /// <param name="force">Forces to download all menus.</param>
        /// <returns>Returns an async Task.</returns>
        public async Task downloadCanteenDishesAsync(bool force)
        {
            if (!force && Util.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            try
            {
                if (!force && !SyncManager.INSTANCE.needSync(this, TIME_TO_SYNC).NEEDS_SYNC)
                {
                    return;
                }
                Uri url = new Uri(Const.MENUS_URL);
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
                                m.nameEmojis = m.name + ' ' + replaceDishStringWithEmojis(m.ingredients, true);
                                menus.Add(m);
                            }
                        }
                    }

                    dB.DeleteAll<CanteenDishTable>();
                    dB.InsertAll(menus);
                }
                SyncManager.INSTANCE.replaceIntoDb(new Sync(this));
            }
            catch (Exception e)
            {
                Logger.Error("Unable to download Canteen Menus", e);
            }
        }

        public async override Task InitManagerAsync()
        {
            dB.CreateTable<CanteenDishTable>();
        }
        #endregion

        #region --Misc Methods (Private)--
        private string addEmojis(string[] ingredients, bool withComma)
        {
            string s = "";
            if (ingredients != null && ingredients.Length > 0)
            {
                foreach (string item in ingredients)
                {
                    switch (item.ToLower())
                    {
                        case "v":
                            s += "\U0001F33D";
                            break;
                        case "s":
                            s += "\U0001F416";
                            break;
                        case "f":
                            s += "\U0001F955";
                            break;
                        case "r":
                            s += "\U0001F404";
                            break;
                        case "99":
                            s += "\U0001F377";
                            break;
                        case "ei":
                            s += "🥚";
                            break;
                        case "en":
                            s += "🥜";
                            break;
                        case "fi":
                            s += "🐟";
                            break;
                        case "kr":
                            s += "🦀";
                            break;
                        case "mi":
                            s += "🥛";
                            break;
                        case "wt":
                            s += "🐙";
                            break;
                        case "sch":
                            s += "🌰";
                            break;
                        case "13":
                            s += "🍫";
                            break;
                        case "k":
                            s += "🐂";
                            break;
                        case "9":
                            s += "🍬";
                            break;
                        case "2":
                            s += "🥫";
                            break;
                        case "msc":
                            s += "🎣";
                            break;
                        case "sc":
                            s += "🥥";
                            break;
                        case "1":
                            s += "🍭";
                            break;
                        case "5":
                            s += "🔶";
                            break;
                        case "6":
                            s += "⚫";
                            break;
                        case "10":
                            s += "💊";
                            break;
                        case "11":
                            s += "🍡";
                            break;
                        case "sw":
                            s += "🔻";
                            break;
                        case "14":
                            s += "🍮";
                            break;
                        case "8":
                            s += "🔷";
                            break;
                        case "gl":
                            s += "🌾";
                            break;
                        case "4":
                            s += "🔬";
                            break;
                        case "3":
                            s += "⚗";
                            break;
                        default:
                            s += item;
                            break;
                    }
                    s += withComma ? ", " : " ";
                }
            }
            return s;
        }

        private string replaceMatches(string s, MatchCollection col, bool withComma)
        {
            string ingredient = "";
            List<string> list = null;
            foreach (Match match in col)
            {
                list = new List<string>();
                foreach (char c in match.Value)
                {
                    if (c != ',' && c != '(' && c != ')' && c != '[' && c != ']')
                    {
                        ingredient += c;
                    }
                    else if (c != '(' && c != '[')
                    {
                        if (!ingredient.Equals(""))
                        {
                            list.Add(ingredient);
                            ingredient = "";
                        }
                    }
                }
                if (list.Count > 0)
                {
                    s = s.Replace(match.Value, addEmojis(list.ToArray(), withComma));
                }
            }
            return s;
        }

        /// <summary>
        /// Generates a url based an the given string for googling images.
        /// </summary>
        /// <param name="dish">The dish for googling.</param>
        /// <returns>Returns a Google Images url.</returns>
        private Uri generateSearchString(CanteenDishTable dish)
        {
            string name = dish.name.Replace(' ', '+');

            string result = @"https://www.google.com/search?hl=en&as_st=y&site=imghp&tbm=isch&source=hp&biw=1502&bih=682&q=" + name + "&oq=" + name;
            return new Uri(result);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--



        #endregion
    }
}
