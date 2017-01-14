using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.classes;
using TUMCampusApp.classes.managers;
using TUMCampusApp.classes.tum;
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

namespace TUMCampusApp.pages
{
    public sealed partial class TuitionFeesPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/01/2017 Created [Fabian Sauter]
        /// </history>
        public TuitionFeesPage()
        {
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
        private void downloadAndShowFees(bool forceRedownload)
        {
            TuitionFeeManager.INSTANCE.downloadFeesAsync(forceRedownload).Wait();
            List<TUMTuitionFee> list = new List<TUMTuitionFee>();
            list = TuitionFeeManager.INSTANCE.getFees();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                showFees(list);
            }).AsTask().Wait();
        }

        private void showFees(List<TUMTuitionFee> list)
        {
            if (list == null || list.Count <= 0)
            {
                noFees_grid.Visibility = Visibility.Visible;
                fees_grid.Visibility = Visibility.Collapsed;
            }
            else
            {
                fees_grid.Visibility = Visibility.Visible;
                noFees_grid.Visibility = Visibility.Collapsed;
                outsBalance_tbx.Text = list[0].money + "€";
                semester_tbx.Text = list[0].semesterDescripion;
                deadline_tbx.Text = list[0].deadline;
            }
            refresh_btn.IsEnabled = true;
            progressBar.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void HyperlinkButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Utillities.launchBrowser(new Uri(@"https://www.tum.de/en/studies/advising/student-financial-aid/"));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => downloadAndShowFees(false));
        }

        private void refresh_btn_Click(object sender, RoutedEventArgs e)
        {
            refresh_btn.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => downloadAndShowFees(true));
        }

        #endregion
    }
}
