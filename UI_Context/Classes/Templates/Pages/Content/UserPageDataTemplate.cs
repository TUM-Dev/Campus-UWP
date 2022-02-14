using System;
using Shared.Classes;
using Storage.Classes.Models.TumOnline;

namespace UI_Context.Classes.Templates.Pages.Content
{
    public class UserPageDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set => SetProperty(ref _IsLoading, value);
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

        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private string _Mail;
        public string Mail
        {
            get => _Mail;
            set => SetProperty(ref _Mail, value);
        }

        private Uri _MailUri;
        public Uri MailUri
        {
            get => _MailUri;
            set => SetProperty(ref _MailUri, value);
        }

        private User _User;
        public User User
        {
            get => _User;
            set => SetUserProperty(value);
        }

        private Identity _Identity;
        public Identity Identity
        {
            get => _Identity;
            set => SetProperty(ref _Identity, value);
        }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private void SetUserProperty(User value)
        {
            if (SetProperty(ref _User, value, nameof(User)))
            {
                Mail = value.Email;
                MailUri = new Uri("mailto:" + value.Email);
                Name = value.FirstName + " " + value.LastName;
                if (!string.IsNullOrEmpty(value.Title))
                {
                    if (value.Title.Contains("B.") || value.Title.Contains("M."))
                    {
                        Name = value.Title + " " + Name;
                    }
                    else
                    {
                        Name += " " + value.Title;
                    }
                }
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
