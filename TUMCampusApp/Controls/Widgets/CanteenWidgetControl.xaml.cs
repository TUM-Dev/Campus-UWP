using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls.Widgets
{
    public sealed partial class CanteenWidgetControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public CanteenTable Canteen
        {
            get { return (CanteenTable)GetValue(CanteenProperty); }
            set
            {
                SetValue(CanteenProperty, value);
                showDishes();
            }
        }
        public static readonly DependencyProperty CanteenProperty = DependencyProperty.Register("Canteen", typeof(CanteenTable), typeof(CanteenWidgetControl), null);

        public CanteenDummyWidgetControl CanteenDummyWidget
        {
            get { return (CanteenDummyWidgetControl)GetValue(CanteenDummyWidgetProperty); }
            set { SetValue(CanteenDummyWidgetProperty, value); }
        }
        public static readonly DependencyProperty CanteenDummyWidgetProperty = DependencyProperty.Register("CanteenDummyWidget", typeof(CanteenDummyWidgetControl), typeof(NewsWidgetControl), null);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/04/2018 Created [Fabian Sauter]
        /// </history>
        public CanteenWidgetControl()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void unfavoriteCanteen()
        {
            if (Canteen != null)
            {
                Visibility = Visibility.Collapsed;
                string canteen_id = Canteen.canteen_id;
                Task.Run(() => CanteenManager.INSTANCE.unfavoriteCanteen(canteen_id));
            }
        }

        private void showDishes()
        {
            loading_ldng.IsLoading = true;
            if (Canteen != null)
            {
                string canteenId = Canteen.canteen_id;
                Task.Run(() =>
                {
                    DateTime date = CanteenDishManager.INSTANCE.getFirstNextDate(canteenId);
                    int dishTypesCount = 0;
                    if (date != DateTime.MaxValue)
                    {
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => canteenDate_tbx.Text = date.AddDays(1).ToString("dd.MM.yyyy")).AsTask();

                        foreach (FavoriteCanteenDishTypeTable f in CanteenManager.INSTANCE.getDishTypesForFavoriteCanteen(canteenId))
                        {
                            List<CanteenDishTable> dishes = CanteenDishManager.INSTANCE.getDishesForType(canteenId, f.dish_type, false, date);

                            if (dishes.Count > 0)
                            {
                                dishTypesCount++;
                                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    DishTypeControl dishTypeControl = new DishTypeControl();
                                    dishTypeControl.addDishes(dishes);
                                    dishTypes_stckp.Children.Add(dishTypeControl);
                                }).AsTask();
                            }
                        }
                    }
                    if (dishTypesCount <= 0)
                    {
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            dishTypes_stckp.Visibility = Visibility.Collapsed;
                            canteenDate_tbx.Visibility = Visibility.Collapsed;
                        }).AsTask();
                    }
                    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => loading_ldng.IsLoading = false).AsTask();
                });
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void slideListItem_sli_SwipeStatusChanged(SlidableListItem sender, SwipeStatusChangedEventArgs args)
        {
            if (args.NewValue == SwipeStatus.Idle)
            {
                if (args.OldValue == SwipeStatus.SwipingPassedLeftThreshold)
                {
                    unfavoriteCanteen();
                }
                else if (args.OldValue == SwipeStatus.SwipingPassedRightThreshold)
                {
                    CanteenDummyWidget?.disableCanteenWidgets();
                }
            }
        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (Canteen != null)
            {
                UIUtils.mainPage.navigateToPage(UIUtils.EnumPage.CanteensPage, Canteen.canteen_id);
            }
        }

        #endregion
    }
}
