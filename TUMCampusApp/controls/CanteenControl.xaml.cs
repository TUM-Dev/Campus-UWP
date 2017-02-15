using System;
using TUMCampusAppAPI.Canteens;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class CanteenControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly Canteen canteen;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
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

        public CanteenControl(Canteen canteen)
        {
            this.InitializeComponent();
            this.canteen = canteen;
            canteenName_tbx.Text = canteen.name;
            canteenAdress_tbx.Text = canteen.address;
            if (canteen.distance >= 1000)
            {
                canteenDistance_tbx.Text = Math.Round(canteen.distance / 1000, 2) + " km";
            }
            else
            {
                canteenDistance_tbx.Text = Math.Round(canteen.distance, 0) + " m";
            }

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
