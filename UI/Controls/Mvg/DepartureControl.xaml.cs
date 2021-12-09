using ExternalData.Classes.Mvg;
using UI_Context.Classes.Context.Controls.Mvg;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Mvg
{
    public sealed partial class DepartureControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public Departure Departure
        {
            get => (Departure)GetValue(DepartureProperty);
            set => SetValue(DepartureProperty, value);
        }
        public static readonly DependencyProperty DepartureProperty = DependencyProperty.Register(nameof(Departure), typeof(Departure), typeof(DepartureControl), new PropertyMetadata(null));

        public readonly DepartureControlContext VIEW_MODEL = new DepartureControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public DepartureControl()
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
