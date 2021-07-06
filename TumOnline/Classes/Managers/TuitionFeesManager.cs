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
    public class TuitionFeesManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly TuitionFeesManager INSTANCE = new TuitionFeesManager();
        private Task<IEnumerable<TuitionFee>> updateTask;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<IEnumerable<TuitionFee>> UpdateAsync(TumOnlineCredentials credentials, bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                return await updateTask.ConfAwaitFalse();
            }

            updateTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(TumOnlineService.TUITION_FEE_STATUS.NAME))
                {
                    Logger.Info("No need to fetch tuition fees. Cache is still valid.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.TuitionFees.Include(ctx.GetIncludePaths(typeof(TuitionFee))).ToList();
                    }
                }
                IEnumerable<TuitionFee> fees = null;
                try
                {
                    fees = await DownloadFeesAsync(credentials, force);
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to request tuition fees with:", e);
                }
                if (!(fees is null))
                {
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        ctx.RemoveRange(ctx.Grades);
                        ctx.AddRange(fees);
                    }
                    CacheDbContext.UpdateCacheEntry(TumOnlineService.GRADES.NAME, DateTime.Now.Add(TumOnlineService.GRADES.VALIDITY));
                }
                else
                {
                    Logger.Info("Loading tuition fees from DB.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.TuitionFees.Include(ctx.GetIncludePaths(typeof(TuitionFee))).ToList();
                    }
                }
                return fees;
            });
            return await updateTask.ConfAwaitFalse();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<IEnumerable<TuitionFee>> DownloadFeesAsync(TumOnlineCredentials credentials, bool force)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.TUITION_FEE_STATUS);
            AccessManager.AddToken(request, credentials);
            XmlDocument doc = await request.RequestDocumentAsync(!force);
            return ParseFees(doc);
        }

        private static List<TuitionFee> ParseFees(XmlDocument doc)
        {
            if (!(doc is null))
            {
                if (!(doc.SelectSingleNode("/error") is null))
                {
                    throw new InvalidTumOnlineResponseException(null, "Failed to request grades from TUM online.", doc.ToString());
                }
                List<TuitionFee> fees = new List<TuitionFee>();
                foreach (XmlNode feeNode in doc.SelectNodes("/rowset/row"))
                {
                    try
                    {
                        TuitionFee fee = ParseFee(feeNode);
                        if (!(fee is null))
                        {
                            fees.Add(fee);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Failed to parse tuition fee with: ", e);
                    }
                }
                return fees;
            }
            return null;
        }

        private static TuitionFee ParseFee(XmlNode gradeNode)
        {
            if (!DateTime.TryParse(gradeNode.SelectSingleNode("frist").InnerText, out DateTime deadline))
            {
                deadline = DateTime.MaxValue;
            }
            if (!double.TryParse(gradeNode.SelectSingleNode("soll").InnerText, out double amount))
            {
                amount = -1;
            }
            return new TuitionFee
            {
                Amount = amount,
                Deadline = deadline,
                SemesterId = gradeNode.SelectSingleNode("semester_id").InnerText,
                SemesterName = gradeNode.SelectSingleNode("semester_bezeichnung").InnerText,
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
