using SQLite.Net.Attributes;
using System;
using System.Globalization;
using Windows.Data.Json;

namespace TUMCampusAppAPI.News
{
    public class News
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public string id { get; set; }
        public string src { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string image { get; set; }
        public DateTime date { get; set; }
        public DateTime created { get; set; }
        public bool dismissed { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/06/2017 Created [Fabian Sauter]
        /// </history>
        public News()
        {

        }

        public News(JsonObject json)
        {
            this.dismissed = false;
            this.id = json.GetNamedString(Const.JSON_NEWS);
            this.src = json.GetNamedString(Const.JSON_SRC);
            this.title = json.GetNamedString(Const.JSON_TITLE);
            this.link = json.GetNamedString(Const.JSON_LINK);
            JsonValue val = json.GetNamedValue(Const.JSON_IMAGE);
            this.image = val.ValueType == JsonValueType.Null ? null : val.Stringify();

            this.date = DateTime.ParseExact(json.GetNamedString(Const.JSON_DATE), "yyyy-MM-dd HH:mm:ss", new CultureInfo("de-DE"));
            this.created = DateTime.ParseExact(json.GetNamedString(Const.JSON_CREATED), "yyyy-MM-dd HH:mm:ss", new CultureInfo("de-DE"));
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
