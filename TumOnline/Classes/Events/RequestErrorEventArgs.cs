using System;
using TumOnline.Classes.Exceptions;

namespace TumOnline.Classes.Events
{
    public class RequestErrorEventArgs: EventArgs
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly Exception EXCEPTION;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public RequestErrorEventArgs(Exception exception)
        {
            EXCEPTION = exception;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public string GenerateErrorMessage()
        {
            if (EXCEPTION is NoAccessTumOnlineException)
            {
                return "No access. Please make sure you have given this token the required rights in TUMonline.";
            }
            else if (EXCEPTION is InvalidTokenTumOnlineException)
            {
                return "Invalid TUMonline token. Please maker sure your TUMonline setup was successful.";
            }
            else
            {
                return "Internal error. Please retry...";
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
