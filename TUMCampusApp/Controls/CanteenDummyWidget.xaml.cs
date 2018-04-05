using System;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class CanteenDummyWidget : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private WidgetControl widgetControl;
        private HomePage homePage;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 19/01/2018 Created [Fabian Sauter]
        /// </history>
        public CanteenDummyWidget(WidgetControl widgetControl, HomePage homePage)
        {
            this.widgetControl = widgetControl;
            this.homePage = homePage;
            this.InitializeComponent();
            Task.Factory.StartNew(() => showCanteensAsync());
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Tries to refresh all canteens and dishes and shows all favorite canteens on the home page.
        /// </summary>
        private async Task showCanteensAsync()
        {
            await CanteenManager.INSTANCE.downloadCanteensAsync(false);
            await CanteenDishManager.INSTANCE.downloadCanteenDishesAsync(false);

            foreach (CanteenTable c in CanteenManager.INSTANCE.getFavoriteCanteens())
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    WidgetControl canteen_wc = new WidgetControl();
                    canteen_wc.WidgetContent = new CanteenWidget(c.canteen_id, canteen_wc);
                    homePage.addWidget(canteen_wc);
                });
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => widgetControl.Visibility = Visibility.Collapsed);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
