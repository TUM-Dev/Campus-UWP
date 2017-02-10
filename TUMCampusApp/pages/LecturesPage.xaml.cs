using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.Classes;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using TUMCampusAppAPI.UserDatas;
using TUMCampusApp.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TUMCampusAppAPI;

namespace TUMCampusApp.Pages
{
    public sealed partial class MyLecturesPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private bool showingOwnLectures;
        private string currentSearchTerm;
        private List<String> searchTerms;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
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


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void disableSearch()
        {
            search_aSB.IsEnabled = false;
            openSearch_btn.IsEnabled = false;
        }

        private void enableSearch()
        {
            search_aSB.IsEnabled = true;
            openSearch_btn.IsEnabled = true;
        }

        private async Task downloadAndShowLecturesTaskAsync(bool forceRedownload)
        {
            try
            {
                await LecturesManager.INSTANCE.downloadLecturesAsync(forceRedownload);
            }
            catch (BaseTUMOnlineException e)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                }).AsTask().Wait();
                return;
            }
            List<TUMOnlineLecture> list = LecturesManager.INSTANCE.getLectures();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                showLectures(list);
            }).AsTask().Wait();
        }

        private async void downloadAndShowQueriedLecturesTask(string query)
        {
            List<TUMOnlineLecture> list = null;
            try
            {
                list = await LecturesManager.INSTANCE.searchForLecturesAsync(query);
            }
            catch (BaseTUMOnlineException e)
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    showNoAccess(e);
                }).AsTask().Wait();
                return;
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                showLectures(list);
            }).AsTask().Wait();
            currentSearchTerm = query;
            showingOwnLectures = false;
        }

        private void showNoAccess(BaseTUMOnlineException e)
        {
            noData_grid.Visibility = Visibility.Visible;
            lectures_stckp.Visibility = Visibility.Collapsed;
            if (e is InvalidTokenTUMOnlineException)
            {
                noData_tbx.Text = "Your token is not activated yet!";
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
            enableSearch();
        }

        private void showLectures(List<TUMOnlineLecture> list)
        {
            lectures_stckp.Children.Clear();
            if(list != null && list.Count > 0)
            {
                semester_tbx.Text = list[0].semesterName;
                for(var i = 0; i < list.Count; i++)
                {
                    LectureControl lC = new LectureControl(list[i], i == list.Count - 1);
                    lectures_stckp.Children.Add(lC);
                }
            }
            else
            {
                semester_tbx.Text = "None found!";
            }
            progressBar.Visibility = Visibility.Collapsed;
            noData_grid.Visibility = Visibility.Collapsed;
            lectures_stckp.Visibility = Visibility.Visible;
            enableSearch();
        }

        private void showOwnLectures()
        {
            if (!showingOwnLectures)
            {
                disableSearch();
                progressBar.Visibility = Visibility.Visible;
                Task.Factory.StartNew(() => downloadAndShowLecturesTaskAsync(false));
                showingOwnLectures = true;
                currentSearchTerm = "";
            }
        }

        private async Task showSearchResultAsync(String query)
        {
            if (!DeviceInfo.isConnectedToInternet())
            {
                await Util.showMessageBoxAsync("Unable to query!\nYour device is not connected to the internet!");
                return;
            }
            disableSearch();
            progressBar.Visibility = Visibility.Visible;
            await Task.Factory.StartNew(() => downloadAndShowQueriedLecturesTask(query));
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            showOwnLectures();
        }

        private void openSearch_btn_Click(object sender, RoutedEventArgs e)
        {
            if (search_aSB.Visibility == Visibility.Visible)
            {
                showOwnLectures();
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
                if(sender.Text.Length < 4)
                {
                    await Util.showMessageBoxAsync("query must be at least 4 chars long!");
                    return;
                }
                if (sender.Text.Equals(currentSearchTerm))
                {
                    return;
                }
                await showSearchResultAsync(sender.Text);
                var results = searchTerms.Where(i => i.Equals(sender.Text)).ToList();
                if(results == null || results.Count <= 0)
                {
                    searchTerms.Add(sender.Text);
                }
            }
        }
        #endregion
    }
}
