﻿using System;
using System.ComponentModel;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Storage;

namespace Storage.Classes
{
    public static class Settings
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly ApplicationDataStorageHelper LOCAL_OBJECT_STORAGE_HELPER = ApplicationDataStorageHelper.GetCurrent(new Microsoft.Toolkit.Helpers.SystemSerializer());

        public static event PropertyChangedEventHandler SettingChanged;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Sets a setting with the given token.
        /// </summary>
        /// <param name="token">The token for saving.</param>
        /// <param name="value">The value that should get stored.</param>
        public static void SetSetting(string token, object value)
        {
            if (value is DateTime d)
            {
                SetSetting(token, d.ToFileTimeUtc());
                return;
            }

            ApplicationData.Current.LocalSettings.Values[token] = value;
            SettingChanged?.Invoke(value, new PropertyChangedEventArgs(token));
        }

        /// <summary>
        /// Returns the setting behind the given token.
        /// </summary>
        /// <param name="token">The token for the setting.</param>
        /// <returns>Returns the setting behind the given token.</returns>
        public static object GetSetting(string token)
        {
            try
            {
                return ApplicationData.Current.LocalSettings.Values[token];
            }
            catch
            {
                return null;
            }
        }

        public static bool GetSettingBoolean(string token)
        {
            return GetSettingBoolean(token, false);
        }

        public static bool GetSettingBoolean(string token, bool fallBackValue)
        {
            object obj = GetSetting(token);
            return obj is null ? fallBackValue : (bool)obj;
        }

        public static string GetSettingString(string token)
        {
            return GetSettingString(token, null);
        }

        public static string GetSettingString(string token, string fallBackValue)
        {
            object obj = GetSetting(token);
            return obj is null ? fallBackValue : (string)obj;
        }

        public static byte GetSettingByte(string token)
        {
            return GetSettingByte(token, 0);
        }

        public static byte GetSettingByte(string token, byte fallBackValue)
        {
            object obj = GetSetting(token);
            return obj is null ? fallBackValue : (byte)obj;
        }

        public static int GetSettingInt(string token)
        {
            return GetSettingInt(token, -1);
        }

        public static int GetSettingInt(string token, int fallBackValue)
        {
            object obj = GetSetting(token);
            return obj is null ? fallBackValue : (int)obj;
        }

        public static ushort GetSettingUshort(string token)
        {
            return GetSettingUshort(token, 0);
        }

        public static ushort GetSettingUshort(string token, ushort fallBackValue)
        {
            object obj = GetSetting(token);
            return obj is null ? fallBackValue : (ushort)obj;
        }

        public static double GetSettingDouble(string token)
        {
            return GetSettingDouble(token, -1);
        }

        public static double GetSettingDouble(string token, double fallBackValue)
        {
            object obj = GetSetting(token);
            return obj is null ? fallBackValue : (double)obj;
        }

        public static DateTime GetSettingDateTime(string token)
        {
            return GetSettingDateTime(token, DateTime.MinValue);
        }

        public static DateTime GetSettingDateTime(string token, DateTime fallBackValue)
        {
            object obj = GetSetting(token);
            return obj is long l ? DateTime.FromFileTimeUtc(l) : fallBackValue;
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
