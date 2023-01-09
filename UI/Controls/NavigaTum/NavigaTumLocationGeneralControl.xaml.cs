using ExternalData.Classes.NavigaTum;
using Storage.Classes;
using UI_Context.Classes.Context.Controls.NavigaTum;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Documents;

namespace UI.Controls.NavigaTum
{
    public sealed partial class NavigaTumLocationGeneralControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public Location CurLocation
        {
            get => (Location)GetValue(CurLocationProperty);
            set => SetValue(CurLocationProperty, value);
        }
        public static readonly DependencyProperty CurLocationProperty = DependencyProperty.Register(nameof(CurLocation), typeof(Location), typeof(NavigaTumLocationGeneralControl), new PropertyMetadata(null, OnCurLocationChanged));

        public NavigaTumLocationGeneralControlContext VIEW_MODEL = new NavigaTumLocationGeneralControlContext();

        private readonly MapElementsLayer MAP_LAYER = new MapElementsLayer
        {
            ZIndex = 0
        };

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NavigaTumLocationGeneralControl()
        {
            InitializeComponent();
            PrepMap();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void PrepMap()
        {
            LoadMapApiKey();

            location_map.Layers.Add(MAP_LAYER);
        }

        private void LoadMapApiKey()
        {
            string token = TokenReader.LoadTokenFromFile(TokenReader.MAP_TOKEN_PATH);
            location_map.MapServiceToken = token;
        }

        private void UpdateView(Location location)
        {
            // Map:
            MAP_LAYER.MapElements.Clear();
            if (location is null)
            {
                return;
            }

            MAP_LAYER.MapElements.Add(new MapIcon
            {
                Location = location.pos,
                Title = location.name,
            });

            location_map.Center = location.pos;
            location_map.ZoomLevel = 16;

            // Info:
            info_tbx.Inlines.Clear();
            foreach (LocationProperty prop in location.properties)
            {
                info_tbx.Inlines.Add(new Run
                {
                    Text = prop.name += ": ",
                    FontWeight = FontWeights.Bold
                });
                info_tbx.Inlines.Add(new Run
                {
                    Text = prop.text
                });
                info_tbx.Inlines.Add(new LineBreak());
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void OnCurLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigaTumLocationGeneralControl control)
            {
                if (e.NewValue is Location location)
                {
                    control.UpdateView(location);
                }
                else
                {
                    control.UpdateView(null);
                }
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

        #endregion
    }
}
