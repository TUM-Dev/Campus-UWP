using TUMCampusApp.Pages;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Classes
{
    public class UIUtils
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static MainPage2 mainPage;

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
                    return UIUtils.getLocalizedString("CanteenDishOfTheDay_Text");
                case "Aktionsessen":
                    return UIUtils.getLocalizedString("CanteenActionDishes_Text");
                case "Biogericht":
                    return UIUtils.getLocalizedString("CanteenBioDish_Text");
                case "StuBistro Gericht":
                    return UIUtils.getLocalizedString("CanteenStuBistroDishes_Text");
                case "Baustellenteller":
                    return UIUtils.getLocalizedString("CanteenBaustellenteller_Text");
                case "Fast Lane":
                    return UIUtils.getLocalizedString("CanteenFastLane_Text");
                case "Mensa Klassiker":
                    return UIUtils.getLocalizedString("CanteenCanteenClassics_Text");
                case "Mensa Spezial":
                    return UIUtils.getLocalizedString("CanteenCanteenSpecial_Text");
                case "Self-Service Grüne Mensa":
                    return UIUtils.getLocalizedString("CanteenSelf-ServiceGreenCanteen_Text");
                case "Self-Service Arcisstraße":
                    return UIUtils.getLocalizedString("CanteenSelf-ServiceArcisstraße_Text");
                case "Self-Service":
                    return UIUtils.getLocalizedString("CanteenSelf-Service_Text");
                case "Aktion":
                    return UIUtils.getLocalizedString("CanteenSpecialDishes_Text");
                case "Beilagen":
                    return UIUtils.getLocalizedString("CanteenSideDishes_Text");
                case "Tagesdessert":
                    return UIUtils.getLocalizedString("CanteenDessertOfTheDay_Text");
                case "Dessert":
                    return UIUtils.getLocalizedString("CanteenDessert_Text");
                default:
                    return dishType;
            }
        }

        /// <summary>
        /// Source: https://social.msdn.microsoft.com/Forums/sqlserver/en-US/0cc87160-5b0c-4fc1-b685-ff50117984f7/uwp-access-control-on-parent-page-through-frame-object?forum=wpdevelop
        /// </summary>
        public static T findParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? findParent<T>(parent);
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
