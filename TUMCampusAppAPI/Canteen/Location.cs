using SQLite.Net.Attributes;

namespace TUMCampusAppAPI.Canteens
{
    public class Location
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string address { get; set; }
        public string category { get; set; }
        public string hours { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public string room { get; set; }
        public string transport { get; set; }
        public string url { get; set; }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="id">Location ID, e.g. 100</param>
        /// <param name="category">Location category, e.g. library, cafeteria, info</param>
        /// <param name="name">Location name, e.g. Studentenwerksbibliothek</param>
        /// <param name="address">Address, e.g. Arcisstr. 21</param>
        /// <param name="room">Room, e.g. MI 00.01.123</param>
        /// <param name="transport">Transportation station name, e.g. U2 Königsplatz</param>
        /// <param name="hours">Opening hours, e.g. Mo–Fr 8–24</param>
        /// <param name="remark">Additional information, e.g. Tel: 089-11111</param>
        /// <param name="url">Location URL, e.g. http://stud.ub.uni-muenchen.de/</param>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public Location(int id, string category, string name, string address, string room, string transport, string hours, string remark, string url) {
            this.id = id;
            this.category = category;
            this.name = name;
            this.address = address;
            this.room = room;
            this.transport = transport;
            this.hours = hours;
            this.remark = remark;
            this.url = url;
        }

        /// <summary>
        /// Basic empty Constructor used for DB
        /// </summary>
        /// /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public Location()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--



        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override string ToString()
        {
            return "id=" + id + ", category=" + category + ", name=" + name + ", address=" + address + ", room=" + room + ", transport="
                + transport + ", hours=" + hours + ", remark=" + remark + ", url=" + url;
        }


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
