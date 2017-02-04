using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.StartScreen;

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

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Sets a setting with the given token.
        /// </summary>
        /// <param name="token">The token for saving.</param>
        /// <param name="value">The value that should get stored.</param>
        public static void setSetting(string token, object value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[token] = value;
        }

        /// <summary>
        /// Returns the setting behind the given token.
        /// </summary>
        /// <param name="token">The token for the setting.</param>
        /// <returns>Returns the setting behind the given token.</returns>
        public static object getSetting(string token)
        {
            return Windows.Storage.ApplicationData.Current.LocalSettings.Values[token];
        }

        public static bool getSettingBoolean(string token)
        {
            object obj = getSetting(token);
            return obj != null && obj is bool && (bool)obj;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Shows a message box on the screen with the given content
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public static async Task showMessageBoxAsync(string message)
        {
            MessageDialog dialog = new MessageDialog(message);
            dialog.Title = "Attention!";
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Trys to parse a given date string to a DateTime obj.
        /// </summary>
        /// <param name="s">The date string.</param>
        /// <returns>Returns the parsed date.</returns>
        public static DateTime getDate(string s)
        {
            try
            {
                return DateTime.Parse(s);
            }
            catch (FormatException e)
            {
                Logger.Error("Unable to parse date string(" + s + ")", e);
                return new DateTime();
            }
        }

        /// <summary>
        /// Launches the web browser with the given url.
        /// </summary>
        /// <param name="url">The url that the browser should show.</param>
        /// <returns>Returns true on success.</returns>
        public static async Task<bool> launchBrowser(Uri url)
        {
            return await Windows.System.Launcher.LaunchUriAsync(url);
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
