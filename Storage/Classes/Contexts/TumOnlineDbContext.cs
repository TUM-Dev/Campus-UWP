using Microsoft.EntityFrameworkCore;
using Storage.Classes.Models.TumOnline;

namespace Storage.Classes.Contexts
{
    public class TumOnlineDbContext: AbstractDbContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public DbSet<Grade> Grades { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumOnlineDbContext() : base("tumOnline.db")
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
