using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class CanteenWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private string canteen_id;
        private DropShadowPanel dsp;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public CanteenWidget(string canteen_id, DropShadowPanel dsp)
        {
            this.canteen_id = canteen_id;
            this.dsp = dsp;
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
        /// Shows all dishes for the selected canteen and date.
        /// </summary>
        private void showDishes()
        {
            CanteenTable canteen = CanteenManager.INSTANCE.getCanteen(canteen_id);
            if (canteen != null)
            {
                canteenName_tbx.Text = canteen.name ?? "";
                DateTime date = CanteenDishManager.getFirstNextDate(canteen_id);

                if (date != DateTime.MaxValue)
                {
                    canteenDate_tbx.Text = date.ToString("dd.MM.yyyy");

                    foreach (FavoriteCanteenDishTypeTable f in CanteenManager.INSTANCE.getDishTypesForFavoriteCanteen(canteen_id))
                    {
                        DishTypeControl dishTypeControl = null;
                        foreach (CanteenDishTable dish in CanteenDishManager.INSTANCE.getDishesForType(canteen_id, f.dish_type, false, date))
                        {
                            if(dishTypeControl == null)
                            {
                                dishTypeControl = new DishTypeControl(dish);
                            }
                            else
                            {
                                dishTypeControl.addDish(dish);
                            }
                        }
                        if(dishTypeControl != null)
                        {
                            dishTypes_stckp.Children.Add(dishTypeControl);
                        }
                    }
                }
                if (dishTypes_stckp.Children.Count <= 0)
                {
                    dishTypes_stckp.Visibility = Visibility.Collapsed;
                    canteenDate_tbx.Visibility = Visibility.Collapsed;
                }
                return;
            }
            dsp.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            showDishes();
        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Utillities.mainPage.navigateToPage(Utillities.EnumPage.CanteensPage, canteen_id);
        }

        #endregion
    }
}
