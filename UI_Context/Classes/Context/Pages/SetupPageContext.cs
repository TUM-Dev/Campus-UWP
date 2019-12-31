using System;
using System.Threading;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes;
using TumOnline.Classes.Exceptions;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Pages;

namespace UI_Context.Classes.Context.Pages
{
    public class SetupPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly SetupPageTemplate MODEL = new SetupPageTemplate();

        private Task autoActivationCheckTask;
        private CancellationTokenSource autoActivationCheckCancelToken;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public SetupPageContext()
        {
            LoadSettings();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<bool> RequestNewTokenAsync()
        {
            MODEL.IsRequestingToken = true;
            try
            {
                MODEL.Token = await AccessManager.RequestNewTokenAsync(MODEL.TumId);
            }
            catch (AbstractTumOnlineException e)
            {
                Logger.Error("Failed to request a new TUMonline token.", e);
                MODEL.IsRequestingToken = false;
                return false;
            }
            catch (Exception e)
            {
                Logger.Error("Failed to request a new TUMonline token. Please report this", e);
                MODEL.IsRequestingToken = false;
                return false;
            }
            MODEL.IsRequestingToken = false;
            return true;
        }

        public async Task OnWhatIsTumOnlineAsync()
        {
            await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("TumOnlineUrl")));
        }

        public async Task OpenDefaultMailAppAsync()
        {
            await UiUtils.LaunchUriAsync(new Uri("mailto:"));
        }

        public async Task<bool> CheckIfTokenIsActivatedAsync()
        {
            if (MODEL.IsTokenActivated)
            {
                return true;
            }

            MODEL.IsCheckingTokenActivation = true;
            MODEL.IsTokenActivated = await AccessManager.IsTokenActivated(MODEL.Token);
            MODEL.IsCheckingTokenActivation = false;
            return MODEL.IsTokenActivated;
        }

        public void StoreIdAndToken()
        {
            Storage.Classes.Settings.SetSetting(SettingsConsts.TUM_ID, MODEL.TumId);
            Vault.StoreCredentials(new TumOnlineCredentials(MODEL.TumId, MODEL.Token));
        }

        public void StartAutoActivationCheck()
        {
            StopAutoActivationCheck();
            autoActivationCheckCancelToken = new CancellationTokenSource();
            MODEL.IsAutomatedActivationCheckRunnig = true;
            autoActivationCheckTask = Task.Run(async () =>
            {
                while (MODEL.IsAutomatedActivationCheckRunnig)
                {
                    MODEL.IsTokenActivated = await AccessManager.IsTokenActivated(MODEL.Token);
                    if (MODEL.IsTokenActivated)
                    {
                        MODEL.IsAutomatedActivationCheckRunnig = false;
                        return;
                    }
                    await Task.Delay(5000);
                    if (MODEL.IsTokenActivated)
                    {
                        MODEL.IsAutomatedActivationCheckRunnig = false;
                        return;
                    }
                }
            }, autoActivationCheckCancelToken.Token);
        }

        public void StopAutoActivationCheck()
        {
            MODEL.IsAutomatedActivationCheckRunnig = false;
            autoActivationCheckCancelToken?.Cancel();
        }

        #endregion

        #region --Misc Methods (Private)--
        private void LoadSettings()
        {
            MODEL.TumId = Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID, "");
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
