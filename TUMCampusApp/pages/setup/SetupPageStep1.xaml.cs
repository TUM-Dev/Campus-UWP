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
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 24/12/2016  Created [Fabian Sauter]
        /// </history>
        public SetupPageStep1()
        {
            this.InitializeComponent();
            string uId = Util.getSettingString(Const.USER_ID);
            studentID_tbx.Text = uId == null ? "" : uId;
            populateFacultiesComboBox();
            faculty_cbox.SelectedIndex = Util.getSettingInt(Const.FACULTY_INDEX);
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

        /// <summary>
        /// Adds all faculties to the faculty_cbox.
        /// </summary>
        private void populateFacultiesComboBox()
        {
            foreach (Faculties f in Enum.GetValues(typeof(Faculties))) {
                faculty_cbox.Items.Add(new ComboBoxItem()
                {
                    Content = Utillities.getLocalizedString(f.ToString() + "_Text"),
                    
                });
            }
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
            next_btn.IsEnabled = false;
            if (!isIdValid())
            {
                MessageDialog message = new MessageDialog(Utillities.getLocalizedString("InvalidId_Text"))
                {
                    Title = Utillities.getLocalizedString("Error_Text")
                };
                await message.ShowAsync();
            }
            else if(faculty_cbox.SelectedIndex < 0)
            {
                MessageDialog message = new MessageDialog(Utillities.getLocalizedString("SelectFaculty_Text"))
                {
                    Title = Utillities.getLocalizedString("Error_Text")
                };
                await message.ShowAsync();
            }
            else
            {
                if(tumOnlineToken_tbx.Visibility == Visibility.Collapsed)
                {
                    string result = await TumManager.INSTANCE.reqestNewTokenAsync(studentID_tbx.Text.ToLower());
                    if (result == null)
                    {
                        MessageDialog message = new MessageDialog(Utillities.getLocalizedString("RequestNewTokenError_Text"))
                        {
                            Title = Utillities.getLocalizedString("Error_Text")
                        };
                        await message.ShowAsync();
                    }
                    else if (result.Contains("Es wurde kein Benutzer zu diesen Benutzerdaten gefunden"))
                    {
                        MessageDialog message = new MessageDialog(Utillities.getLocalizedString("InvalidId_Text"))
                        {
                            Title = Utillities.getLocalizedString("Error_Text")
                        };
                        await message.ShowAsync();
                    }
                    else
                    {
                        Util.setSetting(Const.FACULTY_INDEX, faculty_cbox.SelectedIndex);
                        Util.setSetting(Const.USER_ID, studentID_tbx.Text.ToLower());
                        (Window.Current.Content as Frame).Navigate(typeof(SetupPageStep2));
                    }
                }
                else
                {
                    string token = tumOnlineToken_tbx.Text.ToUpper();
                    if (!TumManager.INSTANCE.isTokenValid(token))
                    {
                        MessageDialog message = new MessageDialog(Utillities.getLocalizedString("InvalidToken_Text"))
                        {
                            Title = Utillities.getLocalizedString("Error_Text")
                        };
                        await message.ShowAsync();
                    }
                    else
                    {
                        Util.setSetting(Const.FACULTY_INDEX, faculty_cbox.SelectedIndex);
                        Util.setSetting(Const.USER_ID, studentID_tbx.Text.ToLower());
                        TumManager.INSTANCE.saveToken(token);
                        (Window.Current.Content as Frame).Navigate(typeof(SetupPageStep2));
                    }
                }
            }
            next_btn.IsEnabled = true;
        }

        private void useExistingToken_btn_Click(object sender, RoutedEventArgs e)
        {
            if(tumOnlineToken_tbx.Visibility == Visibility.Collapsed)
            {
                tumOnlineToken_tbx.Visibility = Visibility.Visible;
                useExistingToken_btn.Content = Utillities.getLocalizedString("SetupPage1DontUseExistingToken");
            }
            else
            {
                tumOnlineToken_tbx.Visibility = Visibility.Collapsed;
                useExistingToken_btn.Content = Utillities.getLocalizedString("SetupPage1UseExistingToken");
            }
        }

        #endregion
    }
}
