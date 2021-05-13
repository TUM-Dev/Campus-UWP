using System.Collections.Generic;
using Shared.Classes;
using Shared.Classes.Collections;
using Storage.Classes.Models.TumOnline;

namespace UI_Context.Classes.Templates.Controls.Grades
{
    public class GradesDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly string HEADER;
        public readonly CustomObservableCollection<Grade> GRADES_GROUP;
        public readonly bool EXPANDED;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GradesDataTemplate(List<Grade> gradesList, bool expanded)
        {
            // There always has to be one grade:
            HEADER = gradesList[0].LectureSemester;
            GRADES_GROUP = new CustomObservableCollection<Grade>(true);
            GRADES_GROUP.AddRange(gradesList);
            EXPANDED = expanded;
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
