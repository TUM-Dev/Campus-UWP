using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Classes;
using Storage.Classes;
using Storage.Classes.Models.TumOnline;
using TumOnline.Classes.Events;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Controls.Lectures;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class LecturesPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly LecturesPageDataTemplate MODEL = new LecturesPageDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public LecturesPageContext()
        {
            LecturesManager.INSTANCE.OnRequestError += OnRequestError;
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
            Task.Run(async () => await LoadLecturesAsync(refresh));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadLecturesAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            MODEL.ShowError = false;
            IEnumerable<Lecture> lectures = await LecturesManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
            AddSortLectures(lectures);
            MODEL.HasLectures = MODEL.LECTURES_COLLECTIONS.Count > 0;
            MODEL.IsLoading = false;
        }

        private void AddSortLectures(IEnumerable<Lecture> lectures)
        {
            // Cache them in lists to prevent UI thread interrupts for each grade:
            Dictionary<string, List<Lecture>> tmp = new Dictionary<string, List<Lecture>>();
            foreach (Lecture lecture in lectures)
            {
                if (!tmp.ContainsKey(lecture.SemesterId))
                {
                    tmp[lecture.SemesterId] = new List<Lecture>();
                }
                tmp[lecture.SemesterId].Add(lecture);
            }

            // Add them to the actual collections:
            List<LecturesDataTemplate> lecturesList = new List<LecturesDataTemplate>();
            foreach (List<Lecture> lList in tmp.Values)
            {
                lecturesList.Add(new LecturesDataTemplate(lList));
            }
            lecturesList.Sort();
            if (lecturesList.Count > 0)
            {
                lecturesList[0].expanded = true;
            }
            MODEL.LECTURES_COLLECTIONS.Replace(lecturesList);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnRequestError(AbstractManager sender, RequestErrorEventArgs e)
        {
            MODEL.ShowError = true;
            MODEL.ErrorMsg = "Failed to load lectures.\n" + e.GenerateErrorMessage();
        }

        #endregion
    }
}
