using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TUMCampusApp.classes.Data;
using TUMCampusApp.pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static TUMCampusApp.classes.Utillities;

namespace TUMCampusApp.Pages
{
    public sealed partial class MainPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--



        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic empty Constructor
        /// </summary>
        /// <history>
        /// 09/12/2016  Created [Fabian Sauter]
        /// </history>
        public MainPage()
        {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += goBackRequest;
            DataStorage.INSTANCE.setMainPage(this);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--



        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--



        #endregion

        #region --Misc Methods (Private)--
        private void navigateToSelectedPage()
        {
            if (mainFrame == null || !splitViewIcons_lb.IsEnabled)
            {
                return;
            }
            switch (splitViewIcons_lb.SelectedIndex)
            {
                case 0:
                    mainFrame.Navigate(typeof(HomePage));
                    break;

                case 1:
                    mainFrame.Navigate(typeof(CanteensPage));
                    break;

                case 2:
                    //mainFrame.Navigate(typeof(PaymentsListPage));
                    break;

                case 3:
                    //mainFrame.Navigate(typeof(SettingsPage));
                    break;

                default:
                    break;
            }
            try
            {
                pageName_tbl.Text = (((splitViewIcons_lb.SelectedItem as ListBoxItem).Content as StackPanel).Children[1] as TextBlock).Text;
            }
            catch(Exception e)
            {

            }
        }

        public void navigateToPage(EnumPage page)
        {
            splitViewIcons_lb.SelectedIndex = (int)page;
            navigateToSelectedPage();
        }

        public void enableBurgerMenue()
        {
            splitViewIcons_lb.IsEnabled = true;
        }

        public void disableBurgerMenue()
        {
            splitViewIcons_lb.IsEnabled = false;
        }

        #endregion

        #region --Misc Methods (Protected)--



        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DataStorage.INSTANCE.dataInitiallyLoaded)
            {
                await DataStorage.INSTANCE.loadAllDataTaskAsync();
                DataStorage.INSTANCE.dataInitiallyLoaded = true;
            }

            if (DataStorage.INSTANCE.settingsData.initialRun)
            {
                mainFrame.Navigate(typeof(SetupPage));
            }
            else
            {
                navigateToSelectedPage();
            }
            navigateToSelectedPage();
        }

        private void openSplitView_hbtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage_spv.IsPaneOpen = !mainPage_spv.IsPaneOpen;
        }

        private void splitViewIcons_lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            navigateToSelectedPage();
        }

        private void goBackRequest(object sender, BackRequestedEventArgs e)
        {
            if (mainFrame.CanGoBack && splitViewIcons_lb.IsEnabled)
            {
                mainFrame.GoBack();
            }
        }

        #endregion
    }
}
