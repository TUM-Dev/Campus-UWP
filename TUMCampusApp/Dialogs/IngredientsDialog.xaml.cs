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
            ingredients_tblck.Text = CanteenDishManager.INSTANCE.replaceDishStringWithEmojis(Utillities.getLocalizedString("IngredientsDialogIngredients_text"), false);
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
