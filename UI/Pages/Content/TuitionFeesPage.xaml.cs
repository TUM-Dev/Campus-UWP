using System;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages.Content;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace UI.Pages.Content
{
    public sealed partial class TuitionFeesPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly TuitionFeesPageContext VIEW_MODEL = new TuitionFeesPageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TuitionFeesPage()
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
        private void OnRefreshClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh();
        }

        private async void OnStudentFinancialAidLinkClicked(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            await UiUtils.LaunchUriAsync(new Uri(@"https://www.tum.de/en/studies/advising/student-financial-aid/"));
        }

        #endregion
    }
}
