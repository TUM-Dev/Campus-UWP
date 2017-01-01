using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TUMCampusApp.classes.canteen;
using TUMCampusApp.classes.managers;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
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

            //Menus:
            if (currentMenus == null)
            {
                return;
            }
            bool b = false;
            foreach (CanteenMenu m in currentMenus)
            {
                if (contains)
                {
                    b = m.typeLong.Contains(name);
                }
                else
                {
                    b = m.typeLong.Equals(name);
                }
                if (b && m.date.DayOfYear == date.DayOfYear)
                {
                    tb = new TextBlock()
                    {
                        Text = m.name,
                        Margin = new Thickness(10, 10, 10, 10),
                        TextWrapping = TextWrapping.WrapWholeWords
                    };
                    menus_sckl.Children.Add(tb);
                }
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int id = UserDataManager.INSTANCE.getLastSelectedCanteenId();
            if (id < 0)
            {
                id = 422;
            }
            currentMenus = CanteenMenueManager.getMenus(id);
            DateTime date = CanteenMenueManager.getFirstNextDate();
            setMenuType("Tagesgericht", true, date);
            setMenuType("Aktionsessen", true, date);

            date = date.AddDays(1);
            canteenDate_tbx.Text = date.Day + "." + date.Month + "." + date.Year;
            Canteen c = CanteenManager.INSTANCE.getCanteenById(id);
            if(c == null)
            {
                canteenName_tbx.Text = "Error";
            }
            else
            {
                canteenName_tbx.Text = c.name;
            }
        }

        #endregion
    }
}
