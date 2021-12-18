using System;
using System.Collections.Generic;
using System.Text;
using ExternalData.Classes.Manager;
using Shared.Classes;
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
                    if (DishManager.LABELS_EMOJI_ALL_LOOKUP.ContainsKey(s))
                    {
                        sb.Append(DishManager.LABELS_EMOJI_ALL_LOOKUP[s]);
                        sb.Append('\t');
                        sb.Append(Localisation.GetLocalizedString("Labels_" + s));
                        sb.Append('\n');
                    }
                    else
                    {
                        sb.Append(s);
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
