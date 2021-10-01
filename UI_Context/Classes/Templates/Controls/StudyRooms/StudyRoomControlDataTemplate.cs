using Microsoft.Toolkit.Uwp.UI.Controls;
using Shared.Classes;
using Shared.Classes.Collections;

namespace UI_Context.Classes.Templates.Controls.StudyRooms
{
    public class StudyRoomControlDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CustomObservableCollection<MetadataItem> ATTRIBUTES = new CustomObservableCollection<MetadataItem>(true);

        private string _OccupiedInfo;
        public string OccupiedInfo
        {
            get => _OccupiedInfo;
            set => SetProperty(ref _OccupiedInfo, value);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


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
