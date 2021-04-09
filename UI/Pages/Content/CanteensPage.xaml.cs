using Storage.Classes;
using UI_Context.Classes.Context.Pages.Content;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Pages.Content
{
    public sealed partial class CanteensPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CanteensPageContext VIEW_MODEL = new CanteensPageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CanteensPage()
        {
            InitializeComponent();
            LoadMapApiKey();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void LoadMapApiKey()
        {
            string token = TokenReader.LoadTokenFromFile(TokenReader.MAP_TOKEN_PATH);
            canteens_map.MapServiceToken = token;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void RefreshAll_mfi_Click(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true, true);
        }

        private void RefreshCanteens_mfi_Click(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true, false);
        }

        private void RefreshDishes_mfi_Click(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(false, true);
        }

        #endregion
    }
}
