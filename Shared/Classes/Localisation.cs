﻿using Windows.ApplicationModel.Resources;

namespace Shared.Classes
{
    public static class Localisation
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static ResourceLoader loader;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns a localized string for the given key.
        /// </summary>
        /// <param name="key">The key for the requested localized string.</param>
        /// <returns>a localized string for the given key.</returns>
        public static string GetLocalizedString(string key)
        {
            if (loader is null)
            {
                loader = ResourceLoader.GetForViewIndependentUse();
            }
            return loader.GetString(key);
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
