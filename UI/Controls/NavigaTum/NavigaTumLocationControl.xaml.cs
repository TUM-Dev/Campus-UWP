using System.ComponentModel;
using ExternalData.Classes.NavigaTum;
using UI_Context.Classes.Context.Controls.NavigaTum;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.NavigaTum
{
    public sealed partial class NavigaTumLocationControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public AbstractSearchResultItem Item
        {
            get => (AbstractSearchResultItem)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(AbstractSearchResultItem), typeof(NavigaTumLocationControl), new PropertyMetadata(null, OnItemChanged));

        public readonly NavigaTumLocationControlContext VIEW_MODEL = new NavigaTumLocationControlContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NavigaTumLocationControl()
        {
            InitializeComponent();
            UpdateViewState(State_Empty);

            VIEW_MODEL.MODEL.PropertyChanged += OnPropertyChanged;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void UpdateViewState(VisualState newState)
        {
            VisualStateManager.GoToState(this, newState.Name, true);
        }

        private void UpdateView(AbstractSearchResultItem item)
        {
            if (item is null)
            {
                UpdateViewState(State_Empty);
                return;
            }

            VIEW_MODEL.UpdateView(item);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigaTumLocationControl control)
            {
                if (e.NewValue is AbstractSearchResultItem item)
                {
                    control.UpdateView(item);
                }
                else
                {
                    control.UpdateView(null);
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (VIEW_MODEL.MODEL.IsSearching)
            {
                UpdateViewState(State_Searching);
            }
            else
            {
                if (VIEW_MODEL.MODEL.CurLocation is null)
                {
                    UpdateViewState(State_Empty);
                }
                else
                {
                    UpdateViewState(State_Content);
                }
            }
        }

        #endregion
    }
}
