using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes;
using Windows.Data.Json;
using TUMCampusApp.classes.canteen;
using TUMCampusApp.classes.sync;
using System.Text.RegularExpressions;
using TUMCampusApp.classes.userData;

namespace TUMCampusApp.classes.managers
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
            dB.CreateTable<CanteenMenu>();
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
        /// <param name="json"> see above </param>
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
		/// <param name="json"> see above </param>
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
        public async Task googleMenuString(string menu)
        {
            await Utillities.launchBrowser(generateSearchString(menu));
        }

        private Uri generateSearchString(string menu)
        {
            menu = getCleanMenuTitle(menu);
            menu = menu.Replace(' ', '+');

            string result = @"https://www.google.com/search?hl=en&as_st=y&site=imghp&tbm=isch&source=hp&biw=1502&bih=682&q=" + menu + "&oq=" + menu;
            return new Uri(result);
        }

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
                    Logger.Info("Sync not required! - CanteenMenuManager");
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

        public string replaceMenuStringWithImages(string s)
        {
            List<string> res = new List<string>();

            Regex reg1 = new Regex(@"\((\w{1,3},?)*\)");
            Regex reg2 = new Regex(@"\[(\w{1,3},?)*\]");
            s = replaceMatches(s, reg1.Matches(s));
            s = replaceMatches(s, reg2.Matches(s));
            if(s.EndsWith(", "))
            {
                s = s.Substring(0, s.Length - 2);
            }
            return s;
        }
        
        public async override Task InitManagerAsync()
        {
        }
        #endregion

        #region --Misc Methods (Private)--
        private string addImages(string[] ingredients)
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
                        case "schh":
                            s += "🌰";
                            break;
                        case "13":
                            s += "🍫";
                            break;
                        default:
                            s += item;
                            break;
                    }
                    s += ", ";
                }
            }
            return s;
        }

        private string replaceMatches(string s, MatchCollection col)
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
                    s = s.Replace(match.Value, addImages(list.ToArray()));
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
