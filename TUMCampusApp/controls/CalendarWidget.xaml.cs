using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Managers;
using TUMCampusApp.Classes.Tum;
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

namespace TUMCampusApp.Controls
{
    public sealed partial class CalendarWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 22/01/2017 Created [Fabian Sauter]
        /// </history>
        public CalendarWidget()
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
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = brush
            };

            Rectangle rect = new Rectangle()
            {
                Fill = brush,
                Margin = new Thickness(0, 5, 0, 5),
                Height = 2
            };

            calendarEntries_sckl.Children.Add(tb);
            calendarEntries_sckl.Children.Add(rect);
        }

        private void ShowCalendarEntry()
        {
            CalendarManager.INSTANCE.syncCalendar();
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
                return;
            }
            addSeperator(entry.dTStrat);
            if (entry == null)
            {
                this.Visibility = Visibility.Collapsed;
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => ShowCalendarEntry());
        }

        #endregion
    }
}
