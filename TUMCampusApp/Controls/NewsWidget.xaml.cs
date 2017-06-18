using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Pages;
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
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Controls
{
    public sealed partial class NewsWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private DropShadowPanel newsWidget_ds;
        private HomePage homePage;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 17/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsWidget(DropShadowPanel newsWidget_ds, HomePage homePage)
        {
            this.newsWidget_ds = newsWidget_ds;
            this.homePage = homePage;
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
            Task t1 = NewsManager.INSTANCE.downloadNewsSourcesAsync(false);
            Task t2 = NewsManager.INSTANCE.downloadNewsAsync(false);
            Task.WaitAll(t1, t2);

            List<News> news = NewsManager.INSTANCE.getNewsForHomePage();
            if(news == null || news.Count <= 0)
            {
                return;
            }

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (News item in news)
                {
                    homePage.addWidget(new DropShadowPanel()
                    {
                        Style = newsWidget_ds.Style,
                        Content = new NewsControl(item),
                        Visibility = Visibility.Visible
                    });
                }
            }).AsTask().Wait();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
