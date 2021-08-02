using System;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using UI_Context.Classes.Context.Dialogs;
using UI_Context.Classes.Templates.Pages.Settings;

namespace UI_Context.Classes.Context.Pages.Settings
{
    public class GeneralSettingsPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly GeneralSettingsPageTemplate MODEL = new GeneralSettingsPageTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public Task ExportLogsAsync()
        {
            return Logger.ExportLogsAsync();
        }

        public async Task DeleteLogsAsync(ConfirmDialogContext viewModel)
        {
            if (viewModel.MODEL.Confirmed)
            {
                await Logger.DeleteLogsAsync();
            }
        }

        public Task ShowAnalyticsCrashesMoreInformationAsync()
        {
            return UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("PrivacyPolicyCrashReportsUrl")));
        }

        public Task ShowLicenceAsync()
        {
            return UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("LicenseUrl")));
        }

        public Task ShowPrivacyPolicy()
        {
            return UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("PrivacyPolicyUrl")));
        }

        public Task ViewOnGithubAsync()
        {
            return UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("GitHubUrl")));
        }

        public Task ReportBugAsync()
        {
            return UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("BugReportsUrl")));
        }

        public Task GiveFeedbackAsync()
        {
            return UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("FeedbackUrl")));
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
