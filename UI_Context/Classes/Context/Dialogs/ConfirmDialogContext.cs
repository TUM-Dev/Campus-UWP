using System;
using System.Threading.Tasks;
using UI_Context.Classes.Templates.Dialogs;

namespace UI_Context.Classes.Context.Dialogs
{
    public sealed class ConfirmDialogContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly ConfirmDialogTemplate MODEL = new ConfirmDialogTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void OnPositive()
        {
            MODEL.Confirmed = true;
        }

        public void OnNegative()
        {
            MODEL.Confirmed = false;
        }

        public Task OnLinkClickedAsync(string link)
        {
            return UiUtils.LaunchUriAsync(new Uri(link));
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
