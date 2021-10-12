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

namespace UI_Context.Classes.Context.Controls.Calendar
{
    public class CalendarWidgetControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CalendarWidgetControlDataTemplate MODEL = new CalendarWidgetControlDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CalendarWidgetControlContext()
        {
            Refresh(false);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh(bool refresh)
        {
            Task.Run(async () => await LoadEventsAsync(refresh));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadEventsAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            try
            {
                IEnumerable<CalendarEvent> events = await CalendarManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
                MODEL.EVENTS.Clear();
                MODEL.EVENTS.AddRange(events.Where(e => e.End > DateTime.Now).Take(5));
            }
            catch (Exception e)
            {
                Logger.Error("Failed to load calendar events!", e);
            }
            MODEL.HasUpcomingEvents = MODEL.EVENTS.Count > 0;
            MODEL.IsLoading = false;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
