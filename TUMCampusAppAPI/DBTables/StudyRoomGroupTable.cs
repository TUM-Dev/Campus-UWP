using SQLite.Net.Attributes;
using Windows.Data.Json;

namespace TUMCampusAppAPI.DBTables
{
    public class StudyRoomGroupTable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 24/02/2017 Created [Fabian Sauter]
        /// </history>
        public StudyRoomGroupTable()
        {

        }

        public StudyRoomGroupTable(JsonObject json)
        {
            this.id = (int)json.GetNamedNumber("nr");
            this.name = json.GetNamedString("name");
            this.description = json.GetNamedString("detail");
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
