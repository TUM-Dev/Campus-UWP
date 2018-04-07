using Data_Manager;
using Microsoft.Toolkit.Uwp.UI.Controls;
using TUMCampusApp.Controls.Widgets;
using TUMCampusApp.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class WidgetControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public UIElement WidgetContent
        {
            get { return (UIElement)GetValue(WidgetContentProperty); }
            set
            {
                SetValue(WidgetContentProperty, value);
                onWidgetContentChanged();
            }
        }
        public static readonly DependencyProperty WidgetContentProperty = DependencyProperty.Register("WidgetContent", typeof(UIElement), typeof(WidgetControl), null);

        public HomePage HPage
        {
            get { return (HomePage)GetValue(HPageProperty); }
            set { SetValue(HPageProperty, value); }
        }
        public static readonly DependencyProperty HPageProperty = DependencyProperty.Register("HPage", typeof(HomePage), typeof(WidgetControl), null);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/04/2018 Created [Fabian Sauter]
        /// </history>
        public WidgetControl()
        {
            this.InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public void setIsLoading(bool isLoading)
        {
            loading_ldng.IsLoading = isLoading;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void disableWidget()
        {
            if (WidgetContent is IHideableWidget hW)
            {
                string token = hW.getSettingsToken();
                if (token != null)
                {
                    hW.onHiding();

                    Settings.setSetting(token, true);
                    HPage?.removeWidget(this);
                }
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        private void onWidgetContentChanged()
        {
            setVisability();
        }

        private void setVisability()
        {
            if (WidgetContent is IHideableWidget hW)
            {
                string token = hW.getSettingsToken();
                if (token != null && Settings.getSettingBoolean(token))
                {
                    HPage?.removeWidget(this);
                    return;
                }
            }
            Visibility = Visibility.Visible;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void slideListItem_sli_SwipeStatusChanged(SlidableListItem sender, SwipeStatusChangedEventArgs args)
        {
            if (args.NewValue == SwipeStatus.Idle)
            {
                if (args.OldValue == SwipeStatus.SwipingPassedLeftThreshold || args.OldValue == SwipeStatus.SwipingPassedRightThreshold)
                {
                    disableWidget();
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setVisability();
        }

        #endregion
    }
}
