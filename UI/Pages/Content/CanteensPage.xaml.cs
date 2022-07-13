using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Storage.Classes;
using Storage.Classes.Models.Canteens;
using UI.Dialogs;
using UI_Context.Classes;
using UI_Context.Classes.Context.Pages.Content;
using Windows.Storage.Streams;
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

        private static readonly RandomAccessStreamReference MENSA_LABEL_STREAM_REF = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Studentenwerk/map_label_mensa.png"));
        private static readonly RandomAccessStreamReference CAFE_LABEL_STREAM_REF = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Studentenwerk/map_label_cafe.png"));
        private static readonly RandomAccessStreamReference BISTRO_LABEL_STREAM_REF = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Studentenwerk/map_label_bistro.png"));
        private static readonly RandomAccessStreamReference MISC_LABEL_STREAM_REF = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Studentenwerk/map_label_misc.png"));

        private static readonly RandomAccessStreamReference MENSA_LABEL_INVERTED_STREAM_REF = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Studentenwerk/map_label_mensa_inverted.png"));
        private static readonly RandomAccessStreamReference CAFE_LABEL_INVERTED_STREAM_REF = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Studentenwerk/map_label_cafe_inverted.png"));
        private static readonly RandomAccessStreamReference BISTRO_LABEL_INVERTED_STREAM_REF = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Studentenwerk/map_label_bistro_inverted.png"));
        private static readonly RandomAccessStreamReference MISC_LABEL_INVERTED_STREAM_REF = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Studentenwerk/map_label_misc_inverted.png"));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CanteensPage()
        {
            InitializeComponent();
            LoadMapApiKey();

            VIEW_MODEL.MODEL.CANTEENS.CollectionChanged += OnCanteensChanged;
            VIEW_MODEL.MODEL.PropertyChanged += OnSelectedCanteenChanged;
            VIEW_MODEL.MODEL.LANGUAGES.CollectionChanged += OnLanguagesChanged;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private RandomAccessStreamReference GetImageForCanteen(string name)
        {
            if (name.StartsWith("mensa"))
            {
                return MENSA_LABEL_STREAM_REF;
            }
            else if (name.StartsWith("stubistro"))
            {
                return BISTRO_LABEL_STREAM_REF;
            }
            else if (name.StartsWith("stucafe"))
            {
                return CAFE_LABEL_STREAM_REF;
            }
            return MISC_LABEL_STREAM_REF;
        }

        private RandomAccessStreamReference GetInvertedImageForCanteen(string name)
        {
            if (name.StartsWith("mensa"))
            {
                return MENSA_LABEL_INVERTED_STREAM_REF;
            }
            else if (name.StartsWith("stubistro"))
            {
                return BISTRO_LABEL_INVERTED_STREAM_REF;
            }
            else if (name.StartsWith("stucafe"))
            {
                return CAFE_LABEL_INVERTED_STREAM_REF;
            }
            return MISC_LABEL_INVERTED_STREAM_REF;
        }

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
                    Visible = true,
                    Image = GetImageForCanteen(c.Id),
                    Tag = c
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

        private void LoadLanguages()
        {
            language_mfsi.Items.Clear();
            language_mfsi.IsEnabled = !VIEW_MODEL.MODEL.LANGUAGES.IsEmpty();
            foreach (Language lang in VIEW_MODEL.MODEL.LANGUAGES)
            {
                ToggleMenuFlyoutItem tmfi = new ToggleMenuFlyoutItem
                {
                    Text = lang.Label,
                    IsChecked = lang.Active,
                    Tag = lang
                };
                tmfi.Click += OnLanguageSelectionChanged;
                language_mfsi.Items.Add(tmfi);
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnRefreshAllClicked(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true, true);
        }

        private void RefreshCanteens_mfi_Click(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.Refresh(true, false);
        }

        private void OnRefreshLanguagesClicked(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.RefreshLanguages();
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

        private async void Labels_mfi_Click(object sender, RoutedEventArgs e)
        {
            LabelsDialog dialog = new LabelsDialog();
            await UiUtils.ShowDialogAsync(dialog);
        }

        private async void Bug_mfi_Click(object sender, RoutedEventArgs e)
        {
            await VIEW_MODEL.ShowEatApiBugAsync();
        }

        private void OnMapElementClicked(MapControl sender, MapElementClickEventArgs args)
        {
            foreach (MapElement element in args.MapElements)
            {
                if (element.Tag is Canteen c)
                {
                    VIEW_MODEL.MODEL.SelectedCanteen = c;
                }
            }
        }

        private void OnMapElementPointerEntered(MapControl sender, MapElementPointerEnteredEventArgs args)
        {
            if (args.MapElement is MapIcon mapIcon && args.MapElement.Tag is Canteen c)
            {
                mapIcon.Image = GetInvertedImageForCanteen(c.Id);
            }
        }

        private void OnMapElementPointerExited(MapControl sender, MapElementPointerExitedEventArgs args)
        {
            if (args.MapElement is MapIcon mapIcon && args.MapElement.Tag is Canteen c)
            {
                mapIcon.Image = GetImageForCanteen(c.Id);
            }
        }

        private void OnTerrainViewClicked(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.MODEL.MapStyle = MapStyle.Terrain;
        }

        private void OnAerialViewClicked(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.MODEL.MapStyle = MapStyle.AerialWithRoads;
        }

        private void OnAerial3DViewClicked(object sender, RoutedEventArgs e)
        {
            VIEW_MODEL.MODEL.MapStyle = MapStyle.Aerial3DWithRoads;
        }

        private void OnLanguagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadLanguages();
        }

        private void OnLanguageSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleMenuFlyoutItem tmfi && tmfi.Tag is Language langClicked)
            {
                // Prevent unselecting all languages:
                if (!tmfi.IsChecked)
                {
                    tmfi.IsChecked = true;
                    return;
                }

                // Update the selected language:
                foreach (Language lang in VIEW_MODEL.MODEL.LANGUAGES)
                {
                    if (lang.Active)
                    {
                        lang.Active = false;
                        lang.Update();
                    }
                    else if (string.Equals(lang.Label, langClicked.Label))
                    {
                        lang.Active = true;
                        lang.Update();
                    }
                }
                LoadLanguages();
            }
        }

        #endregion
    }
}
