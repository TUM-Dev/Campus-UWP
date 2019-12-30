using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes;
using Windows.Storage;

namespace UI_Context.Classes.Templates.Pages.Settings
{
    public class GeneralSettingsPageTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private string _LogFolderPath;
        public string LogFolderPath
        {
            get => _LogFolderPath;
            set => SetProperty(ref _LogFolderPath, value);
        }

        private bool _Analytics;
        public bool Analytics
        {
            get => _Analytics;
            set => SetAnalytics(value);
        }
        private bool _CrashReports;
        public bool CrashReports
        {
            get => _CrashReports;
            set => SetCrashReports(value);
        }
        private bool _ShowWhatsNewDialogOnStartup;
        public bool ShowWhatsNewDialogOnStartup
        {
            get => _ShowWhatsNewDialogOnStartup;
            set => SetBoolInversedProperty(ref _ShowWhatsNewDialogOnStartup, value, SettingsConsts.HIDE_WHATS_NEW_DIALOG);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GeneralSettingsPageTemplate()
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

        private void SetAnalytics(bool value)
        {
            if (SetProperty(ref _Analytics, value, nameof(Analytics)))
            {
                Storage.Classes.Settings.SetSetting(SettingsConsts.DISABLE_ANALYTICS, !value);
                // Task.Run(async () => await AppCenterHelper.SetAnalyticsEnabledAsync(value));
            }
        }

        private void SetCrashReports(bool value)
        {
            if (SetProperty(ref _CrashReports, value, nameof(CrashReports)))
            {
                Storage.Classes.Settings.SetSetting(SettingsConsts.DISABLE_CRASH_REPORTING, !value);
                // Task.Run(async () => await AppCenterHelper.SetCrashesEnabledAsync(value));
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        private void LoadSettings()
        {
            Task.Run(async () =>
            {
                StorageFolder folder = await Logger.GetLogFolderAsync().ConfAwaitFalse();
                LogFolderPath = folder is null ? "" : folder.Path;
            });
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
