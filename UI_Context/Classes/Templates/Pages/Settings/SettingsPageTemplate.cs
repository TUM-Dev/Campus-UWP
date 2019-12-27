using System.Runtime.CompilerServices;
using Shared.Classes;

namespace UI_Context.Classes.Templates.Pages.Settings
{
    public class SettingsPageTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool _DebugSettingsEnabled;
        public bool DebugSettingsEnabled
        {
            get => _DebugSettingsEnabled;
            set => SetBoolProperty(ref _DebugSettingsEnabled, value, Storage.Classes.SettingsConsts.DEBUG_SETTINGS_ENABLED);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public SettingsPageTemplate()
        {
            LoadSettings();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void LoadSettings()
        {
            DebugSettingsEnabled = Storage.Classes.Settings.GetSettingBoolean(Storage.Classes.SettingsConsts.DEBUG_SETTINGS_ENABLED);
        }

        #endregion

        #region --Misc Methods (Private)--
        private bool SetBoolProperty(ref bool storage, bool value, string settingsToken, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref storage, value, propertyName))
            {
                Storage.Classes.Settings.SetSetting(settingsToken, value);
                return true;
            }
            return false;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
