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
using TumOnline.Classes.Events;
using TumOnline.Classes.Exceptions;

namespace TumOnline.Classes.Managers
{
    public class LecturesManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly LecturesManager INSTANCE = new LecturesManager();
        private Task<IEnumerable<Lecture>> updateTask;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<IEnumerable<Lecture>> UpdateAsync(TumOnlineCredentials credentials, bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                try
                {
                    return await updateTask.ConfAwaitFalse();
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Awaiting for lecture task failed with:", e);
                    return new List<Lecture>();
                }
            }

            updateTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(TumOnlineService.LECTURES_PERSONAL.NAME))
                {
                    Logger.Info("No need to fetch lectures. Cache is still valid.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.Lectures.Include(ctx.GetIncludePaths(typeof(Lecture))).ToList();
                    }
                }
                IEnumerable<Lecture> lectures = null;
                try
                {
                    lectures = await DownloadLecturesAsync(credentials, force);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to request lectures with:", e);
                }
                if (!(lectures is null))
                {
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        ctx.RemoveRange(ctx.Lectures);
                        ctx.AddRange(lectures);
                    }
                    CacheDbContext.UpdateCacheEntry(TumOnlineService.LECTURES_PERSONAL.NAME, DateTime.Now.Add(TumOnlineService.LECTURES_PERSONAL.VALIDITY));
                }
                else
                {
                    Logger.Info("Loading lectures from DB.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.Lectures.Include(ctx.GetIncludePaths(typeof(Lecture))).ToList();
                    }
                }
                return lectures;
            });
            try
            {
                return await updateTask.ConfAwaitFalse();
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error("Awaiting for lecture task failed with:", e);
            }
            return new List<Lecture>();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<IEnumerable<Lecture>> DownloadLecturesAsync(TumOnlineCredentials credentials, bool force)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.LECTURES_PERSONAL);
            AccessManager.AddToken(request, credentials);
            XmlDocument doc = await request.RequestDocumentAsync(!force);
            return ParseLectures(doc);
        }

        private static List<Lecture> ParseLectures(XmlDocument doc)
        {
            if (!(doc is null))
            {
                if (!(doc.SelectSingleNode("/error") is null))
                {
                    throw new InvalidTumOnlineResponseException(null, "Failed to request lectures from TUM online.", doc.ToString());
                }
                List<Lecture> lectures = new List<Lecture>();
                foreach (XmlNode lectureNode in doc.SelectNodes("/rowset/row"))
                {
                    Lecture lecture = ParseLecture(lectureNode);
                    if (!(lecture is null))
                    {
                        lectures.Add(lecture);
                    }
                }
                return lectures;
            }
            return null;
        }

        private static Lecture ParseLecture(XmlNode lectureNode)
        {
            if (!int.TryParse(lectureNode.SelectSingleNode("stp_sp_nr").InnerText, out int spNr))
            {
                spNr = -1;
            }
            if (!int.TryParse(lectureNode.SelectSingleNode("stp_lv_nr").InnerText, out int lvNr))
            {
                lvNr = -1;
            }
            if (!double.TryParse(lectureNode.SelectSingleNode("dauer_info").InnerText, out double duration))
            {
                duration = 0;
            }
            if (!double.TryParse(lectureNode.SelectSingleNode("stp_sp_sst").InnerText, out double spSst))
            {
                spSst = 0;
            }
            if (!int.TryParse(lectureNode.SelectSingleNode("org_nr_betreut").InnerText, out int supervisorId))
            {
                supervisorId = -1;
            }
            return new Lecture
            {
                SpNr = spNr,
                LvNr = lvNr,
                Title = lectureNode.SelectSingleNode("stp_sp_titel").InnerText,
                Duration = duration,
                SpSst = spSst,
                TypeLong = lectureNode.SelectSingleNode("stp_lv_art_name").InnerText,
                TypeShort = lectureNode.SelectSingleNode("stp_lv_art_kurz").InnerText,
                SemesterYearName = lectureNode.SelectSingleNode("sj_name").InnerText,
                Semester = lectureNode.SelectSingleNode("semester").InnerText,
                SemesterName = lectureNode.SelectSingleNode("semester_name").InnerText,
                SemesterId = lectureNode.SelectSingleNode("semester_id").InnerText,
                FacultySupervisorId = supervisorId,
                FacultySupervisorName = lectureNode.SelectSingleNode("org_name_betreut").InnerText,
                FacultyId = lectureNode.SelectSingleNode("org_kennung_betreut").InnerText,
                Contributors = lectureNode.SelectSingleNode("vortragende_mitwirkende").InnerText
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
