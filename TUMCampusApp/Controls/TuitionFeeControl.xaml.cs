using System;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.DBTables;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Controls
{
    public sealed partial class TuitionFeeControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private TUMTuitionFeeTable tuitionFee;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 20/07/2017 Created [Fabian Sauter]
        /// </history>
        public TuitionFeeControl(TUMTuitionFeeTable tuitionFee)
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
            semester_tbx.Text = UIUtils.translateSemester(tuitionFee.semesterDescripion);
            DateTime deadLine = DateTime.Parse(tuitionFee.deadline);
            TimeSpan tS = deadLine.Subtract(DateTime.Now);
            deadline_tbx.Text = deadLine.ToString("dd.MM.yyyy") + " ==> " + Math.Round(tS.TotalDays) + " " + UIUtils.getLocalizedString("TuitionFeeControlDaysLeft_Text");
            if (tS.TotalDays <= 30)
            {
                main_grid.Background = new SolidColorBrush(Windows.UI.Colors.DarkRed);
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
