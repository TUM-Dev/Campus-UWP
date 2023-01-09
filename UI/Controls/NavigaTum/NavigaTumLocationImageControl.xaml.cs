using ExternalData.Classes.NavigaTum;
using UI_Context.Classes.Context.Controls.NavigaTum;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.NavigaTum
{
    public sealed partial class NavigaTumLocationImageControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public LocationImage Image
        {
            get => (LocationImage)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(LocationImage), typeof(NavigaTumLocationImagesControl), new PropertyMetadata(null));

        public readonly NavigaTumLocationImageControlContext VIEW_MODEL = new NavigaTumLocationImageControlContext();
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NavigaTumLocationImageControl()
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


        #endregion
    }
}
