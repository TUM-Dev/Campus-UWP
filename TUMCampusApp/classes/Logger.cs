using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMCampusApp.classes
{
    class Logger
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private static string getFilename()
        {
            DateTime date = DateTime.Now;
            return "Log-" + date.Day + "." + date.Month + "." + date.Year;
        }

        private static string getTimeStamp()
        {
            DateTime date = DateTime.Now;
            return date.Day + "." + date.Month + "." + date.Year + " " + date.Hour + ":" + date.Minute + ":" + date.Second;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Adds a Debug message to the log
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public static void Debug(string message)
        {
            addToLog(message, null, "DEBUG");
        }

        /// <summary>
        /// Adds a Info message to the log
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public static void Info(string message)
        {
            addToLog(message, null, "INFO");
        }

        /// <summary>
        /// Adds a Warn message to the log
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public static void Warn(string message)
        {
            addToLog(message, null, "WARN");
        }

        /// <summary>
        /// Adds a Error message to the log
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public static void Error(string message, Exception e)
        {
            addToLog(message, e, "ERROR");
        }
        #endregion

        #region --Misc Methods (Private)--
        private static void addToLog(string message, Exception e, string code)
        {
            addToLogAsync(message, e, code).Wait();
        }

        private static async Task addToLogAsync(string message, Exception e, string code)
        {
            Windows.Storage.StorageFile logFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(getFilename(), Windows.Storage.CreationCollisionOption.OpenIfExists);
            string s = "[" + code + "][" + getTimeStamp() + "]: " + message;
            if (e != null)
            {
                s += ":\n" + e.Message + "\n" + e.StackTrace;
            }
            s += "\n";
            System.Diagnostics.Debug.Write(s);
            await Windows.Storage.FileIO.AppendTextAsync(logFile, s);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
