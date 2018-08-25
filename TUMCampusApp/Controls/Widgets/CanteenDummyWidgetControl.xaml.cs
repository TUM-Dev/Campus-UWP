using Data_Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls.Widgets
{
    public sealed partial class CanteenDummyWidgetControl : UserControl, IHideableWidget
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public WidgetControl WidgetContainer
        {
            get { return (WidgetControl)GetValue(WidgetContainerProperty); }
            set { SetValue(WidgetContainerProperty, value); }
        }
        public static readonly DependencyProperty WidgetContainerProperty = DependencyProperty.Register("WidgetContainer", typeof(WidgetControl), typeof(NewsDummyWidgetControl), null);

        public HomePage HPage
        {
            get { return (HomePage)GetValue(HPageProperty); }
            set { SetValue(HPageProperty, value); }
        }
        public static readonly DependencyProperty HPageProperty = DependencyProperty.Register("HPage", typeof(HomePage), typeof(NewsDummyWidgetControl), null);

        private readonly List<CanteenWidgetControl> CANTEEN_WIDGETS;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/04/2018 Created [Fabian Sauter]
        /// </history>
        public CanteenDummyWidgetControl()
        {
            this.CANTEEN_WIDGETS = new List<CanteenWidgetControl>();
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getSettingsToken()
        {
            return SettingsConsts.DISABLE_CANTEEN_WIDGET;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void onHiding()
        {
            hideAllCanteens();
        }

        public void disableCanteenWidgets()
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => WidgetContainer?.disableWidget()).AsTask();
        }

        #endregion

        #region --Misc Methods (Private)--
        private void loadCanteens()
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => WidgetContainer?.setIsLoading(true)).AsTask();
            Task.Run(async () =>
            {
                Task t2 = CanteenDBManager.INSTANCE.downloadCanteens(false);
                if (t2 != null)
                {
                    await t2;
                }
                Task t = CanteenDishDBManager.INSTANCE.downloadCanteenDishes(false);
                if (t != null)
                {
                    await t;
                }

                foreach (CanteenTable c in CanteenDBManager.INSTANCE.getFavoriteCanteens())
                {
                    t = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        CanteenWidgetControl canteenWidgetControl = new CanteenWidgetControl()
                        {
                            Canteen = c,
                            CanteenDummyWidget = this,
                            HPage = HPage
                        };

                        CANTEEN_WIDGETS.Add(canteenWidgetControl);

                        HPage.addWidget(canteenWidgetControl);
                    }).AsTask();
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (WidgetContainer != null)
                    {
                        WidgetContainer.Visibility = Visibility.Collapsed;
                        WidgetContainer.setIsLoading(false);
                    }
                });
            });
        }

        private void hideAllCanteens()
        {
            for (int i = 0; i < CANTEEN_WIDGETS.Count; i++)
            {
                CANTEEN_WIDGETS[i].Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCanteens();
        }

        #endregion
    }
}
