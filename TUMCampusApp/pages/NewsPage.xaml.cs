using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Controls;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.News;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Pages
{
    public sealed partial class NewsPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool reloadingNews;
        private bool reloadingNewsSources;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 08/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsPage()
        {
            reloadingNews = false;
            reloadingNewsSources = false;
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void reloadNews()
        {
            showNews(false);
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
            Task.Factory.StartNew(() => {
                reloadingNews = true;
                NewsManager.INSTANCE.downloadNewsAsync(forceReload).Wait();
                List<News> news = NewsManager.INSTANCE.getAllNewsFormDb();

                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    news_stckp.Children.Clear();

                    // Showing only the first 50 news
                    int l = news.Count > 50 ? 50 : news.Count;
                    for (int i = 0; i < l; i++)
                    {
                        DropShadowPanel dSP = new DropShadowPanel()
                        {
                            Style = (Style)Resources["ShadowPanelStyle"],
                            Content = new NewsControl(news[i])
                        };
                        if(news[i].date.Date.CompareTo(DateTime.Now) == 0)
                        {
                            news_stckp.Children.Insert(0, dSP);
                        }
                        else
                        {
                            news_stckp.Children.Add(dSP);
                        }
                    }
                    reloadingNews = false;
                    enableUi();
                }).AsTask();
            });
        }

        /// <summary>
        /// Adds all news sources to the newsSources_stckp.
        /// </summary>
        /// <param name="forceReload">Whether to force reload all news sources from the server.</param>
        private void showNewsSources(bool forceReload)
        {
            disableUi();
            Task.Factory.StartNew(() => {
                reloadingNewsSources = true;
                Task t1 = NewsManager.INSTANCE.downloadNewsSourcesAsync(forceReload);
                Task.WaitAll(t1);
                List<NewsSource> sources = NewsManager.INSTANCE.getAllNewsSourcesFormDb();
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    newsSources_stckp.Children.Clear();
                    foreach (NewsSource source in sources)
                    {
                        newsSources_stckp.Children.Add(new NewsSourceControl(source, this)
                        {
                            Margin = new Thickness(10, 0, 10, 10)
                        });
                    }
                    reloadingNewsSources = false;
                    enableUi();
                }).AsTask();
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
            progressBar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Enables the UI and hides the progress bar.
        /// </summary>
        private void enableUi()
        {
            if(!reloadingNews && ! reloadingNewsSources)
            {
                newsSources_scv.IsEnabled = true;
                refresh_pTRV.IsEnabled = true;
                progressBar.Visibility = Visibility.Collapsed;
            }
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
            }
            else
            {
                expandnewsSources();
            }
        }

        private void refresh_pTRV_RefreshRequested(object sender, EventArgs e)
        {
            showNewsSources(true);
            showNews(true);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            showNewsSources(false);
            showNews(false);
        }

        #endregion
    }
}
