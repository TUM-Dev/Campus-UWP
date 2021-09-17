using ExternalData.Classes.Events;

namespace ExternalData.Classes.Manager
{
    public abstract class AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public delegate void OnRequestErrorEventHandler(AbstractManager sender, RequestErrorEventArgs e);
        public event OnRequestErrorEventHandler OnRequestError;

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
        protected void InvokeOnRequestError(RequestErrorEventArgs args)
        {
            OnRequestError?.Invoke(this, args);
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
