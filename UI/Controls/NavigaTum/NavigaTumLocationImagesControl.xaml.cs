using ExternalData.Classes.NavigaTum;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.NavigaTum
{
    public sealed partial class NavigaTumLocationImagesControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public Location CurLocation
        {
            get => (Location)GetValue(CurLocationProperty);
            set => SetValue(CurLocationProperty, value);
        }
        public static readonly DependencyProperty CurLocationProperty = DependencyProperty.Register(nameof(CurLocation), typeof(Location), typeof(NavigaTumLocationImagesControl), new PropertyMetadata(null, OnCurLocationChanged));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NavigaTumLocationImagesControl()
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
        private static void OnCurLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion
    }
}
