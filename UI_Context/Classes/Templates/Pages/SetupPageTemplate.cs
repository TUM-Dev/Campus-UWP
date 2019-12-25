using Shared.Classes;

namespace UI_Context.Classes.Templates.Pages
{
    public class SetupPageTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool _IsValidTumID;
        public bool IsValidTumID
        {
            get => _IsValidTumID;
            set => SetIsValidTumIdProperty(value);
        }

        private bool _CanRequestToken;
        public bool CanRequestToken
        {
            get => _CanRequestToken;
            set => SetProperty(ref _CanRequestToken, value);
        }

        private string _TumId;
        public string TumId
        {
            get => _TumId;
            set => SetProperty(ref _TumId, value);
        }

        private string _Token;
        public string Token
        {
            get => _Token;
            set => SetProperty(ref _Token, value);
        }

        private bool _IsRequestingToken;
        public bool IsRequestingToken
        {
            get => _IsRequestingToken;
            set => SetIsRequestingTokenProperty(value);
        }

        private bool _IsCheckingTokenActivation;
        public bool IsCheckingTokenActivation
        {
            get => _IsCheckingTokenActivation;
            set => SetProperty(ref _IsCheckingTokenActivation, value);
        }

        private bool _IsTokenActivated;
        public bool IsTokenActivated
        {
            get => _IsTokenActivated;
            set => SetProperty(ref _IsTokenActivated, value);
        }

        private bool _IsAutomatedActivationCheckRunnig;
        public bool IsAutomatedActivationCheckRunnig
        {
            get => _IsAutomatedActivationCheckRunnig;
            set => SetProperty(ref _IsAutomatedActivationCheckRunnig, value);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void SetIsValidTumIdProperty(bool value)
        {
            if (SetProperty(ref _IsValidTumID, value, nameof(IsValidTumID)))
            {
                CanRequestToken = value && !IsRequestingToken;
            }
        }

        private void SetIsRequestingTokenProperty(bool value)
        {
            if (SetProperty(ref _IsRequestingToken, value, nameof(IsRequestingToken)))
            {
                CanRequestToken = value && !IsValidTumID;
            }
        }

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
