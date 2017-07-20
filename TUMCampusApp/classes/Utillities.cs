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
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--


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
            if (s.Contains("Wintersemester"))
            {
                return s.Replace("Wintersemester", getLocalizedString("TuitionFeeControlWinterTerm_Text"));
            }
            else
            {
                return s.Replace("Sommersemester", getLocalizedString("TuitionFeeControlSummerTerm_Text"));
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
