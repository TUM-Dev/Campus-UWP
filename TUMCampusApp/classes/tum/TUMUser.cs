using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.Classes.Tum
{
    class TUMUser
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private string fName;
        private string sName;
        private char gen;
        private string nr;
        private string obfuId;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Constructor based on the xml representation of an user
        /// </summary>
        /// <history>
        /// 28/12/2016  Created [Fabian Sauter]
        /// </history>
        public TUMUser(IXmlNode xml)
        {
            fromXml(xml);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public string getFName()
        {
            return fName;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void fromXml(IXmlNode xml)
        {
            if(xml == null)
            {
                return;
            }
            this.fName = xml.SelectSingleNode("vorname").InnerText;
            this.sName = xml.SelectSingleNode("familienname").InnerText;
            this.gen = xml.SelectSingleNode("geschlecht").InnerText[0];
            this.nr = xml.SelectSingleNode("nr").InnerText;
            this.obfuId = xml.SelectSingleNode("obfuscated_id").InnerText;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
