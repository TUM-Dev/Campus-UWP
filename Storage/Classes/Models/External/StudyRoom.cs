using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Storage.Classes.Models.Canteens;

namespace Storage.Classes.Models.External
{
    public class StudyRoom: AbstractCanteensModel
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string ArchitectNumber { get; set; }
        [Required]
        public DateTime BookedUntil { get; set; }
        [Required]
        public DateTime BookedFrom { get; set; }
        [Required]
        public int BookedFor { get; set; }
        [Required]
        public int BookedIn { get; set; }
        [Required]
        public string OccupiedBy { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int BuildingNumber { get; set; }
        [Required]
        public string BuildingCode { get; set; }
        [Required]
        public string BuildingName { get; set; }
        [Required]
        public StudyRoomStatus Status { get; set; }
        [Required]
        public int ResNumber { get; set; }
        [Required]
        public List<StudyRoomAttribute> Attributes { get; set; } = new List<StudyRoomAttribute>();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public bool IsSoonOccupied()
        {
            return Status != StudyRoomStatus.OCCUPIED && BookedIn > 0;
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
