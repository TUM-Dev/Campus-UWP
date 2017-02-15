using SQLite.Net.Attributes;
using System;
using TUMCampusAppAPI.Managers;

namespace TUMCampusAppAPI.Canteens
{
    public class CanteenMenu
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int cafeteriaId { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public string nameEmojis { get; set; }
        public string typeLong { get; set; }
        public int typeNr { get; set; }
        public string typeShort { get; set; }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="id">CanteenMenu Id (empty for addendum)</param>
        /// <param name="cafeteriaId">Canteen ID</param>
        /// <param name="date">Menu date</param>
        /// <param name="typeShort">Short type, e.g. tg</param>
        /// <param name="typeLong">Long type, e.g. Tagesgericht 1</param>
        /// <param name="typeNr">Type ID</param>
        /// <param name="name">Menu name</param>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteenMenu(int id, int cafeteriaId, DateTime date, string typeShort, string typeLong, int typeNr, string name)
        {
            this.id = id;
            this.cafeteriaId = cafeteriaId;
            this.date = date;
            this.typeShort = typeShort;
            this.typeLong = typeLong;
            this.typeNr = typeNr;
            this.name = name;
            this.nameEmojis = CanteenMenueManager.INSTANCE.replaceMenuStringWithImages(name, true);
        }

        /// <summary>
        /// Basic empty Constructor used for DB
        /// </summary>
        /// /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public CanteenMenu()
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
            return "id=" + this.id + " cafeteriaId=" + this.cafeteriaId + " date=" + this.date.ToString() + " typeShort="
                + this.typeShort + " typeLong=" + this.typeLong + " typeNr=" + this.typeNr + " name=" + this.name;
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
