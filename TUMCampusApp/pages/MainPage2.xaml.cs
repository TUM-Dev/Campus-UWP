using Data_Manager;
using System;
using System.Collections.ObjectModel;
using TUMCampusApp.Classes;
using TUMCampusApp.DataTemplates;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Pages
{
    public sealed partial class MainPage2 : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private ObservableCollection<MainPageSplitViewItemTemplate> splitViewItems;
        private Type requestedPage;
        private object requestedPageArgs;

        public Visibility HeaderRectVisability
        {
            get { return (Visibility)GetValue(HeaderRectVisabilityProperty); }
            set { SetValue(HeaderRectVisabilityProperty, value); }
        }
        public static readonly DependencyProperty HeaderRectVisabilityProperty = DependencyProperty.Register(nameof(HeaderRectVisability), typeof(Visibility), typeof(MainPage2), new PropertyMetadata(Visibility.Visible));
        
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 07/04/2018 Created [Fabian Sauter]
        /// </history>
        public MainPage2() : this(typeof(HomePage), null)
        {
        }

        public MainPage2(Type page, string arguments)
        {
            this.requestedPage = page;
            this.splitViewItems = new ObservableCollection<MainPageSplitViewItemTemplate>();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage2_BackRequested;
            UiUtils.mainPage = this;

            loadSplitViewItems();
            this.InitializeComponent();

            if (UiUtils.isApplicationViewApiAvailable() && !UiUtils.isRunningOnMobileDevice())
            {
                HeaderRectVisability = Visibility.Collapsed;

                // Set XAML element as a draggable region.
                CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
                updateTitleBarLayout(titleBar);
                Window.Current.SetTitleBar(titleBar_grid);
                titleBar.IsVisibleChanged += TitleBar_IsVisibleChanged;
                titleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
            }
        }

        private void updateTitleBarLayout(CoreApplicationViewTitleBar titleBar)
        {
            // Get the size of the caption controls area and back button 
            // (returned in logical pixels), and move your content around as necessary.
            leftPaddingColumn_cd.Width = new GridLength(titleBar.SystemOverlayLeftInset);
            rightPaddingColumn_cd.Width = new GridLength(titleBar.SystemOverlayRightInset);
            titleBarButtons_grid.Margin = new Thickness(titleBar.SystemOverlayLeftInset, 0, 0, 0);

            navBackSeperator_rect.Visibility = titleBar.SystemOverlayLeftInset > 0 ? Visibility.Visible : Visibility.Collapsed;

            // Update title bar control size as needed to account for system size changes.
            titleBar_grid.Height = titleBar.Height;
        }

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            updateTitleBarLayout(sender);
        }

        private void TitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Navigates to the given page with the given arguments.
        /// </summary>
        /// <param name="page">The target page.</param>
        /// <param name="args">The navigation arguments.</param>
        public void navigateToPage(Type page, object args)
        {
            if (page != null)
            {
                mainFrame.Navigate(page, args);
            }
        }

        #endregion

        #region --Misc Methods (Private)--
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
        /// Adds all items to the splitViewItems.
        /// </summary>
        private void loadSplitViewItems()
        {
            if (Settings.getSettingBoolean(SettingsConsts.TUM_ONLINE_ENABLED))
            {
                splitViewItems.Add(new MainPageSplitViewItemDescriptionTemplate() { text = UiUtils.getLocalizedString("MainPageMyTUMItem_Text") });
                splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
                {
                    text = UiUtils.getLocalizedString("MainPageCalendarItem_Text"),
                    icon = "\uE787",
                    page = typeof(MyCalendarPage)
                });
                splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
                {
                    text = UiUtils.getLocalizedString("MainPageLecturesItem_Text"),
                    icon = "\uEF16",
                    page = typeof(MyLecturesPage)
                });
                splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
                {
                    text = UiUtils.getLocalizedString("MainPageGradesItem_Text"),
                    icon = "\uEADF",
                    page = typeof(MyGradesPage)
                });
                splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
                {
                    text = UiUtils.getLocalizedString("MainPageTuitionFeesItem_Text"),
                    icon = "$",
                    page = typeof(TuitionFeesPage),
                    iconFont = new FontFamily("Segoe UI"),
                    iconMargin = new Thickness(5, -3, 0, 0),
                    textMargin = new Thickness(5, 0, 0, 0)
                });
            }

            splitViewItems.Add(new MainPageSplitViewItemDescriptionTemplate() { text = UiUtils.getLocalizedString("MainPageGeneralTUMItem_Text") });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageHomeItem_Text"),
                icon = "\uE80F",
                page = typeof(HomePage)
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageCanteensItem_Text"),
                icon = "\uD83C\uDF74",
                page = typeof(CanteensPage2),
                iconFont = new FontFamily("Segoe UI Symbol"),
                iconMargin = new Thickness(0, -3, 0, 0)
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageNewsItem_Text"),
                icon = "\uE701",
                page = typeof(NewsPage)
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageTransportItem_Text"),
                icon = "\uE7C0",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPagePlansItem_Text"),
                icon = "\uE826",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageRoomfinderItem_Text"),
                icon = "\uE816",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageStudyRoomItem_Text"),
                icon = "\uE7BC",
                page = typeof(StudyRoomPage)
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageOpeningHoursItem_Text"),
                icon = "\uE823",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageStudyPlansItem_Text"),
                icon = "\uE762",
                isEnabled = false
            });
            splitViewItems.Add(new MainPageSplitViewItemButtonTemplate()
            {
                text = UiUtils.getLocalizedString("MainPageSettingsItem_Text"),
                icon = "\uE713",
                page = typeof(SettingsPage)
            });
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void openSplitView_hbtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage_spv.IsPaneOpen = !mainPage_spv.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (main_lb.SelectedItem != null && main_lb.SelectedItem is MainPageSplitViewItemButtonTemplate buttonTemplate)
            {
                navigateToPage(buttonTemplate.page, null);
                mainPage_spv.IsPaneOpen = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigated -= MainFrame_Navigated;
            mainFrame.Navigated += MainFrame_Navigated;

            if (requestedPage != null)
            {
                navigateToPage(requestedPage, requestedPageArgs);
                requestedPage = null;
                requestedPageArgs = null;
            }

            setImage();
        }

        /// <summary>
        /// Update the current page name and the selected page.
        /// </summary>
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content != null)
            {
                Type contentType = e.Content.GetType();
                for (int i = 0; i < splitViewItems.Count; i++)
                {
                    if (splitViewItems[i] is MainPageSplitViewItemButtonTemplate buttonTemplate && buttonTemplate.page == contentType)
                    {
                        pageName_tbx.Text = splitViewItems[i].text ?? "";
                        main_lb.SelectedIndex = i;
                        return;
                    }
                }

                // Select no item if the page is not one of the available split view items:
                main_lb.SelectedItem = null;

                if (e.Content is INamedPage page)
                {
                    pageName_tbx.Text = page.getLocalizedName() ?? "";
                }
            }
        }

        private void MainPage2_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (mainFrame.CanGoBack && !e.Handled)
            {
                e.Handled = true;
                if (mainFrame.Content is IBackRequestedPage page && page.onBackRequest())
                {
                }
                else
                {
                    mainFrame.GoBack();
                }
            }
        }

        /// <summary>
        /// Workaround for disabling ListBoxItems:
        /// Source: https://social.technet.microsoft.com/wiki/contents/articles/33548.uwp-disabling-selection-of-items-in-a-listview.aspx
        /// </summary>
        private void ItemDescriptionTextBlock_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is MainPageSplitViewItemDescriptionTemplate template)
            {
                var x = main_lb.ContainerFromItem(template);
                if (x is ListBoxItem item)
                {
                    item.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Workaround for disabling ListBoxItems:
        /// Source: https://social.technet.microsoft.com/wiki/contents/articles/33548.uwp-disabling-selection-of-items-in-a-listview.aspx
        /// </summary>
        private void ItemButtonStackPanel_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is MainPageSplitViewItemButtonTemplate template)
            {
                var x = main_lb.ContainerFromItem(template);
                if (x is ListBoxItem item)
                {
                    item.IsEnabled = template.isEnabled;
                }
            }
        }

        private async void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            await UiUtils.onPageSizeChangedAsync(e);
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            await UiUtils.onPageNavigatedFromAsync();
        }

        private void openSplitView_hbtn_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!double.IsNaN(openSplitView_hbtn.ActualWidth) && openSplitView_hbtn.Margin != null)
            {
                pageName_vb.Margin = new Thickness(openSplitView_hbtn.ActualWidth + openSplitView_hbtn.Margin.Left + 14, 0, 0, 0);
            }
        }
        #endregion
    }
}
