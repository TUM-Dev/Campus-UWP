using Shared.Classes;
using Shared.Classes.Collections;
using UI_Context.Classes.Templates.Controls.Lectures;

namespace UI_Context.Classes.Templates.Pages.Content
{
    public class LecturesPageDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CustomObservableCollection<LecturesDataTemplate> LECTURES_COLLECTIONS = new CustomObservableCollection<LecturesDataTemplate>(true);

        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetProperty(ref _IsLoading, value);
        }

        private bool _HasLectures;
        public bool HasLectures
        {
            get => _HasLectures;
            set => SetProperty(ref _HasLectures, value);
        }
        private bool _ShowError;
        public bool ShowError
        {
            get => _ShowError;
            set => SetProperty(ref _ShowError, value);
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
