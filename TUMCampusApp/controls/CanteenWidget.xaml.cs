using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.classes.canteen;
using TUMCampusApp.classes.managers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace TUMCampusApp.controls
{
    public sealed partial class CanteenWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private List<CanteenMenu> currentMenus;
        private MenuFlyout flyout;
        private string currentSelectedMenu;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public CanteenWidget()
        {
            this.InitializeComponent();

            this.flyout = new MenuFlyout();
            MenuFlyoutItem fOutI = new MenuFlyoutItem();
            fOutI.Text = "Google it!";
            fOutI.Click += FOutI_ClickAsync;
            this.flyout.Items.Add(fOutI);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void setMenuType(int canteenId, string name, bool contains, DateTime date)
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

            List<CanteenMenu> list = CanteenMenueManager.INSTANCE.getMenusForType(canteenId, name, contains, date);
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

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private async void showMenusTaskAsync()
        {
            int id = UserDataManager.INSTANCE.getLastSelectedCanteenId();
            if (id <= 0)
            {
                id = 422;
            }
            await CanteenManager.INSTANCE.downloadCanteensAsync(false);
            await CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(false);

            DateTime date = CanteenMenueManager.getFirstNextDate();
            currentMenus = CanteenMenueManager.getMenus(id);

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                menus_sckl.Children.Clear();
                setMenuType(id, "Tagesgericht", true, date);
                setMenuType(id, "Aktionsessen", true, date);
            }).AsTask().Wait();

            if (date.CompareTo(DateTime.MaxValue) == 0)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    canteenName_tbx.Text = "Error!";
                }).AsTask().Wait();
                return;
            }

            date = date.AddDays(1);
            Canteen c = await CanteenManager.INSTANCE.getCanteenByIdAsync(id);
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                canteenDate_tbx.Text = date.Day + "." + date.Month + "." + date.Year;
                if (c == null)
                {
                    canteenName_tbx.Text = "Error No Canteen!";
                }
                else
                {
                    canteenName_tbx.Text = c.name;
                }
            }).AsTask().Wait();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => showMenusTaskAsync());
        }

        private async void FOutI_ClickAsync(object sender, RoutedEventArgs e)
        {
            await CanteenMenueManager.INSTANCE.googleMenuString(currentSelectedMenu);
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

        #endregion
    }
}
