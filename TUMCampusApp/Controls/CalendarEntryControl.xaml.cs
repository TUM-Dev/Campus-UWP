using System;
using TUMCampusApp.Classes;
using TUMCampusAppAPI;
using TUMCampusAppAPI.DBTables;
using Windows.ApplicationModel.DataTransfer;
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
                location_tbx.Text = Entry.location ?? "-";
                calendar_tbx.Text = UiUtils.GetLocalizedString(Entry.dTStrat.DayOfWeek.ToString() + "_Text") + ", " + Entry.dTStrat.ToString("HH:mm") + " - " + Entry.dTEnd.ToString("HH:mm");
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

                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        private void setClipboardString(string text)
        {
            DataPackage package = new DataPackage();
            package.SetText(text);
            Clipboard.SetContent(package);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void copyName_btn_Click(object sender, RoutedEventArgs e)
        {
            setClipboardString(calendarEntryName_tbx.Text);
        }

        private void copyDescription_btn_Click(object sender, RoutedEventArgs e)
        {
            setClipboardString(calendarEntryDescription_tbx.Text);
        }

        private void copyLocation_btn_Click(object sender, RoutedEventArgs e)
        {
            setClipboardString(location_tbx.Text);
        }

        private void openRoomfinder_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void openUrl_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Entry.url))
            {
                await Util.launchBrowser(new Uri(Entry.url));
            }
        }

        private void MenuFlyout_Opening(object sender, object e)
        {
            if (Entry != null)
            {
                copyName_btn.IsEnabled = true;
                copyDescription_btn.IsEnabled = true;
                copyLocation_btn.IsEnabled = true;
                openRoomfinder_btn.IsEnabled = true;
                openUrl_btn.IsEnabled = true;
            }
            else
            {
                copyName_btn.IsEnabled = false;
                copyDescription_btn.IsEnabled = false;
                copyLocation_btn.IsEnabled = false;
                openRoomfinder_btn.IsEnabled = false;
                openUrl_btn.IsEnabled = false;
            }
        }

        #endregion
    }
}
