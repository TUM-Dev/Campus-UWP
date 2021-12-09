using UI_Context.Classes.Context.Controls.Mvg;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Mvg
{
    public sealed partial class MvgWidgetControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly MvgWidgetControlContext VIEW_MODEL = new MvgWidgetControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public MvgWidgetControl()
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
        private void OnRefreshClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh();
        }

        private void OnSettingsClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        #endregion
    }
}
