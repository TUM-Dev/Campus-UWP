using Microsoft.EntityFrameworkCore;
using Storage.Classes.Models.External;

namespace Storage.Classes.Contexts
{
    public class StudyRoomDbContext: AbstractDbContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public DbSet<StudyRoomGroup> StudyRoomGroups { get; set; }
        public DbSet<StudyRoom> StudyRooms { get; set; }
        public DbSet<StudyRoomAttribute> StudyRoomAttributes { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public StudyRoomDbContext() : base("studyRooms.db")
        {
            // Disable change tracking since we always manually update them and only require them read only:
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.EnsureCreated();
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
