using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Logging.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Windows.Storage;

namespace Storage.Classes.Contexts
{
    public abstract class AbstractDbContext: DbContext, IDisposable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private readonly string DB_PATH;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public AbstractDbContext(string dbFileName)
        {
            DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbFileName);
            Database.EnsureCreated();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override void Dispose()
        {
            bool saved = false;
            while (!saved)
            {
                try
                {
                    SaveChanges();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Logger.Error("DB inconsistency found: ", ex);
                    foreach (EntityEntry entry in ex.Entries)
                    {
                        try
                        {
                            entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                        }
                        catch (Exception e)
                        {
                            Logger.Error($"DB inconsistency fix failed for: {entry.Entity.GetType()}", e);
                            if (Debugger.IsAttached)
                            {
                                Debugger.Break();
                            }
                        }
                    }
                }
            }
            base.Dispose();
        }

        public IEnumerable<string> GetIncludePaths(Type clrEntityType, int maxDepth = int.MaxValue)
        {
            IEntityType entityType = Model.FindEntityType(clrEntityType);
            Stack<INavigation> navHirar = new Stack<INavigation>();
            HashSet<INavigation> navHirarProps = new HashSet<INavigation>();

            HashSet<string> paths = new HashSet<string>();
            GetIncludePathsRec(navHirar, navHirarProps, entityType, paths, 0, maxDepth);
            return paths;
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=" + DB_PATH);
            }
        }

        protected void GetIncludePathsRec(Stack<INavigation> navHirar, HashSet<INavigation> navHirarProps, IEntityType entityType, HashSet<string> paths, int curDepth, int maxDepth)
        {
            if (curDepth < maxDepth)
            {
                foreach (INavigation navigation in entityType.GetNavigations())
                {
                    if (navHirarProps.Add(navigation))
                    {
                        navHirar.Push(navigation);
                        GetIncludePathsRec(navHirar, navHirarProps, navigation.GetTargetType(), paths, curDepth + 1, maxDepth);
                        navHirar.Pop();
                        navHirarProps.Remove(navigation);
                    }
                }
            }
            if (navHirar.Count > 0)
            {
                paths.Add(string.Join(".", navHirar.Reverse().Select(e => e.Name)));
            }
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }

    /// <summary>
    /// Based on: https://stackoverflow.com/questions/49593482/entity-framework-core-2-0-1-eager-loading-on-all-nested-related-entities/49597502#49597502
    /// </summary>
    public static partial class CustomQueryExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths) where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }
    }
}
