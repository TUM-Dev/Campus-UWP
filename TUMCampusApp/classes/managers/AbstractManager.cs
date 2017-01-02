using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes.cache;
using TUMCampusApp.classes.canteen;
using TUMCampusApp.classes.sync;
using TUMCampusApp.classes.userData;
using Windows.Storage;

namespace TUMCampusApp.classes.managers
{
    abstract class AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "data.db");
        protected static SQLiteConnection dB = new SQLiteConnection(new SQLitePlatformWinRT(), DB_PATH);

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public AbstractManager()
        {
            //createDB();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void createDB()
        {
            dB.CreateTable<Canteen>();
            dB.CreateTable<CanteenMenu>();
        }

        public static void resetDB()
        {
            dB.DropTable<Cache>();
            dB.DropTable<Canteen>();
            dB.DropTable<CanteenMenu>();
            dB.DropTable<Sync>();
            dB.DropTable<UserData>();
        }

        public static void deleteDB()
        {
            try
            {
                dB.Close();
                File.Delete(AbstractManager.DB_PATH);
            }
            catch (Exception e)
            {
                Logger.Error("Unable to close or delete the DB", e);
            }
            dB = new SQLiteConnection(new SQLitePlatformWinRT(), DB_PATH);
        }

        public abstract Task InitManagerAsync();

        public void execute(string querry, params object[] args)
        {
            dB.Execute(querry, args);
        }

        public void insert(object obj)
        {
            dB.Insert(obj);
        }

        public void update(object obj)
        {
            dB.InsertOrReplace(obj);
            saveDB();
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--
        protected void replaceIntoDB(Canteen c)
        {
            if (c.id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }
            if (c.name == null || c.name == "")
            {
                throw new ArgumentException("Invalid name.");
            }
            update(c);
        }

        protected void saveDB()
        {
            dB.SaveTransactionPoint();
        }

        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
