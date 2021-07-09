using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes;
using Storage.Classes.Contexts;
using Storage.Classes.Models.TumOnline;
using TumOnline.Classes.Exceptions;

namespace TumOnline.Classes.Managers
{
    public class CalendarManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly CalendarManager INSTANCE = new CalendarManager();
        private Task<IEnumerable<CalendarEvent>> updateTask;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<IEnumerable<CalendarEvent>> UpdateAsync(TumOnlineCredentials credentials, bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                return await updateTask.ConfAwaitFalse();
            }

            updateTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(TumOnlineService.CALENDAR.NAME))
                {
                    Logger.Info("No need to fetch calendar events. Cache is still valid.");
                    return LoadCalendarEventsFromDb();
                }
                IEnumerable<CalendarEvent> events = null;
                try
                {
                    events = await DownloadCalendarEventsAsync(credentials, force);
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to request calendar events with:", e);
                }
                if (!(events is null))
                {
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        ctx.RemoveRange(ctx.CalendarEvents);
                        ctx.AddRange(events);
                    }
                    CacheDbContext.UpdateCacheEntry(TumOnlineService.CALENDAR.NAME, DateTime.Now.Add(TumOnlineService.CALENDAR.VALIDITY));
                }
                else
                {
                    return LoadCalendarEventsFromDb();
                }
                return events;
            });
            return await updateTask.ConfAwaitFalse();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<IEnumerable<CalendarEvent>> DownloadCalendarEventsAsync(TumOnlineCredentials credentials, bool force)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.CALENDAR);
            AccessManager.AddToken(request, credentials);
            request.AddQuery("pMonateVor", "2");
            request.AddQuery("pMonateNach", "5");
            XmlDocument doc = await request.RequestDocumentAsync(!force);
            return ParseCalendarEvents(doc);
        }

        private List<CalendarEvent> LoadCalendarEventsFromDb()
        {
            Logger.Info("Loading calendar events from DB.");
            using (TumOnlineDbContext ctx = new TumOnlineDbContext())
            {
                return ctx.CalendarEvents.Include(ctx.GetIncludePaths(typeof(CalendarEvent))).ToList();
            }
        }

        private static List<CalendarEvent> ParseCalendarEvents(XmlDocument doc)
        {
            if (!(doc is null))
            {
                if (!(doc.SelectSingleNode("/error") is null))
                {
                    throw new InvalidTumOnlineResponseException(null, "Failed to request grades from TUM online.", doc.ToString());
                }
                List<CalendarEvent> events = new List<CalendarEvent>();
                foreach (XmlNode eventNode in doc.SelectNodes("/events/event"))
                {
                    CalendarEvent calEvent = ParseCalendarEvent(eventNode);
                    if (!(calEvent is null))
                    {
                        events.Add(calEvent);
                    }
                }
                return events;
            }
            return null;
        }

        private static CalendarEvent ParseCalendarEvent(XmlNode eventNode)
        {
            DateTime.TryParse(eventNode.SelectSingleNode("dtstart").InnerText, out DateTime start);
            DateTime.TryParse(eventNode.SelectSingleNode("dtend").InnerText, out DateTime end);
            return new CalendarEvent
            {
                Description = eventNode.SelectSingleNode("description").InnerText,
                End = end,
                Location = eventNode.SelectSingleNode("location").InnerText,
                Nr = eventNode.SelectSingleNode("nr").InnerText,
                Start = start,
                Status = eventNode.SelectSingleNode("status").InnerText,
                Title = eventNode.SelectSingleNode("title").InnerText,
                Url = eventNode.SelectSingleNode("url").InnerText
            };
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
