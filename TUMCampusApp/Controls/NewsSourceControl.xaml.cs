using TUMCampusApp.Pages;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class NewsSourceControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private NewsSourceTable source;
        private NewsPage newsPage;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 08/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsSourceControl(NewsSourceTable source, NewsPage newsPage)
        {
            this.source = source;
            this.newsPage = newsPage;
            this.InitializeComponent();
            showNewsSource();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void showNewsSource()
        {
            title_tb.Text = source.title;
            enabled_chbx.IsChecked = source.enabled;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void enabled_chbx_Checked_Changed(object sender, RoutedEventArgs e)
        {
            NewsManager.INSTANCE.updateNewsSourceStatus(source.id, (bool)enabled_chbx.IsChecked);
            newsPage.setNewsSourcesChanged();
        }

        #endregion
    }
}
