using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMCampusAppAPI.Canteens
{
    public class CanteenPrices
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly string PRICE_100 = "1,00";
        private static readonly string PRICE_155 = "1,55";
        private static readonly string PRICE_190 = "1,90";
        private static readonly string PRICE_220 = "2,20";
        private static readonly string PRICE_240 = "2,40";
        private static readonly string PRICE_260 = "2,60";
        private static readonly string PRICE_270 = "2,70";
        private static readonly string PRICE_280 = "2,80";
        private static readonly string PRICE_290 = "2,90";
        private static readonly string PRICE_300 = "3,00";
        private static readonly string PRICE_320 = "3,20";
        private static readonly string PRICE_330 = "3,30";
        private static readonly string PRICE_340 = "3,40";
        private static readonly string PRICE_350 = "3,50";
        private static readonly string PRICE_360 = "3,60";
        private static readonly string PRICE_370 = "3,70";
        private static readonly string PRICE_390 = "3,90";
        private static readonly string PRICE_400 = "4,00";
        private static readonly string PRICE_410 = "4,10";
        private static readonly string PRICE_440 = "4,40";
        private static readonly string PRICE_450 = "4,50";
        private static readonly string PRICE_490 = "4,90";
        private static readonly string PRICE_540 = "5,40";

        private static readonly Dictionary<string, string> STUDENT_PRICES = new Dictionary<string, string>()
        {
            {"Tagesgericht 1", PRICE_100 },
            {"Tagesgericht 2", PRICE_155},
            {"Tagesgericht 3", PRICE_190},
            {"Tagesgericht 4", PRICE_240},

            {"Aktionsessen 1", PRICE_155 },
            {"Aktionsessen 2", PRICE_190},
            {"Aktionsessen 3", PRICE_240},
            {"Aktionsessen 4", PRICE_260},
            {"Aktionsessen 5", PRICE_280},
            {"Aktionsessen 6", PRICE_300},
            {"Aktionsessen 7", PRICE_320},
            {"Aktionsessen 8", PRICE_350},
            {"Aktionsessen 9", PRICE_400},
            {"Aktionsessen 10", PRICE_450},

            {"Biogericht 1", PRICE_155},
            {"Biogericht 2", PRICE_190},
            {"Biogericht 3", PRICE_240},
            {"Biogericht 4", PRICE_260},
            {"Biogericht 5", PRICE_280},
            {"Biogericht 6", PRICE_300},
            {"Biogericht 7", PRICE_320},
            {"Biogericht 8", PRICE_350},
            {"Biogericht 9", PRICE_400},
            {"Biogericht 10", PRICE_450}
        };
        private static readonly Dictionary<string, string> EMPLOYEE_PRICES = new Dictionary<string, string>()
        {
            {"Tagesgericht 1", PRICE_190 },
            {"Tagesgericht 2", PRICE_220},
            {"Tagesgericht 3", PRICE_240},
            {"Tagesgericht 4", PRICE_280},

            {"Aktionsessen 1", PRICE_220},
            {"Aktionsessen 2", PRICE_240},
            {"Aktionsessen 3", PRICE_280},
            {"Aktionsessen 4", PRICE_300},
            {"Aktionsessen 5", PRICE_320},
            {"Aktionsessen 6", PRICE_340},
            {"Aktionsessen 7", PRICE_360},
            {"Aktionsessen 8", PRICE_390},
            {"Aktionsessen 9", PRICE_440},
            {"Aktionsessen 10", PRICE_490},

            {"Biogericht 1", PRICE_220},
            {"Biogericht 2", PRICE_240},
            {"Biogericht 3", PRICE_280},
            {"Biogericht 4", PRICE_300},
            {"Biogericht 5", PRICE_320},
            {"Biogericht 6", PRICE_340},
            {"Biogericht 7", PRICE_360},
            {"Biogericht 8", PRICE_390},
            {"Biogericht 9", PRICE_440},
            {"Biogericht 10", PRICE_490}
        };
        private static readonly Dictionary<string, string> GUEST_PRICES = new Dictionary<string, string>()
        {
            {"Tagesgericht 1", PRICE_240},
            {"Tagesgericht 2", PRICE_270},
            {"Tagesgericht 3", PRICE_290},
            {"Tagesgericht 4", PRICE_330},

            {"Aktionsessen 1", PRICE_270},
            {"Aktionsessen 2", PRICE_290},
            {"Aktionsessen 3", PRICE_330},
            {"Aktionsessen 4", PRICE_350},
            {"Aktionsessen 5", PRICE_370},
            {"Aktionsessen 6", PRICE_390},
            {"Aktionsessen 7", PRICE_410},
            {"Aktionsessen 8", PRICE_440},
            {"Aktionsessen 9", PRICE_490},
            {"Aktionsessen 10", PRICE_540},

            {"Biogericht 1", PRICE_270},
            {"Biogericht 2", PRICE_290},
            {"Biogericht 3", PRICE_330},
            {"Biogericht 4", PRICE_350},
            {"Biogericht 5", PRICE_370},
            {"Biogericht 6", PRICE_390},
            {"Biogericht 7", PRICE_410},
            {"Biogericht 8", PRICE_440},
            {"Biogericht 9", PRICE_490},
            {"Biogericht 10", PRICE_540}
        };

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns a Dictionary with prices for your current role.
        /// </summary>
        public static Dictionary<string, string> getRolePrices()
        {
            string type = Util.getSettingString(Const.ROLE);
            switch (type)
            {
                case "0":
                    return STUDENT_PRICES;

                case "1":
                    return EMPLOYEE_PRICES;

                case "2":
                    return GUEST_PRICES;

                default:
                    return STUDENT_PRICES;
            }
        }

        /// <summary>
        /// Searches for the price of the given menu combined with your current role and returns it.
        /// </summary>
        /// <param name="menuType">The menu type you search the price for.</param>
        /// <returns>Returns the price.</returns>
        public static string getPrice(string menuType)
        {
            getRolePrices().TryGetValue(menuType, out string price);
            return price;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


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
