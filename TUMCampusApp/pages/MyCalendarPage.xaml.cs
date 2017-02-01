using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Managers;
using TUMCampusApp.Classes.Tum;
using TUMCampusApp.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

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
        private void addSeperator(DateTime date)
        {
            Brush brush = Resources["ApplicationPressedForegroundThemeBrush"] as Brush;
            TextBlock tb = new TextBlock()
            {
                Text = date.Day + "." + date.Month + "." + date.Year,
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

        private void addCalendarControl(TUMOnlineCalendarEntry entry)
        {
            CalendarControl cC = new CalendarControl(entry);
            cC.Margin = new Thickness(10, 10, 10, 0);
            calendarEntries_stckp.Children.Add(cC);
        }

        private void showCalendarEntriesTask()
        {
            List<TUMOnlineCalendarEntry> list = CalendarManager.INSTANCE.getEntries();
            list.Sort();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                showEntries(list);
            }).AsTask().Wait();
        }

        private void showEntries(List<TUMOnlineCalendarEntry> list)
        {
            if(list == null || list.Count <= 0)
            {
                progressBar.Visibility = Visibility.Collapsed;
                noData_grid.Visibility = Visibility.Visible;
                calendarEntries_stckp.Visibility = Visibility.Collapsed;
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
                    entry.dTStrat = entry.dTStrat.AddHours(1);
                    entry.dTEnd = entry.dTEnd.AddHours(1);
                    addCalendarControl(entry);
                }
            }
            progressBar.Visibility = Visibility.Collapsed;
            noData_grid.Visibility = Visibility.Collapsed;
            calendarEntries_stckp.Visibility = Visibility.Visible;
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => showCalendarEntriesTask());
        }
        
        #endregion
    }
}
