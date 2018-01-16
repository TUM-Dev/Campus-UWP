using System.Collections.Generic;
using System.Collections.ObjectModel;
using TUMCampusApp.Classes.Events;
using TUMCampusApp.DataTemplates;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class CanteensControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public CanteenTable Canteen
        {
            get { return (CanteenTable)GetValue(CanteenProperty); }
            set { SetValue(CanteenProperty, value); }
        }
        public static readonly DependencyProperty CanteenProperty = DependencyProperty.Register("Canteen", typeof(CanteenTable), typeof(CanteensControl), null);

        public bool Expanded
        {
            get { return (bool)GetValue(ExpandedProperty); }
            set { SetValue(ExpandedProperty, value); }
        }
        public static readonly DependencyProperty ExpandedProperty = DependencyProperty.Register("Expanded", typeof(bool), typeof(CanteensControl), null);

        public delegate void CanteenSelectionChangedEventHandler(CanteensControl canteensControl, CanteenSelectionChangedEventArgs args);
        public delegate void ExpandedChangedEventHandler(CanteensControl canteensControl, ExpandedChangedEventArgs args);

        public event CanteenSelectionChangedEventHandler CanteenSelectionChanged;
        public event ExpandedChangedEventHandler ExpandedChanged;

        private ObservableCollection<CanteenTemplate> canteens;
        private int selectedIndex;
        private bool canteenSelected;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/01/2018 Created [Fabian Sauter]
        /// </history>
        public CanteensControl()
        {
            this.canteens = new ObservableCollection<CanteenTemplate>();
            this.selectedIndex = -1;
            this.InitializeComponent();
            this.Expanded = false;
            this.canteenSelected = false;
            showExpanded();
            reloadCanteens(null);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void setExpanded(bool expanded)
        {
            Expanded = expanded;
            ExpandedChanged?.Invoke(this, new ExpandedChangedEventArgs(expanded));
            showExpanded();
            showSelectedCanteen();
        }

        private CanteenTable getSelectedCanteen()
        {
            if (canteens_list.Items.Count > 0 && canteens_list.SelectedIndex < canteens.Count)
            {
                return canteens[canteens_list.SelectedIndex].canteen;
            }
            return null;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void reloadCanteens(string canteen_id)
        {
            canteens.Clear();
            string lastSelectedCanteen = canteen_id ?? UserDataManager.INSTANCE.getLastSelectedCanteenId();
            List<CanteenTable> c = CanteenManager.INSTANCE.getCanteens();
            for (int i = 0; i < c.Count; i++)
            {
                if (Equals(c[i].canteen_id, lastSelectedCanteen))
                {
                    selectedIndex = i;
                    selectedCanteen_tblck.Text = c[i].name;
                    Canteen = c[i];
                }
                canteens.Add(new CanteenTemplate() { canteen = c[i] });
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        private void showExpanded()
        {
            if (Expanded)
            {
                canteens_list.Visibility = Visibility.Visible;
                selectedCanteen_tblck.Visibility = Visibility.Collapsed;
            }
            else
            {
                canteens_list.Visibility = Visibility.Collapsed;
                selectedCanteen_tblck.Visibility = Visibility.Visible;
            }
        }

        private void selectCanteen()
        {
            if (selectedIndex >= 0 && selectedIndex < canteens_list.Items.Count)
            {
                canteens_list.SelectedIndex = selectedIndex;
            }
        }

        private void showSelectedCanteen()
        {
            selectedIndex = canteens_list.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < canteens.Count)
            {
                Canteen = canteens[selectedIndex].canteen;
                CanteenSelectionChanged?.Invoke(this, new CanteenSelectionChangedEventArgs(canteens[selectedIndex].canteen));
                selectedCanteen_tblck.Text = canteens[selectedIndex].canteen.name;
            }
            else
            {
                selectedCanteen_tblck.Text = "Invalid canteen!";
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void canteens_list_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            selectCanteen();
        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (canteenSelected)
            {
                canteenSelected = false;
                return;
            }
            if (!Expanded)
            {
                setExpanded(true);
                e.Handled = true;
            }
        }

        private void canteens_list_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (Expanded)
            {
                canteenSelected = true;
                setExpanded(false);
            }
        }

        private void canteens_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CanteenTable canteen = getSelectedCanteen();
            if(canteen != null)
            {
                UserDataManager.INSTANCE.setLastSelectedCanteenId(canteen.canteen_id);
            }
        }

        #endregion
    }
}
