using System;
using TUMCampusAppAPI;
using TUMCampusAppAPI.Canteens;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TUMCampusApp.Controls
{
    public sealed partial class CanteenMenuControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CanteenMenu menu;
        private readonly MenuFlyout flyOut;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 26/01/2017 Created [Fabian Sauter]
        /// </history>
        public CanteenMenuControl(CanteenMenu menu)
        {
            this.InitializeComponent();
            this.menu = menu;
            this.flyOut = new MenuFlyout();
            MenuFlyoutItem fOutI = new MenuFlyoutItem();
            fOutI.Text = "Google it!";
            fOutI.Click += FOutGoogleIt_ClickAsync;
            this.flyOut.Items.Add(fOutI);
            fOutI = new MenuFlyoutItem();
            fOutI.Text = "Chefkoch.de (German)";
            fOutI.Click += FOutChefkoch_ClickAsync;
            this.flyOut.Items.Add(fOutI);

            showMenu();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void showMenu()
        {
            if(menu.nameEmojis == null)
            {
                menuTitle_tbx.Text = CanteenMenueManager.INSTANCE.replaceMenuStringWithImages(menu.name, true);
            }
            else
            {
                menuTitle_tbx.Text = menu.nameEmojis;
            }

            string price = CanteenPrices.getPrice(menu.typeLong);
            if(price != null && price != "")
            {
                price_tbx.Text = price + "€";
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (sender != null && sender is CanteenMenuControl)
            {
                flyOut.ShowAt((CanteenMenuControl)sender, e.GetPosition(this));
            }
        }

        private async void FOutGoogleIt_ClickAsync(object sender, RoutedEventArgs e)
        {
            await CanteenMenueManager.INSTANCE.googleMenuString(menu.name);
        }

        private async void FOutChefkoch_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Util.launchBrowser(new Uri(@"http://www.chefkoch.de/rs/s0/" + CanteenMenueManager.INSTANCE.getCleanMenuTitle(menu.name).Replace(' ', '+') + @"/Rezepte.html"));
        }

        #endregion
    }
}
