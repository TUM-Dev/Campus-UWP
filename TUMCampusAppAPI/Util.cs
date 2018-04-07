using Logging;
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
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


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
        /// Tries to parse a given date string to a DateTime obj.
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
