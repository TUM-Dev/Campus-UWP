using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using ExternalData.Classes.Manager;
using Storage.Classes.Models.External;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class StudyRoomsPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly StudyRoomsPageDataTemplate MODEL = new StudyRoomsPageDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public StudyRoomsPageContext()
        {
            StudyRoomsManager.INSTANCE.OnRequestError += OnRequestError;
            Task.Run(async () => await LoadRoomsAsync(false));
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh()
        {
            Task.Run(async () => await LoadRoomsAsync(true));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadRoomsAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            MODEL.ShowError = false;
            StudyRoomGroup oldSelected = MODEL.SelectedGroup;
            IEnumerable<StudyRoomGroup> groups = await StudyRoomsManager.INSTANCE.UpdateAsync(refresh);
            foreach (StudyRoomGroup group in groups)
            {
                group.Rooms.Sort((l, r) =>
                {
                    StudyRoomStatus statusL = l.Status;
                    if (l.IsSoonOccupied())
                    {
                        statusL = StudyRoomStatus.UNKNOWN;
                    }
                    StudyRoomStatus statusR = r.Status;
                    if (r.IsSoonOccupied())
                    {
                        statusR = StudyRoomStatus.UNKNOWN;
                    }
                    return statusL.CompareTo(statusR);
                });
            }
            MODEL.ROOM_GROUPS.Replace(groups);
            // Keep the old selection:
            MODEL.SelectedGroup = oldSelected is null ? MODEL.ROOM_GROUPS.FirstOrDefault() : MODEL.ROOM_GROUPS.Where(g => g.Id == oldSelected.Id).FirstOrDefault();
            MODEL.HasGroups = !MODEL.ROOM_GROUPS.IsEmpty();
            MODEL.IsLoading = false;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnRequestError(AbstractManager sender, RequestErrorEventArgs e)
        {
            MODEL.ShowError = true;
        }

        #endregion
    }
}
