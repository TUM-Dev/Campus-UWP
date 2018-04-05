using Data_Manager;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using TUMCampusApp.Controls.Widgets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TUMCampusApp.Controls
{
    public sealed partial class WidgetControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public object WidgetContent
        {
            get { return GetValue(WidgetContentProperty); }
            set
            {
                SetValue(WidgetContentProperty, value);
                onWidgetContentChanged();
            }
        }
        public static readonly DependencyProperty WidgetContentProperty = DependencyProperty.Register("WidgetContent", typeof(object), typeof(WidgetControl), null);

        public delegate void RemoveWidgetHandler(WidgetControl sender, EventArgs args);

        public event RemoveWidgetHandler RemoveWidget;

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


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void onWidgetContentChanged()
        {
            if (WidgetContent is IHideableWidget hW)
            {
                string setting = hW.getSettingsToken();
                if (setting != null)
                {
                    Visibility = Settings.getSettingBoolean(setting) ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        private void hideWidget()
        {
            if (WidgetContent is IHideableWidget hW)
            {
                string token = hW.getSettingsToken();
                if (token != null)
                {
                    Settings.setSetting(token, true);
                    Visibility = Visibility.Collapsed;
                }
            }
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
                    hideWidget();
                    RemoveWidget?.Invoke(this, new EventArgs());
                }
            }

        }

        #endregion
    }
}
