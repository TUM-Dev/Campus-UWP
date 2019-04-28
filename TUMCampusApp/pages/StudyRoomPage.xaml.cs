using Data_Manager;
using Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusApp.Controls;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Pages
{
    public sealed partial class StudyRoomPage : Page, INamedPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private List<StudyRoomGroupTable> groups;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 24/02/2017 Created [Fabian Sauter]
        /// </history>
        public StudyRoomPage()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UiUtils.GetLocalizedString("StudyRoomPageName_text");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Downloads all study rooms and all study room groups and shows them on the screen.
        /// </summary>
        private async Task downloadAndShowStudyRoomsAsync()
        {
            try
            {
                Task t = StudyRoomDBManager.INSTANCE.downloadStudyRoomsAndGroups();
                if (t != null)
                {
                    await t;
                }
            }
            catch (Exception e)
            {
                Logger.Error("StudyRoomPage - downloadAndShowStudyRoomsTask", e);
            }

            groups = StudyRoomDBManager.INSTANCE.getRoomGroups();
            if (groups == null || groups.Count <= 0)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    noDate_tbx.Visibility = Visibility.Visible;
                    rooms_stckp.Children.Clear();
                    room_groups_cmbb.Items.Clear();
                    enableRefresh();
                });
                return;
            }

            var temp = Settings.getSetting(SettingsConsts.LAST_SELECTED_STUDY_ROOM_GROUP);
            int lastSelectedIndex = 0;
            if (temp != null)
            {
                lastSelectedIndex = (int)temp;
            }

            if (lastSelectedIndex < 0 || lastSelectedIndex > groups.Count - 1)
            {
                lastSelectedIndex = 0;
                Settings.setSetting(SettingsConsts.LAST_SELECTED_STUDY_ROOM_GROUP, 0);
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                noDate_tbx.Visibility = Visibility.Collapsed;
                room_groups_cmbb.Items.Clear();
                foreach (StudyRoomGroupTable g in groups)
                {
                    room_groups_cmbb.Items.Add(new ComboBoxItem() { Content = g.name });
                }
                room_groups_cmbb.SelectedIndex = lastSelectedIndex;
            });

            showRoomsForGroupIdTask(groups[lastSelectedIndex].id);
        }

        /// <summary>
        /// Shows all downloaded study rooms on the screen. This method has to get as a Task!
        /// </summary>
        /// <param name="groupID">The study room group id.</param>
        private void showRoomsForGroupIdTask(int groupID)
        {
            List<StudyRoomTable> rooms = StudyRoomDBManager.INSTANCE.getRooms(groupID);

            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                rooms_stckp.Children.Clear();
                foreach (StudyRoomTable r in rooms)
                {
                    rooms_stckp.Children.Add(new StudyRoomControl(r) { Margin = new Thickness(10, 5, 10, 5) });
                }
                enableRefresh();
            }).AsTask();
        }

        /// <summary>
        /// Disables the refresh button and shows the progress bar.
        /// </summary>
        private void disableRefresh()
        {
            progressBar.Visibility = Visibility.Visible;
            refresh_btn.IsEnabled = false;
        }

        /// <summary>
        /// Enables the refresh button and hides the progress bar.
        /// </summary>
        private void enableRefresh()
        {
            progressBar.Visibility = Visibility.Collapsed;
            refresh_btn.IsEnabled = true;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            disableRefresh();
            Task.Run(() => downloadAndShowStudyRoomsAsync());
        }

        private void refresh_btn_Click(object sender, RoutedEventArgs e)
        {
            disableRefresh();
            Task.Run(() => downloadAndShowStudyRoomsAsync());
        }

        private void room_groups_cmbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (groups == null)
            {
                return;
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                showRoomsForGroupIdTask(groups[room_groups_cmbb.SelectedIndex].id);
                Settings.setSetting(SettingsConsts.LAST_SELECTED_STUDY_ROOM_GROUP, room_groups_cmbb.SelectedIndex);
            }).AsTask();
        }

        #endregion
    }
}
