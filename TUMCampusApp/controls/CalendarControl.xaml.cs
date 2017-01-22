using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TUMCampusApp.classes.tum;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.controls
{
    public sealed partial class CalendarControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private TUMOnlineCalendarEntry entry;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 22/01/2017 Created [Fabian Sauter]
        /// </history>
        public CalendarControl(TUMOnlineCalendarEntry entry)
        {
            this.entry = entry;
            this.InitializeComponent();
            showCalendar();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void showCalendar()
        {
            calendarEntryName_tbx.Text = entry.title;
            location_tbx.Text = entry.location;
            calendar_tbx.Text = entry.dTStrat.DayOfWeek.ToString() + ", " + entry.dTStrat.ToString("HH:mm") + " - " + entry.dTEnd.ToString("HH:mm") + " " + entry.dTStrat.ToString();
        }

        #endregion

        #region --Misc Methods (Protected)--
        

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
