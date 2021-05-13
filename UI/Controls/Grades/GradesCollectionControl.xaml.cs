using UI_Context.Classes.Context.Controls.Grades;
using UI_Context.Classes.Templates.Controls.Grades;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Grades
{
    public sealed partial class GradesCollectionControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public GradesDataTemplate Grades
        {
            get => (GradesDataTemplate)GetValue(GradesProperty);
            set => SetValue(GradesProperty, value);
        }
        public static readonly DependencyProperty GradesProperty = DependencyProperty.Register(nameof(Grades), typeof(GradesDataTemplate), typeof(GradesCollectionControl), new PropertyMetadata(null));

        public readonly GradesCollectionControlContext VIEW_MODEL = new GradesCollectionControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public GradesCollectionControl()
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
