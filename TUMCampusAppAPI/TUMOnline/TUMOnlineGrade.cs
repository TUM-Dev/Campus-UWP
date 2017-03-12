using SQLite.Net.Attributes;
using System;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.TUMOnline
{
    public class TUMOnlineGrade
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public string lvNummer { get; set; }
        public string pvKandNr { get; set; }
        public DateTime date { get; set; }
        public string lvSemester { get; set; }
        public string lvTitel { get; set; }
        public string examinerSurname { get; set; }
        public string uniGradeNameShort { get; set; }
        public string examTypName { get; set; }
        public string mode { get; set; }
        public string studienidentifikator { get; set; }
        public string studyTitle { get; set; }
        public string stStudyNr { get; set; }
        public string lvCredits { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 12/03/2017 Created [Fabian Sauter]
        /// </history>
        public TUMOnlineGrade()
        {

        }

        public TUMOnlineGrade(IXmlNode element)
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
            this.lvNummer = xml.SelectSingleNode("lv_nummer").InnerText;
            this.pvKandNr = xml.SelectSingleNode("datum").InnerText;
            this.date = DateTime.Parse(xml.SelectSingleNode("datum").InnerText);
            this.lvSemester = xml.SelectSingleNode("lv_semester").InnerText;
            this.lvTitel = xml.SelectSingleNode("lv_titel").InnerText;
            this.examinerSurname = xml.SelectSingleNode("pruefer_nachname").InnerText;
            this.uniGradeNameShort = xml.SelectSingleNode("uninotenamekurz").InnerText;
            this.examTypName = xml.SelectSingleNode("exam_typ_name").InnerText;
            this.mode = xml.SelectSingleNode("modus").InnerText;
            this.studienidentifikator = xml.SelectSingleNode("studienidentifikator").InnerText;
            this.studyTitle = xml.SelectSingleNode("studienbezeichnung").InnerText;
            this.stStudyNr = xml.SelectSingleNode("st_studium_nr").InnerText;
            this.lvCredits = xml.SelectSingleNode("lv_credits").InnerText;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
