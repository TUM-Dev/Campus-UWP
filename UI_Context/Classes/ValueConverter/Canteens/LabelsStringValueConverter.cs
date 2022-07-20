using System;
using System.Collections.Generic;
using ExternalData.Classes.Manager;
using Storage.Classes.Models.Canteens;
using Windows.UI.Xaml.Data;

namespace UI_Context.Classes.ValueConverter
{
    public sealed class LabelsStringValueConverter: IValueConverter
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
            string result = "";
            if (value is List<string> labels)
            {
                foreach (string label in labels)
                {
                    Label l = DishManager.LookupLabel(label);
                    result += (l?.Abbreviation ?? label) + ' ';
                }
            }
            return result;
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
