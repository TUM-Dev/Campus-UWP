using Shared.Classes;
using UI.Dialogs;
using UI.Extensions;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace UI.Pages.Settings
{
    public sealed partial class GeneralSettingsPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly GeneralSettingsPageContext VIEW_MODEL = new GeneralSettingsPageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GeneralSettingsPage()
        {
            InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void DeleteLogs_btn_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDialog dialog = new ConfirmDialog("Delete logs:", "Do you really want to **delete** all logs?");
            await UiUtils.ShowDialogAsync(dialog);
            await VIEW_MODEL.DeleteLogsAsync(dialog.VIEW_MODEL);
            await logsFolder_fsc.RecalculateFolderSizeAsync().ConfAwaitFalse();
        }

        private async void ExportLogs_btn_Click(object sender, RoutedEventArgs e)
        {
            await VIEW_MODEL.ExportLogsAsync();
        }

        private void Main_nview_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem == logs_navItem)
            {
                ScrollViewerExtensions.ScrollIntoViewVertically(main_scv, logs_scp, false);
            }
            else if (args.SelectedItem == misc_navItem)
            {
                ScrollViewerExtensions.ScrollIntoViewVertically(main_scv, misc_scp, false);
            }
            else if (args.SelectedItem == analytics_navItem)
            {
                ScrollViewerExtensions.ScrollIntoViewVertically(main_scv, analytics_scp, false);
            }
            else if (args.SelectedItem == about_navItem)
            {
                ScrollViewerExtensions.ScrollIntoViewVertically(main_scv, about_scp, false);
            }
        }

        private void Main_nview_Loaded(object sender, RoutedEventArgs e)
        {
            main_nview.SelectedItem = logs_navItem;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedTo();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedFrom();
        }

        private async void PrivacyPolicy_btn_Click(object sender, RoutedEventArgs e)
        {
            await VIEW_MODEL.ShowPrivacyPolicy().ConfAwaitFalse();
        }

        private async void License_btn_Click(object sender, RoutedEventArgs e)
        {
            await VIEW_MODEL.ShowLicenceAsync().ConfAwaitFalse();
        }

        private async void Feedback_btn_Click(object sender, RoutedEventArgs e)
        {
            await VIEW_MODEL.GiveFeedbackAsync().ConfAwaitFalse();
        }

        private async void ReportBug_btn_Click(object sender, RoutedEventArgs e)
        {
            await VIEW_MODEL.ReportBugAsync().ConfAwaitFalse();
        }

        private async void ViewOnGitHub_btn_Click(object sender, RoutedEventArgs e)
        {
            await VIEW_MODEL.ViewOnGithubAsync().ConfAwaitFalse();
        }

        private async void MoreInformation_hlb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await VIEW_MODEL.ShowAnalyticsCrashesMoreInformationAsync().ConfAwaitFalse();
        }

        #endregion
    }
}
