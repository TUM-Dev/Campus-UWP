using System;

namespace TUMCampusAppAPI.TUMOnline.Exceptions
{
    public abstract class BaseTUMOnlineException : Exception
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        protected readonly string url;
        private readonly string message;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 29/01/2017 Created [Fabian Sauter]
        /// </history>
        public BaseTUMOnlineException(string url, string message)
        {
            this.url = url;
            this.message = message;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override string ToString()
        {
            return base.ToString() + " URL:" + url + " Message:" + message;
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
