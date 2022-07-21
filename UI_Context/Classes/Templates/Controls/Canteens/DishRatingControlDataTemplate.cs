using Shared.Classes;
using Storage.Classes.Models.Canteens;

namespace UI_Context.Classes.Templates.Controls.Canteens
{
    public class DishRatingControlDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetProperty(ref _IsLoading, value);
        }

        private bool _IsUnavailable;
        public bool IsUnavailable
        {
            get => _IsUnavailable;
            set => SetProperty(ref _IsUnavailable, value);
        }

        private bool _HasValidRating;
        public bool HasValidRating
        {
            get => _HasValidRating;
            set => SetProperty(ref _HasValidRating, value);
        }

        private Rating _Rating;
        public Rating Rating
        {
            get => _Rating;
            set => SetProperty(ref _Rating, value);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public DishRatingControlDataTemplate()
        {
            IsLoading = true;
        }

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
