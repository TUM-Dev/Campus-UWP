using System;
using TUMCampusApp.Classes;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static TUMCampusApp.Classes.Utillities;
using TUMCampusAppAPI;

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
            Utillities.mainPage = this;
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
            Utillities.mainPage = this;
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
        /// <summary>
        /// Navigates the frame to the given page.
        /// </summary>
        /// <param name="t">The page, which should get navigated to.</param>
        /// <param name="args">Navigation args.</param>
        public void navigateToPage(Type t, object args)
        {
            mainFrame.Navigate(t, args);
        }

        /// <summary>
        /// Navigates to the given page.
        /// </summary>
        /// <param name="page"></param>
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

        /// <summary>
        /// Enables the burger menu.
        /// </summary>
        public void enableBurgerMenu()
        {
            splitViewIcons_lb.IsEnabled = true;
        }

        /// <summary>
        /// Disables the burger menu.
        /// </summary>
        public void disableBurgerMenu()
        {
            splitViewIcons_lb.IsEnabled = false;
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Navigates to the currently selected page (burger menu).
        /// </summary>
        private void navigateToSelectedPage()
        {
            if (mainFrame == null || !splitViewIcons_lb.IsEnabled)
            {
                return;
            }
            switch (splitViewIcons_lb.SelectedIndex)
            {
                case 1:
                    mainFrame.Navigate(typeof(MyCalendarPage));
                    break;

                case 2:
                    mainFrame.Navigate(typeof(MyLecturesPage));
                    break;

                case 3:
                    mainFrame.Navigate(typeof(MyGradesPage));
                    break;

                case 4:
                    mainFrame.Navigate(typeof(TuitionFeesPage));
                    break;

                case 6:
                    mainFrame.Navigate(typeof(HomePage));
                    break;

                case 7:
                    mainFrame.Navigate(typeof(CanteensPage));
                    break;

                case 12:
                    mainFrame.Navigate(typeof(StudyRoomPage));
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

        /// <summary>
        /// Disables all tum pages if not logged in.
        /// </summary>
        private void setVisiblilityMyTum()
        {
            Visibility v;
            if (Util.getSettingBoolean(Const.TUMO_ENABLED))
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
            mainPage_spv.IsPaneOpen = false;
        }

        private void goBackRequest(object sender, BackRequestedEventArgs e)
        {
            if (mainFrame.CanGoBack && splitViewIcons_lb.IsEnabled && e.Handled == false)
            {
                e.Handled = true;
                mainFrame.GoBack();
            }
        }

        #endregion
    }
}
