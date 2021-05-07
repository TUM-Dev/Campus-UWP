using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logging.Classes;
using Shared.Classes;
using Shared.Classes.Collections;
using Storage.Classes;
using Storage.Classes.Models.TumOnline;
using TumOnline.Classes.Managers;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class GradesPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly GradesPageDataTemplate MODEL = new GradesPageDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GradesPageContext()
        {
            Task.Run(async () => await LoadGradesAsync(false));
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh()
        {
            Task.Run(async () => await LoadGradesAsync(true));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadGradesAsync(bool refresh)
        {
            MODEL.IsLoading = true;
            try
            {
                IEnumerable<Grade> grades = await GradesManager.INSTANCE.UpdateAsync(Vault.LoadCredentials(Storage.Classes.Settings.GetSettingString(SettingsConsts.TUM_ID)), refresh).ConfAwaitFalse();
                SortGrades(grades);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to load grades!", e);
            }
            MODEL.IsLoading = false;
        }

        private void SortGrades(IEnumerable<Grade> grades)
        {
            MODEL.GRADE_COLLECTIONS.Clear();
            // Cache them in lists to prevent UI thread interrupts for each grade:
            Dictionary<string, List<Grade>> tmp = new Dictionary<string, List<Grade>>();
            foreach (Grade grade in grades)
            {
                if (!tmp.ContainsKey(grade.LectureSemester))
                {
                    tmp[grade.LectureSemester] = new List<Grade>();
                }
                tmp[grade.LectureSemester].Add(grade);
            }

            // Add them to the actual collections:
            foreach (List<Grade> gradesList in tmp.Values)
            {
                CustomObservableCollection<Grade> gradesGroup = new CustomObservableCollection<Grade>(true);
                gradesGroup.AddRange(gradesList);
                MODEL.GRADE_COLLECTIONS.Add(gradesGroup);
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
