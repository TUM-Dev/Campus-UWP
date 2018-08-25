using Data_Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls.Widgets
{
    public sealed partial class TuitionFeeWidgetControl : UserControl, IHideableWidget
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public WidgetControl WidgetContainer
        {
            get { return (WidgetControl)GetValue(WidgetContainerProperty); }
            set { SetValue(WidgetContainerProperty, value); }
        }
        public static readonly DependencyProperty WidgetContainerProperty = DependencyProperty.Register("WidgetContainer", typeof(WidgetControl), typeof(TuitionFeeWidgetControl), null);

        public HomePage HPage
        {
            get { return (HomePage)GetValue(HPageProperty); }
            set { SetValue(HPageProperty, value); }
        }
        public static readonly DependencyProperty HPageProperty = DependencyProperty.Register("HPage", typeof(HomePage), typeof(TuitionFeeWidgetControl), null);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/04/2018 Created [Fabian Sauter]
        /// </history>
		public TuitionFeeWidgetControl()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getSettingsToken()
        {
            return SettingsConsts.DISABLE_TUITION_FEE_WIDGET;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void onHiding()
        {
        }

        #endregion

        #region --Misc Methods (Private)--
        private void loadFees()
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => WidgetContainer?.setIsLoading(true)).AsTask();
            Task.Run(async () =>
            {
                try
                {
                    Task t = TuitionFeeDBManager.INSTANCE.downloadFees(false);
                    if (t != null)
                    {
                        await t;
                    }
                }
                catch (BaseTUMOnlineException e)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => showFees(null));
                    return;
                }
                List<TUMTuitionFeeTable> list = TuitionFeeDBManager.INSTANCE.getFees();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => showFees(list));
            });
        }

        /// <summary>
        /// Shows the given fees list on the screen or hides the widget if the list is empty/null.
        /// </summary>
        /// <param name="list">A list of tuition fees.</param>
        private void showFees(List<TUMTuitionFeeTable> list)
        {
            tuitionFees_stckp.Children.Clear();

            if (list == null || list.Count <= 0 || list[0].money == null)
            {
                if (WidgetContainer != null)
                {
                    WidgetContainer.Visibility = Visibility.Collapsed;
                    HPage?.removeWidget(WidgetContainer);
                }
            }
            else
            {
                foreach (var item in list)
                {
                    if (item != null && item.money != null)
                    {
                        tuitionFees_stckp.Children.Add(new TuitionFeeControl(item)
                        {
                            Margin = new Thickness(0, 0, 0, 10)
                        });
                    }
                }
            }
            WidgetContainer.setIsLoading(false);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadFees();
        }

        #endregion
    }
}
