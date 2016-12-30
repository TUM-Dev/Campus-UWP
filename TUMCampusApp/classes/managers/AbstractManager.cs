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

        public void deleteCache()
        {
            dB.DropTable<Canteen>();
            dB.DropTable<CanteenMenu>();
        }

        public static void resetDB()
        {
            dB.DropTable<CanteenMenu>();
            dB.DropTable<Canteen>();
            dB.DropTable<Sync>();
            dB.DropTable<Cache>();
            //execute("DROP TABLE IF EXISTS cache");
            //execute("DROP TABLE IF EXISTS canteens");
            //execute("DROP TABLE IF EXISTS canteens_menus");
            //execute("DROP TABLE IF EXISTS calendar");
            //execute("DROP TABLE IF EXISTS kalendar_events");
            //execute("DROP TABLE IF EXISTS locations");
            //execute("DROP TABLE IF EXISTS news");
            //execute("DROP TABLE IF EXISTS news_sources");
            //execute("DROP TABLE IF EXISTS recents");
            //execute("DROP TABLE IF EXISTS room_locations");
            //execute("DROP TABLE IF EXISTS syncs");
            //execute("DROP TABLE IF EXISTS suggestions_lecture");
            //execute("DROP TABLE IF EXISTS suggestions_mvv");
            //execute("DROP TABLE IF EXISTS suggestions_persons");
            //execute("DROP TABLE IF EXISTS suggestions_rooms");
            //execute("DROP TABLE IF EXISTS unsent_chat_message");
            //execute("DROP TABLE IF EXISTS chat_message");
            //execute("DROP TABLE IF EXISTS chat_room");
            //execute("DROP TABLE IF EXISTS tumLocks");
            //execute("DROP TABLE IF EXISTS openQuestions");
            //execute("DROP TABLE IF EXISTS ownQuestions");
            //execute("DROP TABLE IF EXISTS faculties");
        }

        public virtual async Task initManagerAsync()
        {

        }

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
