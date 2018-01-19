﻿using System;
using TUMCampusAppAPI.DBTables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class GradeControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private TUMOnlineGradeTable grade;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 12/03/2017 Created [Fabian Sauter]
        /// </history>
        public GradeControl(TUMOnlineGradeTable grade)
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


        #endregion
    }
}
