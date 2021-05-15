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
        public readonly string AVERAGE_GRADE;

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
            AVERAGE_GRADE = CalcAvgGrade(gradesList);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private static string CalcAvgGrade(List<Grade> gradesList)
        {
            double sum = 0;
            int count = 0;
            foreach (Grade grade in gradesList)
            {
                if (double.TryParse(grade.GradeShort, out double g))
                {
                    sum += g;
                    ++count;
                }
                else if (grade.GradeShort == "B")
                {
                    ++sum;
                    ++count;
                }
            }
            if (count > 0)
            {
                return (sum / count).ToString();
            }
            return "no_grades";
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
