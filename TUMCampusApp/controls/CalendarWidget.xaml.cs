using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

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
