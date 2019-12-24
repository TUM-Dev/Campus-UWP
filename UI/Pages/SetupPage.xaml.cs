using System;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UI.Pages
{
    public sealed partial class SetupPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly SetupPageContext VIEW_MODEL = new SetupPageContext();

        /// <summary>
        /// Where should we navigate the frame to once we finished?
        /// </summary>
        private Type doneTargetPage = null;
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public SetupPage()
        {
            InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void UpdateViewState(string state)
        {
            VisualStateManager.GoToState(this, state, true);
        }

        private void NavigateAway()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                return;
            }

            if (doneTargetPage is null)
            {
                UiUtils.NavigateToPage(typeof(MainPage));
            }
            else
            {
                UiUtils.NavigateToPage(doneTargetPage);
            }

            // Make sure we remove the last entry from the back stack to prevent navigation back to this page:
            UiUtils.RemoveLastBackStackEntry();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Type type)
            {
                doneTargetPage = type;
            }
        }

        private void WhatIsTumOnline_link_Click(object sender, RoutedEventArgs e)
        {

        }

        private void next1_ipbtn_Click(Controls.IconProgressButtonControl sender, RoutedEventArgs args)
        {

        }

        private void cancel1_ibtn_Click(Controls.IconButtonControl sender, RoutedEventArgs args)
        {

        }

        #endregion
    }
}
