using System;
using TUMCampusAppAPI;
using TUMCampusAppAPI.DBTables;
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
        public readonly CanteenDishTable menu;
        private readonly MenuFlyout flyOut;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 26/01/2017 Created [Fabian Sauter]
        /// </history>
        public CanteenMenuControl(CanteenDishTable menu)
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
        /// <summary>
        /// Shows the current menu on the screen.
        /// </summary>
        private void showMenu()
        {
            menuTitle_tbx.Text = menu.nameEmojis;
            price_tbx.Text = menu.price;
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
            await CanteenDishManager.INSTANCE.googleDishString(menu);
        }

        private async void FOutChefkoch_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Util.launchBrowser(new Uri(@"http://www.chefkoch.de/rs/s0/" + menu.name.Replace(' ', '+') + @"/Rezepte.html"));
        }

        #endregion
    }
}
