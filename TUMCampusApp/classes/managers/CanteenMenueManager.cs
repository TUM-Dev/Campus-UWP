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

namespace TUMCampusApp.classes.managers
{
    class CanteenMenueManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static CanteenMenueManager INSTANCE;
        private static readonly int TIME_TO_SYNC = 86400; // 1 day

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

        public static List<CanteenMenu> getMenus(int id)
        {
            List<CanteenMenu> menus = new List<CanteenMenu>();
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
            return menus;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task downloadCanteenMenusAsync(bool force)
        {
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
