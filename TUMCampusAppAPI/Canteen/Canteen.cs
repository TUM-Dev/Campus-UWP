using SQLite.Net.Attributes;
using System;

namespace TUMCampusAppAPI.Canteens
{
    public class Canteen : IComparable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        public int id { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        // Used for ordering canteen
        public float distance;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="id">Canteen ID, e.g. 412</param>
        /// <param name="name">Name, e.g. MensaX</param>
        /// <param name="address">Address, e.g. Boltzmannstr. 3</param>
        /// <param name="latitude">Coordinates of the canteen</param>
        /// <param name="longitude">Coordinates of the canteen</param>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public Canteen(int id, string name, string address, double latitude, double longitude)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.latitude = latitude;
            this.longitude = longitude;
        }

        /// <summary>
        /// Basic empty Constructor used for DB
        /// </summary>
        /// /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public Canteen()
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
            return this.name;
        }
        
        public int CompareTo(object obj)
        {
            if(!(obj is Canteen))
            {
                return -1;
            }
            Canteen canteen = obj as Canteen;
            if (this.distance < canteen.distance)
            {
                return -1;
            }
            else
            {
                return 1;
            }
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
