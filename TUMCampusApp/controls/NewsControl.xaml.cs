using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TUMCampusAppAPI;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.News;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace TUMCampusApp.Controls
{
    public sealed partial class NewsControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public News News
        {
            get { return (News)GetValue(NewsProperty); }
            set
            {
                SetValue(NewsProperty, value);
                showNews();
            }
        }
        public static readonly DependencyProperty NewsProperty = DependencyProperty.Register("NewsProperty", typeof(News), typeof(NewsControl), null);

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
            if(News == null)
            {
                return;
            }

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                loading_ring.IsActive = true;
            }).AsTask();

            if (News.imageUrl != null)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    image_img.Source = News.imageUrl;
                }).AsTask();
            }
            else
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    image_img.Visibility = Visibility.Collapsed;

                }).AsTask();
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
                title_tbx.Text = News.title;
                NewsSource source = NewsManager.INSTANCE.getNewsSource(News.src);
                src_tbx.Text = source.title ?? News.src;
                date_tbx.Text = News.date.ToLocalTime().ToString("dd.MM.yyyy");
                loading_ring.IsActive = false;
            }).AsTask();
        }

        /// <summary>
        /// Forces the imageEx control to reload the image, if imageLoadingFailedCounter < 3.
        /// </summary>
        private async Task reloadImage()
        {
            if (imageLoadingFailedCounter < 3)
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

        private async void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (News.link == null || loading_ring.IsActive)
            {
                return;
            }
            await Util.launchBrowser(new Uri(News.link));
        }

        private async void image_img_ImageExFailed(object sender, Microsoft.Toolkit.Uwp.UI.Controls.ImageExFailedEventArgs e)
        {
            await reloadImage();
        }

        private void image_img_ImageExOpened(object sender, Microsoft.Toolkit.Uwp.UI.Controls.ImageExOpenedEventArgs e)
        {
            imageLoadingFailedCounter = 0;
        }

        #endregion
    }
}
