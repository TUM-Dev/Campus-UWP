using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Storage.Classes.Models.Canteens;

namespace Storage.Classes.Contexts
{
    public class CanteensDbContext: AbstractDbContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public DbSet<Canteen> Canteens { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Price> Prices { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CanteensDbContext() : base("canteens.db")
        {
            // Disable change tracking since we always manually update them and only require them read only:
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.EnsureCreated();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public static Language GetActiveLanguage()
        {
            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                return ctx.Languages.Where(l => l.Active).FirstOrDefault();
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Based on: https://entityframeworkcore.com/knowledge-base/37370476/how-to-persist-a-list-of-strings-with-entity-framework-core-
            ValueConverter<List<string>, string> splitStringConverter = new ValueConverter<List<string>, string>(v => string.Join(";", v), v => string.IsNullOrWhiteSpace(v) ? new List<string>() : v.Split(new[] { ';' }).ToList());
            // Make sure we can store a list of strings in the DB:
            modelBuilder.Entity<Dish>().Property(nameof(Dish.Labels)).HasConversion(splitStringConverter);
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
