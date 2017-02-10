using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.AppService;

namespace TUMCampusApp.VoiceCommands
{
    public sealed class TUMVoiceCommandService : IBackgroundTask
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private BackgroundTaskDeferral serviceDeferral;
        private VoiceCommandServiceConnection voiceServiceConnection;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 08/02/2017 Created [Fabian Sauter]
        /// </history>
        public TUMVoiceCommandService()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the menus for the last selected canteen.
        /// </summary>
        /// <param name="date">The date for the menus</param>
        /*private List<CanteenMenu> getMenus(DateTime date)
        {
            List<CanteenMenu> list = new List<CanteenMenu>();
            if(CanteenMenueManager.INSTANCE == null)
            {
                CanteenMenueManager.INSTANCE = new CanteenMenueManager();
                UserDataManager.INSTANCE = new UserDataManager();
            }

            int id = UserDataManager.INSTANCE.getLastSelectedCanteenId();
            list.AddRange(CanteenMenueManager.INSTANCE.getMenusForType(id, "Tagesgericht", true, date));
            list.AddRange(CanteenMenueManager.INSTANCE.getMenusForType(id, "Aktionsessen", true, date));
            return list;
        }*/

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            //Take a service deferral so the service isn't terminated.
            this.serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;
            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerDetails != null && triggerDetails.Name == "TUMCampusAppVoiceCommandsService")
            {
                try
                {
                    voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                    voiceServiceConnection.VoiceCommandCompleted += VoiceCommandCompleted;
                    VoiceCommand voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();

                    switch (voiceCommand.CommandName)
                    {
                        case "showMenusForDate":
                            SendCompletionMessageForMenus(voiceCommand);
                            break;
                        // As a last resort, launch the app in the foreground.
                        default:
                            LaunchAppInForeground();
                            break;
                    }
                }
                finally
                {
                    if (this.serviceDeferral != null)
                    {
                        // Complete the service deferral.
                        this.serviceDeferral.Complete();
                    }
                }
            }
        }

        private async void SendCompletionMessageForMenus(VoiceCommand voiceCommand)
        {
            string date = voiceCommand.Properties["date"][0];
            VoiceCommandUserMessage userMessage = new VoiceCommandUserMessage();

            DateTime d;
            if (date.Equals("Today") || date.Equals("Heute"))
            {
                d = DateTime.Now.AddDays(-1);
            }
            else
            {
                d = DateTime.Now;
            }

            var menusTiles = new List<VoiceCommandContentTile>();
            /*List<CanteenMenu> menus = getMenus(d);
            foreach (var m in menus)
            {
                menusTiles.Add(new VoiceCommandContentTile()
                {
                    Title = CanteenMenueManager.INSTANCE.replaceMenuStringWithImages(m.name, true),
                    TextLine1 = m.typeLong
                });
            }*/
            var response = VoiceCommandResponse.CreateResponse(userMessage, menusTiles);
            await voiceServiceConnection.ReportSuccessAsync(response);
        }

        /// <summary>
        /// Launches the app in foreground.
        /// </summary>
        private async void LaunchAppInForeground()
        {
            var userMessage = new VoiceCommandUserMessage();
            userMessage.SpokenMessage = "Launching TUM Campus App";
            await voiceServiceConnection.RequestAppLaunchAsync(VoiceCommandResponse.CreateResponse(userMessage));
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        /// <summary>
        /// When the background task is cancelled, clean up/cancel any ongoing long-running operations.
        /// This cancellation notice may not be due to Cortana directly. The voice command connection will
        /// typically already be destroyed by this point and should not be expected to be active.
        /// </summary>
        /// <param name="sender">This background task instance</param>
        /// <param name="reason">Contains an enumeration with the reason for task cancellation</param>
        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            System.Diagnostics.Debug.WriteLine("Task cancelled, clean up");
            if (this.serviceDeferral != null)
            {
                this.serviceDeferral.Complete();
            }
        }

        private void VoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            if (this.serviceDeferral != null)
            {
                this.serviceDeferral.Complete();
            }
        }

        #endregion
    }
}
