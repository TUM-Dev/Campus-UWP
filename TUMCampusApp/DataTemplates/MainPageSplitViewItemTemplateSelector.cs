using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.DataTemplates
{
    class MainPageSplitViewItemTemplateSelector : DataTemplateSelector
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public DataTemplate mainPageSplitViewItemButtonTemplate { get; set; }
        public DataTemplate mainPageSplitViewItemDescriptionTemplate { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 07/04/2018 Created [Fabian Sauter]
        /// </history>
        public MainPageSplitViewItemTemplateSelector()
        {
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is MainPageSplitViewItemButtonTemplate)
            {
                return mainPageSplitViewItemButtonTemplate;
            }
            else if (item is MainPageSplitViewItemDescriptionTemplate)
            {
                return mainPageSplitViewItemDescriptionTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }

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
