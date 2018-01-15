using System;
using System.Collections.Generic;
using TUMCampusApp.Classes;
using TUMCampusApp.Controls;
using TUMCampusAppAPI.Canteens;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml.Controls;


namespace TUMCampusApp.Pages
{
    public sealed partial class CanteensPage2 : Page, INamedPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private List<DateTime> dishDates;
        private int dishDateOffset;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/01/2018 Created [Fabian Sauter]
        /// </history>
        public CanteensPage2()
        {
            this.dishDateOffset = 0;
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return Utillities.getLocalizedString("CanteenesPageName_Text");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void showDishesForCanteen(CanteenTable canteen)
        {
            dishDates = CanteenDishManager.INSTANCE.getDishDates(canteen.canteen_id);
            if (dishDates != null && dishDates.Count > 0)
            {
                if (dishDates.Count <= dishDateOffset)
                {
                    dishDateOffset = dishDates.Count - 1;
                }
                showDate();
                showDishesForSelctedDate();
            }
        }

        private void showDate()
        {
            day_tbx.Text = Utillities.getLocalizedString(dishDates[dishDateOffset].DayOfWeek.ToString() + "_Text") + ", " + dishDates[dishDateOffset].ToString("dd.MM.yyyy");
        }

        private void showDishesForSelctedDate()
        {
            if (canteens_ctrl.Canteen != null)
            {
                dishType_stckp.Children.Clear();
                DishTypeControl dishTypeControl = null;
                foreach (CanteenDishTable dish in CanteenDishManager.INSTANCE.getDishes(canteens_ctrl.Canteen.canteen_id, dishDates[dishDateOffset]))
                {
                    if (dishTypeControl == null || !Equals(dish.dish_type, dishTypeControl.dishType))
                    {
                        dishTypeControl = new DishTypeControl(dish);
                        dishType_stckp.Children.Add(dishTypeControl);
                    }
                    else
                    {
                        dishTypeControl.addDish(dish);
                    }
                }
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void CanteensControl_CanteenSelectionChanged(CanteensControl canteensControl, Classes.Events.CanteenSelectionChangedEventArgs args)
        {
            if (args.SELECTED_CANTEEN != null)
            {
                showDishesForCanteen(args.SELECTED_CANTEEN);
            }
        }

        private void left_btn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (dishDates != null && dishDateOffset > 0)
            {
                dishDateOffset--;
                showDate();
                showDishesForSelctedDate();
            }
        }

        private void right_btn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (dishDates != null && dishDateOffset < dishDates.Count - 1)
            {
                dishDateOffset++;
                showDate();
                showDishesForSelctedDate();
            }
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (canteens_ctrl.Canteen != null)
            {
                showDishesForCanteen(canteens_ctrl.Canteen);
            }
        }

        #endregion
    }
}
