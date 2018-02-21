using TUMCampusApp.Pages;
using Windows.ApplicationModel.Resources;

namespace TUMCampusApp.Classes
{
    public class Utillities
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public enum EnumPage
        {
            MyCalendarPage,
            MyLecturesPage,
            MyGradesPage,
            TuitionFeesPage,
            HomePage,
            CanteensPage,
            NewsPage,
            MVPage,
            PlansPage,
            RoomfinderPage,
            StudyRoomsPage,
            OpeningHoursPage,
            StudyPlansPage,
            SettingsPage,
            SetupPage,
        }

        public static MainPage mainPage;

        private static ResourceLoader loader;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns a localized string for the given key.
        /// </summary>
        /// <param name="key">The key for the requested localized string.</param>
        /// <returns>a localized string for the given key.</returns>
        public static string getLocalizedString(string key)
        {
            if (loader == null)
            {
                loader = ResourceLoader.GetForCurrentView();
            }
            return loader.GetString(key);

        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Translates the given semester to the right language.
        /// </summary>
        /// <param name="s">The string that should get translated.</param>
        /// <returns>A translated version of the given string.</returns>
        public static string translateSemester(string s)
        {
            if(s == null)
            {
                return null;
            }
            if (s.Contains("Wintersemester") || s.Contains("Winter Semester"))
            {
                return s.Replace("Wintersemester", getLocalizedString("TuitionFeeControlWinterTerm_Text"))
                    .Replace("Winter Semester", getLocalizedString("TuitionFeeControlWinterTerm_Text"));
            }
            else
            {
                return s.Replace("Sommersemester", getLocalizedString("TuitionFeeControlSummerTerm_Text"))
                    .Replace("Sommer Semester", getLocalizedString("TuitionFeeControlSummerTerm_Text"))
                    .Replace("Summer Semester", getLocalizedString("TuitionFeeControlSummerTerm_Text"));
            }
        }

        /// <summary>
        /// Translates the given dish type.
        /// </summary>
        /// <param name="dishType">The dish type you want to translate.</param>
        /// <returns>A translated version of the given dish type.</returns>
        public static string translateDishType(string dishType)
        {
            switch (dishType)
            {
                case "Tagesgericht":
                    return Utillities.getLocalizedString("CanteenDishOfTheDay_Text");
                case "Aktionsessen":
                    return Utillities.getLocalizedString("CanteenActionDishes_Text");
                case "Biogericht":
                    return Utillities.getLocalizedString("CanteenBioDish_Text");
                case "StuBistro Gericht":
                    return Utillities.getLocalizedString("CanteenStuBistroDishes_Text");
                case "Baustellenteller":
                    return Utillities.getLocalizedString("CanteenBaustellenteller_Text");
                case "Fast Lane":
                    return Utillities.getLocalizedString("CanteenFastLane_Text");
                case "Mensa Klassiker":
                    return Utillities.getLocalizedString("CanteenCanteenClassics_Text");
                case "Mensa Spezial":
                    return Utillities.getLocalizedString("CanteenCanteenSpecial_Text");
                case "Self-Service Grüne Mensa":
                    return Utillities.getLocalizedString("CanteenSelf-ServiceGreenCanteen_Text");
                case "Self-Service Arcisstraße":
                    return Utillities.getLocalizedString("CanteenSelf-ServiceArcisstraße_Text");
                case "Self-Service":
                    return Utillities.getLocalizedString("CanteenSelf-Service_Text");
                case "Aktion":
                    return Utillities.getLocalizedString("CanteenSpecialDishes_Text");
                case "Beilagen":
                    return Utillities.getLocalizedString("CanteenSideDishes_Text");
                case "Tagesdessert":
                    return Utillities.getLocalizedString("CanteenDessertOfTheDay_Text");
                case "Dessert":
                    return Utillities.getLocalizedString("CanteenDessert_Text");
                default:
                    return dishType;
            }
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
