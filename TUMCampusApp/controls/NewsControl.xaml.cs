using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusAppAPI;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.News;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Controls
{
    public sealed partial class NewsControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private News news;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 03/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsControl(News news)
        {
            this.news = news;
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
        private async Task showNewsAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                loading_ring.IsActive = true;
            }).AsTask();

            if (news.image != null)
            {
                BitmapImage image = null;
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    image = await NewsManager.INSTANCE.downloadNewsImage(news.image.Substring(1, news.image.Length - 2));
                }).AsTask();

                if (image != null)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        image.ImageOpened += (sender, e) =>
                        {
                            image_img.Width = image.PixelWidth;
                            image_img.Height = image.PixelHeight;
                        };
                        image_img.Source = image;

                    }).AsTask();
                }
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    image_img.Visibility = Visibility.Collapsed;

                }).AsTask();
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                title_tbx.Text = news.title;
                NewsSource source = NewsManager.INSTANCE.getNewsSource(news.src);
                src_tbx.Text = source == null ? news.src : source.title;
                date_tbx.Text = news.date.ToLocalTime().ToString("dd.MM.yyyy");
                loading_ring.IsActive = false;
            }).AsTask();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(async () => await showNewsAsync());
        }

        private async void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(news.link == null || loading_ring.IsActive)
            {
                return;
            }
            await Util.launchBrowser(new Uri(news.link));
        }

        #endregion
    }
}
