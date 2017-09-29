using TUMCampusApp.Classes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TUMCampusApp.Controls
{
    public sealed partial class TutWidget : UserControl
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
        /// 03/01/2017 Created [Fabian Sauter]
        /// </history>
        public TutWidget()
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


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Utillities.mainPage.navigateToPage(Classes.Utillities.EnumPage.SettingsPage);
        }

        #endregion
    }
}
