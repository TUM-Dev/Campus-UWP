using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Controls;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using TUMCampusApp.Dialogs;
using TUMCampusAppAPI;
using TUMCampusApp.Classes.Helpers;
using Windows.UI.Xaml.Navigation;

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
        /// <summary>
        /// Shows all dishes for the given canteen.
        /// </summary>
        /// <param name="canteen">The canteen, you want to show all dishes for.</param>
        private void showDishesForCanteen(CanteenTable canteen)
        {
            dishDates = CanteenDishManager.INSTANCE.getDishDates(canteen.canteen_id);
            if (dishDates != null)
            {
                if (dishDates.Count <= dishDateOffset)
                {
                    dishDateOffset = dishDates.Count - 1;
                }
                else if (dishDates.Count > 0 && dishDateOffset < 0)
                {
                    dishDateOffset = 0;
                }
                showDate();
                showDishesForSelctedDate();
            }
        }

        /// <summary>
        /// Shows the currently selected date.
        /// </summary>
        private void showDate()
        {
            if (dishDateOffset >= 0)
            {
                day_tbx.Text = Utillities.getLocalizedString(dishDates[dishDateOffset].AddDays(1).DayOfWeek.ToString() + "_Text") + ", " + dishDates[dishDateOffset].AddDays(1).ToString("dd.MM.yyyy");
            }
            else
            {
                day_tbx.Text = Utillities.getLocalizedString("CanteenNoMenusFound_Text");
            }
        }

        /// <summary>
        /// Shows all dishes for the selected canteen and date.
        /// </summary>
        private void showDishesForSelctedDate()
        {
            if (canteens_ctrl.Canteen != null)
            {
                dishType_stckp.Children.Clear();
                if (dishDateOffset >= 0 && dishDates.Count > dishDateOffset)
                {
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
        }

        /// <summary>
        /// Disables the refresh buttons.
        /// </summary>
        private void disableRefreshButtons()
        {
            refreshAll_btn.IsEnabled = false;
            refreshCanteenDishes_btn.IsEnabled = false;
            refreshCanteen_btn.IsEnabled = false;
        }

        /// <summary>
        /// Enables all refresh buttons.
        /// </summary>
        private void enableRefreshButtons()
        {
            refreshAll_btn.IsEnabled = true;
            refreshCanteenDishes_btn.IsEnabled = true;
            refreshCanteen_btn.IsEnabled = true;
        }

        /// <summary>
        /// Starts the custom accelerometer for detecting shaking.
        /// </summary>
        private void initAcc()
        {
            CustomAccelerometer.Shaken += CustomAccelerometer_Shaken;
            CustomAccelerometer.Enabled = true;
        }

        /// <summary>
        /// Shows a dialog with a random menu for the currently selected canteen and date.
        /// </summary>
        private async Task showRandomMenuAsync()
        {

        }

        private void refreshAll(bool force)
        {
            disableRefreshButtons();
            loading_prgb.Visibility = Visibility.Visible;
            Task.Factory.StartNew(async () =>
            {
                await CanteenManager.INSTANCE.downloadCanteensAsync(force);
                await CanteenDishManager.INSTANCE.downloadCanteenDishesAsync(force);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    showDishesForSelctedDate();
                    await canteens_ctrl.reloadCanteensAsync(null);
                    showDishesForSelctedDate();
                    loading_prgb.Visibility = Visibility.Collapsed;
                    enableRefreshButtons();
                }).AsTask();
            });
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

        private void left_btn_Click(object sender, RoutedEventArgs e)
        {
            if (dishDates != null && dishDateOffset > 0)
            {
                dishDateOffset--;
                showDate();
                showDishesForSelctedDate();
            }
        }

        private void right_btn_Click(object sender, RoutedEventArgs e)
        {
            if (dishDates != null && dishDateOffset < dishDates.Count - 1 && dishDateOffset >= 0)
            {
                dishDateOffset++;
                showDate();
                showDishesForSelctedDate();
            }
        }

        private void canteens_ctrl_ExpandedChanged(CanteensControl canteensControl, Classes.Events.ExpandedChangedEventArgs args)
        {
            if (args.EXPANDED)
            {
                more_btn.Visibility = Visibility.Collapsed;
                more_btn.Flyout.Hide();
            }
            else
            {
                more_btn.Visibility = Visibility.Visible;
            }
        }

        #endregion

        private void refreshAll_btn_Click(object sender, RoutedEventArgs e)
        {
            disableRefreshButtons();
            loading_prgb.Visibility = Visibility.Visible;
            Task.Factory.StartNew(async () =>
            {
                await CanteenManager.INSTANCE.downloadCanteensAsync(true);
                await CanteenDishManager.INSTANCE.downloadCanteenDishesAsync(true);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    showDishesForSelctedDate();
                    await canteens_ctrl.reloadCanteensAsync(null);
                    if (canteens_ctrl.Canteen != null)
                    {
                        showDishesForCanteen(canteens_ctrl.Canteen);
                    }
                    loading_prgb.Visibility = Visibility.Collapsed;
                    enableRefreshButtons();
                }).AsTask();
            });
        }

        private void refreshCanteen_btn_Click(object sender, RoutedEventArgs e)
        {
            disableRefreshButtons();
            loading_prgb.Visibility = Visibility.Visible;
            Task.Factory.StartNew(async () =>
            {
                await CanteenManager.INSTANCE.downloadCanteensAsync(true);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    loading_prgb.Visibility = Visibility.Collapsed;
                    await canteens_ctrl.reloadCanteensAsync(null);
                    enableRefreshButtons();
                }).AsTask();
            });
        }

        private void refreshCanteenDishes_btn_Click(object sender, RoutedEventArgs e)
        {
            disableRefreshButtons();
            loading_prgb.Visibility = Visibility.Visible;
            Task.Factory.StartNew(async () =>
            {
                await CanteenDishManager.INSTANCE.downloadCanteenDishesAsync(true);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    showDishesForSelctedDate();
                    await canteens_ctrl.reloadCanteensAsync(null);
                    if (canteens_ctrl.Canteen != null)
                    {
                        showDishesForCanteen(canteens_ctrl.Canteen);
                    }
                    loading_prgb.Visibility = Visibility.Collapsed;
                    enableRefreshButtons();
                }).AsTask();
            });
        }

        /// <summary>
        /// Pins the currently selected canteen to the start.
        /// </summary>
        private void pinCanteenTile_btn_Click(object sender, RoutedEventArgs e)
        {
            if (canteens_ctrl.Canteen != null)
            {
                TileHelper.PinTileAsync(canteens_ctrl.Canteen.name, canteens_ctrl.Canteen.name, canteens_ctrl.Canteen.canteen_id, "Assets/Images/CanteenTile.png");
            }
        }

        private async void info_btn_Click(object sender, RoutedEventArgs e)
        {
            IngredientsDialog dialog = new IngredientsDialog();
            await dialog.ShowAsync();
        }

        private async void CustomAccelerometer_Shaken(object sender, EventArgs e)
        {
            await showRandomMenuAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string param = null;
            if (e.Parameter is string)
            {
                param = e.Parameter as string;
            }

            disableRefreshButtons();
            loading_prgb.Visibility = Visibility.Visible;
            Task.Factory.StartNew(async () =>
            {
                await CanteenManager.INSTANCE.downloadCanteensAsync(false);
                await CanteenDishManager.INSTANCE.downloadCanteenDishesAsync(false);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await canteens_ctrl.reloadCanteensAsync(param);
                    if (canteens_ctrl.Canteen != null)
                    {
                        showDishesForCanteen(canteens_ctrl.Canteen);
                    }
                    loading_prgb.Visibility = Visibility.Collapsed;
                    enableRefreshButtons();
                }).AsTask();
            });
        }
    }
}
