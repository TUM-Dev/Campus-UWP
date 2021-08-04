using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Classes.Models.TumOnline
{
    public class Lecture
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SpNr { get; set; }
        public int LvNr { get; set; }
        public string Title { get; set; }
        public double Duration { get; set; }
        public double SpSst { get; set; }
        public string TypeLong { get; set; }
        public string TypeShort { get; set; }
        public string SemesterYearName { get; set; }
        public string Semester { get; set; }
        public string SemesterName { get; set; }
        public string SemesterId { get; set; }
        public int FacultySupervisorId { get; set; }
        public string FacultySupervisorName { get; set; }
        public string FacultyId { get; set; }
        public string Contributors { get; set; }

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
