using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.Canteens;
using TUMCampusApp.Classes.Helpers;
using TUMCampusAppAPI.Managers;
using TUMCampusApp.Controls;
using Windows.Data.Json;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Devices.Notification;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using TUMCampusAppAPI;

namespace TUMCampusApp.Pages
{
    public sealed partial class CanteensPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private Canteen currentCanteen;
        private int currentDayOffset;
        private bool messageBoxShown;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
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
            this.messageBoxShown = false;
        }
        
        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void setNewFavoriteCanteen(Canteen canteen)
        {
            currentCanteen = canteen;
            UserDataManager.INSTANCE.setLastSelectedCanteenId(canteen.id);
            selectedCanteen_tbx.Text = canteen.name;
            expand_btn.Content = "\xE019";
            canteens_scv.Visibility = Visibility.Collapsed;
            Task.Factory.StartNew(() => showCurrentMenus());
        }

        private void setMenuType(string name, bool contains, DateTime date)
        {
            //Description:
            TextBlock tb = new TextBlock()
            {
                Text = name + ':',
                Margin = new Thickness(10, 10, 10, 10),
                FontWeight = FontWeights.ExtraBold
            };
            tb.FontSize += 5;
            menus_sckl.Children.Add(tb);

            //Line:
            Rectangle rect = new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 2,
                Fill = tb.Foreground,
                Margin = new Thickness(10, 0, 10, 0)
            };
            menus_sckl.Children.Add(rect);

            List<CanteenMenu> list = CanteenMenueManager.INSTANCE.getMenusForType(currentCanteen.id, name, contains, date);
            if (list == null)
            {
                return;
            }

            //Menus:
            foreach (CanteenMenu m in list)
            {
                menus_sckl.Children.Add(new CanteenMenuControl(m));
            }
        }

        private string getRandomMenus()
        {
            string s = "Main Course:\n";
            Random r = new Random();
            DateTime date = CanteenMenueManager.getFirstNextDate();
            if (date.Equals(DateTime.MaxValue))
            {
                date = DateTime.Now;
            }
            date = date.AddDays(currentDayOffset);
            List<CanteenMenu> tMenu = CanteenMenueManager.INSTANCE.getMenusForType(currentCanteen.id, "Tagesgericht", true, date);
            List<CanteenMenu> aMenu = CanteenMenueManager.INSTANCE.getMenusForType(currentCanteen.id, "Aktionsessen", true, date);
            List<CanteenMenu> bMenu = CanteenMenueManager.INSTANCE.getMenusForType(currentCanteen.id, "Beilagen", false, date);

            if(aMenu == null || aMenu.Count <= 0 || r.Next(0,4) != 0)
            {
                if(tMenu != null && tMenu.Count > 0)
                {
                    s += "-" + tMenu[r.Next(0,tMenu.Count)].name + "\n";
                }
            }
            else
            {
                s += "-" + aMenu[r.Next(0, aMenu.Count)].name + "\n";
            }

            s += "\nSide Dishes:\n";

            for(int i = 0; i < 2; i++)
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
        private void lockRefreshButtons()
        {
            refreshAll_btn.IsEnabled = false;
            refreshCanteenMenus_btn.IsEnabled = false;
            refreshCanteen_btn.IsEnabled = false;
        }

        private void releaseRefreshButtons()
        {
            refreshAll_btn.IsEnabled = true;
            refreshCanteenMenus_btn.IsEnabled = true;
            refreshCanteen_btn.IsEnabled = true;
        }

        private async Task loadCanteensAsync()
        {
            List<Canteen> list = await LocationManager.INSTANCE.getCanteensAsync();
            if(list == null || list.Count < 1)
            {
                return;
            }
            Canteen temp = null;
            int id = UserDataManager.INSTANCE.getLastSelectedCanteenId();
            if (id >= 0)
            {
                foreach (Canteen c in list)
                {
                    if(c.id == id)
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
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
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

        private void showCurrentMenus()
        {
            if (currentCanteen == null)
            {
                return;
            }
            DateTime date = CanteenMenueManager.getFirstNextDate();
            if (date.Equals(DateTime.MaxValue))
            {
                date = DateTime.Now;
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                menus_sckl.Children.Clear();
                date = date.AddDays(currentDayOffset);

                setMenuType("Tagesgericht", true, date);
                setMenuType("Aktionsessen", true, date);
                setMenuType("Aktion", false, date);
                setMenuType("Beilagen", true, date);

                date = date.AddDays(1);
                day_tbx.Text = date.DayOfWeek.ToString() + ' ' + date.Day + '.' + date.Month + '.' + date.Year;
            }).AsTask().Wait();
        }

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
            dialog.Title = "Ingredients:";
            await dialog.ShowAsync();
        } 

        private void initAcc()
        {
            CustomAccelerometer.Shaken += CustomAccelerometer_ShakenAsync;
            CustomAccelerometer.Enabled = true;
        }

        private async void loadCanteensAndMenusTask()
        {
            await CanteenManager.INSTANCE.downloadCanteensAsync(false);
            await loadCanteensAsync();

            await CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(false);
            showCurrentMenus();
            initAcc();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
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
                expand_btn.Content = "\xE019";
                canteens_scv.Visibility = Visibility.Collapsed;
            }
            else
            {
                expand_btn.Content = "\xE018";
                canteens_scv.Visibility = Visibility.Visible;
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
            if(!refreshCanteen_btn.IsEnabled || !refreshCanteenMenus_btn.IsEnabled)
            {
                return;
            }
            lockRefreshButtons();
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() => 
            {
                CanteenManager.INSTANCE.downloadCanteensAsync(true).Wait();
                loadCanteensAsync().Wait();
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    progressBar.Visibility = Visibility.Collapsed;
                    releaseRefreshButtons();
                }).AsTask().Wait();
            });
        }

        private void refreshCanteenMenus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!refreshCanteenMenus_btn.IsEnabled)
            {
                return;
            }
            lockRefreshButtons();
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(true).Wait();
                showCurrentMenus();
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    progressBar.Visibility = Visibility.Collapsed;
                    releaseRefreshButtons();
                }).AsTask().Wait();
            });
        }

        private void refreshAll_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!refreshCanteen_btn.IsEnabled)
            {
                return;
            }
            lockRefreshButtons();
            progressBar.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                CanteenManager.INSTANCE.downloadCanteensAsync(true).Wait();
                loadCanteensAsync().Wait();
                CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(true).Wait();
                showCurrentMenus();
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    progressBar.Visibility = Visibility.Collapsed;
                    releaseRefreshButtons();
                }).AsTask().Wait();
            });
        }

        private void right_btn_Click(object sender, RoutedEventArgs e)
        {
            currentDayOffset++;
            if (currentDayOffset >= 7)
            {
                currentDayOffset = 0;
            }
            Task.Factory.StartNew(() => showCurrentMenus());
        }

        private void left_btn_Click(object sender, RoutedEventArgs e)
        {
            currentDayOffset--;
            if (currentDayOffset < 0)
            {
                currentDayOffset = 6;
            }
            Task.Factory.StartNew(() => showCurrentMenus());
        }

        private async void CustomAccelerometer_ShakenAsync(object sender, EventArgs args)
        {
            if(currentCanteen == null)
            {
                return;
            }
            MessageDialog message = new MessageDialog(getRandomMenus());
            message.Title = "Random menu:";
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
        #endregion
    }
}
