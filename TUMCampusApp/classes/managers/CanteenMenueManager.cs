using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using Windows.Data.Json;
using TUMCampusApp.Classes.Canteens;
using TUMCampusApp.Classes.Syncs;
using System.Text.RegularExpressions;
using TUMCampusApp.Classes.UserDatas;

namespace TUMCampusApp.Classes.Managers
{
    class CanteenMenueManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CanteenMenueManager INSTANCE;
        private static readonly int TIME_TO_SYNC = 86400; // 1 day
        private static List<CanteenMenu> menus = new List<CanteenMenu>();
        private static int lastSelectedCanteenId = -2;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteenMenueManager()
        {
            
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Convert JSON object to CafeteriaMenu
        /// <p/>
        /// Example JSON: e.g.
        /// {"id":"25544","mensa_id":"411","date":"2011-06-20","type_short"
        /// :"tg","type_long":"Tagesgericht 3","type_nr":"3","name":
        /// "Cordon bleu vom Schwein (mit Formfleischhinterschinken) (S) (1,2,3,8)"}
        /// </summary>
        /// <param name="json">See above</param>
        /// <returns> CafeteriaMenu </returns>
        private static CanteenMenu getFromJson(JsonObject json)
        {
            return new CanteenMenu(int.Parse(json.GetNamedString(Const.JSON_ID)),
                    int.Parse(json.GetNamedString(Const.JSON_MENSA_ID)),
                    Utillities.getDate(json.GetNamedString(Const.JSON_DATE)),
                    json.GetNamedString(Const.JSON_TYPE_SHORT).Replace("\"", "\'"),
                    json.GetNamedString(Const.JSON_TYPE_LONG).Replace("\"", "\'"),
                    int.Parse(json.GetNamedString(Const.JSON_TYPE_NR)),
                    json.GetNamedString(Const.JSON_NAME).Replace("\"", "\'"));
        }

        /// <summary>
		/// Convert JSON object to CafeteriaMenu (addendum)
		/// <p/>
		/// Example JSON: e.g.
		/// {"mensa_id":"411","date":"2011-07-29","name":"Pflaumenkompott"
		/// ,"type_short":"bei","type_long":"Beilagen"}
		/// </summary>
		/// <param name="json">See above</param>
		/// <returns> CafeteriaMenu </returns>
        private static CanteenMenu getFromJsonAddendum(JsonObject json)
        {
            return new CanteenMenu(0,
                    int.Parse(json.GetNamedString(Const.JSON_MENSA_ID)),
                    Utillities.getDate(json.GetNamedString(Const.JSON_DATE)),
                    json.GetNamedString(Const.JSON_TYPE_SHORT).Replace("\"", "\'"),
                    json.GetNamedString(Const.JSON_TYPE_LONG).Replace("\"", "\'"),
                    10,
                    json.GetNamedString(Const.JSON_NAME).Replace("\"", "\'"));
        }

        /// <summary>
        /// Returns the first next date. Based on "Tagesgericht".
        /// </summary>
        /// <returns>Returns the first next date</returns>
        public static DateTime getFirstNextDate()
        {
            DateTime time = DateTime.MaxValue;
            List<CanteenMenu> menus = new List<CanteenMenu>();
            foreach (CanteenMenu m in dB.Query<CanteenMenu>("SELECT * FROM CanteenMenu WHERE typeLong LIKE '%Tagesgericht%'"))
            {
                if(m.date.Date.CompareTo(time.Date) < 0 && m.date.Date.CompareTo(DateTime.Now.Date.AddDays(-1)) >= 0)
                {
                    time = m.date;
                }
            }
            return time;
        }

        /// <summary>
        /// Returns all menus contained in the db that match the given canteen id.
        /// </summary>
        /// <param name="id">Canteen id</param>
        /// <returns>Returns all menus contained in the db.</returns>
        public static List<CanteenMenu> getMenus(int id)
        {
            if(lastSelectedCanteenId == id && !SyncManager.INSTANCE.needSync("last_selected_canteen", TIME_TO_SYNC))
            {
                return menus;
            }
            else
            {
                menus = new List<CanteenMenu>();
                lastSelectedCanteenId = id;
                SyncManager.INSTANCE.replaceIntoDb(new Sync("last_selected_canteen"));
                if (id == -1)
                {
                    foreach (CanteenMenu m in dB.Query<CanteenMenu>("SELECT * FROM CanteenMenu"))
                    {
                        menus.Add(m);
                    }
                    return menus;
                }
                foreach (CanteenMenu m in dB.Query<CanteenMenu>("SELECT * FROM CanteenMenu WHERE cafeteriaId = ?", id))
                {
                    menus.Add(m);
                }
            }
            return menus;
        }

        /// <summary>
        /// Returns all menus that match the given canteen id, type name and date from the db.
        /// </summary>
        /// <param name="canteenID">Canteen id</param>
        /// <param name="name">Canteen menu type</param>
        /// <param name="contains">Whether the given menu type name contains or equals the given menu type name</param>
        /// <param name="date">Menu date</param>
        /// <returns>Returns all menus that match the given canteen id, type name and date from the db.</returns>
        public List<CanteenMenu> getMenusForType(int canteenID, string name, bool contains, DateTime date)
        {
            List<CanteenMenu> cM = getMenus(canteenID);
            if(cM == null || cM.Count <= 0)
            {
                return null;
            }

            List<CanteenMenu> result = new List<CanteenMenu>();
            bool b;
            foreach (CanteenMenu m in cM)
            {
                b = false;
                if (contains)
                {
                    b = m.typeLong.Contains(name);
                }
                else
                {
                    b = m.typeLong.Equals(name);
                }
                if (b && m.date.DayOfYear == date.DayOfYear)
                {
                    result.Add(m);
                }
            }
            return result;
        }

        /// <summary>
        /// Replaces the ingredients with emojis.
        /// </summary>
        /// <param name="s">Menu string</param>
        /// <returns>Returns the replaced menu string</returns>
        public string getCleanMenuTitle(string s)
        {
            Regex reg1 = new Regex(@"\((\w{1,3},?)*\)");
            Regex reg2 = new Regex(@"\[(\w{1,3},?)*\]");
            s = reg1.Replace(s, "");
            s = reg2.Replace(s, "");
            return s;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Launches the web browser and googles for images with the given string.
        /// </summary>
        /// <param name="menu">The string which should be used for googling.</param>
        /// <returns>Returns a async Task.</returns>
        public async Task googleMenuString(string menu)
        {
            await Utillities.launchBrowser(generateSearchString(menu));
        }

        /// <summary>
        /// Generates a url based an the given string for googling images.
        /// </summary>
        /// <param name="menu">The string which should be used for googling.</param>
        /// <returns>The url for googling.</returns>
        private Uri generateSearchString(string menu)
        {
            menu = getCleanMenuTitle(menu);
            menu = menu.Replace(' ', '+');

            string result = @"https://www.google.com/search?hl=en&as_st=y&site=imghp&tbm=isch&source=hp&biw=1502&bih=682&q=" + menu + "&oq=" + menu;
            return new Uri(result);
        }

        /// <summary>
        /// Downloads the menus if necessary or if force == true.
        /// </summary>
        /// <param name="force">Forces to download all menus.</param>
        /// <returns>Returns a async Task.</returns>
        public async Task downloadCanteenMenusAsync(bool force)
        {
            if (!force && Utillities.getSettingBoolean(Const.ONLY_USE_WIFI_FOR_UPDATING) && !DeviceInfo.isConnectedToWifi())
            {
                return;
            }
            try
            {
                if (!force && !SyncManager.INSTANCE.needSync(this, TIME_TO_SYNC))
                {
                    return;
                }
                Uri url = new Uri("http://lu32kap.typo3.lrz.de/mensaapp/exportDB.php?mensa_id=all");
                JsonObject json = await NetUtils.downloadJsonObjectAsync(url);
                List<CanteenMenu> menus = new List<CanteenMenu>();

                JsonArray menu = json.GetNamedArray("mensa_menu");
                foreach (JsonValue val in menu)
                {
                    menus.Add(getFromJson(val.GetObject()));
                }

                JsonArray beilagen = json.GetNamedArray("mensa_beilagen");
                foreach (JsonValue val in beilagen)
                {
                    menus.Add(getFromJsonAddendum(val.GetObject()));
                }
                dB.DeleteAll<CanteenMenu>();
                dB.InsertAll(menus);
                SyncManager.INSTANCE.replaceIntoDb(new Sync(this));
            }
            catch (Exception e)
            {
                Logger.Error("Unable to download Canteen Menus", e);
            }
        }

        /// <summary>
        /// Replaces the ingredients with emojis.
        /// </summary>
        /// <param name="s">Menu string.</param>
        /// <param name="withComma">Whether it should seperate each emoji with a comma.</param>
        /// <returns>Returns the replaced menu string</returns>
        public string replaceMenuStringWithImages(string s, bool withComma)
        {
            List<string> res = new List<string>();

            Regex reg1 = new Regex(@"\((\w{1,3},?)*\)");
            Regex reg2 = new Regex(@"\[(\w{1,3},?)*\]");
            s = replaceMatches(s, reg1.Matches(s), withComma);
            s = replaceMatches(s, reg2.Matches(s), withComma);
            if(s.EndsWith(", "))
            {
                s = s.Substring(0, s.Length - 2);
            }
            return s;
        }
        
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<CanteenMenu>();
        }
        #endregion

        #region --Misc Methods (Private)--
        private string addImages(string[] ingredients, bool withComma)
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
                        case "gqb":
                            s += "\u2122";
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
                        default:
                            s += item;
                            break;
                    }
                    if(withComma)
                    {
                        s += ", ";
                    }
                    else
                    {
                        s += " ";
                    }
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
                    s = s.Replace(match.Value, addImages(list.ToArray(), withComma));
                }
            }
            return s;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--



        #endregion
    }
}
