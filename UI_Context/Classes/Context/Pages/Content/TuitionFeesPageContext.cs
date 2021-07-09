using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes;
using Storage.Classes.Models.TumOnline;
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
            Task.Run(async () => await LoadFeesAsync(false));
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh()
        {
            Task.Run(async () => await LoadFeesAsync(true));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadFeesAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            try
            {
                IEnumerable<TuitionFee> fees = await TuitionFeesManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
                MODEL.TUITION_FEES.Clear();
                MODEL.TUITION_FEES.AddRange(fees);
                MODEL.HasFees = !MODEL.TUITION_FEES.IsEmpty();
            }
            catch (Exception e)
            {
                Logger.Error("Failed to load tuition fees!", e);
            }
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
