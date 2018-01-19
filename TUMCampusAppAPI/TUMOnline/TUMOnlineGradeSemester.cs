using System.Collections.Generic;
using TUMCampusAppAPI.DBTables;

namespace TUMCampusAppAPI.TUMOnline
{
    public class TUMOnlineGradeSemester
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private List<TUMOnlineGradeTable> grades;
        private string semester;
        private string semesterId;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 13/03/2017 Created [Fabian Sauter]
        /// </history>
        public TUMOnlineGradeSemester(TUMOnlineGradeTable grade)
        {
            semesterId = grade.lvSemester;
            if (grade.lvSemester.EndsWith("W"))
            {
                semester = "Winter Semester 20" + grade.lvSemester.Substring(0,2) + "/20" + (int.Parse(grade.lvSemester.Substring(0, 2)) + 1);
            }
            else
            {
                semester = "Summer Semester 20" + grade.lvSemester.Substring(0, 2);
            }
            tryAddGrade(grade);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public List<TUMOnlineGradeTable> getGrades()
        {
            return grades;
        }

        public string getSemester()
        {
            return semester;
        }

        public string getSemesterId()
        {
            return semesterId;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Adds the given grade to the list if the lvSemester is equal or if the list is empty.
        /// </summary>
        /// <param name="grade">The grade that should get added.</param>
        /// <returns>Returns true if it got added to the current semester.</returns>
        public bool tryAddGrade(TUMOnlineGradeTable grade)
        {
            if(grades == null)
            {
                grades = new List<TUMOnlineGradeTable>();
            }
            if (grades.Count <= 0 || grades[0].lvSemester.Equals(grade.lvSemester))
            {
                grades.Add(grade);
                return true;
            }
            return false;
        }

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
