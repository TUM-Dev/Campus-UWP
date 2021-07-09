using Storage.Classes.Models.TumOnline;
using UI_Context.Classes.Context.Controls.Calendar;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.Calendar
{
    public sealed partial class CalendarEventControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CalendarEventControlContext VIEW_MODEL = new CalendarEventControlContext();

        public CalendarEvent CalendarEvent
        {
            get => (CalendarEvent)GetValue(CalendarEventProperty);
            set => SetValue(CalendarEventProperty, value);
        }
        public static readonly DependencyProperty CalendarEventProperty = DependencyProperty.Register(nameof(CalendarEvent), typeof(CalendarEvent), typeof(CalendarEventControl), new PropertyMetadata(null));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CalendarEventControl()
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
