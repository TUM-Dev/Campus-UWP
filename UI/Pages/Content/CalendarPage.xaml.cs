using System;
using UI_Context.Classes.Context.Pages.Content;
using UI_Context.Classes.Templates.Controls.Calendar;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Pages.Content
{
    public sealed partial class CalendarPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CalendarPageContext VIEW_MODEL = new CalendarPageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CalendarPage()
        {
            InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


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
        private void OnRefreshClicked(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true);
        }

        private void OnGoToTodayClicked(object sender, RoutedEventArgs e)
        {
            if (VIEW_MODEL.MODEL.EVENTS_COLLECTIONS.IsEmpty())
            {
                return;
            }
            CalendarEventGroupDataTemplate target = VIEW_MODEL.MODEL.EVENTS_COLLECTIONS[0];
            foreach (CalendarEventGroupDataTemplate tmp in VIEW_MODEL.MODEL.EVENTS_COLLECTIONS)
            {
                if (tmp.Key.Date < DateTime.Now)
                {
                    break;
                }
                target = tmp;
            }
            events_listView.ScrollIntoView(target, ScrollIntoViewAlignment.Leading);
        }

        #endregion
    }
}
