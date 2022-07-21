using Storage.Classes.Models.Canteens;
using UI_Context.Classes.Context.Controls.Canteens;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Canteens
{
    public sealed partial class DishRatingControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly DishRatingControlContext VIEW_MODEL = new DishRatingControlContext();

        public Dish Dish
        {
            get => (Dish)GetValue(DishProperty);
            set => SetValue(DishProperty, value);
        }
        public static readonly DependencyProperty DishProperty = DependencyProperty.Register(nameof(Dish), typeof(Dish), typeof(DishControl), new PropertyMetadata(null, OnDishChanged));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public DishRatingControl()
        {
            InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void UpdateView()
        {
            VIEW_MODEL.UpdateView(Dish);
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void OnDishChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DishRatingControl control)
            {
                control.UpdateView();
            }
        }

        #endregion
    }
}
