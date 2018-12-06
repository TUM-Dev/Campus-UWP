using System.Collections.Generic;
using System.Text;
using TUMCampusApp.Classes;
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
            StringBuilder sb = new StringBuilder();
            addIngredients(sb, CanteenDishDBManager.INGREDIENTS_EMOJI_MISC_LOOKUP);
            sb.Append('\n');
            addIngredients(sb, CanteenDishDBManager.INGREDIENTS_EMOJI_ADDITIONALS_LOOKUP);
            sb.Append('\n');
            addIngredients(sb, CanteenDishDBManager.INGREDIENTS_EMOJI_ALLERGENS_LOOKUP);
            ingredients_tblck.Text = sb.ToString();
        }

        private void addIngredients(StringBuilder sb, Dictionary<string, string> ingredients)
        {
            foreach (var pair in ingredients)
            {
                sb.Append(pair.Value);
                sb.Append('\t');
                sb.Append(UiUtils.getLocalizedString("Ingredient_" + pair.Key));
                sb.Append('\n');
            }
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
