using ExternalData.Classes.NavigaTum;
using Shared.Classes;

namespace UI_Context.Classes.Templates.Controls.NavigaTum
{
    public class NavigaTumLocationControlDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public Location CurLocation
        {
            get => _CurLocation;
            set => SetProperty(ref _CurLocation, value);
        }
        private Location _CurLocation;

        public bool IsSearching
        {
            get => _IsSearching;
            set => SetProperty(ref _IsSearching, value);
        }
        private bool _IsSearching;

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
