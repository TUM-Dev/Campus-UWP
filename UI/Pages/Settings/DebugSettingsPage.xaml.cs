using Shared.Classes;
using UI.Extensions;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages.Settings;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UI.Pages.Settings
{
    public sealed partial class DebugSettingsPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly DebugSettingsPageContext VIEW_MODEL = new DebugSettingsPageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public DebugSettingsPage()
        {
            InitializeComponent();
            UiUtils.ApplyBackground(this);
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
        private void Main_nview_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem == misc_navItem)
            {
                ScrollViewerExtensions.ScrollIntoViewVertically(main_scv, misc_scp, false);
            }
        }

        private void Main_nview_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            main_nview.SelectedItem = misc_navItem;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedTo();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedFrom();
        }

        private async void OpenAppDataFolder_btn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await VIEW_MODEL.OpenAppDataFolderAsync().ConfAwaitFalse();
        }

        #endregion
    }
}
