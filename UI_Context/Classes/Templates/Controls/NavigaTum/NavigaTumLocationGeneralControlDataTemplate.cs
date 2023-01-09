using Shared.Classes;
using Storage.Classes;
using Windows.UI.Xaml.Controls.Maps;

namespace UI_Context.Classes.Templates.Controls.NavigaTum
{
    public class NavigaTumLocationGeneralControlDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private MapStyle _MapStyle;
        public MapStyle MapStyle
        {
            get => _MapStyle;
            set => SetMapStyleProperty(value);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void SetMapStyleProperty(MapStyle mapStyle)
        {
            SetProperty(ref _MapStyle, mapStyle, nameof(MapStyle));
            Storage.Classes.Settings.SetSetting(SettingsConsts.NAVIGATUM_MAP_STYLE, (int)mapStyle);
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
