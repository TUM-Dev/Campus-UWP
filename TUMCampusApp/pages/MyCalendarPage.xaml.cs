using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusApp.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using TUMCampusAppAPI.Syncs;

namespace TUMCampusApp.Pages
{
    public sealed partial class MyCalendarPage : Page
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
        /// 06/01/2017 Created [Fabian Sauter]
        /// </history>
        public MyCalendarPage()
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
        /// Adds a seperator in form of the given date.
        /// </summary>
        /// <param name="date">The date for the seperator.</param>
        private void addSeperator(DateTime date)
        {
            Brush brush = Resources["ApplicationPressedForegroundThemeBrush"] as Brush;
            TextBlock tb = new TextBlock()
            {
                Text = date.DayOfWeek.ToString()  + ", " + date.ToString("dd.MM.yyyy"),
                Margin = new Thickness(10, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = brush
            };

            Rectangle rect = new Rectangle()
            {
                Fill = brush,
                Margin = new Thickness(0, 5, 0, 5),
                Height = 2
            };

            calendarEntries_stckp.Children.Add(tb);
            calendarEntries_stckp.Children.Add(rect);
        }

        /// <summary>
        /// Adds a calendar entry to the calendarEntries_stckp.
        /// </summary>
        /// <param name="entry">The entry that should get added.</param>
        private void addCalendarControl(TUMOnlineCalendarEntry entry)
        {
            CalendarControl cC = new CalendarControl(entry);
            cC.Margin = new Thickness(10, 10, 10, 0);
            calendarEntries_stckp.Children.Add(cC);
        }

        /// <summary>
        /// Shows all calendar entries.
        /// This method should only be called in a seperate task.
        /// </summary>
        private void showCalendarEntriesTask()
        {
            List<TUMOnlineCalendarEntry> list = CalendarManager.INSTANCE.getEntries();
            list.Sort();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                showEntries(list);
            }).AsTask().Wait();
        }

        /// <summary>
        /// Shows all given entries on the screen.
        /// </summary>
        /// <param name="list">A list of entries that should get shown.</param>
        private void showEntries(List<TUMOnlineCalendarEntry> list)
        {
            if(list == null || list.Count <= 0)
            {
                SyncResult syncResult = CalendarManager.INSTANCE.getSyncStatus();
                if(syncResult.STATUS < 0 && syncResult.ERROR_MESSAGE != null)
                {
                    noDataInfo_tbx.Text = syncResult.ERROR_MESSAGE;
                }
                noData_grid.Visibility = Visibility.Visible;
                progressBar.Visibility = Visibility.Collapsed;
                calendarEntries_stckp.Visibility = Visibility.Collapsed;
                refresh_pTRV.IsEnabled = true;
                return;
            }
            calendarEntries_stckp.Children.Clear();
            TUMOnlineCalendarEntry pre = null;
            foreach (TUMOnlineCalendarEntry entry in list)
            {
                if(entry != null && entry.dTStrat.Date.CompareTo(DateTime.Now.Date) >= 0)
                {
                    if(pre == null || entry.dTStrat.Date.CompareTo(pre.dTStrat.Date) > 0)
                    {
                        pre = entry;
                        addSeperator(pre.dTStrat);
                    }
                    entry.dTStrat = entry.dTStrat;
                    entry.dTEnd = entry.dTEnd;
                    addCalendarControl(entry);
                }
            }
            progressBar.Visibility = Visibility.Collapsed;
            noData_grid.Visibility = Visibility.Collapsed;
            calendarEntries_stckp.Visibility = Visibility.Visible;
            refresh_pTRV.IsEnabled = true;
        }

        /// <summary>
        /// Starts a new task and refreshes all calendar entries and displays them on the screen.
        /// </summary>
        private void refreshCalendar()
        {
            refresh_pTRV.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => {
                Task.WaitAny(CalendarManager.INSTANCE.syncCalendarTaskAsync(true));
                showCalendarEntriesTask();
            });
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            refresh_pTRV.IsEnabled = false;
            Task.Factory.StartNew(() => showCalendarEntriesTask());
        }

        private void refresh_pTRV_RefreshRequested(object sender, EventArgs e)
        {
            refreshCalendar();
        }

        #endregion
    }
}
