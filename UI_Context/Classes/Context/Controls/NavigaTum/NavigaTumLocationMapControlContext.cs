using ExternalData.Classes.NavigaTum;
using Logging.Classes;
using Shared.Classes;
using Shared.Classes.Image;
using UI_Context.Classes.Templates.Controls.NavigaTum;

namespace UI_Context.Classes.Context.Controls.NavigaTum
{
    public class NavigaTumLocationMapControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly NavigaTumLocationMapControlDataTemplate MODEL = new NavigaTumLocationMapControlDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void UpdateView(LocationMap map)
        {
            if (map is null)
            {
                MODEL.Image = null;
                return;
            }

            _ = SharedUtils.CallDispatcherAsync(async () =>
            {
                try
                {
                    MODEL.Image = await ImageUtils.DownloadWebPAsync(map.url);
                    Logger.Debug($"NavigaTUM WebP map downloaded from: {map.url}");
                }
                catch (System.Exception e)
                {
                    Logger.Error($"Failed to download NavigaTUM WebP map from: {map.url}", e);
                    MODEL.Image = null;
                }
            });
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
