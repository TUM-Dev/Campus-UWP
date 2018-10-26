using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using TUMCampusApp.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TUMCampusAppAPI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.DBTables;

namespace TUMCampusApp.Pages
{
    public sealed partial class MyLecturesPage : Page, INamedPage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool showingOwnLectures;
        private string currentSearchTerm;
        private List<String> searchTerms;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/01/2017 Created [Fabian Sauter]
        /// </history>
        public MyLecturesPage()
        {
            this.InitializeComponent();
            this.showingOwnLectures = false;
            this.currentSearchTerm = "";
            this.searchTerms = new List<string>();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getLocalizedName()
        {
            return UiUtils.getLocalizedString("LecturesPageName_Text");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Disables the search bar.
        /// </summary>
        private void disableSearch()
        {
            search_aSB.IsEnabled = false;
            openSearch_btn.IsEnabled = false;
        }

        /// <summary>
        /// Enables the search bar.
        /// </summary>
        private void enableSearch()
        {
            search_aSB.IsEnabled = true;
            openSearch_btn.IsEnabled = true;
        }

        /// <summary>
        /// Downloads and shows the personal lectures.
        /// This method should be only called in a separate task.
        /// </summary>
        /// <param name="forceRedownload">Whether cached lectures should be ignored.</param>
        private async Task downloadAndShowLecturesTaskAsync(bool forceRedownload)
        {
            try
            {
                Task t = LecturesDBManager.INSTANCE.downloadLectures(forceRedownload);
                if(t != null)
                {
                    await t;
                }
            }
            catch (BaseTUMOnlineException e)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                });
                return;
            }
            List<TUMOnlineLectureTable> list = LecturesDBManager.INSTANCE.getLectures();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                showLectures(list);
            });
        }

        /// <summary>
        /// Downloads and shows all lectures that match the given query.
        /// This method should be only called in a separate task.
        /// </summary>
        /// <param name="query">The search query. At least three characters.</param>
        private async void downloadAndShowQueriedLecturesTask(string query)
        {
            List<TUMOnlineLectureTable> list = null;
            try
            {
                list = await LecturesDBManager.INSTANCE.searchForLecturesAsync(query);
            }
            catch (BaseTUMOnlineException e)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                });
                return;
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                showLectures(list);
            }).AsTask();
            currentSearchTerm = query;
            showingOwnLectures = false;
        }

        /// <summary>
        /// Shows the no access grid based on the given exception.
        /// </summary>
        /// <param name="e">The caught exception.</param>
        private void showNoAccess(BaseTUMOnlineException e)
        {
            noData_grid.Visibility = Visibility.Visible;
            lectures_stckp.Visibility = Visibility.Collapsed;
            if (e is InvalidTokenTUMOnlineException)
            {
                noDataInfo_tbx.Text = UiUtils.getLocalizedString("LecturesTokenNotActivated_Text");
            }
            else if (e is NoAccessTUMOnlineException)
            {
                noDataInfo_tbx.Text = UiUtils.getLocalizedString("LecturesNoAccessToLectures_Text");
            }
            else
            {
                noDataInfo_tbx.Text = UiUtils.getLocalizedString("LecturesUnknownException_Text") + "\n\n" + e.ToString();
            }
            progressBar.Visibility = Visibility.Collapsed;
            enableSearch();
            refresh_pTRV.IsEnabled = true;
        }

        /// <summary>
        /// Shows all given lectures on the screen.
        /// </summary>
        /// <param name="list">The lectures list that should get shown.</param>
        private void showLectures(List<TUMOnlineLectureTable> list)
        {
            lectures_stckp.Children.Clear();
            if(list != null && list.Count > 0)
            {
                status_tbx.Text = "";
                List<List<LectureControl>> controls = new List<List<LectureControl>>();
                for(var i = 0; i < list.Count; i++)
                {
                    LectureControl lC = new LectureControl(list[i]) {
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    bool found = false;
                    for(var e = 0; e < controls.Count; e++)
                    {
                        if(controls[e] != null && controls[e].First() != null && controls[e].First().lecture.semesterName.Equals(list[i].semesterName))
                        {
                            controls[e].Add(lC);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        controls.Add(new List<LectureControl>() {lC});
                    }
                }

                sortSemesterList(controls);

                for (var i = 0; i < controls.Count; i++)
                {
                    StackPanel stackPanel = new StackPanel()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(10, 0, 10, 0)
                    };
                    for (int e = 0; e < controls[i].Count; e++)
                    {
                        stackPanel.Children.Add(controls[i][e]);
                        if(e == controls[i].Count - 1)
                        {
                            controls[i][e].setRectangleVisability(Visibility.Collapsed);
                        }
                        else
                        {
                            controls[i][e].setRectangleVisability(Visibility.Visible);
                        }
                    }

                    lectures_stckp.Children.Add(new Expander()
                    {
                        Header = UiUtils.translateSemester(controls[i].First().lecture.semesterName),
                        Content = stackPanel,
                        Margin = new Thickness(0, 10, 0, 0),
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        IsExpanded = (i == 0)
                    });
                }
            }
            else
            {
                status_tbx.Text = UiUtils.getLocalizedString("LecturesNoneFound_Text");
                if(showingOwnLectures)
                {
                    SyncResult syncResult = LecturesDBManager.INSTANCE.getSyncStatus();
                    if (syncResult.STATUS < 0 && syncResult.ERROR_MESSAGE != null)
                    {
                        noDataInfo_tbx.Text = syncResult.ERROR_MESSAGE;
                    }
                }
            }
            noData_grid.Visibility = Visibility.Collapsed;
            progressBar.Visibility = Visibility.Collapsed;
            lectures_stckp.Visibility = Visibility.Visible;
            enableSearch();
            refresh_pTRV.IsEnabled = true;
        }

        /// <summary>
        /// Sorts the given list by the lectures semesterName. First entry = current semester.
        /// </summary>
        /// <param name="list">The list that should get sorted.</param>
        private void sortSemesterList(List<List<LectureControl>> list)
        {
            list.Sort((List<LectureControl> a, List<LectureControl> b) => {
                if(a == b)
                {
                    if(a == null || a.Count == b.Count && a.Count == 0)
                    {
                        return 0;
                    }
                }
                else if(a == null || a.Count == 0)
                {
                    return -1;
                }
                else if (b == null || b.Count == 0)
                {
                    return 1;
                }

                string semesterIdA = a[0].lecture.semesterId;
                string semesterIdB = b[0].lecture.semesterId;
                if(semesterIdA.Equals(semesterIdB))
                {
                    return 0;
                }

                int yearA = int.Parse(semesterIdA.Substring(0, 2));
                int yearB = int.Parse(semesterIdB.Substring(0, 2));
                if(yearA - yearB != 0)
                {
                    return yearB - yearA;
                }

                if(semesterIdA.EndsWith("W"))
                {
                    return -1;
                }
                return 1;
            });
        }

        /// <summary>
        /// Starts a new task that shows the personal lectures.
        /// </summary>
        /// <param name="forceRefresh">Whether to ignore cash.</param>
        private void showOwnLectures(bool forceRefresh)
        {
            if (!showingOwnLectures || forceRefresh)
            {
                refresh_pTRV.IsEnabled = false;
                disableSearch();
                progressBar.Visibility = Visibility.Visible;
                Task.Factory.StartNew(() => downloadAndShowLecturesTaskAsync(forceRefresh));
                showingOwnLectures = true;
                currentSearchTerm = "";
            }
        }

        /// <summary>
        /// Starts a new task and shows the result for the given search query.
        /// </summary>
        /// <param name="query">The search query. At least three characters.</param>
        private async Task showSearchResultAsync(String query)
        {
            if (!DeviceInfo.isConnectedToInternet())
            {
                await Util.showMessageBoxAsync(UiUtils.getLocalizedString("LecturesUnableToQuery_Text"));
                return;
            }
            disableSearch();
            progressBar.Visibility = Visibility.Visible;
            refresh_pTRV.IsEnabled = false;
            await Task.Factory.StartNew(() => downloadAndShowQueriedLecturesTask(query));
        }

        /// <summary>
        /// Refreshes the displayed results:
        /// </summary>
        /// <returns></returns>
        private async Task refreshLecturesAsync()
        {
            if (search_aSB.Visibility == Visibility.Visible)
            {
                await searchQuerryAsync(search_aSB.Text);
            }
            else
            {
                showOwnLectures(true);
            }
        }

        /// <summary>
        /// Checks if the given text is at least 4 chars long.
        /// If yes, it will start a new query in a separate task and represent the results on the screen.
        /// If no, it will show an error message box.
        /// </summary>
        /// <param name="query">The query text</param>
        /// <returns></returns>
        private async Task searchQuerryAsync(string query)
        {
            if (query.Length < 4)
            {
                await Util.showMessageBoxAsync(UiUtils.getLocalizedString("LecturesQueryLength_Text"));
                return;
            }
            await showSearchResultAsync(query);
        }
        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            showOwnLectures(false);
        }

        private void openSearch_btn_Click(object sender, RoutedEventArgs e)
        {
            if (search_aSB.Visibility == Visibility.Visible)
            {
                showOwnLectures(false);
                openSearch_btn.Content = "\xE71E";
                search_aSB.Visibility = Visibility.Collapsed;
            }
            else
            {
                openSearch_btn.Content = "\xE10A";
                search_aSB.Visibility = Visibility.Visible;
            }
        }

        private void search_aSB_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            {
                return;
            }
            string searchTerm = sender.Text;
            sender.ItemsSource = searchTerms.Where(i => i.StartsWith(searchTerm)).ToList();
        }

        private void search_aSB_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem as string;
        }

        private async void search_aSB_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if(args.ChosenSuggestion != null)
            {
                await showSearchResultAsync(args.ChosenSuggestion as String);
            }
            else
            {
                if (sender.Text.Equals(currentSearchTerm))
                {
                    return;
                }
                await searchQuerryAsync(sender.Text);
                var results = searchTerms.Where(i => i.Equals(sender.Text)).ToList();
                if (results == null || results.Count <= 0)
                {
                    searchTerms.Add(sender.Text);
                }
            }
        }

        private async void refresh_pTRV_RefreshRequested(object sender, EventArgs e)
        {
            await refreshLecturesAsync();
        }
        #endregion
    }
}
