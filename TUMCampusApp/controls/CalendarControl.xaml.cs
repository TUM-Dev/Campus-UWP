using System;
using TUMCampusApp.Classes.Tum;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Controls
{
    public sealed partial class CalendarControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private TUMOnlineCalendarEntry entry;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 22/01/2017 Created [Fabian Sauter]
        /// </history>
        public CalendarControl(TUMOnlineCalendarEntry entry)
        {
            this.entry = entry;
            this.InitializeComponent();
            showCalendar();
            if(entry.dTStrat.CompareTo(DateTime.Now) <= 0)
            {
                if(entry.dTEnd.CompareTo(DateTime.Now) >= 0)
                {
                    calendarEntryName_tbx.Foreground = new SolidColorBrush(Colors.DarkOrange);
                }
                else
                {
                    calendarEntryName_tbx.Foreground = new SolidColorBrush(Colors.DarkGray);
                }
            }
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
        /// Shows the given calendar entry on the control.
        /// </summary>
        private void showCalendar()
        {
            calendarEntryName_tbx.Text = entry.title;
            location_tbx.Text = entry.location;
            calendar_tbx.Text = entry.dTStrat.DayOfWeek.ToString() + ", " + entry.dTStrat.ToString("HH:mm") + " - " + entry.dTEnd.ToString("HH:mm");
        }

        #endregion

        #region --Misc Methods (Protected)--
        

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
