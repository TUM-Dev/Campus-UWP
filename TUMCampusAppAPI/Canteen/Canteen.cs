using SQLite.Net.Attributes;
using System;

namespace TUMCampusAppAPI.Canteens
{
    public class Canteen : IComparable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        // The canteen id e.g. 'stucafe-adalbertstr'
        public string canteen_id { get; set; }
        // The address of the canteen e.g. 'Adalbertstraße 5, München'
        public string address { get; set; }
        // The name of the canteen e.g. 'StuCafé Adalbertstraße'
        public string name { get; set; }
        // The latitude coordinate of the canteen e.g. '48.151507'
        public double latitude { get; set; }
        // The longitude coordinate of the canteen e.g. '11.579027'
        public double longitude { get; set; }

        [Ignore]
        // Used for ordering canteen
        public float distance { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// A basic empty Constructor.
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

        /// <summary>
        /// Compares to Canteen objects based on the distance.
        /// </summary>
        /// <returns>Returns the difference of both distances.</returns>
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
