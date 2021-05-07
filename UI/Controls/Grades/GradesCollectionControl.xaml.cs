using Shared.Classes.Collections;
using Storage.Classes.Models.TumOnline;
using UI_Context.Classes.Context.Controls.Grades;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Grades
{
    public sealed partial class GradesCollectionControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public CustomObservableCollection<Grade> Grades
        {
            get => (CustomObservableCollection<Grade>)GetValue(GradesProperty);
            set => SetValue(GradesProperty, value);
        }
        public static readonly DependencyProperty GradesProperty = DependencyProperty.Register(nameof(Grades), typeof(CustomObservableCollection<Grade>), typeof(GradesCollectionControl), new PropertyMetadata(null));

        public bool Expanded
        {
            get => (bool)GetValue(ExpandedProperty);
            set => SetValue(ExpandedProperty, value);
        }
        public static readonly DependencyProperty ExpandedProperty = DependencyProperty.Register(nameof(Expanded), typeof(bool), typeof(GradesCollectionControl), new PropertyMetadata(false));

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
