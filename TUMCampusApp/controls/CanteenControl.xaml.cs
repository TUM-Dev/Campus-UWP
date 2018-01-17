using System;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class CanteenControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public CanteenTable Canteen
        {
            get { return (CanteenTable)GetValue(CanteenProperty); }
            set
            {
                SetValue(CanteenProperty, value);
                showCanteen();
            }
        }
        public static readonly DependencyProperty CanteenProperty = DependencyProperty.Register("Canteen", typeof(CanteenTable), typeof(CanteenControl), null);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public CanteenControl()
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
        /// <summary>
        /// Shows the current canteen on the screen.
        /// </summary>
        private void showCanteen()
        {
            if (Canteen != null)
            {
                canteenName_tbx.Text = Canteen.name;
                canteenAdress_tbx.Text = Canteen.address;
                if (Canteen.distance < 0)
                {
                    canteenDistance_tbx.Text = "-";
                }
                else if (Canteen.distance >= 1000)
                {
                    canteenDistance_tbx.Text = Math.Round(Canteen.distance / 1000, 2) + " km";
                }
                else
                {
                    canteenDistance_tbx.Text = Math.Round(Canteen.distance, 0) + " m";
                }
                showFavoriteStar();
            }
        }

        private void showFavoriteStar()
        {
            if(Canteen != null) {
                if (Canteen.favorite)
                {
                    favorite_btn.Content = "\uE735";
                }
                else
                {
                    favorite_btn.Content = "\uE734";
                }
            }
        }

        private async Task showAddCanteenToFavoriteDialogAsync()
        {

        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private async void favorite_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Canteen != null)
            {
                Canteen.favorite = !Canteen.favorite;
                CanteenManager.INSTANCE.update(Canteen);
                if (Canteen.favorite)
                {
                    await showAddCanteenToFavoriteDialogAsync();
                }
                showFavoriteStar();
            }
        }

        #endregion
    }
}
