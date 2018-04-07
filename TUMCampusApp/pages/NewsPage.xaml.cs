using Data_Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Controls;
using TUMCampusApp.DataTemplates;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Pages
{
    public sealed partial class NewsPage : Page, INamedPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool reloadingNews;
        private bool reloadingNewsSources;
        private bool newsSoucesChanged;
        private readonly ObservableCollection<NewsTemplate> newsList;
        private int mostCurrentNewsIndex;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 08/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsPage()
        {
            this.newsSoucesChanged = false;
            this.reloadingNews = false;
            this.reloadingNewsSources = false;
            this.newsList = new ObservableCollection<NewsTemplate>();
            this.mostCurrentNewsIndex = -1;
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UIUtils.getLocalizedString("NewsPageName_Text");
        }

        public void setNewsSourcesChanged()
        {
            newsSoucesChanged = true;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void reloadNews()
        {
            showNews(false);
        }

        public void removeNews(NewsTemplate newsTemplate)
        {
            newsList.Remove(newsTemplate);
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Adds all news to the news_stckp.
        /// </summary>
        /// <param name="forceReload">Whether to force reload all news from the server.</param>
        private void showNews(bool forceReload)
        {
            disableUi();
            Task.Run(async () =>
            {
                reloadingNews = true;
                Task t = NewsManager.INSTANCE.downloadNews(forceReload);
                if (t != null)
                {
                    await t;
                }
                List<NewsTable> news = NewsManager.INSTANCE.getAllNewsFormDb();

                if (Settings.getSettingBoolean(SettingsConsts.NEWS_PAGE_HIDE_READ))
                {
                    news.RemoveAll((n) => n.read);
                }

                if (Settings.getSettingBoolean(SettingsConsts.NEWS_PAGE_HIDE_UNREAD))
                {
                    news.RemoveAll((n) => !n.read);
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    newsList.Clear();

                    // Showing only the first 50 news
                    int l = news.Count > 50 ? 50 : news.Count;
                    if (l <= 0)
                    {
                        noNews_grid.Visibility = Visibility.Visible;
                        navigate_grid.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        noNews_grid.Visibility = Visibility.Collapsed;
                        navigate_grid.Visibility = Visibility.Visible;

                        double dateDiff = -1;
                        double temp;
                        for (int i = 0; i < l; i++)
                        {

                            NewsTemplate nT = new NewsTemplate()
                            {
                                news = news[i],
                                parentPage = this
                            };
                            newsList.Add(nT);
                            temp = (news[i].created - DateTime.Now).Duration().TotalHours;
                            if (mostCurrentNewsIndex < 0 || temp < dateDiff)
                            {
                                dateDiff = temp;
                                mostCurrentNewsIndex = i;
                            }
                        }
                        if (mostCurrentNewsIndex > 0)
                        {
                            refresh_pTRV.UpdateLayout();
                        }
                    }

                    reloadingNews = false;
                    enableUi();
                });
            });
        }

        /// <summary>
        /// Adds all news sources to the newsSources_stckp.
        /// </summary>
        /// <param name="forceReload">Whether to force reload all news sources from the server.</param>
        private void showNewsSources(bool forceReload)
        {
            disableUi();
            Task.Run(async () =>
            {
                reloadingNewsSources = true;
                Task t1 = NewsManager.INSTANCE.downloadNewsSources(forceReload);
                if (t1 != null)
                {
                    await t1;
                }
                List<NewsSourceTable> sources = NewsManager.INSTANCE.getAllNewsSourcesFormDb();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    newsSources_stckp.Children.Clear();
                    foreach (NewsSourceTable source in sources)
                    {
                        newsSources_stckp.Children.Add(new NewsSourceControl(source, this)
                        {
                            Margin = new Thickness(10, 0, 10, 10)
                        });
                    }
                    reloadingNewsSources = false;
                    enableUi();
                });
            });
        }

        /// <summary>
        /// Collapses news sources.
        /// </summary>
        private void collapsenewsSources()
        {
            expand_btn.Content = "\xE019";
            newsSources_scv.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Expands news sources.
        /// </summary>
        private void expandnewsSources()
        {
            expand_btn.Content = "\xE018";
            newsSources_scv.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Disables the UI and shows the progress bar.
        /// </summary>
        private void disableUi()
        {
            newsSources_scv.IsEnabled = false;
            refresh_pTRV.IsEnabled = false;
            scrollUp_btn.IsEnabled = false;
            goToToday_btn.IsEnabled = false;
            more_btn.IsEnabled = false;
            hideRead_tglmfo.IsEnabled = false;
            hideUnread_tglmfo.IsEnabled = false;
            more_mfo.Hide();
            progressBar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Enables the UI and hides the progress bar.
        /// </summary>
        private void enableUi()
        {
            if (!reloadingNews && !reloadingNewsSources)
            {
                newsSources_scv.IsEnabled = true;
                refresh_pTRV.IsEnabled = true;
                scrollUp_btn.IsEnabled = true;
                goToToday_btn.IsEnabled = true;
                more_btn.IsEnabled = true;
                hideRead_tglmfo.IsEnabled = true;
                hideUnread_tglmfo.IsEnabled = true;
                progressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void setupMFO()
        {
            hideRead_tglmfo.IsChecked = Settings.getSettingBoolean(SettingsConsts.NEWS_PAGE_HIDE_READ);
            hideUnread_tglmfo.IsChecked = Settings.getSettingBoolean(SettingsConsts.NEWS_PAGE_HIDE_UNREAD);
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void expand_btn_Click(object sender, RoutedEventArgs e)
        {
            if (newsSources_scv.Visibility == Visibility.Visible)
            {
                collapsenewsSources();
                if (newsSoucesChanged)
                {
                    newsSoucesChanged = false;
                    reloadNews();
                }
            }
            else
            {
                expandnewsSources();
                newsSoucesChanged = false;
            }
        }

        private void refresh_pTRV_RefreshRequested(object sender, EventArgs e)
        {
            showNewsSources(true);
            showNews(true);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            setupMFO();
            showNewsSources(false);
            showNews(false);
        }

        private void scrollUp_btn_Click(object sender, RoutedEventArgs e)
        {
            if (newsList.Count > 0)
            {
                refresh_pTRV.ScrollIntoView(newsList[0]);
            }
        }

        private void goToToday_btn_Click(object sender, RoutedEventArgs e)
        {
            if (mostCurrentNewsIndex > 0)
            {
                refresh_pTRV.ScrollIntoView(newsList[mostCurrentNewsIndex]);
            }
        }

        private void hideRead_tglmfo_Click(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.NEWS_PAGE_HIDE_READ, hideRead_tglmfo.IsChecked);
            showNews(false);
        }

        private void hideUnread_tglmfo_Click(object sender, RoutedEventArgs e)
        {
            Settings.setSetting(SettingsConsts.NEWS_PAGE_HIDE_UNREAD, hideUnread_tglmfo.IsChecked);
            showNews(false);
        }

        #endregion
    }
}
