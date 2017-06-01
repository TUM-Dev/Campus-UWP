using SQLite.Net.Attributes;
using Windows.Data.Json;

namespace TUMCampusAppAPI.News
{
    public class NewsSource
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string src { get; set; }
        public string title { get; set; }
        public string icon { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 01/06/2017 Created [Fabian Sauter]
        /// </history>
        public NewsSource()
        {

        }

        public NewsSource(JsonObject json)
        {
            this.src = json.GetNamedString(Const.JSON_SRC);
            this.icon = json.GetNamedString(Const.JSON_ICON);
            this.title = json.GetNamedString(Const.JSON_TITLE);
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
