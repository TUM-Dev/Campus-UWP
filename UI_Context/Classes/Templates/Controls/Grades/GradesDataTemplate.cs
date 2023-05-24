using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Shared.Classes;
using Shared.Classes.Collections;
using Storage.Classes.Models.TumOnline;

namespace UI_Context.Classes.Templates.Controls.Grades
{
    public class GradesDataTemplate: AbstractDataTemplate, IComparable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly Regex SEMESTER_REGEX = new Regex(@"(\d\d)([W|S])");

        public readonly string HEADER;
        public readonly CustomObservableCollection<Grade> GRADES_GROUP;
        public bool expanded;
        public readonly string AVERAGE_GRADE;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GradesDataTemplate(List<Grade> gradesList)
        {
            // There always has to be one grade:
            HEADER = gradesList[0].LectureSemester;
            GRADES_GROUP = new CustomObservableCollection<Grade>(true);
            GRADES_GROUP.AddRange(gradesList);
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
                if (double.TryParse(grade.GradeShort, NumberStyles.Float, new CultureInfo("de-DE"), out double g))
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
                return (sum / count).ToString(CultureInfo.CreateSpecificCulture("de-DE"));
            }
            return "no_grades";
        }

        public int CompareTo(object obj)
        {
            if (obj is GradesDataTemplate other)
            {
                Match matchOther = SEMESTER_REGEX.Match(other.HEADER);
                if (matchOther is null || !matchOther.Success)
                {
                    return 1;
                }
                Match match = SEMESTER_REGEX.Match(HEADER);
                if (match is null || !match.Success)
                {
                    return -1;
                }

                if (other.HEADER == HEADER)
                {
                    return 0;
                }

                int yearOther = int.Parse(matchOther.Groups[1].Value);
                int year = int.Parse(match.Groups[1].Value);

                if (yearOther == year)
                {
                    if (matchOther.Groups[2].Value == "W")
                    {
                        return 1;
                    }
                    return -1;
                }
                return yearOther - year;
            }
            return 1;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
