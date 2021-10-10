using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Classes;
using Storage.Classes;
using Storage.Classes.Models.TumOnline;
using TumOnline.Classes.Events;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Controls.Calendar;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class CalendarPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CalendarPageDataTemplate MODEL = new CalendarPageDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CalendarPageContext()
        {
            CalendarManager.INSTANCE.OnRequestError += OnRequestError;
            Task.Run(async () => await LoadEventsAsync(false));
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh()
        {
            Task.Run(async () => await LoadEventsAsync(true));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadEventsAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            MODEL.ShowError = false;
            IEnumerable<CalendarEvent> events = await CalendarManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
            AddSortedEvents(events);
            MODEL.HasEvents = MODEL.EVENTS_COLLECTIONS.Count > 0;
            MODEL.HasUpcomingEvents = MODEL.HasEvents && MODEL.EVENTS_COLLECTIONS[0].Key > DateTime.Now;
            MODEL.IsLoading = false;
        }

        private void AddSortedEvents(IEnumerable<CalendarEvent> events)
        {
            IEnumerable<CalendarEventGroupDataTemplate> query = from ev in events group ev by ev.Start.Date into d orderby d.Key ascending select new CalendarEventGroupDataTemplate(d) { Key = d.Key };
            MODEL.EVENTS_COLLECTIONS.Clear();
            MODEL.EVENTS_COLLECTIONS.AddRange(query);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnRequestError(AbstractManager sender, RequestErrorEventArgs e)
        {
            MODEL.ShowError = true;
            MODEL.ErrorMsg = "Failed to load calendar events.\n" + e.GenerateErrorMessage();
        }

        #endregion
    }
}
