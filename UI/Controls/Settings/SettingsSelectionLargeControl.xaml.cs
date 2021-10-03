using UI_Context.Classes;
using UI_Context.Classes.Templates.Controls.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Settings
{
    public sealed partial class SettingsSelectionLargeControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public SettingsPageButtonTemplate Model
        {
            get => (SettingsPageButtonTemplate)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(SettingsPageButtonTemplate), typeof(SettingsSelectionLargeControl), new PropertyMetadata(new SettingsPageButtonTemplate
        {
            Description = "Description",
            Glyph = "\uE9CE",
            Name = "Name",
            NavTarget = null
        }));

        public bool EnableNavigationOnClick
        {
            get => (bool)GetValue(EnableNavigationOnClickProperty);
            set => SetValue(EnableNavigationOnClickProperty, value);
        }
        public static readonly DependencyProperty EnableNavigationOnClickProperty = DependencyProperty.Register(nameof(EnableNavigationOnClick), typeof(bool), typeof(SettingsSelectionLargeControl), new PropertyMetadata(true));

        public delegate void ClickHandler(SettingsSelectionLargeControl sender, RoutedEventArgs args);
        public event ClickHandler Click;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public SettingsSelectionLargeControl()
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
        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
            if (EnableNavigationOnClick)
            {
                UiUtils.NavigateToPage(Model?.NavTarget);
            }
        }

        #endregion
    }
}
