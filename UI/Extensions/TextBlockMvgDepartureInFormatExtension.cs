using System;
using ExternalData.Classes.Mvg;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace UI.Extensions
{
    public static class TextBlockMvgDepartureInFormatExtension
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly DependencyProperty DepartureProperty = DependencyProperty.Register("Departure", typeof(Departure), typeof(TextBlockMvgDepartureInFormatExtension), new PropertyMetadata(null, OnDepartureChanged));


        private static readonly SolidColorBrush RED_BRUSH = new SolidColorBrush(Colors.Red);
        private static readonly SolidColorBrush GREEN_BRUSH = new SolidColorBrush(Colors.Green);
        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public static Departure GetDeparture(DependencyObject obj)
        {
            return (Departure)obj.GetValue(DepartureProperty);
        }

        public static void SetDeparture(DependencyObject obj, Departure value)
        {
            obj.SetValue(DepartureProperty, value);
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private static void OnDepartureChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is TextBlock textBlock)
            {
                textBlock.Inlines.Clear();
                if (args.NewValue is Departure departure)
                {
                    int inMinutes = (int)Math.Round((departure.departureTime - DateTime.Now).TotalMinutes, 0);
                    if (inMinutes <= 0)
                    {
                        textBlock.Inlines.Add(new Run
                        {
                            Text = "NOW"
                        });
                    }
                    else if (inMinutes < 60)
                    {
                        textBlock.Inlines.Add(new Run
                        {
                            Text = $"{inMinutes} min."
                        });
                    }
                    else
                    {
                        textBlock.Inlines.Add(new Run
                        {
                            Text = $"{inMinutes / 60} h {inMinutes % 60} min."
                        });
                    }
                    if (departure.delay != 0)
                    {
                        textBlock.Inlines.Add(new Run
                        {
                            Text = " ("
                        });
                        SolidColorBrush brush;
                        string text;
                        if (departure.delay > 0)
                        {
                            brush = RED_BRUSH;
                            text = $"+{departure.delay}";
                        }
                        else
                        {
                            brush = GREEN_BRUSH;
                            text = $"{departure.delay}";
                        }
                        textBlock.Inlines.Add(new Run
                        {
                            Text = text,
                            Foreground = brush
                        });
                        textBlock.Inlines.Add(new Run
                        {
                            Text = " min)"
                        });
                    }
                }
            }
        }

        #endregion
    }
}
