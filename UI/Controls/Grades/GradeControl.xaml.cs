using Storage.Classes.Models.TumOnline;
using UI_Context.Classes.Context.Controls.Grades;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Grades
{
    public sealed partial class GradeControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public Grade Grade
        {
            get => (Grade)GetValue(GradeProperty);
            set => SetValue(GradeProperty, value);
        }
        public static readonly DependencyProperty GradeProperty = DependencyProperty.Register(nameof(Grade), typeof(Grade), typeof(GradesCollectionControl), new PropertyMetadata(null));

        public readonly GradeControlContext VIEW_MODEL = new GradeControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GradeControl()
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


        #endregion
    }
}
