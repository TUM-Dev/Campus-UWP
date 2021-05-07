using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Classes.Models.TumOnline
{
    public class Grade
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CandidateNumber { get; set; }
        public string LectureNumber { get; set; }
        public string LectureSemester { get; set; }
        public string LectureTite { get; set; }
        public int LectureCredits { get; set; }
        public string ExaminerSurname { get; set; }
        public string GradeShort { get; set; }
        public string ExamType { get; set; }
        public string ExamMode { get; set; }
        public string StudyIdentifier { get; set; }
        public string StudyTitle { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


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
