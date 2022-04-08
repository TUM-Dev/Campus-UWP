using System.Collections.Generic;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using ExternalData.Classes.Manager;
using Shared.Classes;
using Storage.Classes.Models.News;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class NewsPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly NewsPageTemplate MODEL = new NewsPageTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NewsPageContext()
        {
            NewsManager.INSTANCE.OnRequestError += OnRequestError;
            Refresh(false);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh(bool refresh)
        {
            Task.Run(async () => await LoadNewsAsync(refresh));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadNewsAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            MODEL.ShowError = false;
            List<NewsSource> newsSources = await NewsManager.INSTANCE.UpdateNewsSourcesAsync(refresh).ConfAwaitFalse();
            MODEL.NEWS_SOURCES_COLLECTIONS.Clear();
            MODEL.NEWS_SOURCES_COLLECTIONS.AddRange(newsSources);
            MODEL.HasNewsSources = MODEL.NEWS_SOURCES_COLLECTIONS.Count > 0;
            MODEL.IsLoading = false;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnRequestError(AbstractManager sender, RequestErrorEventArgs e)
        {
            MODEL.ShowError = true;
            MODEL.ErrorMsg = $"Failed to load news/news sources.\n{e}";
        }

        #endregion
    }
}
