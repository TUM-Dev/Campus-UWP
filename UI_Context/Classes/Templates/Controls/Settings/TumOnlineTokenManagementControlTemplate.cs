using Shared.Classes;
using Storage.Classes;

namespace UI_Context.Classes.Templates.Controls.Settings
{
    public class TumOnlineTokenManagementControlTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool _SetupDone;
        public bool SetupDone
        {
            get => _SetupDone;
            set => SetProperty(ref _SetupDone, value);
        }

        private string _TumId;
        public string TumId
        {
            get => _TumId;
            set => SetProperty(ref _TumId, value);
        }

        private string _Token;
        public string Token
        {
            get => _Token;
            set => SetProperty(ref _Token, value);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumOnlineTokenManagementControlTemplate()
        {
            LoadSettings();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void LoadSettings()
        {
            TumId = Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID, "");
            if (!string.IsNullOrEmpty(TumId))
            {
                TumOnlineCredentials credentials = Vault.LoadCredentials(TumId);
                Token = credentials.TOKEN;
                SetupDone = !string.IsNullOrEmpty(Token);
            }
            else
            {
                SetupDone = false;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
