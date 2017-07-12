using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.Syncs;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Pages
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
        /// <summary>
        /// Downloads and shows the current tution fee status.
        /// This method should only be called in a seperate task.
        /// </summary>
        /// <param name="forceRedownload">Whether the cache should get ignored.</param>
        private async Task downloadAndShowFeesAsync(bool forceRedownload)
        {
            try
            {
                await TuitionFeeManager.INSTANCE.downloadFeesAsync(forceRedownload);
            }
            catch (BaseTUMOnlineException e)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                }).AsTask().Wait();
                return;
            }
            List<TUMTuitionFee> list = new List<TUMTuitionFee>();
            list = TuitionFeeManager.INSTANCE.getFees();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                showFees(list);
            }).AsTask().Wait();
        }

        /// <summary>
        /// Shows the no access grid based on the given exception.
        /// </summary>
        /// <param name="e">The cought exception.</param>
        private void showNoAccess(BaseTUMOnlineException e)
        {
            noFees_grid.Visibility = Visibility.Collapsed;
            fees_grid.Visibility = Visibility.Collapsed;
            noData_grid.Visibility = Visibility.Visible;

            if (e is InvalidTokenTUMOnlineException)
            {
                noDataInfo_tbx.Text = "You didn't give the token the required rights for accessing your TUM calendar.";
            }
            else if (e is NoAccessTUMOnlineException)
            {
                noDataInfo_tbx.Text = "Your token is either unknown or not activated yet.";
            }
            else
            {
                noDataInfo_tbx.Text = "An unknown error occured. Please try again.\n\n" + e.ToString();
            }
            progressBar.Visibility = Visibility.Collapsed;
            refresh_pTRV.IsEnabled = true;
        }

        /// <summary>
        /// Shows the given tution fees on the screen.
        /// </summary>
        /// <param name="list">The tution fees that should get shown on the screen.</param>
        private void showFees(List<TUMTuitionFee> list)
        {
            noData_grid.Visibility = Visibility.Collapsed;
            if (list == null || list.Count <= 0 || list[0].money == null || double.Parse(list[0].money) <= 0)
            {
                SyncResult syncResult = TuitionFeeManager.INSTANCE.getSyncStatus();
                if (syncResult.STATUS < 0 && syncResult.ERROR_MESSAGE != null)
                {
                    noDataInfo_tbx.Text = syncResult.ERROR_MESSAGE;
                    noFees_grid.Visibility = Visibility.Collapsed;
                    noData_grid.Visibility = Visibility.Visible;
                }
                else
                {
                    noFees_grid.Visibility = Visibility.Visible;
                    noData_grid.Visibility = Visibility.Collapsed;
                }
                fees_grid.Visibility = Visibility.Collapsed;
            }
            else
            {
                fees_grid.Visibility = Visibility.Visible;
                noFees_grid.Visibility = Visibility.Collapsed;
                outsBalance_tbx.Text = list[0].money + "€";
                semester_tbx.Text = list[0].semesterDescripion;
                DateTime deadLine = DateTime.Parse(list[0].deadline);
                TimeSpan tS = deadLine.Subtract(DateTime.Now);
                deadline_tbx.Text = deadLine.Day + "." + deadLine.Month + "." + deadLine.Year + " ==> " + Math.Round(tS.TotalDays) + " Days left!";
                if (tS.TotalDays <= 30)
                {
                    main_grid.Background = new SolidColorBrush(Windows.UI.Colors.DarkRed);
                }
            }
            progressBar.Visibility = Visibility.Collapsed;
            refresh_pTRV.IsEnabled = true;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void HyperlinkButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Util.launchBrowser(new Uri(@"https://www.tum.de/en/studies/advising/student-financial-aid/"));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            refresh_pTRV.IsEnabled = false;
            Task.Factory.StartNew(() => downloadAndShowFeesAsync(false));
        }

        private void refresh_pTRV_RefreshRequested(object sender, EventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            refresh_pTRV.IsEnabled = false;
            Task.Factory.StartNew(() => downloadAndShowFeesAsync(true));
        }
        #endregion
    }
}
