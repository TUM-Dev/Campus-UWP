using Shared.Classes;

namespace UI_Context.Classes.Templates.Pages.Content
{
    public class NewsPageTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetProperty(ref _IsLoading, value);
        }

        private bool _HasNews;
        public bool HasLectures
        {
            get => _HasNews;
            set => SetProperty(ref _HasNews, value);
        }

        private bool _HasNewsSources;
        public bool HasNewsSources
        {
            get => _HasNewsSources;
            set => SetProperty(ref _HasNewsSources, value);
        }

        private bool _ShowError;
        public bool ShowError
        {
            get => _ShowError;
            set => SetProperty(ref _ShowError, value);
        }

        private string _ErrorMsg;
        public string ErrorMsg
        {
            get => _ErrorMsg;
            set => SetProperty(ref _ErrorMsg, value);
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
