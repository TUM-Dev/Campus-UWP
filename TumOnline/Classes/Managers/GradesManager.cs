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
    public class GradesManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly GradesManager INSTANCE = new GradesManager();
        private Task<IEnumerable<Grade>> updateTask;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<IEnumerable<Grade>> UpdateAsync(TumOnlineCredentials credentials, bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                return await updateTask.ConfAwaitFalse();
            }

            updateTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(TumOnlineService.GRADES.NAME))
                {
                    Logger.Info("No need to fetch grades. Cache is still valid.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.Grades.Include(ctx.GetIncludePaths(typeof(Grade))).ToList();
                    }
                }
                IEnumerable<Grade> grades = null;
                try
                {
                    grades = await DownloadGradesAsync(credentials, force);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to request grades with:", e);
                }
                if (!(grades is null))
                {
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        ctx.RemoveRange(ctx.Grades);
                        ctx.AddRange(grades);
                    }
                    CacheDbContext.UpdateCacheEntry(TumOnlineService.GRADES.NAME, DateTime.Now.Add(TumOnlineService.GRADES.VALIDITY));
                }
                else
                {
                    Logger.Info("Loading grades from DB.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.Grades.Include(ctx.GetIncludePaths(typeof(Grade))).ToList();
                    }
                }
                return grades;
            });
            return await updateTask.ConfAwaitFalse();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<IEnumerable<Grade>> DownloadGradesAsync(TumOnlineCredentials credentials, bool force)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.GRADES);
            AccessManager.AddToken(request, credentials);
            XmlDocument doc = await request.RequestDocumentAsync(!force);
            return ParseGrades(doc);
        }

        private static List<Grade> ParseGrades(XmlDocument doc)
        {
            if (!(doc is null))
            {
                if (!(doc.SelectSingleNode("/error") is null))
                {
                    throw new InvalidTumOnlineResponseException(null, "Failed to request grades from TUM online.", doc.ToString());
                }
                List<Grade> grades = new List<Grade>();
                foreach (XmlNode gradeNode in doc.SelectNodes("/rowset/row"))
                {
                    Grade grade = ParseGrade(gradeNode);
                    if (!(grade is null))
                    {
                        grades.Add(grade);
                    }
                }
                return grades;
            }
            return null;
        }

        private static Grade ParseGrade(XmlNode gradeNode)
        {
            DateTime.TryParse(gradeNode.SelectSingleNode("datum").InnerText, out DateTime date);
            if (!int.TryParse(gradeNode.SelectSingleNode("lv_credits").InnerText, out int credits))
            {
                credits = -1;
            }
            return new Grade
            {
                CandidateNumber = gradeNode.SelectSingleNode("pv_kand_nr").InnerText,
                Date = date,
                ExaminerSurname = gradeNode.SelectSingleNode("pruefer_nachname").InnerText,
                ExamMode = gradeNode.SelectSingleNode("modus").InnerText,
                ExamType = gradeNode.SelectSingleNode("exam_typ_name").InnerText,
                GradeShort = gradeNode.SelectSingleNode("uninotenamekurz").InnerText,
                LectureCredits = credits,
                LectureNumber = gradeNode.SelectSingleNode("lv_nummer").InnerText,
                LectureSemester = gradeNode.SelectSingleNode("lv_semester").InnerText,
                LectureTite = gradeNode.SelectSingleNode("lv_titel").InnerText,
                StudyIdentifier = gradeNode.SelectSingleNode("studienidentifikator").InnerText,
                StudyTitle = gradeNode.SelectSingleNode("studienbezeichnung").InnerText
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
