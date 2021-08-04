using UI_Context.Classes.Context.Controls.Lectures;
using UI_Context.Classes.Templates.Controls.Lectures;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Lectures
{
    public sealed partial class LecturesCollectionControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public LecturesDataTemplate Lectures
        {
            get => (LecturesDataTemplate)GetValue(LecturesProperty);
            set => SetValue(LecturesProperty, value);
        }
        public static readonly DependencyProperty LecturesProperty = DependencyProperty.Register(nameof(Lectures), typeof(LecturesDataTemplate), typeof(LecturesCollectionControl), new PropertyMetadata(null));

        public readonly LecturesCollectionControlContext VIEW_MODEL = new LecturesCollectionControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public LecturesCollectionControl()
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
