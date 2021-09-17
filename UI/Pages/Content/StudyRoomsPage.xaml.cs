using UI_Context.Classes.Context.Pages.Content;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Pages.Content
{
    public sealed partial class StudyRoomsPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly StudyRoomsPageContext VIEW_MODEL = new StudyRoomsPageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public StudyRoomsPage()
        {
            InitializeComponent();
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
        private void OnRefreshClicked(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh();
        }

        #endregion
    }
}
