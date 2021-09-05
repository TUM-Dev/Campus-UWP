using Shared.Classes;
using Shared.Classes.Collections;
using Storage.Classes.Models.TumOnline;

namespace UI_Context.Classes.Templates.Controls.Calendar
{
    public class CalendarEventControlDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CustomObservableCollection<CalendarEvent> EVENTS = new CustomObservableCollection<CalendarEvent>(true);

        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetProperty(ref _IsLoading, value);
        }

        private bool _HasUpcomingEvents;
        public bool HasUpcomingEvents
        {
            get => _HasUpcomingEvents;
            set => SetProperty(ref _HasUpcomingEvents, value);
        }
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
