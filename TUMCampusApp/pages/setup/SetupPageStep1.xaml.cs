using System;
using System.Text.RegularExpressions;
using TUMCampusAppAPI.Managers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TUMCampusAppAPI;
using TUMCampusApp.Classes;

namespace TUMCampusApp.Pages.Setup
{
    public sealed partial class SetupPageStep1 : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 24/12/2016  Created [Fabian Sauter]
        /// </history>
        public SetupPageStep1()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Checks wether the current student id is valid.
        /// </summary>
        private bool isIdValid()
        {
            Regex reg = new Regex("[a-z]{2}[0-9]{2}[a-z]{3}");
            return reg.Match(studentID_tbx.Text.ToLower()).Success;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void skip_btn_Click(object sender, RoutedEventArgs e)
        {
            Util.setSetting(Const.TUMO_ENABLED, false);
            Util.setSetting(Const.HIDE_WIZARD_ON_STARTUP, true);
            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
        }

        private async void next_btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (!isIdValid())
            {
                MessageDialog message = new MessageDialog(Utillities.getLocalizedString("InvalidId_Text"));
                message.Title = Utillities.getLocalizedString("Error_Text");
                await message.ShowAsync();
                return;
            }

            string result = await TumManager.INSTANCE.reqestNewTokenAsync(studentID_tbx.Text.ToLower());
            if (result == null)
            {
                MessageDialog message = new MessageDialog(Utillities.getLocalizedString("RequestNewTokenError_Text"));
                message.Title = Utillities.getLocalizedString("Error_Text");
                await message.ShowAsync();
                return;
            }
            if(result.Contains("Es wurde kein Benutzer zu diesen Benutzerdaten gefunden"))
            {
                MessageDialog message = new MessageDialog(Utillities.getLocalizedString("InvalidId_Text"));
                message.Title = Utillities.getLocalizedString("Error_Text");
                await message.ShowAsync();
                return;
            }
            Util.setSetting(Const.USER_ID, studentID_tbx.Text.ToLower());
            (Window.Current.Content as Frame).Navigate(typeof(SetupPageStep2));
        }

        #endregion
    }
}
