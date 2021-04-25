using System.Collections.Generic;
using System.Text;
using Canteens.Classes.Manager;
using Shared.Classes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Dialogs
{
    public sealed partial class IngredientsDialog: ContentDialog
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public string Ingredients
        {
            get => (string)GetValue(IngredientsProperty);
            set => SetValue(IngredientsProperty, value);
        }
        public static readonly DependencyProperty IngredientsProperty = DependencyProperty.Register(nameof(Ingredients), typeof(string), typeof(IngredientsDialog), new PropertyMetadata(""));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public IngredientsDialog()
        {
            InitializeComponent();
            LoadIngredients();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void LoadIngredients()
        {
            StringBuilder sb = new StringBuilder();
            AddIngredients(sb, DishManager.INGREDIENTS_EMOJI_MISC_LOOKUP);
            sb.Append('\n');
            AddIngredients(sb, DishManager.INGREDIENTS_EMOJI_ADDITIONALS_LOOKUP);
            sb.Append('\n');
            AddIngredients(sb, DishManager.INGREDIENTS_EMOJI_ALLERGENS_LOOKUP);
            Ingredients = sb.ToString();
        }

        private void AddIngredients(StringBuilder sb, Dictionary<string, string> ingredients)
        {
            foreach (KeyValuePair<string, string> pair in ingredients)
            {
                sb.Append(pair.Value);
                sb.Append('\t');
                sb.Append(Localisation.GetLocalizedString("Ingredient_" + pair.Key));
                sb.Append('\n');
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
