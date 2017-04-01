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
using Windows.UI.Text;
using Windows.UI.Xaml.Shapes;

namespace TUMCampusApp.Pages
{
    public sealed partial class MyGradesPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
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
            Task.Factory.StartNew(() => downloadAndShowGradesTaskAsync());
        }

        /// <summary>
        /// Downloads and shows all grades. This method should only get called by a separate task.
        /// </summary>
        private async void downloadAndShowGradesTaskAsync()
        {
            try
            {
                await GradesManager.INSTANCE.downloadGradesAsync();
            }
            catch (BaseTUMOnlineException e)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                }).AsTask();
                return;
            }

            List<TUMOnlineGradeSemester> list = GradesManager.INSTANCE.getGradesSemester();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 showGrades(list);
             }).AsTask();
        }

        /// <summary>
        /// Shows the no access grid based on the given exception.
        /// </summary>
        /// <param name="e">The cought exception.</param>
        private void showNoAccess(BaseTUMOnlineException e)
        {
            noData_grid.Visibility = Visibility.Visible;
            grades_stckp.Visibility = Visibility.Collapsed;
            if (e is InvalidTokenTUMOnlineException)
            {
                noData_tbx.Text = "Either the token is not activated or you didn't give it the required rights for this operation!";
            }
            else if (e is NoAccessTUMOnlineException)
            {
                noData_tbx.Text = "No access on your lectures!";
            }
            else
            {
                noData_tbx.Text = "Unknown exception!\n" + e.ToString();
            }
            progressBar.Visibility = Visibility.Collapsed;
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
                noData_grid.Visibility = Visibility.Visible;
            }
            else
            {
                noData_grid.Visibility = Visibility.Collapsed;
                foreach (TUMOnlineGradeSemester s in list)
                {
                    showSemester(s);
                }
            }
            progressBar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows a given semester on the screen.
        /// </summary>
        /// <param name="semester">A semester that should get shown on the screen.</param>
        private void showSemester(TUMOnlineGradeSemester semester)
        {
            //Semester name
            TextBlock tb = new TextBlock()
            {
                Text = semester.getSemester(),
                Margin = new Thickness(10, 10, 10, 10),
                FontWeight = FontWeights.ExtraBold
            };
            tb.FontSize += 5;
            grades_stckp.Children.Add(tb);

            //Line:
            Rectangle rect = new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 2,
                Fill = tb.Foreground,
                Margin = new Thickness(10, 0, 10, 0)
            };
            grades_stckp.Children.Add(rect);

            //Semester grades
            foreach (TUMOnlineGrade grade in semester.getGrades())
            {
                grades_stckp.Children.Add(new GradeControl(grade));
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
