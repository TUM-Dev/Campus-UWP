using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes;
using TUMCampusApp.classes.canteen;
using TUMCampusApp.classes.helpers;
using TUMCampusApp.classes.managers;
using TUMCampusApp.controls;
using Windows.Data.Json;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Devices.Notification;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
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

namespace TUMCampusApp.Pages
{
    public sealed partial class CanteensPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private Canteen currentCanteen;
        private int currentDayOffset;
        private MenuFlyout flyout;
        private string currentSelectedMenu;
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

            this.flyout = new MenuFlyout();
            MenuFlyoutItem fOutI = new MenuFlyoutItem();
            fOutI.Text = "Google it!";
            fOutI.Click += FOutI_ClickAsync;
            this.flyout.Items.Add(fOutI);
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
            loadCanteenMenus();
        }

        private void setMenuType(string name, bool contains, DateTime date)
        {
            //Description:
            TextBlock tb = new TextBlock()
            {
                Text = name + ':',
                Margin = new Thickness(10, 10, 10, 10)
            };
            tb.FontSize += 5;
            tb.FontWeight = FontWeights.ExtraBold;
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
                tb = new TextBlock()
                {
                    Text = m.name,
                    Margin = new Thickness(10, 10, 10, 10),
                    TextWrapping = TextWrapping.WrapWholeWords
                };
                tb.RightTapped += Tb_RightTapped;
                menus_sckl.Children.Add(tb);
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
            date = date.AddDays(currentDayOffset + 1);
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
            selectedCanteen_tbx.Text = temp.name;
            currentCanteen = temp;
            foreach (Canteen c in list)
            {
                CanteenControl cC = new CanteenControl(c);
                cC.HorizontalAlignment = HorizontalAlignment.Stretch;
                cC.PointerReleased += canteen_Click;
                canteens_sckl.Children.Add(cC);
            }
        }

        private void loadCanteenMenus()
        {
            if(currentCanteen != null)
            {
                showCurrentMenus();
            }
        }

        private void showCurrentMenus()
        {
            DateTime date = CanteenMenueManager.getFirstNextDate();
            if (date.Equals(DateTime.MaxValue))
            {
                date = DateTime.Now;
            }
            date = date.AddDays(currentDayOffset);
            menus_sckl.Children.Clear();
            setMenuType("Tagesgericht" , true, date);
            setMenuType("Aktionsessen", true, date);
            setMenuType("Aktion", false, date);
            setMenuType("Beilagen", true, date);

            date = date.AddDays(1);
            day_tbx.Text = date.DayOfWeek.ToString() + ' ' + date.Day + '.' + date.Month + '.' + date.Year;
        }

        private async Task showInfoBoxAsync()
        {
            string s = "(f) meathless dish\n"
                + "(v) vegan dish\n"
                + "(S) dish with pork\n"
                + "(R) dish with beef\n"
                + "(99) dish with alcohol\n"
                + "(GQB) Certified Quality - Bavaria\n"
                + "\n"
                + "1 with dyestuff\n"
                + "2 with preservative\n"
                + "3 with antioxidant\n"
                + "4 with flavor enhancers\n"
                + "5 sulphured\n"
                + "6 blackened (olive)\n"
                + "8 with phosphate\n"
                + "9 with sweeteners\n"
                + "10 contains a source of phenylalanine\n"
                + "11 with sugar and sweeteners";
            MessageDialog dialog = new MessageDialog(CanteenMenueManager.replaceMenuStringWithImages(s));
            dialog.Title = "Ingredients:";
            await dialog.ShowAsync();
        } 

        private void initAcc()
        {
            CustomAccelerometer.Shaken += CustomAccelerometer_ShakenAsync;
            CustomAccelerometer.Enabled = true;
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void Page_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await CanteenManager.INSTANCE.downloadCanteensAsync(false);
            await loadCanteensAsync();

            await CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(false);
            loadCanteenMenus();
            initAcc();
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

        private async void refreshCanteen_btn_Click(object sender, RoutedEventArgs e)
        {
            await CanteenManager.INSTANCE.downloadCanteensAsync(true);
            await loadCanteensAsync();
        }

        private async void refreshCanteenMenus_btn_Click(object sender, RoutedEventArgs e)
        {
            await CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(true);
            loadCanteenMenus();
        }

        private async void refreshAll_btn_Click(object sender, RoutedEventArgs e)
        {
            await CanteenManager.INSTANCE.downloadCanteensAsync(true);
            await loadCanteensAsync();
            await CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(true);
            loadCanteenMenus();
        }

        private void right_btn_Click(object sender, RoutedEventArgs e)
        {
            currentDayOffset++;
            if(currentDayOffset >= 7)
            {
                currentDayOffset = 0;
            }
            showCurrentMenus();
        }

        private void left_btn_Click(object sender, RoutedEventArgs e)
        {
            currentDayOffset--;
            if(currentDayOffset < 0)
            {
                currentDayOffset = 6;
            }
            showCurrentMenus();
        }

        private void Tb_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (sender != null && sender is TextBlock)
            {
                TextBlock tb = sender as TextBlock;
                currentSelectedMenu = tb.Text;
                flyout.ShowAt(tb);
            }
        }

        private async void FOutI_ClickAsync(object sender, RoutedEventArgs e)
        {
            await CanteenMenueManager.INSTANCE.googleMenuString(currentSelectedMenu);
        }

        private async void CustomAccelerometer_ShakenAsync(object sender, EventArgs args)
        {
            if(currentCanteen == null)
            {
                return;
            }
            MessageDialog message = new MessageDialog(getRandomMenus());
            message.Title = "Random menu:";
            message.Content = getRandomMenus();
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
