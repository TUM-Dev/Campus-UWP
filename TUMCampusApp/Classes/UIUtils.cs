using Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using Windows.ApplicationModel.Resources;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Classes
{
    public static class UiUtils
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static MainPage2 mainPage
        {
            get => _mainPage;
            set
            {
                _mainPage = value;
                mainFrame = value.getMainFrame();
            }
        }
        private static MainPage2 _mainPage;
        public static Frame mainFrame { get; private set; }

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
        public static string GetLocalizedString(string key)
        {
            if (loader == null)
            {
                loader = ResourceLoader.GetForCurrentView();
            }
            return loader.GetString(key);

        }

        public static bool IsApplicationViewApiAvailable()
        {
            return ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView");
        }

        public static bool IsStatusBarApiAvailable()
        {
            return ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");
        }

        /// <summary>
        /// The KeyboardAccelerator class got introduced with v10.0.16299.0.
        /// Source: https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.input.keyboardaccelerator
        /// </summary>
        /// <returns></returns>
        public static bool IsKeyboardAcceleratorApiAvailable()
        {
            return ApiInformation.IsTypePresent("Windows.UI.Xaml.Input.KeyboardAccelerator");
        }

        public static List<KeyboardAccelerator> GetGoBackKeyboardAccelerators()
        {
            return new List<KeyboardAccelerator>
            {
                new KeyboardAccelerator
                {
                    Key = VirtualKey.Back
                },
                new KeyboardAccelerator
                {
                    Key = VirtualKey.Left
                },
                new KeyboardAccelerator
                {
                    Key = VirtualKey.GoBack
                }
            };
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public static void SetupWindow(Application application)
        {
            //PC customization
            if (IsApplicationViewApiAvailable())
            {
                ApplicationView appView = ApplicationView.GetForCurrentView();
                bool isDarkTheme = Application.Current.RequestedTheme == ApplicationTheme.Dark;

                //Dye title bar buttons:
                appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonInactiveForegroundColor = (isDarkTheme) ? Colors.White : Colors.Black;
                appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonForegroundColor = (isDarkTheme) ? Colors.White : Colors.Black;

                // Extend window:
                Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            }

            //Mobile customization
            if (IsStatusBarApiAvailable())
            {

                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundColor = ((SolidColorBrush)application.Resources["TumBlueBrush"]).Color;
                    statusBar.BackgroundOpacity = 1;
                }
            }
        }

        /// <summary>
        /// Translates the given semester to the right language.
        /// </summary>
        /// <param name="s">The string that should get translated.</param>
        /// <returns>A translated version of the given string.</returns>
        public static string TranslateSemester(string s)
        {
            if (s == null)
            {
                return null;
            }
            if (s.Contains("Wintersemester") || s.Contains("Winter Semester"))
            {
                return s.Replace("Wintersemester", GetLocalizedString("TuitionFeeControlWinterTerm_Text"))
                    .Replace("Winter Semester", GetLocalizedString("TuitionFeeControlWinterTerm_Text"));
            }
            else
            {
                return s.Replace("Sommersemester", GetLocalizedString("TuitionFeeControlSummerTerm_Text"))
                    .Replace("Sommer Semester", GetLocalizedString("TuitionFeeControlSummerTerm_Text"))
                    .Replace("Summer Semester", GetLocalizedString("TuitionFeeControlSummerTerm_Text"));
            }
        }

        /// <summary>
        /// Translates the given dish type.
        /// </summary>
        /// <param name="dishType">The dish type you want to translate.</param>
        /// <returns>A translated version of the given dish type.</returns>
        public static string TranslateDishType(string dishType)
        {
            switch (dishType)
            {
                case "Tagesgericht":
                    return GetLocalizedString("CanteenDishOfTheDay_Text");
                case "Aktionsessen":
                    return GetLocalizedString("CanteenActionDishes_Text");
                case "Biogericht":
                    return GetLocalizedString("CanteenBioDish_Text");
                case "StuBistro Gericht":
                    return GetLocalizedString("CanteenStuBistroDishes_Text");
                case "Baustellenteller":
                    return GetLocalizedString("CanteenBaustellenteller_Text");
                case "Fast Lane":
                    return GetLocalizedString("CanteenFastLane_Text");
                case "Mensa Klassiker":
                    return GetLocalizedString("CanteenCanteenClassics_Text");
                case "Mensa Spezial":
                    return GetLocalizedString("CanteenCanteenSpecial_Text");
                case "Self-Service Grüne Mensa":
                    return GetLocalizedString("CanteenSelf-ServiceGreenCanteen_Text");
                case "Self-Service Arcisstraße":
                    return GetLocalizedString("CanteenSelf-ServiceArcisstraße_Text");
                case "Self-Service":
                    return GetLocalizedString("CanteenSelf-Service_Text");
                case "Aktion":
                    return GetLocalizedString("CanteenSpecialDishes_Text");
                case "Beilagen":
                    return GetLocalizedString("CanteenSideDishes_Text");
                case "Tagesdessert":
                    return GetLocalizedString("CanteenDessertOfTheDay_Text");
                case "Dessert":
                    return GetLocalizedString("CanteenDessert_Text");
                default:
                    return dishType;
            }
        }

        /// <summary>
        /// Source: https://social.msdn.microsoft.com/Forums/sqlserver/en-US/0cc87160-5b0c-4fc1-b685-ff50117984f7/uwp-access-control-on-parent-page-through-frame-object?forum=wpdevelop
        /// </summary>
        public static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        /// <summary>
        /// Launches the default application for the given Uri.
        /// </summary>
        /// <param name="url">The Uri that defines the application that should get launched.</param>
        /// <returns>Returns true on success.</returns>
        public static async Task<bool> LaunchUriAsync(Uri url)
        {
            return await Windows.System.Launcher.LaunchUriAsync(url);
        }

        public static async Task<ContentDialogResult> ShowDialogAsyncQueue(ContentDialog dialog)
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
        public static bool IsRunningOnMobileDevice()
        {
            return AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Mobile");
        }

        /// <summary>
        /// Hides the StatusBar on Windows Mobile devices asynchronously.
        /// </summary>
        public static async Task HideStatusBarAsync()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }
        }

        /// <summary>
        /// Shows the StatusBar on Windows Mobile devices asynchronously.
        /// </summary>
        public static async Task ShowStatusBarAsync()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().ShowAsync();
            }
        }

        /// <summary>
        /// Manages the Windows Mobile StatusBar asynchronously.
        /// </summary>
        public static async Task OnPageNavigatedFromAsync()
        {
            if (IsRunningOnMobileDevice())
            {
                await ShowStatusBarAsync();
            }
        }

        /// <summary>
        /// Manages the Windows Mobile StatusBar asynchronously.
        /// </summary>
        public static async Task OnPageSizeChangedAsync(SizeChangedEventArgs e)
        {
            if (IsRunningOnMobileDevice())
            {
                if (e.NewSize.Height < e.NewSize.Width)
                {
                    await HideStatusBarAsync();
                }
                else
                {
                    await ShowStatusBarAsync();
                }
            }
        }

        public static bool OnGoBackRequested(Frame frame)
        {
            if (frame is null)
            {
                Logger.Error("Failed to execute back request - frame is null!");
                return false;
            }
            else
            {
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                    return true;
                }
                return false;
            }
        }

        public static void RemoveLastBackStackEntry()
        {
            if (!(mainFrame is null) && mainFrame.BackStackDepth > 0)
            {
                mainFrame.BackStack.RemoveAt(mainFrame.BackStack.Count - 1);
            }
        }

        public static bool NavigateToPage(Type pageType)
        {
            return NavigateToPage(pageType, null);
        }

        public static bool NavigateToPage(Type pageType, object parameter)
        {
            if (pageType is null)
            {
                Logger.Error("Failed to navigate to given page type - type is null!");
                return false;
            }
            if (!(mainFrame is null))
            {
                if (mainFrame.Content is null || mainFrame.Content.GetType() != pageType)
                {
                    return mainFrame.Navigate(pageType, parameter);
                }
                else
                {
                    Logger.Warn("No need to navigate to page " + pageType.ToString() + " - already on it.");
                    return false;
                }
            }
            else
            {
                Logger.Error("Failed to navigate to " + pageType.ToString() + " - mainFrame is null!");
                return false;
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
