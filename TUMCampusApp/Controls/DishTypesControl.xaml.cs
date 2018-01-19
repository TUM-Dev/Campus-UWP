using System.Collections.Generic;
using System.Collections.ObjectModel;
using TUMCampusApp.Classes;
using TUMCampusApp.DataTemplates;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class DishTypesControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private ObservableCollection<DishTypeTemplate> dishTypes;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 19/01/2018 Created [Fabian Sauter]
        /// </history>
        public DishTypesControl()
        {
            this.dishTypes = new ObservableCollection<DishTypeTemplate>();
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public List<string> getSelectedDishTypes()
        {
            List<string> types = new List<string>();
            foreach (object o in dishTypes_list.SelectedItems)
            {
                if (o is CanteenDishTable)
                {
                    types.Add((o as CanteenDishTable).dish_type);
                }
            }
            return types;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void loadDishTypes()
        {
            dishTypes.Clear();
            foreach (CanteenDishTable dishType in CanteenDishManager.INSTANCE.getAllDishTypes())
            {
                dishTypes.Add(new DishTypeTemplate()
                {
                    dishType = dishType.dish_type,
                    dishTypeLocalized = Utillities.translateDishType(dishType.dish_type)
                });
            }
            if (dishTypes.Count <= 0)
            {
                dishTypes_list.Visibility = Visibility.Collapsed;
                noneFound_tbx.Visibility = Visibility.Visible;
            }
            else
            {

                dishTypes_list.Visibility = Visibility.Visible;
                noneFound_tbx.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadDishTypes();
        }

        #endregion
    }
}
