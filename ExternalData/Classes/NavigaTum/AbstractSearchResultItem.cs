using ExternalData.Classes.Manager;

namespace ExternalData.Classes.NavigaTum
{
    public abstract class AbstractSearchResultItem
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public string id;
        public string name;
        public string subtext;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override string ToString()
        {
            return name.Replace(NavigaTumManager.POST_HIGHLIGHT, "").Replace(NavigaTumManager.PRE_HIGHLIGHT, "");
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
