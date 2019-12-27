using System;
using UI.Pages.Content;
using UI.Pages.Settings;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages.Content;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UI.Pages
{
    public sealed partial class MainPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly HomePageContext VIEW_MODEL = new HomePageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedTo();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            titleBar.OnPageNavigatedFrom();
        }

        private void NavigationView_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                UiUtils.NavigateToPage(typeof(SettingsPage));
                return;
            }

            FrameNavigationOptions navOptions = new FrameNavigationOptions
            {
                TransitionInfoOverride = args.RecommendedNavigationTransitionInfo
            };
            Type targetPage = null;
            if (args.InvokedItemContainer == calendar_navItem)
            {

            }
            else if (args.InvokedItemContainer == canteens_navItem)
            {

            }
            else if (args.InvokedItemContainer == grades_navItem)
            {

            }
            else if (args.InvokedItemContainer == home_navItem)
            {
                targetPage = typeof(HomePage);
            }
            else if (args.InvokedItemContainer == lectures_navItem)
            {

            }
            else if (args.InvokedItemContainer == news_navItem)
            {

            }
            else if (args.InvokedItemContainer == calendar_navItem)
            {

            }
            else if (args.InvokedItemContainer == tuitionFees_navItem)
            {

            }

            if (!(targetPage is null))
            {
                contentFrame.NavigateToType(targetPage, null, navOptions);
            }
        }

        private void NavigationView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Navigate by default to the home page:
            if (sender is Microsoft.UI.Xaml.Controls.NavigationView navView)
            {
                navView.SelectedItem = home_navItem;
                contentFrame.Navigate(typeof(HomePage));
            }
        }

        #endregion
    }
}
