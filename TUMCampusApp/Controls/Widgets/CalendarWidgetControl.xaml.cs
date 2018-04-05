using Data_Manager;
using System;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls.Widgets
{
    public sealed partial class CalendarWidgetControl : UserControl, IHideableWidget
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public WidgetControl WidgetContainer
        {
            get { return (WidgetControl)GetValue(WidgetContainerProperty); }
            set { SetValue(WidgetContainerProperty, value); }
        }
        public static readonly DependencyProperty WidgetContainerProperty = DependencyProperty.Register("WidgetContainer", typeof(WidgetControl), typeof(CalendarWidgetControl), null);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/04/2018 Created [Fabian Sauter]
        /// </history>
        public CalendarWidgetControl()
        {
            Application.Current.Resuming += Current_Resuming;
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getSettingsToken()
        {
            return SettingsConsts.DISABLE_CALENDAR_WIDGET;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void onHiding()
        {
        }

        /// <summary>
        /// Adds a custom separator.
        /// </summary>
        /// <param name="date"></param>
        private void addSeperator(DateTime date)
        {
            nextDate_tbx.Text = date.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Starts a new task and shows the next TUMOnlineCalendarEntry.
        /// </summary>
        private void loadCalendarEntry()
        {
            Task.Run(() =>
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => WidgetContainer?.setIsLoading(true)).AsTask();

                CalendarManager.INSTANCE.syncCalendar();
                TUMOnlineCalendarTable entry = CalendarManager.INSTANCE.getNextEntry();

                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => showCalendarEntry(entry)).AsTask();
            });
        }

        /// <summary>
        /// Shows the given TUMOnlineCalendarTable.
        /// </summary>
        /// <param name="entry">The TUMOnlineCalendarTable that should get shown. If null will hide the widget.</param>
        private void showCalendarEntry(TUMOnlineCalendarTable entry)
        {
            if (entry == null)
            {
                if (WidgetContainer != null)
                {
                    WidgetContainer.Visibility = Visibility.Collapsed;
                }
                return;
            }
            addSeperator(entry.dTStrat);
            if (entry == null)
            {
                if (WidgetContainer != null)
                {
                    WidgetContainer.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                entry_cec.Entry = entry;
            }
            WidgetContainer?.setIsLoading(false);
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCalendarEntry();
        }

        private void Current_Resuming(object sender, object e)
        {
            loadCalendarEntry();
        }

        #endregion
    }
}
