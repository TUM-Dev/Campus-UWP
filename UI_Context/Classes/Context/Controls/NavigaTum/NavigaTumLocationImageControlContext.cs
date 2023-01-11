using ExternalData.Classes.NavigaTum;
using Logging.Classes;
using Shared.Classes;
using Shared.Classes.Image;
using UI_Context.Classes.Templates.Controls.NavigaTum;

namespace UI_Context.Classes.Context.Controls.NavigaTum
{
    public class NavigaTumLocationImageControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly NavigaTumLocationImageControlDataTemplate MODEL = new NavigaTumLocationImageControlDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void UpdateView(LocationImage image)
        {
            if (image is null)
            {
                MODEL.Image = null;
            }

            _ = SharedUtils.CallDispatcherAsync(async () =>
            {
                try
                {
                    MODEL.Image = await ImageUtils.DownloadWebPAsync(image.url);
                    Logger.Debug($"NavigaTUM WebP image downloaded from: {image.url}");
                }
                catch (System.Exception e)
                {
                    Logger.Error($"Failed to download NavigaTUM WebP image from: {image.url}", e);
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
