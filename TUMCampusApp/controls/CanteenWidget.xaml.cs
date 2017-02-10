using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes.Canteens;
using TUMCampusApp.Classes.Managers;
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

namespace TUMCampusApp.Controls
{
    public sealed partial class CanteenWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private DropShadowPanel dSP;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public CanteenWidget(DropShadowPanel dSP)
        {
            this.dSP = dSP;
            this.InitializeComponent();
            Task.Factory.StartNew(() => showMenusTaskAsync());
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
                menus_sckl.Children.Add(new CanteenMenuControl(m));
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
            await CanteenManager.INSTANCE.downloadCanteensAsync(false);
            await CanteenMenueManager.INSTANCE.downloadCanteenMenusAsync(false);

            DateTime date = CanteenMenueManager.getFirstNextDate();

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                menus_sckl.Children.Clear();
                setMenuType(id, "Tagesgericht", true, date);
                setMenuType(id, "Aktionsessen", true, date);
            }).AsTask().Wait();

            if (date.CompareTo(DateTime.MaxValue) == 0)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    canteenName_tbx.Text = "Error!";
                    splashProgressRing.Visibility = Visibility.Collapsed;
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
                splashProgressRing.Visibility = Visibility.Collapsed;
            }).AsTask().Wait();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
