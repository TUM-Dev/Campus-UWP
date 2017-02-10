using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Controls
{
    public sealed partial class TuitionFeeWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private DropShadowPanel dSP;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 22/01/2017 Created [Fabian Sauter]
        /// </history>
        public TuitionFeeWidget(DropShadowPanel dSP)
        {
            this.dSP = dSP;
            this.InitializeComponent();
            Task.Factory.StartNew(() => ShowTuitionFeesAsync());
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private async Task ShowTuitionFeesAsync()
        {
            try
            {
                await TuitionFeeManager.INSTANCE.downloadFeesAsync(false);
            }
            catch (BaseTUMOnlineException e)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    showFees(null);
                }).AsTask().Wait();
                return;
            }
            List<TUMTuitionFee> list = TuitionFeeManager.INSTANCE.getFees();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                showFees(list);
            }).AsTask().Wait();
        }

        private void showFees(List<TUMTuitionFee> list)
        {
            if (list == null || list.Count <= 0 || list[0].money == null || double.Parse(list[0].money) <= 0)
            {
                dSP.Visibility = Visibility.Collapsed;
            }
            else
            {
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
            progressRing.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
