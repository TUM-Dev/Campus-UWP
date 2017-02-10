using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.Classes.Tum
{
    public class TUMTuitionFee
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string money { get; set; }
        public string deadline { get; set; }
        public string semesterDescripion { get; set; }
        public string semesterId { get; set; }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/01/2017 Created [Fabian Sauter]
        /// </history>
        public TUMTuitionFee(IXmlNode xml)
        {
            fromXml(xml);
        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 05/01/2017 Created [Fabian Sauter]
        /// </history>
        public TUMTuitionFee()
        {

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
            this.money = xml.SelectSingleNode("soll").InnerText;
            this.deadline = xml.SelectSingleNode("frist").InnerText;
            this.semesterDescripion = xml.SelectSingleNode("semester_bezeichnung").InnerText;
            this.semesterId = xml.SelectSingleNode("semester_id").InnerText;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
