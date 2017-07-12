using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class CalendarWidget : UserControl
    {
        private DropShadowPanel calendarWidget_ds;

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
        /// 29/01/2017 Created [Fabian Sauter]
        /// </history>
        public CalendarWidget(DropShadowPanel dSP)
        {
            this.dSP = dSP;
            this.InitializeComponent();
            Task.Factory.StartNew(() => ShowCalendarEntry());
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
        /// Adds a custom seperator.
        /// </summary>
        /// <param name="date"></param>
        private void addSeperator(DateTime date)
        {
            nextDate_tbx.Text = date.Day + "." + date.Month + "." + date.Year;
        }

        /// <summary>
        /// Shows the current TUMOnlineCalendarEntry on the control.
        /// </summary>
        private void ShowCalendarEntry()
        {
            TUMOnlineCalendarEntry entry = CalendarManager.INSTANCE.getNextEntry();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                showCalendarEntry(entry);
            }).AsTask().Wait();
        }

        private void showCalendarEntry(TUMOnlineCalendarEntry entry)
        {
            if(entry == null)
            {
                dSP.Visibility = Visibility.Collapsed;
                return;
            }
            addSeperator(entry.dTStrat);
            if (entry == null)
            {
                dSP.Visibility = Visibility.Collapsed;
            }
            else
            {
                calendarEntries_sckl.Children.Add(new CalendarControl(entry));
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
