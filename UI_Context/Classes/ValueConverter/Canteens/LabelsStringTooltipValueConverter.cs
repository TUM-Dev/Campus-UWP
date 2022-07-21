using System;
using System.Collections.Generic;
using System.Text;
using ExternalData.Classes.Manager;
using Logging.Classes;
using Storage.Classes.Models.Canteens;
using Windows.UI.Xaml.Data;

namespace UI_Context.Classes.ValueConverter
{
    public sealed class LabelsStringTooltipValueConverter: IValueConverter
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
            StringBuilder sb = new StringBuilder("Labels:\n");
            if (value is List<string> labels)
            {
                foreach (string s in labels)
                {
                    Label label = CanteenManager.INSTANCE.LookupLabel(s);
                    if (label is null)
                    {
                        Logger.Error($"Unknown label canteen dish found: {s}");
                    }
                    else
                    {
                        sb.Append(label.Abbreviation);
                        sb.Append('\t');
                        sb.Append(label.GetTranslatedName());
                        sb.Append('\n');
                    }
                }
            }
            return sb.ToString().TrimEnd();
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
