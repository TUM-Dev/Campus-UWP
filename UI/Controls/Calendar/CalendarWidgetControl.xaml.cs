using System.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using UI.Pages;
using UI.Pages.Content;
using UI_Context.Classes;
using UI_Context.Classes.Context.Controls.Calendar;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UI.Controls.Calendar
{
    public sealed partial class CalendarWidgetControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CalendarWidgetControlContext VIEW_MODEL = new CalendarWidgetControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CalendarWidgetControl()
        {
            InitializeComponent();
            VIEW_MODEL.MODEL.EVENTS.PropertyChanged += OnEventsCollectionPropertyChanged;
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
        private void OnRefreshClicked(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true);
        }

        private void OnSettingsClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnCalendarEventSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height == e.PreviousSize.Height)
            {
                return;
            }

            if (e.NewSize.Height < EventsFV.MinHeight)
            {
                EventsFV.Height = EventsFV.MinHeight;
            }
            else
            {
                EventsFV.MaxHeight = e.NewSize.Height;
            }
        }

        private void OnPigsPagerSelectedIndexChanged(PipsPager sender, PipsPagerSelectedIndexChangedEventArgs args)
        {
            if (sender.SelectedPageIndex > EventsFV.Items.Count)
            {
                return;
            }
            EventsFV.SelectedIndex = sender.SelectedPageIndex;
        }

        private void OnFlipViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventsFV.SelectedIndex > EventsPP.NumberOfPages)
            {
                return;
            }
            EventsPP.SelectedPageIndex = EventsFV.SelectedIndex;
        }

        private void OnEventsCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Count"))
            {
                EventsPP.NumberOfPages = VIEW_MODEL.MODEL.EVENTS.Count;
            }
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            UiUtils.NavigateToPage(typeof(CalendarPage), MainPage.GetContentFrame());
        }

        #endregion
    }
}
