using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Dialogs
{
    public sealed partial class IngredientsDialog : ContentDialog
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 16/01/2018 Created [Fabian Sauter]
        /// </history>
        public IngredientsDialog()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Shows the ingredients string on the screen.
        /// </summary>
        private void showIngredients()
        {
            string s =
                  "(1)\t with dyestuff\n"
                + "(2)\t with preservative\n"
                + "(3)\t with antioxidant\n"
                + "(4)\t with flavor enhancers\n"
                + "(5)\t sulphured\n"
                + "(6)\t blackened (olive)\n"
                + "(8)\t with phosphate\n"
                + "(9)\t with sweeteners\n"
                + "(10)\t contains a source of phenylalanine\n"
                + "(11)\t with sugar and sweeteners\n"
                + "(13)\t with cocoa-containing grease\n"
                + "(14)\t with gelatin\n"
                + "(99)\t dish with alcohol\n"
                + "\n"
                + "(f)\t meatless dish\n"
                + "(v)\t vegan dish\n"
                + "(S)\t dish with pork\n"
                + "(R)\t dish with beef\n"
                + "(K)\t dish with veal\n"
                + "(GQB)\t Certified Quality - Bavaria\n"
                + "(MSC)\t Marine Stewardship Council\n"
                + "(Kn)\t dish with garlic\n"
                + "(Ei)\t dish with chicken egg\n"
                + "(En)\t dish with peanut\n"
                + "(Fi)\t dish with fish\n"
                + "(Gl)\t dish with gluten-containing cereals\n"
                + "(GlW)\t dish with wheat\n"
                + "(GlR)\t dish with rye\n"
                + "(GlG)\t dish with barley\n"
                + "(GlH)\t dish with oats\n"
                + "(GlD)\t dish with spelt\n"
                + "(Kr)\t dish with crustaceans\n"
                + "(Lu)\t dish with lupines\n"
                + "(Mi)\t dish with milk and lactose\n"
                + "(Sc)\t dish with shell fruits\n"
                + "(ScM)\t dish with almonds\n"
                + "(ScH)\t dish with hazelnuts\n"
                + "(ScW)\t dish with Walnuts\n"
                + "(ScC)\t dish with cashew nuts\n"
                + "(ScP)\t dish with pistachios\n"
                + "(ScP)\t dish with pistachios\n"
                + "(Se)\t dish with sesame seeds\n"
                + "(Sf)\t dish with mustard\n"
                + "(Sl)\t dish with celery\n"
                + "(So)\t dish with soy\n"
                + "(Sw)\t dish with sulfur dioxide and sulfites\n"
                + "(Wt)\t dish with mollusks\n";

            ingredients_tblck.Text = CanteenDishManager.INSTANCE.replaceDishStringWithEmojis(s, false);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            showIngredients();
        }

        #endregion
    }
}
