using System;
using Shared.Classes;
using UI.Pages.Content;
using UI.Pages.Settings;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages.Content;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace UI.Pages
{
    public sealed partial class MainPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly HomePageContext VIEW_MODEL = new HomePageContext();
        private readonly MainPageNavigationPage[] PAGES;
        private Microsoft.UI.Xaml.Controls.NavigationViewItem selectedItem;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;

            PAGES = new MainPageNavigationPage[7]
            {
                new MainPageNavigationPage(typeof(HomePage), home_navItem, "Home"),
                new MainPageNavigationPage(typeof(CanteensPage), canteens_navItem, "Canteens"),
                new MainPageNavigationPage(typeof(GradesPage), grades_navItem, "Grades"),
                new MainPageNavigationPage(typeof(LecturesPage), lectures_navItem, "Lectures"),
                new MainPageNavigationPage(typeof(CanteensPage), news_navItem, "News"),
                new MainPageNavigationPage(typeof(CalendarPage), calendar_navItem, "Calendar"),
                new MainPageNavigationPage(typeof(TuitionFeesPage), tuitionFees_navItem, "Tuition Fees")
            };
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void UpdateNavigationView(Type targetPage)
        {
            foreach (MainPageNavigationPage page in PAGES)
            {
                if (page.TARGET_PAGE == targetPage)
                {
                    main_navView.SelectedItem = page.NAV_ITEM;
                    return;
                }
            }
            throw new InvalidOperationException($"Failed to update main navigation view. Invalid target page ({targetPage})");
        }

        private void NavigateToPage(Microsoft.UI.Xaml.Controls.NavigationViewItemBase target, NavigationTransitionInfo info)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions
            {
                TransitionInfoOverride = info
            };

            foreach (MainPageNavigationPage page in PAGES)
            {
                if (page.NAV_ITEM == target)
                {
                    UiUtils.NavigateToType(page.TARGET_PAGE, null, contentFrame, navOptions);
                    titleBar.Text = page.PAGE_NAME;
                    return;
                }
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedTo();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedFrom();
        }

        private void OnNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                UiUtils.NavigateToPage(typeof(SettingsPage));
                return;
            }
            NavigateToPage(args.InvokedItemContainer, args.RecommendedNavigationTransitionInfo);
        }

        private void OnNavigationViewLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Navigate by default to the home page:
            if (sender is Microsoft.UI.Xaml.Controls.NavigationView navView)
            {
                navView.SelectedItem = home_navItem;
                contentFrame.Navigate(typeof(HomePage));
            }
        }

        private void OnContentFrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            UpdateNavigationView(e.SourcePageType);
        }

        private async void OnPageSelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            bool revert = false;
            if (args.SelectedItem == tumOnline_navItem)
            {
                await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("TumOnlineUrl")));
                revert = true;
            }
            else if (args.SelectedItem == tumLive_navItem)
            {
                await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("TumLiveUrl")));
                revert = true;
            }
            else if (args.SelectedItem == tumZoom_navItem)
            {
                await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("TumZoomUrl")));
                revert = true;
            }
            else if (args.SelectedItem == bbb_navItem)
            {
                await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("BBBUrl")));
                revert = true;
            }
            else if (args.SelectedItem == tumSexy_navItem)
            {
                await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("TumSexyUrl")));
                revert = true;
            }
            else if (args.SelectedItem == moodle_navItem)
            {
                await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("MoodleUrl")));
                revert = true;
            }
            else if (args.SelectedItem == zulip_navItem)
            {
                await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("ZulipUrl")));
                revert = true;
            }


            if (revert)
            {
                main_navView.SelectedItem = selectedItem;
            }
            else if (args.SelectedItem is Microsoft.UI.Xaml.Controls.NavigationViewItem navItem)
            {
                selectedItem = navItem;
            }
        }

        #endregion
    }
}
