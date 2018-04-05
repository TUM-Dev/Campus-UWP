using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using TUMCampusApp.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI;

namespace TUMCampusApp.Pages
{
    public sealed partial class MyCalendarPage : Page, INamedPage
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
        /// 06/01/2017 Created [Fabian Sauter]
        /// </history>
        public MyCalendarPage()
        {
            this.InitializeComponent();
            Application.Current.Resuming += new EventHandler<Object>(onAppResumed);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UIUtils.getLocalizedString("MyCalendarPageName_Text");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Adds a separator in form of the given date.
        /// </summary>
        /// <param name="date">The date for the separator.</param>
        private void addSeperator(DateTime date)
        {
            Brush brushLine = Resources["ApplicationPressedForegroundThemeBrush"] as Brush;
            Brush brushText = Resources["CalendarDatePickerTextForeground"] as Brush;
            TextBlock tb = new TextBlock()
            {
                Text = UIUtils.getLocalizedString(date.DayOfWeek.ToString() + "_Text") + ", " + date.ToString("dd.MM.yyyy"),
                Margin = new Thickness(10, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = brushText
            };

            Rectangle rect = new Rectangle()
            {
                Fill = brushLine,
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
        private void addCalendarControl(TUMOnlineCalendarTable entry)
        {
            CalendarEntryControl cC = new CalendarEntryControl()
            {
                Entry = entry,
                Margin = new Thickness(0, 10, 0, 0)
            };
            calendarEntries_stckp.Children.Add(cC);
        }

        /// <summary>
        /// Shows all calendar entries.
        /// This method should only be called in a separate task.
        /// </summary>
        private void showCalendarEntriesTask()
        {
            List<TUMOnlineCalendarTable> list = CalendarManager.INSTANCE.getEntries();
            list.Sort();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => showEntries(list)).AsTask();
        }

        /// <summary>
        /// Shows all given entries on the screen.
        /// </summary>
        /// <param name="list">A list of entries that should get shown.</param>
        private void showEntries(List<TUMOnlineCalendarTable> list)
        {
            if (list == null || list.Count <= 0)
            {
                SyncResult syncResult = CalendarManager.INSTANCE.getSyncStatus();
                if (syncResult.STATUS < 0 && syncResult.ERROR_MESSAGE != null)
                {
                    noDataInfo_tbx.Text = UIUtils.getLocalizedString("MyCalendarGeneralError_Text") + "\n\n" + syncResult.ERROR_MESSAGE;
                    noEntries_grid.Visibility = Visibility.Collapsed;
                    noData_grid.Visibility = Visibility.Visible;
                }
                else
                {
                    noEntries_grid.Visibility = Visibility.Visible;
                    noData_grid.Visibility = Visibility.Collapsed;
                }
                progressBar.Visibility = Visibility.Collapsed;
                calendarEntries_stckp.Visibility = Visibility.Collapsed;
                refresh_pTRV.IsEnabled = true;
                return;
            }
            calendarEntries_stckp.Children.Clear();
            TUMOnlineCalendarTable pre = null;
            bool foundOne = false;
            foreach (TUMOnlineCalendarTable entry in list)
            {
                if (entry != null && entry.dTStrat.Date.CompareTo(DateTime.Now.Date) >= 0)
                {
                    foundOne = true;
                    if (pre == null || entry.dTStrat.Date.CompareTo(pre.dTStrat.Date) > 0)
                    {
                        pre = entry;
                        addSeperator(pre.dTStrat);
                    }
                    entry.dTStrat = entry.dTStrat;
                    entry.dTEnd = entry.dTEnd;
                    addCalendarControl(entry);
                }
            }
            if (!foundOne)
            {
                noEntries_grid.Visibility = Visibility.Visible;
                calendarEntries_stckp.Visibility = Visibility.Collapsed;
            }
            else
            {
                noEntries_grid.Visibility = Visibility.Collapsed;
                calendarEntries_stckp.Visibility = Visibility.Visible;
            }
            progressBar.Visibility = Visibility.Collapsed;
            noData_grid.Visibility = Visibility.Collapsed;
            refresh_pTRV.IsEnabled = true;
        }

        /// <summary>
        /// Starts a new task and refreshes all calendar entries if force == true and displays them on the screen.
        /// </summary>
        private void refreshCalendar(bool force)
        {
            refresh_pTRV.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            Task.Run(async () =>
            {
                Task t = CalendarManager.INSTANCE.syncCalendar(force);
                if(t != null)
                {
                    await t;
                }
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
            refreshCalendar(false);
        }

        private void refresh_pTRV_RefreshRequested(object sender, EventArgs e)
        {
            refreshCalendar(true);
        }

        private void onAppResumed(object sender, object e)
        {
            refreshCalendar(false);
        }

        #endregion
    }
}
