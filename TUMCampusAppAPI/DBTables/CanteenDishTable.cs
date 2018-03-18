using SQLite;
using System;
using System.Globalization;
using Windows.Data.Json;

namespace TUMCampusAppAPI.DBTables
{
    public class CanteenDishTable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [NotNull]
        // The id of the canteen e.g. 'mensa-martinsried'
        public string canteen_id { get; set; }
        [NotNull]
        // The name of the dish e.g. 'Kaiserschmarrn mit Apfelmus'
        public string name { get; set; }
        public string nameEmojis { get; set; }
        [NotNull]
        // The ingredients of the menu e.g. ''
        public string ingredients { get; set; }
        [NotNull]
        // The dish type e.g. 'Tagesgericht 4'
        public string dish_type { get; set; }
        [NotNull]
        // The price of the dish e.g '2.4€' or 'N/A'
        public string price { get; set; }
        [NotNull]
        // The date of the dish e.g. '2017-12-22'
        public DateTime date { get; set; }

        // For converting doubles to Euro strings
        private static readonly CultureInfo CULTURE_INFO = new CultureInfo("fr-FR");

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic empty Constructor used for DB
        /// </summary>
        /// <history>
        /// 14/01/2018  Created [Fabian Sauter]
        /// </history>
        public CanteenDishTable()
        {

        }

        public CanteenDishTable(JsonObject json, string canteen_id)
        {
            this.canteen_id = canteen_id;
            this.name = json.GetNamedString(Const.JSON_NAME);
            this.ingredients = json.GetNamedString(Const.JSON_INGREDIENTS) ?? "";
            this.dish_type = json.GetNamedString(Const.JSON_DISH_TYPE) ?? "";
            this.date = DateTime.ParseExact(json.GetNamedString(Const.JSON_DATE), "yyyy-MM-dd", CULTURE_INFO);
            JsonValue p = json.GetNamedValue("price");
            // If a price in form of e.g. '2.4' is given, add €:
            if(p == null)
            {
                this.price = "N/A";
            }
            else if (p.ValueType == JsonValueType.Number)
            {
                this.price = p.Stringify() + '€';
                this.price = p.GetNumber().ToString("C2", CULTURE_INFO);
            }
            else
            {
                this.price = p.Stringify();
            }
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--



        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override string ToString()
        {
            return "id=" + this.id + " canteen_id=" + this.canteen_id + " date=" + this.date.ToString()
                + " name=" + this.name;
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
