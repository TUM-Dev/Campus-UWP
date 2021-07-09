using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes;
using Storage.Classes.Models.TumOnline;
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
            try
            {
                IEnumerable<CalendarEvent> events = await CalendarManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
                AddSortedEvents(events);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to load calendar events!", e);
            }
            MODEL.HasEvents = MODEL.EVENTS_COLLECTIONS.Count > 0;
            MODEL.IsLoading = false;
        }

        private void AddSortedEvents(IEnumerable<CalendarEvent> events)
        {
            IEnumerable<CalendarEventGroupDataTemplate> query = from ev in events group ev by ev.Start.Date into d orderby d.Key select new CalendarEventGroupDataTemplate(d) { Key = d.Key.ToString() };
            MODEL.EVENTS_COLLECTIONS.Clear();
            MODEL.EVENTS_COLLECTIONS.AddRange(query);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
