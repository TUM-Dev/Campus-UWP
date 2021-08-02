using System;
using System.Globalization;
using Storage.Classes.Models.TumOnline;
using Windows.UI.Xaml.Data;

namespace UI_Context.Classes.ValueConverter.Calendar
{
    public sealed class CalendarEventTimeStringValueConverter: IValueConverter
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
            if (value is CalendarEvent calendarEvent)
            {
                if (calendarEvent.Start.Date == calendarEvent.End.Date)
                {
                    return calendarEvent.Start.ToString("t", DateTimeFormatInfo.InvariantInfo) + " - " + calendarEvent.End.ToString("t", DateTimeFormatInfo.InvariantInfo);
                }
                return calendarEvent.Start.ToString("t", DateTimeFormatInfo.InvariantInfo) + " - " + calendarEvent.End.ToString("g", DateTimeFormatInfo.InvariantInfo);
            }
            return "-";
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
