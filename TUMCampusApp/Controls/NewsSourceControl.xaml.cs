using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TUMCampusApp.Pages;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.News;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Controls
{
    public sealed partial class NewsSourceControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private NewsSource source;
        private NewsPage newsPage;
        private bool inital_Checked_Changed;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 08/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsSourceControl(NewsSource source, NewsPage newsPage)
        {
            this.source = source;
            this.newsPage = newsPage;
            this.inital_Checked_Changed = false;
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
            if(inital_Checked_Changed)
            {
                NewsManager.INSTANCE.updateNewsSourceStatus(source.id, (bool)enabled_chbx.IsChecked);
                newsPage.reloadNews();
            }
            else
            {
                inital_Checked_Changed = true;
            }
        }

        #endregion
    }
}
