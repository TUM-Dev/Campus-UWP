using System;
using System.Threading.Tasks;

namespace TUMCampusApp.Classes
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
            return "Log-" + date.Day + "." + date.Month + "." + date.Year + ".log";
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

        /// <summary>
        /// Adds a Error message to the log
        /// </summary>
        /// <history>
        /// 01/01/2017 Created [Fabian Sauter]
        /// </history>
        public static void Error(string message)
        {
            Error(message, null);
        }
        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Creates a task that adds the given message and exception to the log.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="e">The thrown exception.</param>
        /// <param name="code">The log code (INFO, DEBUG, ...)</param>
        private static void addToLog(string message, Exception e, string code)
        {
            Task t = addToLogAsync(message, e, code);
        }

        /// <summary>
        /// Adds the given message and exception to the log.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="e">The thrown exception.</param>
        /// <param name="code">The log code (INFO, DEBUG, ...)</param>
        private static async Task addToLogAsync(string message, Exception e, string code)
        {
            await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("Logs", Windows.Storage.CreationCollisionOption.OpenIfExists);
            Windows.Storage.StorageFile logFile = await (await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Logs")).CreateFileAsync(getFilename(), Windows.Storage.CreationCollisionOption.OpenIfExists);
            string s = "[" + code + "][" + getTimeStamp() + "]: " + message;
            if (e != null)
            {
                s += ":\n" + e.Message + "\n" + e.StackTrace;
            }
            System.Diagnostics.Debug.WriteLine(s);
            await Windows.Storage.FileIO.AppendTextAsync(logFile, s + Environment.NewLine);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
