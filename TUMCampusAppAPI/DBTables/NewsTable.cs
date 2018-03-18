﻿using SQLite;
using System;
using System.Globalization;
using Windows.Data.Json;

namespace TUMCampusAppAPI.DBTables
{
    public class NewsTable : IComparable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public string id { get; set; }
        public string src { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string imageUrl { get; set; }
        public DateTime date { get; set; }
        public DateTime created { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsTable()
        {

        }

        public NewsTable(JsonObject json)
        {
            this.id = json.GetNamedString(Const.JSON_NEWS);
            this.src = json.GetNamedString(Const.JSON_SRC);

            this.title = json.GetNamedString(Const.JSON_TITLE);
            if (src.Equals("2"))
            {
                int index = this.title.IndexOf(':');
                if(index >= 0 && index < this.title.Length - 3)
                {
                    this.title = this.title.Substring(index + 2);
                }
            }
            this.link = json.GetNamedString(Const.JSON_LINK);
            JsonValue val = json.GetNamedValue(Const.JSON_IMAGE);
            this.imageUrl = val.ValueType == JsonValueType.Null ? null : val.Stringify();
            if(imageUrl != null)
            {
                this.imageUrl = imageUrl.Replace("\"", "");
            }

            this.date = DateTime.ParseExact(json.GetNamedString(Const.JSON_DATE), "yyyy-MM-dd HH:mm:ss", new CultureInfo("de-DE"));
            this.created = DateTime.ParseExact(json.GetNamedString(Const.JSON_CREATED), "yyyy-MM-dd HH:mm:ss", new CultureInfo("de-DE"));
        }

        public int CompareTo(object obj)
        {
            if(obj == null || !(obj is NewsTable))
            {
                return -1;
            }
            return date.CompareTo((obj as NewsTable).date);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


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
