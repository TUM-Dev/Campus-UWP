using System;
using ExternalData.Classes.NavigaTum;
using UI_Context.Classes.Context.Controls.NavigaTum;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace UI.Controls.NavigaTum
{
    public sealed partial class NavigaTumLocationImageControl: UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public LocationImage Image
        {
            get => (LocationImage)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(LocationImage), typeof(NavigaTumLocationImageControl), new PropertyMetadata(null, OnImageChanged));

        public readonly NavigaTumLocationImageControlContext VIEW_MODEL = new NavigaTumLocationImageControlContext();
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public NavigaTumLocationImageControl()
        {
            InitializeComponent();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        private void UpdateView(LocationImage image)
        {
            VIEW_MODEL.UpdateView(image);

            UpdateContent(author_tbx, author_border, image.authorName, image.authorUrl);
            UpdateContent(license_tbx, license_border, image.licenseName, image.licenseUrl);

        }

        #endregion

        #region --Misc Methods (Private)--
        private static void UpdateContent(TextBlock tbx, Border border, string text, string url)
        {
            if (string.IsNullOrEmpty(text))
            {
                border.Visibility = Visibility.Collapsed;
                return;
            }
            border.Visibility = Visibility.Visible;

            tbx.Inlines.Clear();
            if (string.IsNullOrEmpty(url))
            {
                tbx.Inlines.Add(new Run
                {
                    Foreground = tbx.Foreground,
                    Text = text,
                });
            }
            else
            {
                tbx.Inlines.Add(new Hyperlink
                {
                    Foreground = tbx.Foreground,
                    Inlines = {
                        new Run
                        {
                            Foreground = tbx.Foreground,
                            Text = text,
                        }
                    },
                    NavigateUri = new Uri(url)
                });
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigaTumLocationImageControl control)
            {
                if (e.NewValue is LocationImage image)
                {
                    control.UpdateView(image);
                }
                else
                {
                    control.UpdateView(null);
                }
            }
        }

        private void OnImageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Fixes the unlimited space available since all items a re inside a stack panel.
            // So we manually update the max width here, based on the image size.
            if (!double.IsNaN(e.NewSize.Width))
            {
                text_grid.MaxWidth = e.NewSize.Width;
            }
        }

        #endregion
    }
}
