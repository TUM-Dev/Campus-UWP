using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TUMCampusApp.classes;
using TUMCampusApp.classes.canteen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Controls
{
    public sealed class CanteenControl : Control
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
        /// 12/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteenControl(Canteen canteen)
        {
            this.DefaultStyleKey = typeof(CanteenControl);
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.Margin = new Thickness(10, 10, 10, 0);
            this.canteen = canteen;
            this.CanteenName = canteen.name;
            this.CanteenAdress = canteen.address;
            if(canteen.distance >= 1000)
            {
                this.CanteenDistance = Math.Round(canteen.distance / 1000, 2) + " km";
            }
            else
            {
                this.CanteenDistance = Math.Round(canteen.distance, 0) + " m";
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
        //--------------------------------------------------------Propertys:---------------------------------------------------------------------\\
        #region --Propertys--
        public string CanteenName
        {
            get { return (string)GetValue(CanteenNameParoperty); }
            set { SetValue(CanteenNameParoperty, value); }
        }
        public static readonly DependencyProperty CanteenNameParoperty = DependencyProperty.Register("CanteenName", typeof(string), typeof(CanteenControl), new PropertyMetadata("Default Name"));

        public string CanteenAdress
        {
            get { return (string)GetValue(CanteenAdressProperty); }
            set { SetValue(CanteenAdressProperty, value); }
        }
        public static readonly DependencyProperty CanteenAdressProperty = DependencyProperty.Register("CanteenAdress", typeof(string), typeof(CanteenControl), new PropertyMetadata("Default Adress"));

        public string CanteenDistance
        {
            get { return (string)GetValue(CanteenDistanceProperty); }
            set { SetValue(CanteenDistanceProperty, value); }
        }
        public static readonly DependencyProperty CanteenDistanceProperty = DependencyProperty.Register("CanteenDistance", typeof(string), typeof(CanteenControl), new PropertyMetadata("Default Distance"));

        #endregion
    }
}
