using System;
using TUMCampusApp.Classes;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static TUMCampusApp.Classes.UIUtils;
using TUMCampusAppAPI;
using Windows.UI.Popups;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Data_Manager;

namespace TUMCampusApp.Pages
{
    public sealed partial class MainPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic empty Constructor
        /// </summary>
        /// <history>
        /// 09/12/2016  Created [Fabian Sauter]
        /// </history>
        public MainPage()
        {
            InitializeComponent();
            UIUtils.mainPage = this;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += goBackRequest;
            setVisiblilityMyTum();
            navigateToSelectedPage(null);
            NetworkInformation.NetworkStatusChanged += new NetworkStatusChangedEventHandler(onNetworkStatusChangedAsync);
        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 09/12/2016  Created [Fabian Sauter]
        /// </history>
        public MainPage(EnumPage page, string args)
        {
            UIUtils.mainPage = this;
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += goBackRequest;
            setVisiblilityMyTum();
            navigateToPage(page, args);
            NetworkInformation.NetworkStatusChanged += new NetworkStatusChangedEventHandler(onNetworkStatusChangedAsync);
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
        /// <param name="page">The page to navigate to.</param>
        /// <param name="args">Navigation args.</param>
        public void navigateToPage(EnumPage page, object args)
        {
            int index = (int)page + 1;
            if (index > 4)
            {
                index++;
            }
            splitViewIcons_lb.SelectedIndex = index;
            navigateToSelectedPage(args);
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
        /// <param name="args">Navigation args.</param>
        private void navigateToSelectedPage(object args)
        {
            if (mainFrame == null || !splitViewIcons_lb.IsEnabled)
            {
                return;
            }
            switch (splitViewIcons_lb.SelectedIndex)
            {
                case 1:
                    navigateToPage(typeof(MyCalendarPage), args);
                    break;

                case 2:
                    navigateToPage(typeof(MyLecturesPage), args);
                    break;

                case 3:
                    navigateToPage(typeof(MyGradesPage), args);
                    break;

                case 4:
                    navigateToPage(typeof(TuitionFeesPage), args);
                    break;

                case 6:
                    navigateToPage(typeof(HomePage), args);
                    break;

                case 7:
                    navigateToPage(typeof(CanteensPage2), args);
                    break;

                case 8:
                    navigateToPage(typeof(NewsPage), args);
                    break;

                case 11:
                    navigateToPage(typeof(RoomfinderPage), args);
                    break;

                case 12:
                    navigateToPage(typeof(StudyRoomPage), args);
                    break;

                case 15:
                    navigateToPage(typeof(SettingsPage), args);
                    break;

                default:
                    break;
            }
            showPageName();
        }

        /// <summary>
        /// Disables all tum pages if not logged in.
        /// </summary>
        private void setVisiblilityMyTum()
        {
            Visibility v;
            if (Settings.getSettingBoolean(SettingsConsts.TUMO_ENABLED))
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

        /// <summary>
        /// Updates the color of networkConnectionStatus_tblck.
        /// Red if the device is not connected and green if the device is connected to the internet.
        /// </summary>
        private void updateConnectionStatus()
        {
            if (DeviceInfo.isConnectedToInternet())
            {
                networkConnectionStatus_tblck.Foreground = new SolidColorBrush(Colors.DarkGreen);
            }
            else
            {
                networkConnectionStatus_tblck.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
        }

        /// <summary>
        /// Sets the faculty_img source based on the selected faculty.
        /// </summary>
        private void setImage()
        {
            int facultyIndex = Settings.getSettingInt(SettingsConsts.FACULTY_INDEX);
            switch (facultyIndex)
            {
                case 0:
                case 3:
                case 5:
                    faculty_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/im.png"));
                    break;
                case 1:
                case 2:
                    faculty_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/mw.png"));
                    break;
                default:
                    faculty_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/wear_tuition_fee1.png"));
                    break;
            }
        }

        /// <summary>
        /// Shows the name of the current page.
        /// </summary>
        private void showPageName()
        {
            if (mainFrame != null && mainFrame.Content is INamedPage)
            {
                try
                {
                    pageName_tbl.Text = (mainFrame.Content as INamedPage).getLocalizedName();
                }
                catch (Exception e)
                {
                    Logger.Error("Unable to get the name from the selected ListBoxItem", e);
                }
            }
        }

        #endregion

        #region --Misc Methods (Protected)--



        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            updateConnectionStatus();
            setImage();
        }

        private void openSplitView_hbtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage_spv.IsPaneOpen = !mainPage_spv.IsPaneOpen;
        }

        private void splitViewIcons_lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            navigateToSelectedPage(null);
            mainPage_spv.IsPaneOpen = false;
        }

        private void goBackRequest(object sender, BackRequestedEventArgs e)
        {
            if (mainFrame.CanGoBack && splitViewIcons_lb.IsEnabled && e.Handled == false)
            {
                e.Handled = true;
                mainFrame.GoBack();
                showPageName();
            }
        }

        private async void networkConnectionStatus_btn_Click(object sender, RoutedEventArgs e)
        {
            string text = "";
            if (DeviceInfo.isConnectedToInternet())
            {
                text += UIUtils.getLocalizedString("NetworkConnectionStatusConnected_Text");
            }
            else
            {
                text += UIUtils.getLocalizedString("NetworkConnectionStatusDisconnected_Text");
            }
            MessageDialog message = new MessageDialog(text)
            {
                Title = UIUtils.getLocalizedString("NetworkConnectionStatusBase_Text")
            };
            await message.ShowAsync();
        }

        private async void onNetworkStatusChangedAsync(object sender)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                updateConnectionStatus();
            });
        }

        #endregion
    }
}
