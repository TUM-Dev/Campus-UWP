using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
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
        private DropShadowPanel dsp;
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
        public CanteenDummyWidget(DropShadowPanel dsp, HomePage homePage)
        {
            this.dsp = dsp;
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

            foreach (FavoriteCanteenDishTypeTable f in CanteenManager.INSTANCE.getFavoriteCanteens())
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    dsp.Visibility = Visibility.Collapsed;
                    DropShadowPanel canteen_dsp = new DropShadowPanel()
                    {
                        Style = dsp.Style,
                        Visibility = Visibility.Visible
                    };
                    canteen_dsp.Content = new CanteenWidget(f.canteen_id, canteen_dsp);
                    homePage.addWidget(canteen_dsp);
                });
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => dsp.Visibility = Visibility.Collapsed);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
