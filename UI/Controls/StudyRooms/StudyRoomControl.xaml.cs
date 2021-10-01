using System;
using Storage.Classes.Models.External;
using UI_Context.Classes;
using UI_Context.Classes.Context.Controls.StudyRooms;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace UI.Controls.StudyRooms
{
    public sealed partial class StudyRoomControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly StudyRoomControlContext VIEW_MODEL = new StudyRoomControlContext();

        public StudyRoom Room
        {
            get => (StudyRoom)GetValue(RoomProperty);
            set => SetValue(RoomProperty, value);
        }
        public static readonly DependencyProperty RoomProperty = DependencyProperty.Register(nameof(StudyRoom), typeof(StudyRoom), typeof(StudyRoomControl), new PropertyMetadata(null, OnRoomChanged));

        private readonly DelegateCommand<object> COMMAND;
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public StudyRoomControl()
        {
            COMMAND = new DelegateCommand<object>(OnCommandExecuted);
            InitializeComponent();
            UpdateViewState(Unknown);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void UpdateView(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is StudyRoom room)
            {
                if (room.IsSoonOccupied())
                {
                    UpdateViewState(SoonOccupied);
                }
                else if (room.Status == StudyRoomStatus.OCCUPIED)
                {
                    UpdateViewState(Occupied);
                }
                else if (room.Status == StudyRoomStatus.FREE)
                {
                    UpdateViewState(Free);
                }
                else
                {
                    UpdateViewState(Unknown);
                }
                VIEW_MODEL.UpdateView(room, COMMAND);
            }
        }

        private void UpdateViewState(VisualState newState)
        {
            VisualStateManager.GoToState(this, newState.Name, true);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void OnRoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StudyRoomControl control)
            {
                control.UpdateView(e);
            }
        }

        private async void OnCommandExecuted(object args)
        {
            if (args is StudyRoomAttribute attribute)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = attribute.Name,
                    Content = attribute.Details,
                    CloseButtonText = "OK"
                };
                await UiUtils.ShowDialogAsync(dialog);
            }
        }

        private async void OnOccupiedInfoClicked(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            Uri uri = new Uri("https://campus.tum.de/tumonline/wbKalender.wbRessource?pResNr=" + Room.ResNumber);
            await UiUtils.LaunchUriAsync(uri);
        }

        private async void OnLocationClicked(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            Uri uri = new Uri("https://campus.tum.de/tumonline/ris.einzelraum?raumkey=" + Room.Id);
            await UiUtils.LaunchUriAsync(uri);
        }

        #endregion
    }
}
