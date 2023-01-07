using ExternalData.Classes.Manager;
using UI_Context.Classes;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace UI.Extensions
{
    public sealed class NavigaTumSearchResultNameFormatExtension
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly DependencyProperty FormattedNameProperty = DependencyProperty.Register("FormattedNameText", typeof(string), typeof(NavigaTumSearchResultNameFormatExtension), new PropertyMetadata("Loading...", OnFormattedNameChanged));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public static string GetFormattedText(DependencyObject obj)
        {
            return (string)obj.GetValue(FormattedNameProperty);
        }

        public static void SetFormattedText(DependencyObject obj, string value)
        {
            obj.SetValue(FormattedNameProperty, value);
        }

        private static SolidColorBrush GetHighlightForegroundBrush()
        {
            return ThemeUtils.GetThemeResource<SolidColorBrush>("TumBlueBrandBrush");
        }

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
        private static void OnFormattedNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBlock textBlock))
            {
                return;
            }

            // Clear all inlines:
            textBlock.Inlines.Clear();

            // Empty message:
            if (!(e.NewValue is string name) || string.IsNullOrWhiteSpace(name))
            {
                textBlock.Inlines.Add(new Run { Text = "" });
                return;
            }

            // Find the highlighted part:
            int startIndex = name.IndexOf(NavigaTumManager.PRE_HIGHLIGHT);
            int endIndex = name.IndexOf(NavigaTumManager.POST_HIGHLIGHT);
            while (startIndex >= 0 && endIndex > 0 && endIndex > startIndex)
            {
                textBlock.Inlines.Add(new Run { Text = name.Substring(0, startIndex) });
                textBlock.Inlines.Add(new Run
                {
                    Text = name.Substring(startIndex + NavigaTumManager.PRE_HIGHLIGHT.Length, endIndex - startIndex - NavigaTumManager.PRE_HIGHLIGHT.Length),
                    Foreground = GetHighlightForegroundBrush(),
                    FontWeight = FontWeights.Bold
                });

                name = name.Substring(endIndex + NavigaTumManager.POST_HIGHLIGHT.Length);
                startIndex = name.IndexOf(NavigaTumManager.PRE_HIGHLIGHT);
                endIndex = name.IndexOf(NavigaTumManager.POST_HIGHLIGHT);
            }

            if (!string.IsNullOrEmpty(name))
            {
                textBlock.Inlines.Add(new Run { Text = name });
            }
        }

        #endregion
    }
}
