using System.Diagnostics;
using ExternalData.Classes.NavigaTum;
using UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Controls.NavigaTum
{
    public sealed partial class SearchResultItemControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public AbstractSearchResultItem Item
        {
            get => (AbstractSearchResultItem)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(AbstractSearchResultItem), typeof(SearchResultItemControl), new PropertyMetadata(null, OnItemChanged));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public SearchResultItemControl()
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
        private void UpdateView(AbstractSearchResultItem item)
        {
            Debug.Assert(!(item is null));

            NavigaTumSearchResultNameFormatExtension.SetFormattedText(name_tbx, item.name);
            icon_ficon.Glyph = item is RoomSearchResultItem ? "\uE707" : "\uE80F";
            caption_tbx.Text = item is RoomSearchResultItem roomItem ? $"{roomItem.subtextBold} - {roomItem.subtext}" : item.subtext;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchResultItemControl control && e.NewValue is AbstractSearchResultItem item)
            {
                control.UpdateView(item);
            }
        }

        #endregion
    }
}
