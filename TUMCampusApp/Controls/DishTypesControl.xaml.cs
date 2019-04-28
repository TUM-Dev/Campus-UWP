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
        /// <summary>
        /// Returns all selected dish types.
        /// </summary>
        /// <returns>A list of selected dish types.</returns>
        public List<string> getSelectedDishTypes()
        {
            List<string> types = new List<string>();
            foreach (object o in dishTypes_list.SelectedItems)
            {
                if (o is DishTypeTemplate)
                {
                    types.Add((o as DishTypeTemplate).dishType);
                }
            }
            return types;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Loads all dish types and adds them to dishTypes list.
        /// </summary>
        private void loadDishTypes()
        {
            dishTypes.Clear();
            foreach (CanteenDishTable dishType in CanteenDishDBManager.INSTANCE.getAllDishTypes())
            {
                dishTypes.Add(new DishTypeTemplate()
                {
                    dishType = dishType.dish_type,
                    dishTypeLocalized = UiUtils.TranslateDishType(dishType.dish_type)
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

        private void selectAll_btn_Click(object sender, RoutedEventArgs e)
        {
            dishTypes_list.SelectAll();
        }

        private void unSelectAll_btn_Click(object sender, RoutedEventArgs e)
        {
            dishTypes_list.SelectedItems?.Clear();
        }

        #endregion
    }
}
