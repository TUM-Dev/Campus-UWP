using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
            this.Expanded = false;
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Sets expanded and opens or closes it.
        /// </summary>
        private void setExpanded(bool expanded)
        {
            if (expanded)
            {
                open();
            }
            else
            {
                close();
            }
        }

        /// <summary>
        /// Returns the selected canteen.
        /// </summary>
        /// <returns>The currently selected canteen.</returns>
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
        /// <summary>
        /// Hides the canteens and shows the currently selected one.
        /// </summary>
        public void close()
        {
            Expanded = false;
            showExpanded();
        }

        /// <summary>
        /// Shows all canteens and hides the currently selected one.
        /// </summary>
        public void open()
        {
            Expanded = true;
            showExpanded();
        }

        /// <summary>
        /// Reloads all canteens and shows them on the screen.
        /// </summary>
        /// <param name="canteen_id">If not null will show the given canteen as selected one.</param>
        public async Task reloadCanteensAsync(string canteen_id)
        {
            canteens.Clear();
            string lastSelectedCanteen = canteen_id ?? UserDataDBManager.INSTANCE.getLastSelectedCanteenId();
            List<CanteenTable> c = await CanteenDBManager.INSTANCE.getCanteensWithDistanceAsync();
            sortCanteens(c);
            for (int i = 0; i < c.Count; i++)
            {
                canteens.Add(new CanteenTemplate() { canteen = c[i] });
                if (Equals(c[i].canteen_id, lastSelectedCanteen))
                {
                    selectedCanteen_tblck.Text = c[i].name;
                    Canteen = c[i];
                    canteens_list.SelectedItem = canteens[i];
                }
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Sorts a list of canteens based on favorite and distance.
        /// </summary>
        /// <param name="list">The list of canteens to sort.</param>
        private void sortCanteens(List<CanteenTable> list)
        {
            list.Sort((a, b) =>
            {
                if (a == b)
                {
                    return 0;
                }
                else if (a == null)
                {
                    return -1;
                }
                else if (b == null)
                {
                    return 1;
                }

                if (a.favorite == b.favorite)
                {
                    if(a.distance == b.distance)
                    {
                        return 0;
                    }
                    else if (a.distance > b.distance)
                    {
                        return 1;
                    }
                    return -1;
                }
                if (a.favorite)
                {
                    return -1;
                }
                return 1;
            });
        }

        /// <summary>
        /// Shows and hides controls based on Expanded.
        /// </summary>
        private void showExpanded()
        {
            if (Expanded)
            {
                canteens_list.Visibility = Visibility.Visible;
                selectedCanteen_grid.Visibility = Visibility.Collapsed;
            }
            else
            {
                canteens_list.Visibility = Visibility.Collapsed;
                selectedCanteen_grid.Visibility = Visibility.Visible;
            }
            ExpandedChanged?.Invoke(this, new ExpandedChangedEventArgs(Expanded));
        }

        /// <summary>
        /// Shows the currently selected canteen and triggers the CanteenSelectionChanged event.
        /// </summary>
        private void showSelectedCanteen()
        {
            int selectedIndex = canteens_list.SelectedIndex;
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
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            showExpanded();
            await reloadCanteensAsync(null);
        }

        private void canteens_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CanteenTable canteen = getSelectedCanteen();
            if (canteen != null)
            {
                UserDataDBManager.INSTANCE.setLastSelectedCanteenId(canteen.canteen_id);
                showSelectedCanteen();
            }
            close();
        }

        private void selectedCanteen_grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            open();
            e.Handled = true;
        }

        #endregion
    }
}
