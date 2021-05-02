using Storage.Classes.Contexts;

namespace Storage.Classes.Models.Cache
{
    public class AbstractCacheModel: AbstractModel
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override void Add()
        {
            using (CacheDbContext ctx = new CacheDbContext())
            {
                ctx.Add(this);
            }
        }

        public override void Update()
        {
            using (CacheDbContext ctx = new CacheDbContext())
            {
                ctx.Update(this);
            }
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
