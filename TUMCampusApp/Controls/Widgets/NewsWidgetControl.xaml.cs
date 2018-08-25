using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using TUMCampusAppAPI.DBTables;
using TUMCampusAppAPI.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls.Widgets
{
    public sealed partial class NewsWidgetControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public NewsTable News
        {
            get { return (NewsTable)GetValue(NewsProperty); }
            set { SetValue(NewsProperty, value); }
        }
        public static readonly DependencyProperty NewsProperty = DependencyProperty.Register("News", typeof(NewsTable), typeof(NewsWidgetControl), null);

        public NewsDummyWidgetControl NewsDummyWidget
        {
            get { return (NewsDummyWidgetControl)GetValue(NewsDummyWidgetProperty); }
            set { SetValue(NewsDummyWidgetProperty, value); }
        }
        public static readonly DependencyProperty NewsDummyWidgetProperty = DependencyProperty.Register("NewsDummyWidget", typeof(NewsDummyWidgetControl), typeof(NewsWidgetControl), null);

        public HomePage HPage
        {
            get { return (HomePage)GetValue(HPageProperty); }
            set { SetValue(HPageProperty, value); }
        }
        public static readonly DependencyProperty HPageProperty = DependencyProperty.Register("HPage", typeof(HomePage), typeof(NewsWidgetControl), null);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 06/04/2018 Created [Fabian Sauter]
        /// </history>
        public NewsWidgetControl()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void markAsRead()
        {
            HPage?.removeWidget(this);
            if (News != null)
            {
                string id = News.id;
                Task.Run(() =>
                {
                    NewsDBManager.INSTANCE.updateNewsRead(id, true);
                });
            }
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void slideListItem_sli_SwipeStatusChanged(SlidableListItem sender, SwipeStatusChangedEventArgs args)
        {
            if (args.NewValue == SwipeStatus.Idle)
            {
                if (args.OldValue == SwipeStatus.SwipingPassedLeftThreshold)
                {
                    markAsRead();
                }
                else if (args.OldValue == SwipeStatus.SwipingPassedRightThreshold)
                {
                    NewsDummyWidget?.disableNewsWidgets();
                }
            }
        }

        #endregion
    }
}
