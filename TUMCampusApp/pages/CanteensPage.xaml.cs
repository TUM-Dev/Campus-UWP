using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Canteens;
using TUMCampusApp.Classes.Helpers;
using TUMCampusAppAPI.Managers;
using TUMCampusApp.Controls;
using Windows.Phone.Devices.Notification;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using TUMCampusAppAPI;
using TUMCampusApp.Classes;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Pages
{
    public sealed partial class CanteensPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private Canteen currentCanteen;
        private int currentDayOffset;
        private List<DateTime> menuDates;
        private bool messageBoxShown;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteensPage()
        {
            this.InitializeComponent();
            this.currentDayOffset = 0;
            this.menuDates = null;
            this.messageBoxShown = false;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Sets the given canteen as the new favourite canteen.
        /// </summary>
        /// <param name="canteen">The canteen that should be the new favourite one.</param>
        private void setNewFavoriteCanteen(Canteen canteen)
        {
            currentCanteen = canteen;
            UserDataManager.INSTANCE.setLastSelectedCanteenId(canteen.id);
            selectedCanteen_tbx.Text = canteen.name;
            collapseCanteens();
            Task.Factory.StartNew(() => showCurrentMenus());
        }

        /// <summary>
        /// Displays all menus that match the given name, date and the current canteen id.
        /// </summary>
        /// <param name="name">The "typeLong" of the canteen menus.</param>
        /// <param name="contains">Whether the menus typeLong should be equal or just contain the given name.</param>
        /// <param name="date">The menus date.</param>
        private void setMenuType(string name, string labelText, bool contains, DateTime date)
        {
            List<CanteenMenu> list = CanteenMenueManager.INSTANCE.getMenusForType(currentCanteen.id, name, contains, date);
            if (list == null || list.Count <= 0)
            {
                return;
            }
            Brush brushLine = Resources["ApplicationPressedForegroundThemeBrush"] as Brush;
            Brush brushText = Resources["CalendarDatePickerTextForeground"] as Brush;

            //Description:
            TextBlock tb = new TextBlock()
            {
                Text = labelText + ':',
                Margin = new Thickness(10, 10, 10, 10),
                Foreground = brushText
            };
            menus_sckl.Children.Add(tb);

            //Line:
            Rectangle rect = new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 2,
                Fill = brushLine,
                Margin = new Thickness(10, 0, 10, 0)
            };
            menus_sckl.Children.Add(rect);

            //Menus:
            foreach (CanteenMenu m in list)
            {
                menus_sckl.Children.Add(new CanteenMenuControl(m));
            }
        }

        /// <summary>
        /// Returns a random menu as a string.
        /// </summary>
        private string getRandomMenus()
        {
            string s = Utillities.getLocalizedString("CanteenMainCourse_Text") + ":\n";
            Random r = new Random();
            if (menuDates == null && menuDates.Count <= 0)
            {
                return Utillities.getLocalizedString("CanteenNoMenusFoundFor_Text") + currentCanteen.name;
            }
            DateTime date = menuDates[0];
            if (date.Equals(DateTime.MaxValue))
            {
                date = DateTime.Now;
            }
            date = menuDates[currentDayOffset];
            List<CanteenMenu> tMenu = CanteenMenueManager.INSTANCE.getMenusForType(currentCanteen.id, "Tagesgericht", true, date);
            List<CanteenMenu> aMenu = CanteenMenueManager.INSTANCE.getMenusForType(currentCanteen.id, "Aktionsessen", true, date);
            List<CanteenMenu> bMenu = CanteenMenueManager.INSTANCE.getMenusForType(currentCanteen.id, "Beilagen", false, date);

            if (aMenu == null || aMenu.Count <= 0 || r.Next(0, 4) != 0)
            {
                if (tMenu != null && tMenu.Count > 0)
                {
                    s += "-" + tMenu[r.Next(0, tMenu.Count)].nameEmojis + "\n";
                }
            }
            else
            {
                s += "-" + aMenu[r.Next(0, aMenu.Count)].nameEmojis + "\n";
            }

            s += "\n" + Utillities.getLocalizedString("CanteenSideDishes_Text") + ":\n";

            for (int i = 0; i < 2; i++)
            {
                if (bMenu != null && bMenu.Count > 0)
                {
                    s += "-" + bMenu[r.Next(0, bMenu.Count)].name + "\n";
                }
            }

            return s;
        }
        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Collapses the canteens.
        /// </summary>
        private void collapseCanteens()
        {
            expand_btn.Content = "\xE019";
            canteens_scv.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Expands the canteens.
        /// </summary>
        private void expandCanteens()
        {
            expand_btn.Content = "\xE018";
            canteens_scv.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Disables the refresh buttons.
        /// </summary>
        private void disableRefreshButtons()
        {
            refreshAll_btn.IsEnabled = false;
            refreshCanteenMenus_btn.IsEnabled = false;
            refreshCanteen_btn.IsEnabled = false;
        }

        /// <summary>
        /// Enables all refresh buttons.
        /// </summary>
        private void enableRefreshButtons()
        {
            refreshAll_btn.IsEnabled = true;
            refreshCanteenMenus_btn.IsEnabled = true;
            refreshCanteen_btn.IsEnabled = true;
        }

        /// <summary>
        /// Loads all canteens and shows them on the screen. This method should be only called in a separate task.
        /// </summary>
        /// <returns></returns>
        private async Task loadCanteensAsync()
        {
            List<Canteen> list = await LocationManager.INSTANCE.getCanteensAsync();
            if (list == null || list.Count < 1)
            {
                return;
            }
            Canteen temp = null;
            int id = UserDataManager.INSTANCE.getLastSelectedCanteenId();
            if (id >= 0)
            {
                foreach (Canteen c in list)
                {
                    if (c.id == id)
                    {
                        temp = c;
                        break;
                    }
                }
            }
            if (temp == null)
            {
                temp = list[0];
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                selectedCanteen_tbx.Text = temp.name;
                currentCanteen = temp;
                foreach (Canteen c in list)
                {
                    CanteenControl cC = new CanteenControl(c);
                    cC.HorizontalAlignment = HorizontalAlignment.Stretch;
                    cC.PointerReleased += canteen_Click;
                    canteens_sckl.Children.Add(cC);
                }
            }).AsTask();
        }

        /// <summary>
        /// Loads and shows all menus for the currently selected canteen and the current date offset.
        /// This method should be only called in a separate task.
        /// </summary>
        private void showCurrentMenus()
        {
            if (currentCanteen == null)
            {
                return;
            }
            menuDates = CanteenMenueManager.INSTANCE.getMenuDates(currentCanteen.id);
            if (menuDates == null || menuDates.Count <= 0)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    menus_sckl.Children.Clear();
                    day_tbx.Text = "";
                    menus_sckl.Children.Add(new TextBlock()
                    {
                        Text = Utillities.getLocalizedString("CanteenNoMenusFound_Text"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 25
                    });
                }).AsTask();
                return;
            }

            DateTime date = menuDates[0];

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                menus_sckl.Children.Clear();
                date = menuDates[currentDayOffset];

                setMenuType("Tagesgericht", Utillities.getLocalizedString("CanteenDishOfTheDay_Text"), true, date);
                setMenuType("Aktionsessen", Utillities.getLocalizedString("CanteenActionDishes_Text"), true, date);
                setMenuType("Self-Service", Utillities.getLocalizedString("CanteenSelf-Service_Text"), false, date);
                setMenuType("Aktion", Utillities.getLocalizedString("CanteenSpecialDishes_Text"), false, date);
                setMenuType("Beilagen", Utillities.getLocalizedString("CanteenSideDishes_Text"), true, date);

                date = date.AddDays(1);
                day_tbx.Text = Utillities.getLocalizedString(date.DayOfWeek.ToString() + "_Text") + ", " + date.ToString("dd.MM.yyyy");
            }).AsTask().Wait();
        }

        /// <summary>
        /// Shows an info box for all ingredients.
        /// </summary>
        private async Task showInfoBoxAsync()
        {
            string s = "(1)\t with dyestuff\n"
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
                + "(f)\t meathless dish\n"
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
                + "(GlG)\t dish with gerst\n"
                + "(GlH)\t dish with oats\n"
                + "(GlD)\t dish with spelt\n"
                + "(Kr)\t dish with crustaceans\n"
                + "(Lu)\t dish with lupines\n"
                + "(Mi)\t dish with milk and lactose\n"
                + "(Sc)\t dish with shell fruits\n"
                + "(ScM)\t dish with almonds\n"
                + "(ScH)\t dish with hazlenuts\n"
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
            MessageDialog dialog = new MessageDialog(CanteenMenueManager.INSTANCE.replaceMenuStringWithImages(s, false));
            dialog.Title = Utillities.getLocalizedString("CanteenIngredients_Text");
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Starts the custom accelerometer for detecting shaking.
        /// </summary>
        private void initAcc()
        {
            CustomAccelerometer.Shaken += CustomAccelerometer_ShakenAsync;
            CustomAccelerometer.Enabled = true;
        }

        /// <summary>
        /// Loads and shows all canteens and their menus.
        /// This method should be only called in a separate task.
        /// </summary>
        private async void loadCanteensAndMenusTask()
        {
            await CanteenManager.INSTANCE.downloadCanteensAsync(false);
            await loadCanteensAsync();

            await CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(false);
            showCurrentMenus();
            initAcc();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                progressBar.Visibility = Visibility.Collapsed;
            }).AsTask();
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => loadCanteensAndMenusTask());
        }

        private void expand_btn_Click(object sender, RoutedEventArgs e)
        {
            if (canteens_scv.Visibility == Visibility.Visible)
            {
                collapseCanteens();
            }
            else
            {
                expandCanteens();
            }
        }

        private async void info_btn_Click(object sender, RoutedEventArgs e)
        {
            await showInfoBoxAsync();
        }

        private void canteen_Click(object sender, RoutedEventArgs e)
        {
            setNewFavoriteCanteen((sender as CanteenControl).canteen);
        }

        private void refreshCanteen_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!refreshCanteen_btn.IsEnabled || !refreshCanteenMenus_btn.IsEnabled)
            {
                return;
            }
            disableRefreshButtons();
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                CanteenManager.INSTANCE.downloadCanteensAsync(true).Wait();
                loadCanteensAsync().Wait();
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    progressBar.Visibility = Visibility.Collapsed;
                    enableRefreshButtons();
                }).AsTask().Wait();
            });
        }

        private void refreshCanteenMenus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!refreshCanteenMenus_btn.IsEnabled)
            {
                return;
            }
            disableRefreshButtons();
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(true).Wait();
                showCurrentMenus();
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    progressBar.Visibility = Visibility.Collapsed;
                    enableRefreshButtons();
                }).AsTask().Wait();
            });
        }

        private void refreshAll_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!refreshCanteen_btn.IsEnabled)
            {
                return;
            }
            disableRefreshButtons();
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                CanteenManager.INSTANCE.downloadCanteensAsync(true).Wait();
                loadCanteensAsync().Wait();
                CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(true).Wait();
                showCurrentMenus();
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    progressBar.Visibility = Visibility.Collapsed;
                    enableRefreshButtons();
                }).AsTask().Wait();
            });
        }

        private void right_btn_Click(object sender, RoutedEventArgs e)
        {
            if (menuDates == null || menuDates.Count <= 0)
            {
                return;
            }
            currentDayOffset++;
            if (currentDayOffset >= menuDates.Count)
            {
                currentDayOffset = 0;
            }
            Task.Factory.StartNew(() => showCurrentMenus());
        }

        private void left_btn_Click(object sender, RoutedEventArgs e)
        {
            if (menuDates == null || menuDates.Count <= 0)
            {
                return;
            }
            currentDayOffset--;
            if (currentDayOffset < 0)
            {
                currentDayOffset = menuDates.Count - 1;
            }
            Task.Factory.StartNew(() => showCurrentMenus());
        }

        private async void CustomAccelerometer_ShakenAsync(object sender, EventArgs args)
        {
            if (currentCanteen == null)
            {
                return;
            }
            MessageDialog message = new MessageDialog(getRandomMenus());
            message.Title = Utillities.getLocalizedString("CanteenRandomMenu_Text");
            try
            {
                if (!messageBoxShown)
                {
                    VibrationDevice v = VibrationDevice.GetDefault();
                    if (v != null)
                    {
                        v.Vibrate(TimeSpan.FromMilliseconds(200));
                    }
                }
                messageBoxShown = true;
                await message.ShowAsync();
                messageBoxShown = false;
            }
            catch (Exception e)
            {
                Logger.Warn("Caught an exception during shake to show a random menu:\n" + e.StackTrace);
            }
        }

        private void pinCanteenTile_btn_Click(object sender, RoutedEventArgs e)
        {
            TileHelper.PinTileAsync("Canteens", "Canteens", "canteens", "Assets/Images/CanteenTile.png");
        }

        private void menus_scv_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            collapseCanteens();
        }
        
        #endregion
    }
}
