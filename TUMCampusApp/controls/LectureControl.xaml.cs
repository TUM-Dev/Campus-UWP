using TUMCampusApp.Classes;
using TUMCampusApp.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using TUMCampusAppAPI.DBTables;

namespace TUMCampusApp.Controls
{
    public sealed partial class LectureControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public TUMOnlineLectureTable lecture;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/01/2017 Created [Fabian Sauter]
        /// </history>
        public LectureControl(TUMOnlineLectureTable lecture)
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
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Sets the visability of the line at the bottom of the control.
        /// </summary>
        /// <param name="visability">Show or hide line.</param>
        public void setRectangleVisability(Visibility visability)
        {
            rect.Visibility = visability;
        }

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
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            UIUtils.mainPage?.navigateToPage(typeof(LectureInformationPage), null);
        }

        #endregion
    }
}
