using Logging.Classes;
using Storage.Classes;
using UI.Classes.Events.Mvg;
using UI_Context.Classes.Context.Controls.Mvg;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UI.Controls.Mvg
{
    public sealed partial class MvgWidgetControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly MvgWidgetControlContext VIEW_MODEL = new MvgWidgetControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public MvgWidgetControl()
        {
            InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void UpdateViewState(VisualState newState)
        {
            VisualStateManager.GoToState(this, newState.Name, true);
        }

        private void SwitchToStationSearch()
        {
            UpdateViewState(Edit_State);
            StationSearch.SetStation(VIEW_MODEL.MODEL.CurStation);
            StationSearch.Focus(FocusState.Programmatic);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnStationSelectionChanged(StationSearchControl sender, StationSelectionChangedEventArgs args)
        {
            VIEW_MODEL.ChangeStation(args.STATION);
            UpdateViewState(View_State);
        }

        private void OnEditStationClicked(object sender, RoutedEventArgs e)
        {
            SwitchToStationSearch();
        }

        private void OnStationTitleTapped(object sender, TappedRoutedEventArgs e)
        {
            SwitchToStationSearch();
        }

        #endregion
    }
}
