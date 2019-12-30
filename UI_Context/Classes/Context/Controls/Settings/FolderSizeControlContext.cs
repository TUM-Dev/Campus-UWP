using System.Threading.Tasks;
using UI_Context.Classes.Templates.Controls.Settings;
using Windows.UI.Xaml;

namespace UI_Context.Classes.Context.Controls.Settings
{
    public sealed class FolderSizeControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly FolderSizeControlTemplate MODEL = new FolderSizeControlTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public Task UpdateViewAsync(DependencyPropertyChangedEventArgs e)
        {
            if (!Equals(e.OldValue, e.NewValue) && e.NewValue is string path)
            {
                return MODEL.UpdateViewAsync(path);
            }
            return null;
        }

        public Task RecalculateFolderSizeAsync(string path)
        {
            return MODEL.UpdateViewAsync(path);
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
