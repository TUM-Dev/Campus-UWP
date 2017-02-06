using System;
using System.Threading.Tasks;
using Windows.Storage;

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

        /// <summary>
        /// Opens the log folder.
        /// </summary>
        /// <returns>An async Task.</returns>
        public static async Task openLogFolderAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            await Windows.System.Launcher.LaunchFolderAsync(folder);
        }

        /// <summary>
        /// Deletes the "Logs" folder and creates a new empty one.
        /// </summary>
        /// <returns>An async Task.</returns>
        public static async Task deleteLogsAsync()
        {
            StorageFolder folder = (StorageFolder)await ApplicationData.Current.LocalFolder.TryGetItemAsync("Logs");
            if(folder != null)
            {
                await folder.DeleteAsync();
            }
            await ApplicationData.Current.LocalFolder.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists);
            Info("Deleted logs!");
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
            await ApplicationData.Current.LocalFolder.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists);
            StorageFile logFile = await (await ApplicationData.Current.LocalFolder.GetFolderAsync("Logs")).CreateFileAsync(getFilename(), CreationCollisionOption.OpenIfExists);
            string s = "[" + code + "][" + getTimeStamp() + "]: " + message;
            if (e != null)
            {
                s += ":\n" + e.Message + "\n" + e.StackTrace;
            }
            System.Diagnostics.Debug.WriteLine(s);
            await FileIO.AppendTextAsync(logFile, s + Environment.NewLine);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
