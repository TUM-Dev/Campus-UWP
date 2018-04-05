using System;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.DBTables;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Controls
{
    public sealed partial class CalendarEntryControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public TUMOnlineCalendarTable Entry
        {
            get { return (TUMOnlineCalendarTable)GetValue(EntryProperty); }
            set
            {
                SetValue(EntryProperty, value);
                showEntry();
            }
        }
        public static readonly DependencyProperty EntryProperty = DependencyProperty.Register("Entry", typeof(TUMOnlineCalendarTable), typeof(CalendarEntryControl), null);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/04/2018 Created [Fabian Sauter]
        /// </history>
        public CalendarEntryControl()
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
        /// Shows the current calendar entry on the control.
        /// </summary>
        private void showEntry()
        {
            if (Entry != null)
            {
                calendarEntryName_tbx.Text = Entry.title;
                location_tbx.Text = Entry.location;
                calendar_tbx.Text = UIUtils.getLocalizedString(Entry.dTStrat.DayOfWeek.ToString() + "_Text") + ", " + Entry.dTStrat.ToString("HH:mm") + " - " + Entry.dTEnd.ToString("HH:mm");
                if (!string.IsNullOrWhiteSpace(Entry.description))
                {
                    calendarEntryDescription_tbx.Text = Entry.description;
                }
                else
                {
                    calendarEntryDescription_tbx.Visibility = Visibility.Collapsed;
                }

                if (Entry.dTStrat.CompareTo(DateTime.Now) <= 0)
                {
                    if (Entry.dTEnd.CompareTo(DateTime.Now) >= 0)
                    {
                        calendarEntryName_tbx.Foreground = new SolidColorBrush(Colors.DarkOrange);
                    }
                    else
                    {
                        calendarEntryName_tbx.Foreground = new SolidColorBrush(Colors.DarkGray);
                    }
                }
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
