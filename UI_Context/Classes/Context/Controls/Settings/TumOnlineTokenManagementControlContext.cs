using Storage.Classes;
using UI_Context.Classes.Context.Dialogs;
using UI_Context.Classes.Templates.Controls.Settings;

namespace UI_Context.Classes.Context.Controls.Settings
{
    public class TumOnlineTokenManagementControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly TumOnlineTokenManagementControlTemplate MODEL = new TumOnlineTokenManagementControlTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void DeleteTumOnlineTokenAndId(ConfirmDialogContext ctx)
        {
            if (ctx.MODEL.Confirmed)
            {
                Storage.Classes.Settings.SetSetting(SettingsConsts.TUM_ID, "");
                Vault.DeleteAllVaults();
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
