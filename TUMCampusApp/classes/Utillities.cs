using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace TUMCampusApp.classes
{
    public class Utillities
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public enum EnumPage
        {
            SetupPage,
            HomePage,
            CanteensPage,
            NewsPage,
            MVPage,
            PlansPage,
            RoomfinderPage,
            StudyRoomsPage,
            OpeningHoursPage,
            StudyPlansPage,
            SettingsPage
        }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--


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
            dialog.Title = "Achtung!";
            await dialog.ShowAsync();
        }

        public static DateTime getDate(String s)
        {
            try
            {
                return DateTime.Parse(s);
            }
            catch (FormatException e)
            {
                //log(e, str);
                return new DateTime();
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
