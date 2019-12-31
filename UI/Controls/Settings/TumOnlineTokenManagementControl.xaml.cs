using System;
using Shared.Classes;
using UI.Dialogs;
using UI.Pages;
using UI_Context.Classes;
using UI_Context.Classes.Context.Controls.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Settings
{
    public sealed partial class TumOnlineTokenManagementControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly TumOnlineTokenManagementControlContext VIEW_MODEL = new TumOnlineTokenManagementControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumOnlineTokenManagementControl()
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
        private void setup_btn_Click(IconButtonControl sender, RoutedEventArgs args)
        {
            Type curPage = null;
            if (Window.Current.Content is Frame frame)
            {
                curPage = frame.CurrentSourcePageType;
            }
            UiUtils.NavigateToPage(typeof(SetupPage), curPage);
        }

        private async void delete_ibtn_Click(IconButtonControl sender, RoutedEventArgs args)
        {
            ConfirmDialog dialog = new ConfirmDialog("Delete TUM ID and Token", "Do you really want to **delete** the local copy of you TUM ID and TUMonline token?");
            await UiUtils.ShowDialogAsync(dialog).ConfAwaitFalse();
            VIEW_MODEL.DeleteTumOnlineTokenAndId(dialog.VIEW_MODEL);
        }

        #endregion
    }
}
