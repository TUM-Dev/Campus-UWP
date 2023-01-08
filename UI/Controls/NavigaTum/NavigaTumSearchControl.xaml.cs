using ExternalData.Classes.NavigaTum;
using UI_Context.Classes.Context.Controls.NavigaTum;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.NavigaTum
{
    public sealed partial class NavigaTumSearchControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public AbstractSearchResultItem SelectedItem
        {
            get => (AbstractSearchResultItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(AbstractSearchResultItem), typeof(NavigaTumSearchControl), new PropertyMetadata(null));

        public readonly NavigaTumSearchControlContext VIEW_MODEL = new NavigaTumSearchControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NavigaTumSearchControl()
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
        private void UpdateSuggestions()
        {
            VIEW_MODEL.Search(searchAsb.Text.Trim());
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            UpdateSuggestions();
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            UpdateSuggestions();
            searchAsb.IsSuggestionListOpen = true;
        }

        private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                VIEW_MODEL.Search(sender.Text.Trim());
            }
        }

        private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is AbstractSearchResultItem item)
            {
                SelectedItem = item;
            }
        }

        #endregion
    }
}
