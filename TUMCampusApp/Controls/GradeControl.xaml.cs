using System;
using TUMCampusAppAPI.TUMOnline;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class GradeControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private TUMOnlineGrade grade;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 12/03/2017 Created [Fabian Sauter]
        /// </history>
        public GradeControl(TUMOnlineGrade grade)
        {
            this.grade = grade;
            this.InitializeComponent();
            name_tbx.Text = grade.lvTitel;
            description_tbx.Text = grade.examTypName + " - " + grade.mode;
            profName_tbx.Text = grade.examinerSurname;
            if (grade.date != DateTime.MinValue)
            {
                profName_tbx.Text += " - " + grade.date.ToString("dd.MM.yyyy");
            }
            grade_tbx.Text = grade.uniGradeNameShort;
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
