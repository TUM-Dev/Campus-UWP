using TUMCampusApp.Classes;
using TUMCampusAppAPI.Canteens;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class DishTypeControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public string dishType;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/01/2018 Created [Fabian Sauter]
        /// </history>
        public DishTypeControl(CanteenDishTable dish)
        {
            this.dishType = dish.dish_type;
            this.InitializeComponent();
            addDish(dish);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void addDish(CanteenDishTable dish)
        {
            if (Equals(dishType, dish.dish_type))
            {
                dishes_stckp.Children.Add(new CanteenMenuControl(dish));
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        private string getTranslateDishType()
        {
            switch (dishType)
            {
                case "Tagesgericht":
                    return Utillities.getLocalizedString("CanteenDishOfTheDay_Text");
                case "Aktionsessen":
                    return Utillities.getLocalizedString("CanteenActionDishes_Text");
                case "Biogericht":
                    return Utillities.getLocalizedString("CanteenBioDish_Text");
                case "StuBistro Gericht":
                    return Utillities.getLocalizedString("CanteenStuBistroDishes_Text");
                case "Baustellenteller":
                    return Utillities.getLocalizedString("CanteenBaustellenteller_Text");
                case "Fast Lane":
                    return Utillities.getLocalizedString("CanteenFastLane_Text");
                case "Mensa Klassiker":
                    return Utillities.getLocalizedString("CanteenCanteenClassics_Text");
                case "Mensa Spezial":
                    return Utillities.getLocalizedString("CanteenCanteenSpecial_Text");
                case "Self-Service Grüne Mensa":
                    return Utillities.getLocalizedString("CanteenSelf-ServiceGreenCanteen_Text");
                case "Self-Service Arcisstraße":
                    return Utillities.getLocalizedString("CanteenSelf-ServiceArcisstraße_Text");
                case "Self-Service":
                    return Utillities.getLocalizedString("CanteenSelf-Service_Text");
                case "Aktion":
                    return Utillities.getLocalizedString("CanteenSpecialDishes_Text");
                case "Beilagen":
                    return Utillities.getLocalizedString("CanteenSideDishes_Text");
                case "Tagesdessert":
                    return Utillities.getLocalizedString("CanteenDessertOfTheDay_Text");
                case "Dessert":
                    return Utillities.getLocalizedString("CanteenDessert_Text");
                default:
                    return dishType;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            dishType_tbx.Text = getTranslateDishType() + ':';
        }

        #endregion

    }
}
