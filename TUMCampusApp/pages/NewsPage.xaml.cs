using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.News;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TUMCampusApp.Pages
{
    public sealed partial class NewsPage : Page
    {
        public NewsPage()
        {
            this.InitializeComponent();
            NewsManager.INSTANCE.downloadNewsAsync(true);
            NewsManager.INSTANCE.downloadNewsSourcesAsync(true);
            List<News> news = NewsManager.INSTANCE.getAllNewsFormDb();
            foreach (News item in news)
            {
                TextBlock tB = new TextBlock();
                tB.Text = item.title;
                news_stckp.Children.Add(tB);
            }
        }
    }
}
