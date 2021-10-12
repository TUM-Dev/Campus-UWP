using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Storage.Classes;
using Storage.Classes.Models.Canteens;
using UI.Dialogs;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages.Content;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace UI.Pages.Content
{
    public sealed partial class CanteensPage: Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CanteensPageContext VIEW_MODEL = new CanteensPageContext();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CanteensPage()
        {
            InitializeComponent();
            LoadMapApiKey();

            VIEW_MODEL.MODEL.CANTEENS.CollectionChanged += OnCanteensChanged;
            VIEW_MODEL.MODEL.PropertyChanged += OnSelectedCanteenChanged;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void LoadMapApiKey()
        {
            string token = TokenReader.LoadTokenFromFile(TokenReader.MAP_TOKEN_PATH);
            canteens_map.MapServiceToken = token;
        }

        private void LoadMapIcons()
        {
            canteens_map.Layers.Clear();
            if (VIEW_MODEL.MODEL.CANTEENS.Count > 0)
            {
                Canteen[] canteens = VIEW_MODEL.MODEL.CANTEENS.ToArray();
                List<MapElement> mapElements = canteens.Select(c => (MapElement)new MapIcon
                {
                    Location = c.Location.ToGeopoint(),
                    IsEnabled = true,
                    Title = c.Name,
                    Visible = true
                }).ToList();

                canteens_map.Layers.Add(new MapElementsLayer
                {
                    ZIndex = 1,
                    MapElements = mapElements
                });
                if (VIEW_MODEL.MODEL.SelectedCanteen is null)
                {
                    canteens_map.Center = VIEW_MODEL.MODEL.CANTEENS[0].Location.ToGeopoint();
                    canteens_map.ZoomLevel = 16;
                }
                else
                {
                    ZoomToSelectedCanteen();
                }
            }
        }

        private void ZoomToSelectedCanteen()
        {
            if (!(VIEW_MODEL.MODEL.SelectedCanteen is null))
            {
                canteens_map.Center = VIEW_MODEL.MODEL.SelectedCanteen.Location.ToGeopoint();
                canteens_map.ZoomLevel = 16;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void RefreshAll_mfi_Click(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true, true);
        }

        private void RefreshCanteens_mfi_Click(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true, false);
        }

        private void RefreshDishes_mfi_Click(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(false, true);
        }

        private void OnCanteensChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadMapIcons();
        }

        private void OnSelectedCanteenChanged(object sender, PropertyChangedEventArgs e)
        {
            ZoomToSelectedCanteen();
        }

        private void nextDate_btn_Click(Controls.IconButtonControl sender, RoutedEventArgs args)
        {
            VIEW_MODEL.NextDate();
        }

        private void prevDate_btn_Click(Controls.IconButtonControl sender, RoutedEventArgs args)
        {
            VIEW_MODEL.PrevDate();
        }

        private async void Ingredients_mfi_Click(object sender, RoutedEventArgs e)
        {
            IngredientsDialog dialog = new IngredientsDialog();
            await UiUtils.ShowDialogAsync(dialog);
        }

        private async void Bug_mfi_Click(object sender, RoutedEventArgs e)
        {
            await VIEW_MODEL.ShowEatApiBugAsync();
        }

        #endregion
    }
}
