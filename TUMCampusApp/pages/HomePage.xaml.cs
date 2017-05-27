using TUMCampusApp.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TUMCampusAppAPI;
using System;

namespace TUMCampusApp.Pages
{
    public sealed partial class HomePage : Page
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
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public HomePage()
        {
            this.InitializeComponent();
            showWidgets();
            Application.Current.Resuming += new EventHandler<Object>(onAppResumed);
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
        /// Shows all widgets that didn't got disabled in the settings.
        /// </summary>
        private void showWidgets()
        {
            if (!Util.getSettingBoolean(Const.DISABLE_EXAMPLE_WIDGET))
            {
                exampleWidget_ds.Visibility = Visibility.Visible;
            }
            if (!Util.getSettingBoolean(Const.DISABLE_CANTEEN_WIDGET))
            {
                canteenWidget_ds.Content = new CanteenWidget(canteenWidget_ds);
                canteenWidget_ds.Visibility = Visibility.Visible;
            }
            if (!Util.getSettingBoolean(Const.DISABLE_TUITION_FEE_WIDGET))
            {
                tutionFeeWidget_ds.Content = new TuitionFeeWidget(tutionFeeWidget_ds);
                tutionFeeWidget_ds.Visibility = Visibility.Visible;
            }
            if (!Util.getSettingBoolean(Const.DISABLE_CALENDAR_WIDGET))
            {
                calendarWidget_ds.Content = new CalendarWidget(calendarWidget_ds);
                calendarWidget_ds.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void onAppResumed(Object sender, Object e)
        {
            if (!Util.getSettingBoolean(Const.DISABLE_CALENDAR_WIDGET))
            {
                calendarWidget_ds.Content = new CalendarWidget(calendarWidget_ds);
                calendarWidget_ds.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}
