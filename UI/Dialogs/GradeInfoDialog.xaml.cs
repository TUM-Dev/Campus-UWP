using Storage.Classes.Models.TumOnline;
using UI_Context.Classes.Context.Dialogs;
using Windows.UI.Xaml.Controls;

namespace UI.Dialogs
{
    public sealed partial class GradeInfoDialog: ContentDialog
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly GradeInfoDialogContext VIEW_MODEL = new GradeInfoDialogContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GradeInfoDialog(Grade grade)
        {
            VIEW_MODEL.MODEL.Grade = grade;
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
