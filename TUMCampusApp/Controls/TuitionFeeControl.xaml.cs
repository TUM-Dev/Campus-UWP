using System;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.TUMOnline;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Controls
{
    public sealed partial class TuitionFeeControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private TUMTuitionFee tuitionFee;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 20/07/2017 Created [Fabian Sauter]
        /// </history>
        public TuitionFeeControl(TUMTuitionFee tuitionFee)
        {
            this.tuitionFee = tuitionFee;
            this.InitializeComponent();
            showTuitionFee();
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
        /// Shows the tuitionFee object on the screen.
        /// </summary>
        private void showTuitionFee()
        {
            if(tuitionFee == null)
            {
                return;
            }

            outsBalance_tbx.Text = tuitionFee.money + "€";
            semester_tbx.Text = translateSemesterDescription(tuitionFee.semesterDescripion);
            DateTime deadLine = DateTime.Parse(tuitionFee.deadline);
            TimeSpan tS = deadLine.Subtract(DateTime.Now);
            deadline_tbx.Text = deadLine.Day + "." + deadLine.Month + "." + deadLine.Year + " ==> " + Math.Round(tS.TotalDays) + " " + Utillities.getLocalizedString("TuitionFeeControlDaysLeft_Text");
            if (tS.TotalDays <= 30)
            {
                main_grid.Background = new SolidColorBrush(Windows.UI.Colors.DarkRed);
            }
        }

        /// <summary>
        /// Translates the given semester description to the right language.
        /// </summary>
        /// <param name="s">The string that should get translated.</param>
        /// <returns>A translated version of the given string.</returns>
        private string translateSemesterDescription(string s)
        {
            if (s.Contains("Wintersemester"))
            {
                return s.Replace("Wintersemester", Utillities.getLocalizedString("TuitionFeeControlWinterTerm_Text"));
            }
            else
            {
                return s.Replace("Sommersemester", Utillities.getLocalizedString("TuitionFeeControlSummerTerm_Text"));
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
