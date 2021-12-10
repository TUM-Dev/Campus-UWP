using System;
using ExternalData.Classes.Mvg;
using Shared.Classes;
using Shared.Classes.Collections;

namespace UI_Context.Classes.Templates.Controls.Mvg
{
    public class MvgWidgetControlDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetProperty(ref _IsLoading, value);
        }
        private bool _IsLoading;

        public bool HasDepartures
        {
            get => _HasDepartures;
            set => SetProperty(ref _HasDepartures, value);
        }
        private bool _HasDepartures;

        public DateTime LastUpdate
        {
            get => _LastUpdate;
            set => SetProperty(ref _LastUpdate, value);
        }
        private DateTime _LastUpdate;

        public Station CurStation
        {
            get => _CurStation;
            set => SetProperty(ref _CurStation, value);
        }
        private Station _CurStation;

        public readonly CustomObservableCollection<Departure> DEPARTURES = new CustomObservableCollection<Departure>(true);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


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
