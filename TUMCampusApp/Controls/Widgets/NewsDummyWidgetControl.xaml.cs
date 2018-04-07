using Data_Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls.Widgets
{
    public sealed partial class NewsDummyWidgetControl : UserControl, IHideableWidget
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public WidgetControl WidgetContainer
        {
            get { return (WidgetControl)GetValue(WidgetContainerProperty); }
            set { SetValue(WidgetContainerProperty, value); }
        }
        public static readonly DependencyProperty WidgetContainerProperty = DependencyProperty.Register("WidgetContainer", typeof(WidgetControl), typeof(NewsDummyWidgetControl), null);

        public HomePage HPage
        {
            get { return (HomePage)GetValue(HPageProperty); }
            set { SetValue(HPageProperty, value); }
        }
        public static readonly DependencyProperty HPageProperty = DependencyProperty.Register("HPage", typeof(HomePage), typeof(NewsDummyWidgetControl), null);

        private readonly List<NewsWidgetControl> NEWS_WIDGETS;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/04/2018 Created [Fabian Sauter]
        /// </history>
        public NewsDummyWidgetControl()
        {
            this.NEWS_WIDGETS = new List<NewsWidgetControl>();
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getSettingsToken()
        {
            return SettingsConsts.DISABLE_NEWS_WIDGET;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void onHiding()
        {
            hideAllNews();
        }

        public void disableNewsWidgets()
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => WidgetContainer?.disableWidget()).AsTask();
        }

        #endregion

        #region --Misc Methods (Private)--
        private void loadNews()
        {
            Task.Run(async () =>
            {
                Task t = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => WidgetContainer?.setIsLoading(true)).AsTask();

                Task t1 = NewsManager.INSTANCE.downloadNewsSources(false);
                Task t2 = NewsManager.INSTANCE.downloadNews(false);
                if (t1 != null)
                {
                    await t1;
                }
                if (t2 != null)
                {
                    await t2;
                }

                List<NewsTable> news = NewsManager.INSTANCE.getNewsForHomePage();

                t = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    hideAllNews();
                    NEWS_WIDGETS.Clear();

                    if (news != null || news.Count > 0)
                    {
                        foreach (NewsTable item in news)
                        {
                            NewsWidgetControl newsWidgetControl = new NewsWidgetControl()
                            {
                                News = item,
                                NewsDummyWidget = this,
                                HPage = HPage
                            };
                            NEWS_WIDGETS.Add(newsWidgetControl);
                            HPage?.addWidget(newsWidgetControl);
                        }
                    }

                    if (WidgetContainer != null)
                    {
                        WidgetContainer.Visibility = Visibility.Collapsed;
                        WidgetContainer.setIsLoading(false);
                    }
                }).AsTask();
            });
        }

        private void hideAllNews()
        {
            for (int i = 0; i < NEWS_WIDGETS.Count; i++)
            {
                NEWS_WIDGETS[i].Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadNews();
        }

        #endregion
    }
}
