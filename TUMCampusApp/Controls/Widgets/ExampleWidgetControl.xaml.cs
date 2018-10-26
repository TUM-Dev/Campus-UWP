using Data_Manager;
using TUMCampusApp.Classes;
using TUMCampusApp.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TUMCampusApp.Controls.Widgets
{
    public sealed partial class ExampleWidgetControl : UserControl, IHideableWidget
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
        /// 05/04/2018 Created [Fabian Sauter]
        /// </history>
        public ExampleWidgetControl()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getSettingsToken()
        {
            return SettingsConsts.DISABLE_EXAMPLE_WIDGET;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void onHiding()
        {
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UiUtils.mainPage?.navigateToPage(typeof(SettingsPage), null);
        }

        #endregion
    }
}
