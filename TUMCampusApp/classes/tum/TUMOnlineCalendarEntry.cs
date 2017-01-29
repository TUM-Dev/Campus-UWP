using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.Classes.Tum
{
    public class TUMOnlineCalendarEntry : IComparable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public int nr { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public DateTime dTStrat { get; set; }
        public DateTime dTEnd { get; set; }
        public string location { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 20/01/2017 Created [Fabian Sauter]
        /// </history>
        public TUMOnlineCalendarEntry()
        {

        }

        public TUMOnlineCalendarEntry(IXmlNode element)
        {
            fromXml(element);
        }


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public Appointment getAppointment()
        {
            Appointment app = new Appointment();

            app.Subject = title;
            app.Details = description;
            app.Location = location;
            app.StartTime = dTStrat;
            app.Duration = dTEnd.Subtract(dTStrat);
            app.BusyStatus = AppointmentBusyStatus.Busy;

            return app;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override bool Equals(object obj)
        {
            if(obj != null && obj is TUMOnlineCalendarEntry)
            {
                TUMOnlineCalendarEntry cE = obj as TUMOnlineCalendarEntry;
                if(cE.title.Equals(title) && cE.description.Equals(description) && cE.dTStrat.Equals(dTStrat) && cE.dTEnd.Equals(dTEnd) && cE.url.Equals(url))
                {
                    return true;
                }
            }
            return false;
        }

        public int CompareTo(object obj)
        {
            if(obj != null && obj is TUMOnlineCalendarEntry)
            {
                return dTStrat.CompareTo((obj as TUMOnlineCalendarEntry).dTStrat);
            }
            return 1;
        }

        #endregion

        #region --Misc Methods (Private)--
        private void fromXml(IXmlNode xml)
        {
            if (xml == null)
            {
                return;
            }
            this.nr = int.Parse(xml.SelectSingleNode("nr").InnerText);
            this.status = xml.SelectSingleNode("status").InnerText;
            this.title = xml.SelectSingleNode("title").InnerText;
            this.url = xml.SelectSingleNode("url").InnerText;
            this.description = xml.SelectSingleNode("description").InnerText;
            this.dTStrat = DateTime.Parse(xml.SelectSingleNode("dtstart").InnerText);
            this.dTEnd = DateTime.Parse(xml.SelectSingleNode("dtend").InnerText);
            this.location = xml.SelectSingleNode("location").InnerText;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}