using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.Classes.Tum
{
    class TUMOnlineLectureInformation
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public string teachingContent { get; set; }
        public string prerequisites { get; set; }
        public string learningTarget { get; set; }
        public string startDate { get; set; }
        public string testMode { get; set; }
        public string note { get; set; }
        public string datesUrl { get; set; }
        public string exameDatesUrl { get; set; }
        public string teachingMethod { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 15/01/2017 Created [Fabian Sauter]
        /// </history>
        public TUMOnlineLectureInformation(IXmlNode element)
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
        private void fromXml(IXmlNode element)
        {
            if (element == null)
            {
                return;
            }
            this.teachingContent = element.SelectSingleNode("lehrinhalt").InnerText;
            this.prerequisites = element.SelectSingleNode("voraussetzung_lv ").InnerText;
            this.learningTarget = element.SelectSingleNode("lehrziel").InnerText;
            this.startDate = element.SelectSingleNode("ersttermin").InnerText;
            this.testMode = element.SelectSingleNode("pruefmodus").InnerText;
            this.note = element.SelectSingleNode("anmerkung").InnerText;
            this.datesUrl = element.SelectSingleNode("termine_url").InnerText;
            this.exameDatesUrl = element.SelectSingleNode("pruef_termine_url").InnerText;
            this.teachingMethod = element.SelectSingleNode("lehrmethode").InnerText;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
