using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Storage.Classes.Models.Cache;

namespace Storage.Classes.Contexts
{
    public class CacheDbContext: AbstractDbContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public DbSet<CacheLine> CacheLines { get; set; }
        public DbSet<CacheEntry> CacheEntries { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CacheDbContext() : base("cache.db")
        {
            // Disable change tracking since we always manually update them and only require them read only:
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.EnsureCreated();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Checks if there exists a <see cref="CacheEntry"/> for the given <paramref name="id"/> that is not older than <paramref name="maxAge"/>.
        /// </summary>
        /// <param name="id">The ID of the <see cref="CacheEntry"/>.</param>
        /// <param name="maxAge">The maximum age, the <see cref="CacheEntry"/> is allowed to be.</param>
        /// <returns>True in case there exists a valid <see cref="CacheEntry"/> which is not older than <paramref name="maxAge"/>.</returns>
        public static bool IsCacheEntryUpToDate(string id, TimeSpan maxAge)
        {
            CacheEntry entry = null;
            using (CacheDbContext ctx = new CacheDbContext())
            {
                entry = ctx.CacheEntries.Where(e => string.Equals(e.Id, id)).FirstOrDefault();
            }

            return !(entry is null) && (entry.LastUpdated.Add(maxAge) > DateTime.Now);
        }

        /// <summary>
        /// Checks if there exists a <see cref="CacheLine"/> for the given <paramref name="id"/> that is not older than <paramref name="maxAge"/>.
        /// </summary>
        /// <param name="id">The ID of the <see cref="CacheLine"/>.</param>
        /// <param name="maxAge">The maximum age, the <see cref="CacheLine"/> is allowed to be.</param>
        /// <returns>The cached data in case there exists a valid <see cref="CacheLine"/> which is not older than <paramref name="maxAge"/>. Else, null.</returns>
        public static string GetCacheLine(string id, TimeSpan maxAge)
        {
            CacheLine line = null;
            using (CacheDbContext ctx = new CacheDbContext())
            {
                line = ctx.CacheLines.Where(e => string.Equals(e.Id, id)).FirstOrDefault();
            }
            return !(line is null) && (line.LastUpdated.Add(maxAge) > DateTime.Now) ? line.Data : null;
        }

        public static void UpdateCacheEntry(string id, DateTime lastUpdated)
        {
            CacheEntry entry = null;
            using (CacheDbContext ctx = new CacheDbContext())
            {
                entry = ctx.CacheEntries.Where(e => string.Equals(e.Id, id)).FirstOrDefault();
            }
            if (entry is null)
            {
                entry = new CacheEntry
                {
                    Id = id,
                    LastUpdated = lastUpdated
                };
                entry.Add();
            }
            else
            {
                entry.LastUpdated = lastUpdated;
                entry.Update();
            }
        }

        public static void UpdateCacheLine(string id, DateTime lastUpdated, string data)
        {
            CacheLine line = null;
            using (CacheDbContext ctx = new CacheDbContext())
            {
                line = ctx.CacheLines.Where(e => string.Equals(e.Id, id)).FirstOrDefault();
            }
            if (line is null)
            {
                line = new CacheLine
                {
                    Id = id,
                    LastUpdated = lastUpdated,
                    Data = data
                };
                line.Add();
            }
            else
            {
                line.LastUpdated = lastUpdated;
                line.Data = data;
                line.Update();
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
