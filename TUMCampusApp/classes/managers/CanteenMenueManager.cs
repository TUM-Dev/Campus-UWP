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
                if(m.date.Date.CompareTo(time.Date) < 0 && m.date.Date.CompareTo(DateTime.Now.Date) >= -1)
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
                        m.name = replaceMenuStringWithImages(m.name);
                        menus.Add(m);
                    }
                    return menus;
                }
                foreach (CanteenMenu m in dB.Query<CanteenMenu>("SELECT * FROM CanteenMenu WHERE cafeteriaId = ?", id))
                {
                    m.name = replaceMenuStringWithImages(m.name);
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

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task googleMenuString(string menu)
        {
            await Utillities.launchBrowser(generateSearchString(menu));
        }

        private Uri generateSearchString(string menu)
        {
            if (menu.Contains('('))
            {
                menu = menu.Substring(0, menu.IndexOf('('));
            }
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

        public static string replaceMenuStringWithImages(string s)
        {
            for(int i = 0; i < 5; i++)
            {
                s = s.Replace("(v)", "\U0001F33D ");
                s = s.Replace("( v,", "\U0001F33D ");
                s = s.Replace(" v,", "\U0001F33D ");
                s = s.Replace("v)", "\U0001F33D ");

                s = s.Replace("(S)", "\U0001F416 ");
                s = s.Replace("(S,", "\U0001F416 ");
                s = s.Replace(" S,", "\U0001F416 ");
                s = s.Replace(" S)", "\U0001F416 ");

                s = s.Replace("(f)", "\U0001F955 ");
                s = s.Replace("(f,", "\U0001F955 ");
                s = s.Replace(" f,", "\U0001F955 ");
                s = s.Replace(" f)", "\U0001F955 ");

                s = s.Replace("(R)", "\U0001F404 ");
                s = s.Replace("(R,", "\U0001F404 ");
                s = s.Replace(" R,", "\U0001F404 ");
                s = s.Replace(" R)", "\U0001F404 ");

                s = s.Replace("(99)", "\U0001F377 ");
                s = s.Replace("(99,", "\U0001F377 ");
                s = s.Replace(" 99,", "\U0001F377 ");
                s = s.Replace(" 99)", "\U0001F377 ");

                s = s.Replace("(GQB)", "\u2122 ");
                s = s.Replace("(GQB,", "\u2122 ");
                s = s.Replace(" GQB,", "\u2122 ");
                s = s.Replace(" GQB)", "\u2122 ");
            }
            return s;
        }

        public async override Task InitManagerAsync()
        {
        }
        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--



        #endregion
    }
}
