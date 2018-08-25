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
    public sealed partial class CanteensPage2 : Page, INamedPage, IBackRequestedPage
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
            this.dishDates = null;
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UIUtils.getLocalizedString("CanteenesPageName_Text");
        }

        private int getNextDishDateOffset()
        {
            if (dishDates != null && dishDates.Count > 0)
            {
                DateTime dateToday = DateTime.Now;
                if (dateToday.Hour >= 16) // If it's after 16 o' clock show the menus for the next day
                {
                    dateToday = dateToday.AddDays(1);
                }

                for (int i = 0; i < dishDates.Count; i++)
                {
                    if (dishDates[i].Date.CompareTo(dateToday.Date) >= 0)
                    {
                        return i;
                    }
                }
                return dishDates.Count - 1;
            }
            return -1;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public bool onBackRequest()
        {
            if (canteens_ctrl.Expanded)
            {
                canteens_ctrl.close();
                return true;
            }
            return false;
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Shows all dishes for the given canteen.
        /// </summary>
        /// <param name="canteen">The canteen, you want to show all dishes for.</param>
        private void showDishesForCanteen(CanteenTable canteen)
        {
            dishDates = CanteenDishDBManager.INSTANCE.getDishDates(canteen.canteen_id);
            dishDateOffset = getNextDishDateOffset();
            showDate();
            showDishesForSelctedDate();
        }

        /// <summary>
        /// Shows the currently selected date.
        /// </summary>
        private void showDate()
        {
            if (dishDates != null && dishDates.Count > 0)
            {
                left_btn.IsEnabled = true;
                right_btn.IsEnabled = true;
            }
            else
            {
                left_btn.IsEnabled = false;
                right_btn.IsEnabled = false;
            }

            if (dishDateOffset >= 0)
            {
                day_tbx.Text = UIUtils.getLocalizedString(dishDates[dishDateOffset].DayOfWeek.ToString() + "_Text") + ", " + dishDates[dishDateOffset].ToString("dd.MM.yyyy");
            }
            else
            {
                day_tbx.Text = UIUtils.getLocalizedString("CanteenNoMenusFound_Text");
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
                    foreach (CanteenDishTable dish in CanteenDishDBManager.INSTANCE.getDishes(canteens_ctrl.Canteen.canteen_id, dishDates[dishDateOffset]))
                    {
                        if (dishTypeControl == null || !Equals(dish.dish_type, dishTypeControl.dishType))
                        {
                            dishTypeControl = new DishTypeControl();
                            dishType_stckp.Children.Add(dishTypeControl);
                        }
                        dishTypeControl.addDish(dish);
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
        /// Refreshes all canteens and dishes.
        /// </summary>
        /// <param name="force">Whether to force the refresh.</param>
        private void refreshAll(bool force)
        {
            disableRefreshButtons();
            loading_prgb.Visibility = Visibility.Visible;
            Task.Run(async () =>
            {
                Task t2 = CanteenDBManager.INSTANCE.downloadCanteens(force);
                if (t2 != null)
                {
                    await t2;
                }
                Task t = CanteenDishDBManager.INSTANCE.downloadCanteenDishes(force);
                if (t != null)
                {
                    await t;
                }
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
                left_btn.IsEnabled = true;
                right_btn.IsEnabled = true;

                showDishesForCanteen(args.SELECTED_CANTEEN);
            }
            else
            {
                left_btn.IsEnabled = false;
                right_btn.IsEnabled = false;
            }
        }

        private void left_btn_Click(object sender, RoutedEventArgs e)
        {
            if (dishDates != null)
            {
                if (dishDateOffset > 0)
                {
                    dishDateOffset--;
                }
                else
                {
                    dishDateOffset = dishDates.Count - 1;
                }
                showDate();
                showDishesForSelctedDate();
            }
        }

        private void right_btn_Click(object sender, RoutedEventArgs e)
        {
            if (dishDates != null)
            {
                if (dishDateOffset < dishDates.Count - 1)
                {
                    dishDateOffset++;
                }
                else
                {
                    dishDateOffset = 0;
                }
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
                contentOverlay_grid.Visibility = Visibility.Visible;
            }
            else
            {
                more_btn.Visibility = Visibility.Visible;
                contentOverlay_grid.Visibility = Visibility.Collapsed;
            }
        }

        private void refreshAll_btn_Click(object sender, RoutedEventArgs e)
        {
            disableRefreshButtons();
            loading_prgb.Visibility = Visibility.Visible;
            Task.Run(async () =>
            {
                Task t2 = CanteenDBManager.INSTANCE.downloadCanteens(true);
                if (t2 != null)
                {
                    await t2;
                }
                Task t = CanteenDishDBManager.INSTANCE.downloadCanteenDishes(true);
                if (t != null)
                {
                    await t;
                }
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
            Task.Run(async () =>
            {
                Task t2 = CanteenDBManager.INSTANCE.downloadCanteens(true);
                if (t2 != null)
                {
                    await t2;
                }
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
            Task.Run(async () =>
            {
                Task t = CanteenDishDBManager.INSTANCE.downloadCanteenDishes(true);
                if (t != null)
                {
                    await t;
                }
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

        private void CustomAccelerometer_Shaken(object sender, EventArgs e)
        {
            // ToDo: Show random menu
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
            Task.Run(async () =>
            {
                Task t2 = CanteenDBManager.INSTANCE.downloadCanteens(false);
                if (t2 != null)
                {
                    await t2;
                }
                Task t = CanteenDishDBManager.INSTANCE.downloadCanteenDishes(false);
                if (t != null)
                {
                    await t;
                }
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

        private void contentOverlay_grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            canteens_ctrl.close();
        }

        private async void openMap_btn_Click(object sender, RoutedEventArgs e)
        {
            if (canteens_ctrl.Canteen != null)
            {
                // Encode '~' and '-' in canteen name:
                string canteenName = canteens_ctrl.Canteen.name.Replace("_", "%255F");
                canteenName = canteenName.Replace("~", "%7E");

                // Open bing maps:
                Uri mapCanteenUri = new Uri(@"bingmaps:?collection=point." + canteens_ctrl.Canteen.latitude + "_" + canteens_ctrl.Canteen.longitude + "_" + canteenName);
                await Windows.System.Launcher.LaunchUriAsync(mapCanteenUri);
            }
        }

        #endregion
    }
}
