using Storage.Classes.Models.TumOnline;
using UI_Context.Classes.Context.Controls.TuitionFees;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.TuitionFees
{
    public sealed partial class TuitionFeeControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly TuitionFeeControlContext VIEW_MODEL = new TuitionFeeControlContext();

        public TuitionFee TuitionFee
        {
            get => (TuitionFee)GetValue(GradeProperty);
            set => SetValue(GradeProperty, value);
        }
        public static readonly DependencyProperty GradeProperty = DependencyProperty.Register(nameof(TuitionFee), typeof(TuitionFee), typeof(TuitionFeeControl), new PropertyMetadata(null));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TuitionFeeControl()
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
