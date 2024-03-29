﻿using System.Threading.Tasks;
using Shared.Classes;
using Storage.Classes;
using TumOnline.Classes.Events;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class UserPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly UserPageDataTemplate MODEL = new UserPageDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public UserPageContext()
        {
            IdentityManager.INSTANCE.OnRequestError += OnRequestError;
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
            Task.Run(async () => await LoadUserAsync(refresh));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadUserAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            MODEL.ShowError = false;
            MODEL.Identity = await IdentityManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
            if (MODEL.Identity is null)
            {
                MODEL.User = null;
            }
            else
            {
                MODEL.User = await UserManager.INSTANCE.UpdateUserAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), MODEL.Identity.ObfuscatedId, refresh).ConfAwaitFalse();
            }
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
            MODEL.ErrorMsg = "Failed to load user.\n" + e.GenerateErrorMessage();
        }

        #endregion
    }
}
