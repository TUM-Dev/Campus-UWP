using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Controls;
using TUMCampusAppAPI;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Pages
{
    public sealed partial class TuitionFeesPage : Page, INamedPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
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
        public string getLocalizedName()
        {
            return UiUtils.GetLocalizedString("TuitionFeesPageName_text");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Downloads and shows the current tuition fee status.
        /// This method should only be called in a separate task.
        /// </summary>
        /// <param name="forceRedownload">Whether the cache should get ignored.</param>
        private async Task downloadAndShowFeesAsync(bool forceRedownload)
        {
            try
            {
                Task t = TuitionFeeDBManager.INSTANCE.downloadFees(forceRedownload);
                if (t != null)
                {
                    await t;
                }
            }
            catch (BaseTUMOnlineException e)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                }).AsTask();
                return;
            }
            List<TUMTuitionFeeTable> list = new List<TUMTuitionFeeTable>();
            list = TuitionFeeDBManager.INSTANCE.getFees();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                showFees(list);
            }).AsTask();
        }

        /// <summary>
        /// Shows the no access grid based on the given exception.
        /// </summary>
        /// <param name="e">The caught exception.</param>
        private void showNoAccess(BaseTUMOnlineException e)
        {
            noFees_grid.Visibility = Visibility.Collapsed;
            noData_grid.Visibility = Visibility.Visible;

            if (e is InvalidTokenTUMOnlineException)
            {
                noDataInfo_tbx.Text = UiUtils.GetLocalizedString("TuitionFeeNoAccess_Text");
            }
            else if (e is NoAccessTUMOnlineException)
            {
                noDataInfo_tbx.Text = UiUtils.GetLocalizedString("TuitionFeeTokenNotActivated_Text");
            }
            else
            {
                noDataInfo_tbx.Text = UiUtils.GetLocalizedString("TuitionFeeNoUnknownError_Text") + "\n\n" + e.ToString();
            }
            progressBar.Visibility = Visibility.Collapsed;
            refresh_pTRV.IsEnabled = true;
        }

        /// <summary>
        /// Shows the given tuition fees on the screen.
        /// </summary>
        /// <param name="list">The tuition fees that should get shown on the screen.</param>
        private void showFees(List<TUMTuitionFeeTable> list)
        {
            noData_grid.Visibility = Visibility.Collapsed;
            tuitionFees_stckp.Children.Clear();

            if (list == null || list.Count <= 0 || list[0].money == null)
            {
                SyncResult syncResult = TuitionFeeDBManager.INSTANCE.getSyncStatus();
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
            }
            else
            {
                noFees_grid.Visibility = Visibility.Collapsed;
                foreach (var item in list)
                {
                    if (item != null && item.money != null)
                    {
                        tuitionFees_stckp.Children.Add(new TuitionFeeControl(item)
                        {
                            Margin = new Thickness(0, 0, 0, 10)
                        });
                    }
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
