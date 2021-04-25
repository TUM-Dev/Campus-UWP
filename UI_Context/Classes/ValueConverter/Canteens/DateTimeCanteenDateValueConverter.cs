using System;
using Windows.UI.Xaml.Data;

namespace UI_Context.Classes.ValueConverter
{
    public sealed class DateTimeCanteenDateValueConverter: IValueConverter
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
            if (value is DateTime date)
            {
                if (date != DateTime.MinValue && date != DateTime.MaxValue)
                {
                    if (date.Date == DateTime.Now.Date.AddDays(-1))
                    {
                        return "Yesterday";
                    }
                    if (date.Date == DateTime.Now.Date.AddDays(1))
                    {
                        return "Tomorrow";
                    }
                    return date.ToString("dddd, dd MMMM");
                }
            }
            return "No dishes found!";
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
