using ExternalData.Classes.NavigaTum;
using Microsoft.Toolkit.Uwp.UI.Controls;
using UI_Context.Classes.Context.Controls.NavigaTum;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace UI.Controls.NavigaTum
{
    public sealed partial class NavigaTumLocationMapControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public Location CurLocation
        {
            get => (Location)GetValue(CurLocationProperty);
            set => SetValue(CurLocationProperty, value);
        }
        public static readonly DependencyProperty CurLocationProperty = DependencyProperty.Register(nameof(CurLocation), typeof(Location), typeof(NavigaTumLocationImagesControl), new PropertyMetadata(null, OnCurLocationChanged));

        public readonly NavigaTumLocationMapControlContext VIEW_MODEL = new NavigaTumLocationMapControlContext();

        private static readonly SolidColorBrush CROSSHAIR_BRUSH = new SolidColorBrush(Colors.Red);
        private static readonly SolidColorBrush CROSSHAIR_DOT_FILL_BRUSH = new SolidColorBrush(Colors.White);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NavigaTumLocationMapControl()
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
        private void UpdateAvailableMaps()
        {
            if (CurLocation is null)
            {
                return;
            }

            for (int i = 0; i < CurLocation.maps.Count; i++)
            {
                if (string.Equals(CurLocation.maps[i].id, CurLocation.defaultMap))
                {
                    maps_cbx.SelectedIndex = i;
                }
            }
        }

        private void ClearCrosshair()
        {
            overlay_canvas.Children.Clear();
        }

        private void UpdateCrosshair()
        {
            ClearCrosshair();
            if (VIEW_MODEL.MODEL.Image is null || double.IsNaN(map_img.ActualWidth) || double.IsNaN(map_img.ActualHeight) || map_img.ActualWidth <= 0 || map_img.ActualHeight <= 0)
            {
                return;
            }

            // Check if there is a valid selected map:
            if (!(maps_cbx.SelectedItem is LocationMap map))
            {
                return;
            }

            double factorX = map_img.ActualWidth / VIEW_MODEL.MODEL.Image.PixelWidth;
            double factorY = map_img.ActualHeight / VIEW_MODEL.MODEL.Image.PixelHeight;

            double strokeThickness = 4;


            // Horizontal line:
            overlay_canvas.Children.Add(new Line
            {
                X1 = 0,
                Y1 = (map.y * factorY) - (strokeThickness / 2),
                X2 = map_img.ActualWidth,
                Y2 = (map.y * factorY) - (strokeThickness / 2),
                Stroke = CROSSHAIR_BRUSH,
                StrokeThickness = strokeThickness,
                StrokeDashArray = new DoubleCollection { 1, 2 }
            });

            // Vertical line:
            overlay_canvas.Children.Add(new Line
            {
                X1 = (map.x * factorX) - (strokeThickness / 2),
                Y1 = 0,
                X2 = (map.x * factorX) - (strokeThickness / 2),
                Y2 = map_img.ActualHeight,
                Stroke = CROSSHAIR_BRUSH,
                StrokeThickness = strokeThickness,
                StrokeDashArray = new DoubleCollection { 1, 2 }
            });

            // Dot:
            double dotOutRadius = 10;
            Ellipse dotOut = new Ellipse
            {
                Fill = CROSSHAIR_BRUSH,
                Width = dotOutRadius * 2,
                Height = dotOutRadius * 2,
                StrokeThickness = 0
            };
            Canvas.SetLeft(dotOut, (map.x * factorX) - dotOutRadius);
            Canvas.SetTop(dotOut, (map.y * factorY) - dotOutRadius);
            overlay_canvas.Children.Add(dotOut);

            double dotInRadius = 4;
            Ellipse dotIn = new Ellipse
            {
                Fill = CROSSHAIR_DOT_FILL_BRUSH,
                Width = dotInRadius * 2,
                Height = dotInRadius * 2,
                StrokeThickness = 0
            };
            Canvas.SetLeft(dotIn, (map.x * factorX) - dotInRadius);
            Canvas.SetTop(dotIn, (map.y * factorY) - dotInRadius);
            overlay_canvas.Children.Add(dotIn);

        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (maps_cbx.SelectedItem is LocationMap map)
            {
                VIEW_MODEL.UpdateView(map);
            }
            else
            {
                VIEW_MODEL.UpdateView(null);
            }
        }

        private static void OnCurLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigaTumLocationMapControl control)
            {
                control.UpdateAvailableMaps();
            }
        }

        private void OnImageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCrosshair();
        }
        private void OnImageOpened(object sender, ImageExOpenedEventArgs e)
        {
            UpdateCrosshair();
        }

        private void OnImageOpenFailed(object sender, ImageExFailedEventArgs e)
        {
            ClearCrosshair();
        }

        #endregion
    }
}
