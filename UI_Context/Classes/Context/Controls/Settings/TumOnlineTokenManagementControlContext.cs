using System.Threading.Tasks;
using Storage.Classes;
using Storage.Classes.Contexts;
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
        public async Task DeleteTumOnlineTokenAndIdAsync(ConfirmDialogContext ctx)
        {
            if (ctx.MODEL.Confirmed)
            {
                // Delete token:
                Storage.Classes.Settings.SetSetting(SettingsConsts.TUM_ID, "");
                Vault.DeleteAllVaults();

                // Delete DB:
                using (TumOnlineDbContext dbCtx = new TumOnlineDbContext())
                {
                    await dbCtx.Database.EnsureDeletedAsync();
                    await dbCtx.Database.EnsureCreatedAsync();
                }
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
