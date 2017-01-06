using System;
using System.Diagnostics;
using TUMCampusApp.classes;
using TUMCampusApp.classes.managers;
using TUMCampusApp.classes.userData;
using TUMCampusApp.pages;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            DeviceInfo.INSTANCE.mainPage = this;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += goBackRequest;
            setVisiblilityMyTum();
            navigateToSelectedPage();
        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 09/12/2016  Created [Fabian Sauter]
        /// </history>
        public MainPage(EnumPage page)
        {
            DeviceInfo.INSTANCE.mainPage = this;
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += goBackRequest;
            setVisiblilityMyTum();
            navigateToPage(page);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void navigateToPage(EnumPage page)
        {
            int index = (int)page + 1;
            if(index > 4)
            {
                index++;
            }
            splitViewIcons_lb.SelectedIndex = index;
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

        #region --Misc Methods (Private)--
        private void navigateToSelectedPage()
        {
            if (mainFrame == null || !splitViewIcons_lb.IsEnabled)
            {
                return;
            }
            switch (splitViewIcons_lb.SelectedIndex)
            {
                case 4:
                    mainFrame.Navigate(typeof(TuitionFeesPage));
                    break;

                case 6:
                    mainFrame.Navigate(typeof(HomePage));
                    break;

                case 7:
                    mainFrame.Navigate(typeof(CanteensPage));
                    break;

                case 15:
                    mainFrame.Navigate(typeof(SettingsPage));
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
                Logger.Error("Unable to get the name from the selected ListBoxItem", e);
            }
        }

        private void setVisiblilityMyTum()
        {
            Visibility v;
            if (Utillities.getSettingBoolean(Const.TUMO_ENABLED))
            {
                v = Visibility.Visible;
            }
            else
            {
                v = Visibility.Collapsed;
            }
            myTUM_lbi.Visibility = v;
            tumCalendar_lbi.Visibility = v;
            tumMyLectures_lbi.Visibility = v;
            tumMyGrades_lbi.Visibility = v;
            tumTuitionFees_lbi.Visibility = v;
        }

        #endregion

        #region --Misc Methods (Protected)--



        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
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
