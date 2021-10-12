﻿using UI_Context.Classes.Context.Pages.Content;
using Windows.UI.Xaml.Controls;

namespace UI.Pages.Content
{
    public sealed partial class LecturesPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly LecturesPageContext VIEW_MODEL = new LecturesPageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public LecturesPage()
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


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnRefreshClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true);
        }

        #endregion
    }
}
