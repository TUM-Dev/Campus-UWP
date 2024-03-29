﻿using Storage.Classes.Models.TumOnline;
using UI.Dialogs;
using UI_Context.Classes;
using UI_Context.Classes.Context.Controls.Lectures;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Lectures
{
    public sealed partial class LectureControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public Lecture Lecture
        {
            get => (Lecture)GetValue(LectureProperty);
            set => SetValue(LectureProperty, value);
        }
        public static readonly DependencyProperty LectureProperty = DependencyProperty.Register(nameof(Lecture), typeof(Lecture), typeof(LecturesCollectionControl), new PropertyMetadata(null));

        public readonly LectureControlContext VIEW_MODEL = new LectureControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public LectureControl()
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
        private async void OnInfoClicked(object sender, RoutedEventArgs e)
        {
            LectureInfoDialog dialog = new LectureInfoDialog(Lecture);
            await UiUtils.ShowDialogAsync(dialog);
        }

        #endregion
    }
}
