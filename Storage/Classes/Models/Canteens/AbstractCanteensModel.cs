using Storage.Classes.Contexts;

namespace Storage.Classes.Models.Canteens
{
    public class AbstractCanteensModel: AbstractModel
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
            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                ctx.Add(this);
            }
        }

        public override void Update()
        {
            using (CanteensDbContext ctx = new CanteensDbContext())
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
