using System;
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
    public class IdentityManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly IdentityManager INSTANCE = new IdentityManager();
        private Task<Identity> updateTask;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<Identity> UpdateAsync(TumOnlineCredentials credentials, bool force)
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
                    Logger.Error("Awaiting for identity task failed with:", e);
                    return null;
                }
            }

            updateTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(TumOnlineService.ID.NAME))
                {
                    Logger.Info("No need to fetch identity. Cache is still valid.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.Identities.Include(ctx.GetIncludePaths(typeof(Identity))).FirstOrDefault();
                    }
                }
                Identity identity = null;
                try
                {
                    identity = await DownloadIdentityAsync(credentials, force);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to request identity with:", e);
                }
                if (!(identity is null))
                {
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        ctx.RemoveRange(ctx.Identities);
                        ctx.Add(identity);
                    }
                    CacheDbContext.UpdateCacheEntry(TumOnlineService.ID.NAME, DateTime.Now.Add(TumOnlineService.ID.VALIDITY));
                }
                else
                {
                    Logger.Info("Loading identity from DB.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.Identities.Include(ctx.GetIncludePaths(typeof(Identity))).FirstOrDefault();
                    }
                }
                return identity;
            });
            try
            {
                return await updateTask.ConfAwaitFalse();
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error("Awaiting for identity task failed with:", e);
            }
            return null;
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<Identity> DownloadIdentityAsync(TumOnlineCredentials credentials, bool force)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.ID);
            AccessManager.AddToken(request, credentials);
            XmlDocument doc = await request.RequestDocumentAsync(!force);
            return ParseIdentity(doc);
        }

        private static Identity ParseIdentity(XmlDocument doc)
        {
            if (!(doc is null))
            {
                if (!(doc.SelectSingleNode("/error") is null))
                {
                    throw new InvalidTumOnlineResponseException(null, "Failed to request identity from TUM online.", doc.ToString());
                }
                XmlNodeList rows = doc.SelectNodes("/rowset/row");
                if (rows.Count > 0)
                {
                    XmlNode row = rows[0];
                    XmlNode obfuscatedIds = row.SelectSingleNode("obfuscated_ids");
                    return new Identity
                    {
                        Id = row.SelectSingleNode("kennung").InnerText,
                        FirstName = row.SelectSingleNode("vorname").InnerText,
                        LastName = row.SelectSingleNode("familienname").InnerText,
                        ObfuscatedId = row.SelectSingleNode("obfuscated_id").InnerText,
                        ObfuscatedStudentId = obfuscatedIds.SelectSingleNode("studierende").InnerText,
                        ObfuscatedStaffId = obfuscatedIds.SelectSingleNode("bedienstete").InnerText,
                        ObfuscatedExternalId = obfuscatedIds.SelectSingleNode("extern").InnerText,
                    };
                }
            }
            return null;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
