using System;
using Microsoft.UI.Xaml.Controls;

namespace UI.Pages
{
    public class MainPageNavigationPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly Type TARGET_PAGE;
        public readonly NavigationViewItem NAV_ITEM;
        public readonly string PAGE_NAME;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public MainPageNavigationPage(Type targetPage, NavigationViewItem navItem, string pageName)
        {
            TARGET_PAGE = targetPage;
            NAV_ITEM = navItem;
            PAGE_NAME = pageName;
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


        #endregion
    }
}
