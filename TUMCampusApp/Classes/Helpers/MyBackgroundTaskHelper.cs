using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusAppAPI;
using Windows.ApplicationModel.Background;

namespace TUMCampusApp.Classes.Helpers
{
    public class MyBackgroundTaskHelper
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly string BACKGROUND_TASK_NAME = "TUMCampusAppBackgroundTask";

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Registers a background task if necessarry.
        /// </summary>
        public async static void RegisterBackgroundTask()
        {
            if (BackgroundTaskHelper.IsBackgroundTaskRegistered(BACKGROUND_TASK_NAME))
            {
                return;
            }
            await BackgroundExecutionManager.RequestAccessAsync();
            BackgroundTaskHelper.Register(BACKGROUND_TASK_NAME, "TUMCampusApp.BackgroundTask.BackgroundTask", new TimeTrigger(60, false), false, true, new SystemCondition(SystemConditionType.FreeNetworkAvailable), new SystemCondition(SystemConditionType.UserNotPresent), new SystemCondition(SystemConditionType.InternetAvailable));
            Logger.Info("Registered the " + BACKGROUND_TASK_NAME + " background task.");
        }

        /// <summary>
        /// Removes a background task if necessarry.
        /// </summary>
        public static void RemoveBackgroundTask()
        {
            BackgroundTaskHelper.Unregister(BACKGROUND_TASK_NAME);
            Logger.Info("Unregistered the " + BACKGROUND_TASK_NAME + " background task.");
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
