using System;
using Logging.Classes;
using Storage.Classes;
using UI_Context.Classes.Templates.Controls.NavigaTum;
using Windows.UI.Xaml.Controls.Maps;

namespace UI_Context.Classes.Context.Controls.NavigaTum
{
    public class NavigaTumLocationGeneralControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly NavigaTumLocationGeneralControlDataTemplate MODEL = new NavigaTumLocationGeneralControlDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NavigaTumLocationGeneralControlContext()
        {
            LoadSettings();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void LoadSettings()
        {
            int mapStyle = Storage.Classes.Settings.GetSettingInt(SettingsConsts.NAVIGATUM_MAP_STYLE, (int)MapStyle.Terrain);
            try
            {
                MODEL.MapStyle = (MapStyle)mapStyle;
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to parse NavigaTUM MapStyle from {mapStyle}.", e);
                MODEL.MapStyle = MapStyle.Terrain;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
