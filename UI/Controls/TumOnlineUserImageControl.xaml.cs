using UI_Context.Classes.Context.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls
{
    public sealed partial class TumOnlineUserImageControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly TumOnlineUserImageControlContext VIEW_MODEL = new TumOnlineUserImageControlContext();

        public string ObfuscatedId
        {
            get => (string)GetValue(ObfuscatedIdProperty);
            set => SetValue(ObfuscatedIdProperty, value);
        }
        public static readonly DependencyProperty ObfuscatedIdProperty = DependencyProperty.Register(nameof(ObfuscatedId), typeof(string), typeof(TumOnlineUserImageControl), new PropertyMetadata(null, OnObfuscatedIdChanged));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumOnlineUserImageControl()
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
        private static void OnObfuscatedIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TumOnlineUserImageControl control)
            {
                control.VIEW_MODEL.UpdateView(control.ObfuscatedId);
            }
        }

        private void OnPersonPictureSizeChnaged(object sender, SizeChangedEventArgs e)
        {
            double size = e.NewSize.Height > e.NewSize.Width ? e.NewSize.Width : e.NewSize.Height;

            if (size > loading_pgr.MaxWidth)
            {
                loading_pgr.Width = loading_pgr.MaxWidth;
            }
            else
            {
                loading_pgr.Width = size;
            }

            if (size > loading_pgr.MaxHeight)
            {
                loading_pgr.Height = loading_pgr.MaxHeight;
            }
            else
            {
                loading_pgr.Height = size;
            }
        }

        #endregion
    }
}
