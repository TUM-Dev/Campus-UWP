using Storage.Classes.Models.External;
using UI_Context.Classes.Templates.Controls.StudyRooms;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.StudyRooms
{
    public sealed partial class StudyRoomControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly StudyRoomControlDataTemplate VIEW_MODEL = new StudyRoomControlDataTemplate();

        public StudyRoom Room
        {
            get => (StudyRoom)GetValue(RoomProperty);
            set => SetValue(RoomProperty, value);
        }
        public static readonly DependencyProperty RoomProperty = DependencyProperty.Register(nameof(StudyRoom), typeof(StudyRoom), typeof(StudyRoomControl), new PropertyMetadata(null));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public StudyRoomControl()
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
