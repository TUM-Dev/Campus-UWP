using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Shared.Classes;
using UI.Controls.Settings;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages.Settings;
using UI_Context.Classes.Templates.Controls.Settings;
using UI_Context.Classes.Templates.Pages.Settings;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UI.Pages.Settings
{
    public sealed partial class SettingsPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly SettingsPageContext VIEW_MODEL = new SettingsPageContext();
        private readonly ObservableCollection<SettingsPageButtonTemplate> SETTINGS_PAGES = new ObservableCollection<SettingsPageButtonTemplate>
        {
            new SettingsPageButtonTemplate {Glyph = "\uE9E9", Name = "General", Description = "Logs, Crash Reporting, About", NavTarget = typeof(GeneralSettingsPage)},
            new SettingsPageButtonTemplate {Glyph = "\xE774", Name = "TUMonline", Description = "Token, Access Rights", NavTarget = typeof(TumOnlineSettingsPage)},
        };

        private readonly SettingsPageButtonTemplate DEBUG_SETTINGS = new SettingsPageButtonTemplate { Glyph = "\uEBE8", Name = "Debug", Description = "Debug Test Features", NavTarget = typeof(DebugSettingsPage) };

        private FrameworkElement LastPopUpElement = null;
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public SettingsPage()
        {
            InitializeComponent();
            UiUtils.ApplyBackground(this);
            LoadAppVersion();
            VIEW_MODEL.MODEL.PropertyChanged += MODEL_PropertyChanged;

            if (VIEW_MODEL.MODEL.DebugSettingsEnabled)
            {
                SETTINGS_PAGES.Add(DEBUG_SETTINGS);
            }
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void LoadAppVersion()
        {
            name_run.Text = Package.Current.DisplayName;
            PackageVersion version = Package.Current.Id.Version;
            StringBuilder sb = new StringBuilder("v.");
            sb.Append(version.Major);
            sb.Append('.');
            sb.Append(version.Minor);
            sb.Append('.');
            sb.Append(version.Build);
            sb.Append('.');
            sb.Append(version.Revision);
            version_run.Text = sb.ToString();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void SettingsSelectionControl_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!(DeviceFamilyHelper.IsMouseInteractionMode() && sender is SettingsSelectionLargeControl settingsSelection))
            {
                return;
            }

            LastPopUpElement = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(settingsSelection) as FrameworkElement) as FrameworkElement;
            Canvas.SetZIndex(LastPopUpElement, 10);
            AnimationBuilder.Create().Scale(to: 1.05, easingType: EasingType.Sine).Start(LastPopUpElement);
        }

        private void SettingsSelectionControl_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (LastPopUpElement is null)
            {
                return;
            }

            Canvas.SetZIndex(LastPopUpElement, 0);
            AnimationBuilder.Create().Scale(to: 1.0, easingType: EasingType.Sine).Start(LastPopUpElement);
            LastPopUpElement = null;
        }

        private void Version_tbx_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            VIEW_MODEL.OnVersionTextTapped();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedTo();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedFrom();
        }

        private void MODEL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is SettingsPageTemplate settingsPageDataTemplate)
            {
                switch (e.PropertyName)
                {
                    case nameof(settingsPageDataTemplate.DebugSettingsEnabled):
                        if (settingsPageDataTemplate.DebugSettingsEnabled)
                        {
                            debugSettings_notification.Show("Debug settings enabled.", 5000);
                            if (!SETTINGS_PAGES.Contains(DEBUG_SETTINGS))
                            {
                                SETTINGS_PAGES.Add(DEBUG_SETTINGS);
                            }
                        }
                        else
                        {
                            debugSettings_notification.Show("Debug settings disabled.", 5000);
                            SETTINGS_PAGES.Remove(DEBUG_SETTINGS);
                        }
                        break;
                }
            }
        }

        #endregion
    }
}
