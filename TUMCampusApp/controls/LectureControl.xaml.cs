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
    public sealed partial class LectureControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private TUMOnlineLecture lecture;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/01/2017 Created [Fabian Sauter]
        /// </history>
        public LectureControl(TUMOnlineLecture lecture, bool last)
        {
            this.lecture = lecture;
            this.InitializeComponent();

            if(lecture == null)
            {
                return;
            }
            name_tbx.Text = lecture.title;
            description_tbx.Text = lecture.typeLong + " - " + lecture.semesterId + " - " + lecture.duration + " SWS";
            profName_tbx.Text = lecture.existingContributors;
            if (last)
            {
                rect.Visibility = Visibility.Collapsed;
            }
            else
            {
                rect.Fill = profName_tbx.Foreground;
            }
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
