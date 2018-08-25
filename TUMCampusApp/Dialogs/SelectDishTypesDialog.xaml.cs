using System.Collections.Generic;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Dialogs
{
    public sealed partial class SelectDishTypesDialog : ContentDialog
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public string Canteen_id
        {
            get { return (string)GetValue(Canteen_idProperty); }
            set { SetValue(Canteen_idProperty, value); }
        }
        public static readonly DependencyProperty Canteen_idProperty = DependencyProperty.Register("Canteen_id", typeof(string), typeof(SelectDishTypesDialog), null);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 19/01/2018 Created [Fabian Sauter]
        /// </history>
        public SelectDishTypesDialog()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns all selected dish types.
        /// </summary>
        /// <returns>A list of selected dish types.</returns>
        public List<string> getSelectedDishTypes()
        {
            return dishes_contrl.getSelectedDishTypes();
        }

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
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if(Canteen_id != null)
            {
                CanteenDBManager.INSTANCE.setFavoriteCanteenDishTypes(Canteen_id, dishes_contrl.getSelectedDishTypes());
            }
            Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        #endregion
    }
}
