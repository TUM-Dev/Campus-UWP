using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusApp.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using TUMCampusAppAPI;

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
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
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


        #endregion
    }
}
