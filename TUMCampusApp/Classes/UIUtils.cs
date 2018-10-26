using System;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using Windows.ApplicationModel.Resources;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Classes
{
    public class UIUtils
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static MainPage2 mainPage;

        private static ResourceLoader loader;
        private static TaskCompletionSource<ContentDialog> contentDialogShowRequest;

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
            if (s == null)
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
                    return getLocalizedString("CanteenDishOfTheDay_Text");
                case "Aktionsessen":
                    return getLocalizedString("CanteenActionDishes_Text");
                case "Biogericht":
                    return getLocalizedString("CanteenBioDish_Text");
                case "StuBistro Gericht":
                    return getLocalizedString("CanteenStuBistroDishes_Text");
                case "Baustellenteller":
                    return getLocalizedString("CanteenBaustellenteller_Text");
                case "Fast Lane":
                    return getLocalizedString("CanteenFastLane_Text");
                case "Mensa Klassiker":
                    return getLocalizedString("CanteenCanteenClassics_Text");
                case "Mensa Spezial":
                    return getLocalizedString("CanteenCanteenSpecial_Text");
                case "Self-Service Grüne Mensa":
                    return getLocalizedString("CanteenSelf-ServiceGreenCanteen_Text");
                case "Self-Service Arcisstraße":
                    return getLocalizedString("CanteenSelf-ServiceArcisstraße_Text");
                case "Self-Service":
                    return getLocalizedString("CanteenSelf-Service_Text");
                case "Aktion":
                    return getLocalizedString("CanteenSpecialDishes_Text");
                case "Beilagen":
                    return getLocalizedString("CanteenSideDishes_Text");
                case "Tagesdessert":
                    return getLocalizedString("CanteenDessertOfTheDay_Text");
                case "Dessert":
                    return getLocalizedString("CanteenDessert_Text");
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

        /// <summary>
        /// Launches the default application for the given Uri.
        /// </summary>
        /// <param name="url">The Uri that defines the application that should get launched.</param>
        /// <returns>Returns true on success.</returns>
        public static async Task<bool> launchUriAsync(Uri url)
        {
            return await Windows.System.Launcher.LaunchUriAsync(url);
        }

        public static async Task<ContentDialogResult> showDialogAsyncQueue(ContentDialog dialog)
        {
            // Make sure it gets invoked by the UI thread:
            if (!Window.Current.Dispatcher.HasThreadAccess)
            {
                throw new InvalidOperationException("This method can only be invoked from UI thread.");
            }

            while (contentDialogShowRequest != null)
            {
                await contentDialogShowRequest.Task;
            }

            contentDialogShowRequest = new TaskCompletionSource<ContentDialog>();
            var result = await dialog.ShowAsync();
            contentDialogShowRequest.SetResult(dialog);
            contentDialogShowRequest = null;

            return result;
        }

        /// <summary>
        /// Checks whether the current device is a Windows Mobile device.
        /// </summary>
        public static bool isRunningOnMobileDevice()
        {
            return AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Mobile");
        }

        /// <summary>
        /// Hides the StatusBar on Windows Mobile devices asynchronously.
        /// </summary>
        public static async Task hideStatusBarAsync()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }
        }

        /// <summary>
        /// Shows the StatusBar on Windows Mobile devices asynchronously.
        /// </summary>
        public static async Task showStatusBarAsync()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().ShowAsync();
            }
        }

        /// <summary>
        /// Manages the Windows Mobile StatusBar asynchronously.
        /// </summary>
        public static async Task onPageNavigatedFromAsync()
        {
            if (isRunningOnMobileDevice())
            {
                await showStatusBarAsync();
            }
        }

        /// <summary>
        /// Manages the Windows Mobile StatusBar asynchronously.
        /// </summary>
        public static async Task onPageSizeChangedAsync(SizeChangedEventArgs e)
        {
            if (isRunningOnMobileDevice())
            {
                if (e.NewSize.Height < e.NewSize.Width)
                {
                    await hideStatusBarAsync();
                }
                else
                {
                    await showStatusBarAsync();
                }
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
