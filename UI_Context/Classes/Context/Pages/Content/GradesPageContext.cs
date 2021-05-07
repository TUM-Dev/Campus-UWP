using System.Threading.Tasks;
using Shared.Classes;
using Storage.Classes;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class GradesPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly GradesPageDataTemplate MODEL = new GradesPageDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GradesPageContext()
        {
            Task.Run(async () => await LoadGradesAsync(false));
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh()
        {
            Task.Run(async () => await LoadGradesAsync(true));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadGradesAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            await GradesManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
            MODEL.IsLoading = false;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
