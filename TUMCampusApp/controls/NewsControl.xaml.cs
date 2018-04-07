using Data_Manager;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Threading.Tasks;
using TUMCampusApp.Controls.Widgets;
using TUMCampusApp.DataTemplates;
using TUMCampusApp.Pages;
using TUMCampusAppAPI;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TUMCampusApp.Controls
{
    public sealed partial class NewsControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public NewsTable News
        {
            get { return (NewsTable)GetValue(NewsProperty); }
            set
            {
                SetValue(NewsProperty, value);
                showNews();
            }
        }
        public static readonly DependencyProperty NewsProperty = DependencyProperty.Register("NewsProperty", typeof(NewsTable), typeof(NewsControl), null);

        public NewsPage ParentPage
        {
            get { return (NewsPage)GetValue(ParentPageProperty); }
            set { SetValue(ParentPageProperty, value); }
        }
        public static readonly DependencyProperty ParentPageProperty = DependencyProperty.Register("ParentPage", typeof(NewsPage), typeof(NewsControl), null);

        public NewsTemplate NewsTemp
        {
            get { return (NewsTemplate)GetValue(NewsTempProperty); }
            set { SetValue(NewsTempProperty, value); }
        }
        public static readonly DependencyProperty NewsTempProperty = DependencyProperty.Register("NewsTemp", typeof(NewsTemplate), typeof(NewsControl), null);

        public NewsWidgetControl NewsWidget
        {
            get { return (NewsWidgetControl)GetValue(NewsWidgetProperty); }
            set { SetValue(NewsWidgetProperty, value); }
        }
        public static readonly DependencyProperty NewsWidgetProperty = DependencyProperty.Register("NewsWidget", typeof(NewsWidgetControl), typeof(NewsControl), null);

        private int imageLoadingFailedCounter;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 03/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsControl()
        {
            this.imageLoadingFailedCounter = 0;
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
        /// <summary>
        /// Shows the current news on the screen.
        /// </summary>
        private void showNews()
        {
            if (News == null)
            {
                return;
            }

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => loading_ldng.IsLoading = true).AsTask();

            if (!string.IsNullOrEmpty(News.imageUrl))
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    image_img.Source = News.imageUrl;
                    image_img.Visibility = Visibility.Visible;
                }).AsTask();
            }
            else
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => image_img.Visibility = Visibility.Collapsed).AsTask();
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (News.src != null && News.src.Equals("2"))
                {
                    logo_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/TU-Film.png"));
                }
                else
                {
                    logo_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/BadgeLogo.scale-200.png"));
                }
                NewsSourceTable source = NewsManager.INSTANCE.getNewsSource(News.src);
                src_tbx.Text = source.title ?? (News.src ?? "");
                date_tbx.Text = News.date.ToString("dd.MM.yyyy");
                loading_ldng.IsLoading = false;
            }).AsTask();
        }

        /// <summary>
        /// Forces the imageEx control to reload the image, if imageLoadingFailedCounter < 3.
        /// </summary>
        private async Task reloadImage()
        {
            if (imageLoadingFailedCounter < 3 && !string.IsNullOrEmpty(News.imageUrl))
            {
                imageLoadingFailedCounter++;
                await ImageCache.Instance.RemoveAsync(new Uri[] { new Uri(News.imageUrl) });
                var source = image_img.Source;
                image_img.Source = null;
                image_img.Source = source;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            showNews();
        }

        private async void main_grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (News == null || News.link == null || loading_ldng.IsLoading)
            {
                return;
            }
            await Util.launchBrowser(new Uri(News.link));
        }

        private async void image_img_ImageExFailed(object sender, ImageExFailedEventArgs e)
        {
            await reloadImage();
        }

        private void image_img_ImageExOpened(object sender, ImageExOpenedEventArgs e)
        {
            imageLoadingFailedCounter = 0;
        }

        private void read_btn_Click(object sender, RoutedEventArgs e)
        {
            if (News != null)
            {
                News.read = !News.read;

                // Update button color:
                NewsControlReadButtonValueConverter converter = (NewsControlReadButtonValueConverter)Resources["newsControlReadButtonValueConverter"];
                read_btn.Foreground = (SolidColorBrush)converter.Convert(News.read, null, null, null);

                string id = News.id;
                bool read = News.read;
                Task.Run(() => NewsManager.INSTANCE.updateNewsRead(id, read));

                if (News.read)
                {
                    if (ParentPage != null && NewsTemp != null && Settings.getSettingBoolean(SettingsConsts.NEWS_PAGE_HIDE_READ))
                    {
                        ParentPage.removeNews(NewsTemp);
                    }
                    if (NewsWidget != null)
                    {
                        NewsWidget.markAsRead();
                    }
                }
                else
                {
                    if (ParentPage != null && NewsTemp != null && Settings.getSettingBoolean(SettingsConsts.NEWS_PAGE_HIDE_UNREAD))
                    {
                        ParentPage.removeNews(NewsTemp);
                    }
                }
            }
        }

        #endregion
    }
}
