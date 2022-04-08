using System;
using System.Threading;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Shared.Classes.Image;
using Storage.Classes;
using Storage.Classes.Models.TumOnline;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Controls;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace UI_Context.Classes.Context.Controls
{
    public class TumOnlineUserImageControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly TumOnlineUserImageControlTemplate MODEL = new TumOnlineUserImageControlTemplate();

        private readonly SemaphoreSlim LOADING_SEMA = new SemaphoreSlim(1);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void UpdateView(string obfuscatedId)
        {
            Task.Run(async () =>
            {
                // Ensure no updates are run in parallel:
                await LOADING_SEMA.WaitAsync();
                if (MODEL.IsLoading)
                {
                    LOADING_SEMA.Release();
                    return;
                }
                MODEL.IsLoading = true;
                LOADING_SEMA.Release();

                if (string.IsNullOrEmpty(obfuscatedId))
                {
                    MODEL.Image = null;
                }
                else
                {
                    User user = await UserManager.INSTANCE.UpdateUserAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), obfuscatedId, false).ConfAwaitFalse();
                    try
                    {
                        SoftwareBitmap img = null;
                        if (!(user.Image is null) && user.Image.Length > 0)
                        {
                            img = await ImageUtils.ToSoftwareBitmapImageAsync(user.Image);
                        }
                        if (!(img is null))
                        {
                            await SharedUtils.CallDispatcherAsync(async () =>
                            {
                                try
                                {
                                    MODEL.Image = new SoftwareBitmapSource();
                                    await MODEL.Image.SetBitmapAsync(img);
                                }
                                catch (Exception e)
                                {
                                    Logger.Error("Failed to set TUMonlie user image as SoftwareBitmapSource.", e);
                                }
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Failed to load TUMonline user image from bytes.", e);
                    }
                }
                MODEL.IsLoading = false;
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
