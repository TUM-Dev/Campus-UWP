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
