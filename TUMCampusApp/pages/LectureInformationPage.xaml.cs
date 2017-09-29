using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        #region --Constructors--
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
        /// <summary>
        /// Downloads and shows information for the current lecture.
        /// This method should be only called in a separate task.
        /// </summary>
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
                    lectureName_tbx.Text = Utillities.getLocalizedString("LectureInfosUnableToGatherInformation_Text");
                    progressBar.Visibility = Visibility.Collapsed;
                }).AsTask().Wait();
                return;
            }
            lectureInfo = list[0];
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                showLectureInformation();
            }).AsTask().Wait();
        }

        /// <summary>
        /// Shows the no access grid if for example the token is no enabled.
        /// </summary>
        /// <param name="e">The cought exception.</param>
        private void showNoAccess(BaseTUMOnlineException e)
        {
            noData_grid.Visibility = Visibility.Visible;
            info_stckp.Visibility = Visibility.Collapsed;

            if (e is InvalidTokenTUMOnlineException)
            {
                noData_tbx.Text = Utillities.getLocalizedString("TokenNotActivated_Text");
            }
            else if (e is NoAccessTUMOnlineException)
            {
                noData_tbx.Text = Utillities.getLocalizedString("NoAccessToTuitionFees_Text");
            }
            else
            {
                noData_tbx.Text = Utillities.getLocalizedString("UnknownException_Text") + "\n\n" + e.ToString();
            }
            progressBar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows information on screen for the current lecture.
        /// </summary>
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

        /// TODO: Add lecture Appointments page.
        private void lectureAppointments_btn_Click(object sender, RoutedEventArgs e)
        {
            //DeviceInfo.INSTANCE.mainPage.navigateToPage(typeof(LectureInformationPage), lecture);
        }
        #endregion
    }
}
