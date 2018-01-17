using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace TUMCampusApp.Controls
{
    public sealed partial class CanteenWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private DropShadowPanel dSP;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
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
        /// <summary>
        /// Shows all menus on the screen that are associated with the given name, canteen id and date.
        /// </summary>
        /// <param name="canteen_id">The id of the requested canteen.</param>
        /// <param name="name">The "typeLong" name for the menus.</param>
        /// <param name="contains">Whether the menu name should equal or just contains the given name.</param>
        /// <param name="date">The menu date.</param>
        private void setMenuType(string canteen_id, string name, string labelText, bool contains, DateTime date)
        {
            List<CanteenDishTable> list = CanteenDishManager.INSTANCE.getDishesForType(canteen_id, name, contains, date);
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
            foreach (CanteenDishTable m in list)
            {
                menus_sckl.Children.Add(new CanteenDishControl(m));
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Shows all menus for the last selected canteen on the screen. Has to get call in a separate task!
        /// </summary>
        private async void showMenusTaskAsync()
        {
            string canteen_id = UserDataManager.INSTANCE.getLastSelectedCanteenId();
            await CanteenManager.INSTANCE.downloadCanteensAsync(false);
            await CanteenDishManager.INSTANCE.downloadCanteenDishesAsync(false);

            DateTime date = CanteenDishManager.getFirstNextDate(canteen_id);

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                menus_sckl.Children.Clear();
                setMenuType(canteen_id, "Tagesgericht", Utillities.getLocalizedString("CanteenDishOfTheDay_Text"), true, date);
                setMenuType(canteen_id, "Aktionsessen", Utillities.getLocalizedString("CanteenActionDishes_Text"), true, date);
                setMenuType(canteen_id, "Self-Service", Utillities.getLocalizedString("CanteenSelf-Service_Text"), false, date);
                if (menus_sckl.Children.Count <= 0)
                {
                    menus_sckl.Children.Add(new TextBlock()
                    {
                        Text = "No menus found!",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 25
                    });
                    splashProgressRing.Visibility = Visibility.Collapsed;
                    dSP.Visibility = Visibility.Collapsed;
                }
            });

            if (date.CompareTo(DateTime.MaxValue) == 0)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    canteenName_tbx.Text = "Error!";
                    splashProgressRing.Visibility = Visibility.Collapsed;
                    dSP.Visibility = Visibility.Collapsed;
                });
                return;
            }

            date = date.AddDays(1);
            CanteenTable c = await CanteenManager.INSTANCE.getCanteenByIdAsync(canteen_id);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                canteenDate_tbx.Text = date.ToString("dd.MM.yyyy");
                if (c == null)
                {
                    canteenName_tbx.Text = "Error No Canteen!";
                }
                else
                {
                    canteenName_tbx.Text = c.name;
                }
                splashProgressRing.Visibility = Visibility.Collapsed;
            });
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
