using SQLite;
using System;
using System.Globalization;
using Windows.Data.Json;

namespace TUMCampusAppAPI.DBTables
{
    [Table(DBTableConsts.STUDY_ROOM_TABLE)]
    public class StudyRoomTable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string location { get; set; }
        public DateTime occupied_till { get; set; }
        public int group_id { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 29/01/2017 Created [Fabian Sauter]
        /// </history>
        public StudyRoomTable()
        {

        }

        public StudyRoomTable(JsonObject json)
        {
            this.id = (int)json.GetNamedNumber("raum_nr");
            this.name = json.GetNamedString("raum_name");
            this.code = json.GetNamedString("raum_code");
            this.location = json.GetNamedString("gebaeude_name");
            string occupied_t = json.GetNamedString("belegung_bis");
            if(occupied_t == null || occupied_t == "")
            {
                return;
            }
            try
            {
                this.occupied_till = DateTime.ParseExact(json.GetNamedString("belegung_bis"), "yyyy-MM-dd HH:mm:ss", new CultureInfo("de-DE"));
            }
            catch
            {
            }
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
