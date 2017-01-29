using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Managers;
using TUMCampusApp.Classes.Tum;
using TUMCampusApp.Classes.Tum.Exceptions;
using TUMCampusApp.Classes.UserDatas;
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

namespace TUMCampusApp.Pages
{
    public sealed partial class LectureInformationPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private TUMOnlineLecture lecture;
        private TUMOnlineLectureInformation lectureInfo;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/01/2017 Created [Fabian Sauter]
        /// </history>
        public LectureInformationPage()
        {
            this.InitializeComponent();
            this.lecture = null;
            this.lectureInfo = null;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private async void downloadAndShowLectureInformationTask()
        {
            List<TUMOnlineLectureInformation> list = null;
            try
            {
                list = await LecturesManager.INSTANCE.searchForLectureInformationAsync(lecture.sp_nr.ToString());
            }
            catch (BaseTUMOnlineException e)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                }).AsTask().Wait();
                return;
            }

            if (list == null || list.Count <= 0)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    lectureName_tbx.Text = "Unable to gather information!";
                    progressBar.Visibility = Visibility.Collapsed;
                }).AsTask().Wait();
                return;
            }
            lectureInfo = list[0];
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                showLectureInformation();
            }).AsTask().Wait();
        }

        private void showNoAccess(BaseTUMOnlineException e)
        {
            noData_grid.Visibility = Visibility.Visible;
            info_stckp.Visibility = Visibility.Collapsed;

            if (e is InvalidTokenTUMOnlineException)
            {
                noData_tbx.Text = "Your token is not activated yet!";
            }
            else if (e is NoAccessTUMOnlineException)
            {
                noData_tbx.Text = "No access on your tuition fee status!";
            }
            else
            {
                noData_tbx.Text = "Unknown exception!\n" + e.ToString();
            }
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void showLectureInformation()
        {
            lectureName_tbx.Text = lecture.title;
            detail_tbx.Text = lecture.semesterName + "\n" + lecture.typeLong + " - " + lecture.duration + " SWS";
            lectureBeginn_tbx.Text = lectureInfo.startDate;
            lectureContributors_tbx.Text = lecture.existingContributors;
            lectureContent_tbx.Text = lectureInfo.teachingContent;
            method_tbx.Text = lectureInfo.teachingMethod;
            targets_tbx.Text = lectureInfo.learningTarget;
            examinationAids_tbx.Text = lectureInfo.testMode;
            info_stckp.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            lecture = e.Parameter as TUMOnlineLecture;
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => downloadAndShowLectureInformationTask());
        }

        private void lectureAppointments_btn_Click(object sender, RoutedEventArgs e)
        {
            //DeviceInfo.INSTANCE.mainPage.navigateToPage(typeof(LectureInformationPage), lecture);
        }
        #endregion
    }
}
