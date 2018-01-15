using System;
using TUMCampusAppAPI.Canteens;

namespace TUMCampusApp.Classes.Events
{
    public class CanteenSelectionChangedEventArgs : EventArgs
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CanteenTable SELECTED_CANTEEN;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/01/2018 Created [Fabian Sauter]
        /// </history>
        public CanteenSelectionChangedEventArgs(CanteenTable selectedCanteen)
        {
            this.SELECTED_CANTEEN = selectedCanteen;
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
