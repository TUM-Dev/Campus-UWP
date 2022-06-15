namespace Storage.Classes
{
    public class TumOnlineCredentials
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly string TUM_ID;
        public readonly string TOKEN;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumOnlineCredentials(string tumId, string token)
        {
            TUM_ID = tumId;
            TOKEN = token;
        }

        public TumOnlineCredentials(string tumId) : this(tumId, "") { }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns true in case the TUMonline credentials stored here are valid and contain a valid TUM-ID and token.
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(TUM_ID) && !string.IsNullOrEmpty(TOKEN);
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
