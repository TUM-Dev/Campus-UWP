using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Storage.Classes.Models.Canteens
{
    public class Label: AbstractCanteensModel
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string EnumName { get; set; }
        [Required]
        public string Abbreviation { get; set; }
        [Required]
        public List<LabelTranslation> Translations { get; set; } = new List<LabelTranslation>();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the translated labeld for the current system language or in English if the language is not available.
        /// In case English is also not available, it will return the <see cref="EnumName"/>.
        /// </summary>
        public string GetTranslatedName()
        {
            LabelTranslation english = null;
            foreach (LabelTranslation translation in Translations)
            {
                if (string.Equals(translation.Lang, CultureInfo.CurrentCulture.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase))
                {
                    return translation.Text;
                }
                else if (string.Equals(translation.Lang, "en", StringComparison.OrdinalIgnoreCase))
                {
                    english = translation;
                }
            }

            if (!(english is null))
            {
                return english.Text;
            }
            return EnumName;
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


        #endregion
    }
}
