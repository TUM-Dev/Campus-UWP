using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
            Task.Factory.StartNew(() => showNews());
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void showNews()
        {
            if (news.image != null)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    image_img.Source = await NewsManager.INSTANCE.downloadNewsImage(news.image.Substring(1, news.image.Length - 2));
                }).AsTask();
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                title_tbx.Text = news.title;
                text_tbx.Text = news.image == null ? "null" : news.image;
                link_hl.Content = news.link;
            }).AsTask();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
