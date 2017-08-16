using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace TUMCampusAppAPI
{
    public class Util
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
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
            try
            {
                return Windows.Storage.ApplicationData.Current.LocalSettings.Values[token];
            }
            catch
            {
                return null;
            }
        }

        public static bool getSettingBoolean(string token)
        {
            object obj = getSetting(token);
            return obj != null && obj is bool && (bool)obj;
        }

        public static string getSettingString(string token)
        {
            object obj = getSetting(token);
            return obj == null ? null : (string)obj;
        }

        public static byte getSettingByte(string token)
        {
            object obj = getSetting(token);
            return obj == null ? (byte)0 : (byte)obj;
        }

        public static int getSettingInt(string token)
        {
            object obj = getSetting(token);
            return obj == null ? -1 : (int)obj;
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
