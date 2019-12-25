using UI_Context.Classes.Context.Controls;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UI.Controls
{
    public sealed partial class TumIdTextBoxControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly TumIdTextBoxControlContext VIEW_MODEL = new TumIdTextBoxControlContext();

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(TumIdTextBoxControl), new PropertyMetadata(""));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(TumIdTextBoxControl), new PropertyMetadata(""));

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(TumIdTextBoxControl), new PropertyMetadata(false));

        public event KeyEventHandler EnterKeyDown;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumIdTextBoxControl()
        {
            InitializeComponent();
            VIEW_MODEL.MODEL.PropertyChanged += MODEL_PropertyChanged;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void UpdateText()
        {
            if (!string.Equals(Text, VIEW_MODEL.MODEL.Text))
            {
                VIEW_MODEL.MODEL.Text = Text;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void MODEL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VIEW_MODEL.MODEL.IsValid):
                    IsValid = VIEW_MODEL.MODEL.IsValid;
                    break;

                case nameof(VIEW_MODEL.MODEL.Text):
                    Text = VIEW_MODEL.MODEL.Text;
                    break;
            }
        }

        private void TextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            int selectionStart = sender.SelectionStart;
            sender.Text = sender.Text.ToLowerInvariant();
            sender.SelectionStart = selectionStart;
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                EnterKeyDown?.Invoke(this, e);
                if (e.Handled)
                {
                    return;
                }
            }

            OnKeyDown(e);
        }

        #endregion
    }
}
