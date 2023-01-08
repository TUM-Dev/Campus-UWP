using System.Threading.Tasks;
using ExternalData.Classes.Manager;
using ExternalData.Classes.NavigaTum;
using UI_Context.Classes.Templates.Controls.NavigaTum;

namespace UI_Context.Classes.Context.Controls.NavigaTum
{
    public class NavigaTumLocationControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly NavigaTumLocationControlDataTemplate MODEL = new NavigaTumLocationControlDataTemplate();

        private AbstractSearchResultItem nextItem = null;
        private AbstractSearchResultItem curItem = null;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void UpdateView(AbstractSearchResultItem item)
        {
            Task.Run(async () => await UpdateViewAsync(item));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task UpdateViewAsync(AbstractSearchResultItem item)
        {
            // Prevent multiple runs at the same time:
            lock (this)
            {
                if (MODEL.IsSearching)
                {
                    nextItem = item;
                    return;
                }
                MODEL.IsSearching = true;
            }

            do
            {
                if (!string.Equals(curItem?.id, item?.id))
                {
                    Location location = await NavigaTumManager.INSTANCE.GetLocationInfoAsync(item.id);
                    MODEL.CurLocation = location;
                    curItem = item;
                }

                lock (this)
                {
                    if (nextItem is null)
                    {
                        MODEL.IsSearching = false;
                        return;
                    }
                    if (string.Equals(curItem?.id, item?.id))
                    {
                        nextItem = null;
                        return;
                    }
                    item = nextItem;
                    nextItem = null;
                }
            } while (true);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
