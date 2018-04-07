using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.DataTemplates
{
    class MainPageSplitViewItemButtonTemplate : MainPageSplitViewItemTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public FontFamily iconFont { get; set; }
        public string icon { get; set; }
        public Thickness iconMargin { get; set; }
        public Type page { get; set; }
        public bool isEnabled { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 07/04/2018 Created [Fabian Sauter]
        /// </history>
        public MainPageSplitViewItemButtonTemplate()
        {
            this.iconFont = new FontFamily("Segoe MDL2 Assets");
            this.textMargin = new Thickness(10, 0, 0, 0);
            this.page = null;
            this.isEnabled = true;
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
