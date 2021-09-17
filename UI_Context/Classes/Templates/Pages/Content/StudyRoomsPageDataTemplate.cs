using Shared.Classes;
using Shared.Classes.Collections;
using Storage.Classes.Models.External;

namespace UI_Context.Classes.Templates.Pages.Content
{
    public class StudyRoomsPageDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetProperty(ref _IsLoading, value);
        }

        private bool _HasFees;
        public bool HasGroups
        {
            get => _HasFees;
            set => SetProperty(ref _HasFees, value);
        }

        private bool _HasRooms;
        public bool HasRooms
        {
            get => _HasRooms;
            set => SetProperty(ref _HasRooms, value);
        }

        private bool _ShowError;
        public bool ShowError
        {
            get => _ShowError;
            set => SetProperty(ref _ShowError, value);
        }

        private StudyRoomGroup _SelectedGroup;
        public StudyRoomGroup SelectedGroup
        {
            get => _SelectedGroup;
            set => SetSelectedGroupProperty(value);
        }

        public readonly CustomObservableCollection<StudyRoomGroup> ROOM_GROUPS = new CustomObservableCollection<StudyRoomGroup>(true);
        public readonly CustomObservableCollection<StudyRoom> ROOMS = new CustomObservableCollection<StudyRoom>(true);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void SetSelectedGroupProperty(StudyRoomGroup value)
        {
            if (SetProperty(ref _SelectedGroup, value, nameof(SelectedGroup)))
            {
                HasRooms = !(value is null) && value.Rooms.Count > 0;
                ROOMS.Clear();
                if (!(value is null))
                {
                    ROOMS.AddRange(value.Rooms);
                }
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


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
