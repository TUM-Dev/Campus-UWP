using System;
using TUMCampusAppAPI.Canteens;
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
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
