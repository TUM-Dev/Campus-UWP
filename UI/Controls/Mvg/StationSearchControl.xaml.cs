using ExternalData.Classes.Mvg;
using UI.Classes.Events.Mvg;
using UI_Context.Classes.Context.Controls.Mvg;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Mvg
{
    public sealed partial class StationSearchControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly StationSearchControlContext VIEW_MODEL = new StationSearchControlContext();

        public delegate void StationSelectionChangedHandler(StationSearchControl sender, StationSelectionChangedEventArgs args);
        public event StationSelectionChangedHandler StationSelectionChanged;
        private Station curStation;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public StationSearchControl()
        {
            InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public void SetStation(Station station)
        {
            stationAsb.Text = station.name;
            curStation = station;
        }

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
        private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                VIEW_MODEL.Search(sender.Text.ToLower().Trim());
            }
        }

        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

            if (args.ChosenSuggestion is Station station)
            {
                curStation = station;
                StationSelectionChanged?.Invoke(this, new StationSelectionChangedEventArgs(station));
            }
            else if (!(curStation is null))
            {
                if (string.Equals(args.QueryText, curStation.name))
                {
                    StationSelectionChanged?.Invoke(this, new StationSelectionChangedEventArgs(curStation));
                }
            }
        }

        #endregion
    }
}
