using TUMCampusApp.Controls;
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
            showWidgets();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UIUtils.getLocalizedString("HomePageName_Text");
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
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Shows all widgets.
        /// </summary>
        private void showWidgets()
        {
            canteenWidget_ds.WidgetContent = new CanteenDummyWidget(canteenWidget_ds, this);
            tutionFeeWidget_ds.WidgetContent = new TuitionFeeWidget(tutionFeeWidget_ds);
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void widgets_stckp_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {

        }

        #endregion
    }
}
