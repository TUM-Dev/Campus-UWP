using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Classes;
using Storage.Classes;
using Storage.Classes.Models.TumOnline;
using TumOnline.Classes.Events;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class TuitionFeesPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly TuitionFeesPageDataTemplate MODEL = new TuitionFeesPageDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TuitionFeesPageContext()
        {
            TuitionFeesManager.INSTANCE.OnRequestError += OnRequestError;
            Refresh(false);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh(bool refresh)
        {
            Task.Run(async () => await LoadFeesAsync(refresh));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadFeesAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            MODEL.ShowError = false;
            IEnumerable<TuitionFee> fees = await TuitionFeesManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
            MODEL.TUITION_FEES.Replace(fees);
            MODEL.HasFees = !MODEL.TUITION_FEES.IsEmpty();
            MODEL.IsLoading = false;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnRequestError(AbstractManager sender, RequestErrorEventArgs e)
        {
            MODEL.ShowError = true;
            MODEL.ErrorMsg = "Failed to load tuition fees.\n" + e.GenerateErrorMessage();
        }

        #endregion
    }
}
