using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UI_Context.Classes.ValueConverter.Grades
{
    public class GradeBrushValueConverter: IValueConverter
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s)
            {
                if (double.TryParse(s, out double grade))
                {
                    if (grade < 2)
                    {
                        return new SolidColorBrush(Colors.DarkGreen);
                    }
                    if (grade < 3)
                    {
                        return new SolidColorBrush(Colors.YellowGreen);
                    }
                    if (grade <= 4)
                    {
                        return new SolidColorBrush(Colors.DarkOrange);
                    }
                    return new SolidColorBrush(Colors.DarkRed);
                }
                if (s == "B")
                {
                    return new SolidColorBrush(Colors.DarkGreen);
                }
            }
            return ThemeUtils.GetThemeResource<SolidColorBrush>("TumBlueBrush");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

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
