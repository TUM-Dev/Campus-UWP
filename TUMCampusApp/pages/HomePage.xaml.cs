using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TUMCampusApp.Classes;

namespace TUMCampusApp.Pages
{
    public sealed partial class HomePage : Page, INamedPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public HomePage()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UiUtils.GetLocalizedString("HomePageName_Text");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Adds a widget to the widgets_stckp.
        /// </summary>
        /// <param name="widget">The widget that should get added to the widgets_stckp list.</param>
        public void addWidget(UIElement widget)
        {
            widgets_stckp.Children.Add(widget);
            updateWidgetsVisibility();
        }

        public void removeWidget(UIElement widget)
        {
            widgets_stckp.Children.Remove(widget);
            updateWidgetsVisibility();
        }

        #endregion

        #region --Misc Methods (Private)--
        private void updateWidgetsVisibility()
        {
            if (widgets_stckp.Children.Count <= 0)
            {
                noWidgets_nwc.Visibility = Visibility.Visible;
                widgets_stckp.Visibility = Visibility.Collapsed;
            }
            else
            {
                noWidgets_nwc.Visibility = Visibility.Collapsed;
                widgets_stckp.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void widgets_stckp_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            updateWidgetsVisibility();
        }

        #endregion
    }
}
