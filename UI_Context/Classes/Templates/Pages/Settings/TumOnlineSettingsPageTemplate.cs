using System.Runtime.CompilerServices;
using Shared.Classes;
using Storage.Classes;

namespace UI_Context.Classes.Templates.Pages.Settings
{
    public class TumOnlineSettingsPageTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool _EnableWindowsCalendarIntegration;
        public bool EnableWindowsCalendarIntegration
        {
            get => _EnableWindowsCalendarIntegration;
            set => SetBoolInversedProperty(ref _EnableWindowsCalendarIntegration, value, SettingsConsts.DISABLE_WINDOWS_CALENDAR_INTEGRATION);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumOnlineSettingsPageTemplate()
        {
            LoadSettings();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private bool SetBoolInversedProperty(ref bool storage, bool value, string settingsToken, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref storage, value, propertyName))
            {
                Storage.Classes.Settings.SetSetting(settingsToken, !value);
                return true;
            }
            return false;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void LoadSettings()
        {
            EnableWindowsCalendarIntegration = !Storage.Classes.Settings.GetSettingBoolean(SettingsConsts.DISABLE_WINDOWS_CALENDAR_INTEGRATION);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
