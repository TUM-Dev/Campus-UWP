using UI_Context.Classes.Context.Controls.NavigaTum;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UI.Controls.NavigaTum
{
    public sealed partial class NavigaTumSearchControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
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

        private void OnFocusEngaged(Control sender, FocusEngagedEventArgs args)
        {
            UpdateSuggestions();
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            UpdateSuggestions();
        }

        private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                VIEW_MODEL.Search(sender.Text.Trim());
            }
        }

        private void OnKeyUp(object sender, KeyRoutedEventArgs e)
        {

        }

        #endregion
    }
}
