using Storage.Classes.Models.TumOnline;
using UI_Context.Classes.Context.Dialogs;
using Windows.UI.Xaml.Controls;

namespace UI.Dialogs
{
    public sealed partial class LectureInfoDialog: ContentDialog
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly LectureInfoDialogContext VIEW_MODEL = new LectureInfoDialogContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public LectureInfoDialog(Lecture lecture)
        {
            VIEW_MODEL.MODEL.Lecture = lecture;
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


        #endregion
    }
}
