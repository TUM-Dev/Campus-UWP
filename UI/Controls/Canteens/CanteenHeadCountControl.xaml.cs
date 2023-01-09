using Storage.Classes.Models.Canteens;
using UI.Controls.NavigaTum;
using UI_Context.Classes.Context.Controls.Canteens;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Canteens
{
    public sealed partial class CanteenHeadCountControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CanteenHeadCountControlContext VIEW_MODEL = new CanteenHeadCountControlContext();

        public Canteen SelectedCanteen
        {
            get => (Canteen)GetValue(SelectedCanteenProperty);
            set => SetValue(SelectedCanteenProperty, value);
        }
        public static readonly DependencyProperty SelectedCanteenProperty = DependencyProperty.Register(nameof(SelectedCanteen), typeof(Canteen), typeof(NavigaTumLocationControl), new PropertyMetadata(null, OnSelectedCanteenChanged));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CanteenHeadCountControl()
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
        private void UpdateView(Canteen canteen)
        {
            VIEW_MODEL.UpdateView(canteen);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void OnSelectedCanteenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CanteenHeadCountControl control && e.NewValue is Canteen canteen)
            {
                control.UpdateView(canteen);
            }
        }

        #endregion
    }
}
