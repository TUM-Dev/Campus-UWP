﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Logging.Classes;
using Microsoft.Toolkit.Uwp.Helpers;
using Shared.Classes;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UI_Context.Classes
{
    public static class UiUtils
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static TaskCompletionSource<ContentDialog> contentDialogShowRequest;
        private static readonly Regex HEX_COLOR_REGEX = new Regex("^#[0-9a-fA-F]{6}$");
        public static readonly char[] TRIM_CHARS = { ' ', '\t', '\n', '\r' };
        private static readonly Random RANDOM = new Random();

        public static bool IsWindowActivated
        {
            get;
            private set;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
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
            if (Window.Current.Content is Frame frame)
            {
                RemoveLastBackStackEntry(frame);
            }
        }

        public static void RemoveLastBackStackEntry(Frame frame)
        {
            if (frame.BackStackDepth > 0)
            {
                frame.BackStack.RemoveAt(frame.BackStack.Count - 1);
            }
        }

        public static bool NavigateToPage(Type pageType)
        {
            if (Window.Current.Content is Frame frame)
            {
                return NavigateToPage(pageType, frame);
            }
            else
            {
                Logger.Error("Failed to navigate to " + pageType.ToString() + " - Window.Current.Content is not of type Frame!");
                return false;
            }
        }

        public static bool NavigateToPage(Type pageType, Frame frame)
        {
            return NavigateToPage(pageType, null, frame);
        }

        public static bool NavigateToPage(Type pageType, object parameter)
        {
            if (Window.Current.Content is Frame frame)
            {
                return NavigateToPage(pageType, parameter, frame);
            }
            else
            {
                Logger.Error("Failed to navigate to " + pageType.ToString() + " - Window.Current.Content is not of type Frame!");
                return false;
            }
        }

        public static bool NavigateToPage(Type pageType, object parameter, Frame frame)
        {
            if (pageType is null)
            {
                Logger.Error("Failed to navigate to given page type - type is null!");
                return false;
            }
            if (frame.Content is null || frame.Content.GetType() != pageType)
            {
                return frame.Navigate(pageType, parameter);
            }
            else
            {
                Logger.Warn("No need to navigate to page " + pageType.ToString() + " - already on it.");
                return false;
            }
        }

        public static bool NavigateToType(Type pageType, object parameter, Frame frame, FrameNavigationOptions navigationOptions)
        {
            if (pageType is null)
            {
                Logger.Error("Failed to navigate to given page type - type is null!");
                return false;
            }
            if (frame.Content is null || frame.Content.GetType() != pageType)
            {
                return frame.NavigateToType(pageType, parameter, navigationOptions);
            }
            else
            {
                Logger.Warn("No need to navigate to page " + pageType.ToString() + " - already on it.");
                return false;
            }
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

        public static bool IsHexColor(string color)
        {
            return color != null && HEX_COLOR_REGEX.Match(color).Success;
        }

        public static void SetClipboardText(string text)
        {
            DataPackage package = new DataPackage();
            package.SetText(text ?? "");
            Clipboard.SetContent(package);
        }

        public static CoreVirtualKeyStates GetVirtualKeyState(VirtualKey key)
        {
            return CoreWindow.GetForCurrentThread().GetKeyState(key);
        }

        public static bool IsVirtualKeyDown(VirtualKey key)
        {
            return GetVirtualKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Shows the given content dialog on the screen, if no other dialog is shown right now.
        /// If there is an other one being shown, the dialog will be shown afterwards.
        /// </summary>
        /// <param name="dialog">The dialog that should get shown.</param>
        public static async Task<ContentDialogResult> ShowDialogAsync(ContentDialog dialog)
        {
            // Make sure it gets invoked by the UI thread:
            if (!Window.Current.Dispatcher.HasThreadAccess)
            {
                throw new InvalidOperationException("This method can only be invoked from UI thread.");
            }

            while (!(contentDialogShowRequest is null))
            {
                await contentDialogShowRequest.Task;
            }

            contentDialogShowRequest = new TaskCompletionSource<ContentDialog>();
            ContentDialogResult result = await dialog.ShowAsync();
            contentDialogShowRequest.SetResult(dialog);
            contentDialogShowRequest = null;

            return result;
        }

        /// <summary>
        /// Launches the default application for the given Uri.
        /// </summary>
        /// <param name="url">The Uri that defines the application that should get launched.</param>
        /// <returns>Returns true on success.</returns>
        public static async Task<bool> LaunchUriAsync(Uri url)
        {
            return await Launcher.LaunchUriAsync(url);
        }

        public static void SetupWindow(Application application)
        {
            // PC, Mobile:
            if (IsApplicationViewApiAvailable())
            {
                ApplicationView appView = ApplicationView.GetForCurrentView();

                // Dye title:
                Brush windowBrush = ThemeUtils.GetThemeResource<Brush>("AppBackgroundAcrylicWindowBrush", application.Resources);
                if (windowBrush is Microsoft.UI.Xaml.Media.AcrylicBrush acrylicWindowBrush)
                {
                    appView.TitleBar.BackgroundColor = acrylicWindowBrush.TintColor;
                }
                else
                {
                    appView.TitleBar.BackgroundColor = ((SolidColorBrush)windowBrush).Color;
                }

                //Dye title bar buttons:
                appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonInactiveForegroundColor = HexStringToColor("#19FFFFFF");
                appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonForegroundColor = HexStringToColor("#FFFFFFFF");
                appView.TitleBar.ButtonHoverBackgroundColor = HexStringToColor("#FF1A1A1A");
                appView.TitleBar.ButtonHoverForegroundColor = HexStringToColor("#FFFFFFFF");

                // Extend window:
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            }

            // Mobile:
            if (IsStatusBarApiAvailable())
            {

                StatusBar statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundColor = ThemeUtils.GetThemeResource<SolidColorBrush>("AppBackgroundAcrylicElementBrush", application.Resources).Color;
                    statusBar.BackgroundOpacity = 1.0d;
                }
            }
        }

        /// <summary>
        /// Returns true in case the operating system supports the Mica effect.
        /// This is true for Windows 11 (>= 10.0.22000.0) and if the device is no Xbox or HoloLense.
        /// <para/>
        /// See: https://docs.microsoft.com/en-us/windows/apps/design/style/mica
        /// </summary>
        public static bool IsMicaSupported()
        {
            OSVersion version = SystemInformation.Instance.OperatingSystemVersion;
            bool validVersion = version.Major >= 10 && version.Minor >= 0 && version.Build >= 22000;
            DeviceFamilyType deviceFamilyType = DeviceFamilyHelper.GetDeviceFamilyType();
            bool validDeviceFamily = deviceFamilyType != DeviceFamilyType.Mobile && deviceFamilyType != DeviceFamilyType.HoloLens && deviceFamilyType != DeviceFamilyType.Xbox;
            return validVersion && validDeviceFamily;
        }

        /// <summary>
        /// Applies the Mica background effect in case it's available. Else it will fall back to the "AppBackgroundAcrylicWindowBrush" brush.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> the background should be set.</param>
        public static void ApplyBackground(Control control)
        {
            if (IsMicaSupported())
            {
                Microsoft.UI.Xaml.Controls.BackdropMaterial.SetApplyToRootOrPageBackground(control, true);
            }
            else
            {
                control.Background = ThemeUtils.GetThemeResource<Brush>("AppBackgroundAcrylicWindowBrush");
            }
        }

        /// <summary>
        /// Sets up the listener for the current window activated state.
        /// </summary>
        public static void SetupWindowActivatedListener()
        {
            IsWindowActivated = true;
            Window.Current.Activated -= Current_Activated;
            Window.Current.Activated += Current_Activated;
        }

        /// <summary>
        /// Converts the given Color to a hex string e.g. #012345.
        /// </summary>
        /// <param name="c">The Color that should get converted.</param>
        /// <returns>A hex string representing the given Color object.</returns>
        public static string ColorToHexString(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        /// <summary>
        /// Converts the given hex string into a Color object.
        /// </summary>
        /// <param name="hexString">The hex color string e.g. #012345.</param>
        /// <returns>Returns the Color object representing the given hex color.</returns>
        public static Color HexStringToColor(string hexString)
        {
            hexString = hexString.Replace("#", string.Empty);
            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;
            if (hexString.Length > 6)
            {
                a = (byte)Convert.ToUInt32(hexString.Substring(0, 2), 16);
                r = (byte)Convert.ToUInt32(hexString.Substring(2, 2), 16);
                g = (byte)Convert.ToUInt32(hexString.Substring(4, 2), 16);
                b = (byte)Convert.ToUInt32(hexString.Substring(6, 2), 16);
            }
            else
            {
                r = (byte)Convert.ToUInt32(hexString.Substring(0, 2), 16);
                g = (byte)Convert.ToUInt32(hexString.Substring(2, 2), 16);
                b = (byte)Convert.ToUInt32(hexString.Substring(4, 2), 16);
            }

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Converts the given hex string into a SolidColorBrush object.
        /// </summary>
        /// <param name="hexString">The hex color string e.g. #012345.</param>
        /// <returns>Returns the SolidColorBrush object representing the given hex color.</returns>
        public static SolidColorBrush HexStringToBrush(string hexString)
        {
            return new SolidColorBrush(HexStringToColor(hexString));
        }

        /// <summary>
        /// Generates a random bare JID.
        /// e.g. 'chat.shakespeare.lit'
        /// </summary>
        /// <returns>A random bare JID string.</returns>
        public static string GenRandomBareJid()
        {
            StringBuilder sb = new StringBuilder(GenRandomString(RANDOM.Next(4, 10)));
            sb.Append('@');
            sb.Append(GenRandomString(RANDOM.Next(4, 6)));
            sb.Append('.');
            sb.Append(GenRandomString(RANDOM.Next(2, 4)));
            return sb.ToString();
        }

        /// <summary>
        /// Returns a random RGB color as hex string in HTML notation.
        /// e.g. #FFFFFF
        /// </summary>
        public static string GenRandomHexColor()
        {
            return string.Format("#{0:X6}", RANDOM.Next(0x1000000));
        }

        /// <summary>
        /// Generates a random string and returns it.
        /// Based on: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        /// </summary>
        /// <param name="length">The length of the string that should be generated.</param>
        /// <returns>A random string.</returns>
        private static string GenRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[RANDOM.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Hides the StatusBar on Windows Mobile devices.
        /// </summary>
        public static async Task HideStatusBarAsync()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }
        }

        /// <summary>
        /// Shows the StatusBar on Windows Mobile devices.
        /// </summary>
        public static async Task ShowStatusBarAsync()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().ShowAsync();
            }
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            IsWindowActivated = e.WindowActivationState != CoreWindowActivationState.Deactivated;
        }

        #endregion
    }
}
