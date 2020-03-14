using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Canteens
{
    public sealed partial class MenuControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public string DishType
        {
            get => (string)GetValue(DishTypeProperty);
            set => SetValue(DishTypeProperty, value);
        }
        public static readonly DependencyProperty DishTypeProperty = DependencyProperty.Register(nameof(DishType), typeof(string), typeof(MenuControl), new PropertyMetadata(null));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public MenuControl()
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
