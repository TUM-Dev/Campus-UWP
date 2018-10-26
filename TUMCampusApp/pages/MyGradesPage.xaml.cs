using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using TUMCampusAppAPI.Managers;
using System.Collections.Generic;
using TUMCampusAppAPI.TUMOnline;
using Windows.UI.Core;
using System;
using TUMCampusApp.Controls;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using TUMCampusApp.Classes;
using TUMCampusAppAPI;

namespace TUMCampusApp.Pages
{
    public sealed partial class MyGradesPage : Page, INamedPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/01/2017 Created [Fabian Sauter]
        /// </history>
        public MyGradesPage()
        {
            this.InitializeComponent();
            downloadAndShowGrades();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UiUtils.getLocalizedString("MyGradesPageName_Text");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Creates a new Task which downloads and shows all grades.
        /// </summary>
        private void downloadAndShowGrades()
        {
            progressBar.Visibility = Visibility.Visible;
            refresh_pTRV.IsEnabled = false;
            Task.Factory.StartNew(() => downloadAndShowGradesTaskAsync(false));
        }

        /// <summary>
        /// Downloads and shows all grades. This method should only get called by a separate task.
        /// </summary>
        private async void downloadAndShowGradesTaskAsync(bool force)
        {
            try
            {
                Task t = GradesDBManager.INSTANCE.downloadGrades(force);
                if (t != null)
                {
                    await t;
                }
            }
            catch (BaseTUMOnlineException e)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                }).AsTask();
                return;
            }

            List<TUMOnlineGradeSemester> list = GradesDBManager.INSTANCE.getGradesSemester();
            sortSemesterList(list);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 showGrades(list);
             }).AsTask();
        }

        /// <summary>
        /// Sorts the given list by the grades semesterId. First entry = current semester.
        /// </summary>
        /// <param name="list">The list that should get sorted.</param>
        private void sortSemesterList(List<TUMOnlineGradeSemester> list)
        {
            list.Sort((TUMOnlineGradeSemester a, TUMOnlineGradeSemester b) =>
            {
                if (a == b)
                {
                    if (a == null || a.getGrades().Count == b.getGrades().Count && a.getGrades().Count == 0)
                    {
                        return 0;
                    }
                }
                else if (a == null || a.getGrades().Count == 0)
                {
                    return -1;
                }
                else if (b == null || b.getGrades().Count == 0)
                {
                    return 1;
                }

                string semesterIdA = a.getSemesterId();
                string semesterIdB = b.getSemesterId();
                if (semesterIdA.Equals(semesterIdB))
                {
                    return 0;
                }

                int yearA = int.Parse(semesterIdA.Substring(0, 2));
                int yearB = int.Parse(semesterIdB.Substring(0, 2));
                if (yearA - yearB != 0)
                {
                    return yearB - yearA;
                }

                if (semesterIdA.EndsWith("W"))
                {
                    return -1;
                }
                return 1;
            });
        }

        /// <summary>
        /// Shows the no access grid based on the given exception.
        /// </summary>
        /// <param name="e">The caught exception.</param>
        private void showNoAccess(BaseTUMOnlineException e)
        {
            noData_grid.Visibility = Visibility.Visible;
            grades_stckp.Visibility = Visibility.Collapsed;
            if (e is InvalidTokenTUMOnlineException)
            {
                noDataInfo_tbx.Text = UiUtils.getLocalizedString("GradesTokenNotActivated_Text");
            }
            else if (e is NoAccessTUMOnlineException)
            {
                noDataInfo_tbx.Text = UiUtils.getLocalizedString("GradesNoAccess_Text");
            }
            else
            {
                noDataInfo_tbx.Text = UiUtils.getLocalizedString("GradesUnknownException_Text") + "\n\n" + e.ToString();
            }
            progressBar.Visibility = Visibility.Collapsed;
            refresh_pTRV.IsEnabled = true;
        }

        /// <summary>
        /// Shows the given list of semester with all grades on the screen.
        /// </summary>
        /// <param name="list">A list of semester with grades.</param>
        private void showGrades(List<TUMOnlineGradeSemester> list)
        {
            grades_stckp.Children.Clear();
            if (list == null || list.Count <= 0)
            {
                SyncResult syncResult = GradesDBManager.INSTANCE.getSyncStatus();
                if (syncResult.STATUS < 0 && syncResult.ERROR_MESSAGE != null)
                {
                    noDataInfo_tbx.Text = syncResult.ERROR_MESSAGE;
                    noGrades_grid.Visibility = Visibility.Collapsed;
                    noData_grid.Visibility = Visibility.Visible;
                }
                else
                {
                    noGrades_grid.Visibility = Visibility.Visible;
                    noData_grid.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                noData_grid.Visibility = Visibility.Collapsed;
                noGrades_grid.Visibility = Visibility.Collapsed;
                grades_stckp.Visibility = Visibility.Visible;
                for (int i = 0; i < list.Count; i++)
                {
                    showSemester(list[i], i == 0);
                }
            }
            progressBar.Visibility = Visibility.Collapsed;
            refresh_pTRV.IsEnabled = true;
        }

        /// <summary>
        /// Shows a given semester on the screen.
        /// </summary>
        /// <param name="semester">A semester that should get shown on the screen.</param>
        private void showSemester(TUMOnlineGradeSemester semester, bool currentSemester)
        {
            StackPanel stackPanel = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 10)
            };
            for (int i = 0; i < semester.getGrades().Count; i++)
            {
                if (i == semester.getGrades().Count - 1)
                {
                    GradeControl gC = new GradeControl(semester.getGrades()[i]);
                    gC.setRectangleVisability(Visibility.Collapsed);
                    stackPanel.Children.Add(gC);
                }
                else
                {
                    stackPanel.Children.Add(new GradeControl(semester.getGrades()[i]));
                }
            }

            grades_stckp.Children.Add(new Expander()
            {
                Header = UiUtils.translateSemester(semester.getSemester()),
                Content = stackPanel,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                IsExpanded = currentSemester
            });
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void refresh_pTRV_RefreshRequested(object sender, EventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            refresh_pTRV.IsEnabled = false;
            Task.Factory.StartNew(() => downloadAndShowGradesTaskAsync(true));
        }

        #endregion
    }
}
