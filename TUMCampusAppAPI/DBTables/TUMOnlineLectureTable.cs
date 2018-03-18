using SQLite;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.DBTables
{
    public class TUMOnlineLectureTable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public int sp_nr { get; set; }
        public int lv_nr { get; set; }
        public string duration { get; set; }
        public string title { get; set; }
        public string typeLong { get; set; }
        public string typeShort { get; set; }
        public string semester { get; set; }
        public string semesterJearName { get; set; }
        public string semesterName { get; set; }
        public string semesterId { get; set; }
        public int supervisorId { get; set; }
        public string facSupervisorName { get; set; }
        public string facSupervisorId { get; set; }
        public string existingContributors { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 13/01/2017 Created [Fabian Sauter]
        /// </history>
        public TUMOnlineLectureTable()
        {

        }

        public TUMOnlineLectureTable(IXmlNode element)
        {
            fromXml(element);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void fromXml(IXmlNode xml)
        {
            if (xml == null)
            {
                return;
            }
            this.sp_nr = int.Parse(xml.SelectSingleNode("stp_sp_nr").InnerText);
            this.lv_nr = int.Parse(xml.SelectSingleNode("stp_lv_nr").InnerText);
            this.duration = xml.SelectSingleNode("dauer_info").InnerText;
            this.title = xml.SelectSingleNode("stp_sp_titel").InnerText;
            this.typeLong = xml.SelectSingleNode("stp_lv_art_name").InnerText;
            this.typeShort = xml.SelectSingleNode("stp_lv_art_kurz").InnerText;
            this.semester = xml.SelectSingleNode("semester").InnerText;
            this.semesterJearName = xml.SelectSingleNode("sj_name").InnerText;
            this.semesterName = xml.SelectSingleNode("semester_name").InnerText;
            this.semesterId = xml.SelectSingleNode("semester_id").InnerText;
            this.supervisorId = int.Parse(xml.SelectSingleNode("org_nr_betreut").InnerText);
            this.facSupervisorName = xml.SelectSingleNode("org_name_betreut").InnerText;
            this.facSupervisorId = xml.SelectSingleNode("org_kennung_betreut").InnerText;
            this.existingContributors = xml.SelectSingleNode("vortragende_mitwirkende").InnerText;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
