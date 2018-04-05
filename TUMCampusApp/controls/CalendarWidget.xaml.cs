using System;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class CalendarWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private WidgetControl widgetControl;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 29/01/2017 Created [Fabian Sauter]
        /// </history>
        public CalendarWidget(WidgetControl widgetControl)
        {
            this.widgetControl = widgetControl;
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
        /// Adds a custom separator.
        /// </summary>
        /// <param name="date"></param>
        private void addSeperator(DateTime date)
        {
            nextDate_tbx.Text = date.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Shows the current TUMOnlineCalendarEntry on the control.
        /// </summary>
        private void ShowCalendarEntry()
        {
            CalendarManager.INSTANCE.syncCalendar(false);
            TUMOnlineCalendarTable entry = CalendarManager.INSTANCE.getNextEntry();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                showCalendarEntry(entry);
            }).AsTask();
        }

        private void showCalendarEntry(TUMOnlineCalendarTable entry)
        {
            if(entry == null)
            {
                widgetControl.Visibility = Visibility.Collapsed;
                return;
            }
            addSeperator(entry.dTStrat);
            if (entry == null)
            {
                widgetControl.Visibility = Visibility.Collapsed;
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
