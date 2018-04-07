using TUMCampusApp.Classes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TUMCampusApp.Controls
{
    public sealed partial class NoWidgetsControl : UserControl
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
        /// 07/04/2018 Created [Fabian Sauter]
        /// </history>
        public NoWidgetsControl()
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
            UIUtils.mainPage?.navigateToPage(UIUtils.EnumPage.SettingsPage, null);
        }

        #endregion
    }
}
