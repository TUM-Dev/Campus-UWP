﻿using SQLite;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Windows.Data.Json;

namespace TUMCampusAppAPI.DBTables
{
    [Table(DBTableConsts.CANTEEN_DISH_TABLE)]
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
        // The name of the dish with ingredient emojis e.g. 'Kaiserschmarrn mit Apfelmus 🥕'
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
            this.name = json.GetNamedString(Consts.JSON_NAME);
            JsonArray ingredientsArr = json.GetNamedArray(Consts.JSON_INGREDIENTS);
            StringBuilder sb = new StringBuilder();
            // Convert all ingredients to a single string separated by whitespaces:
            foreach (IJsonValue ingredient in ingredientsArr)
            {
                sb.Append(ingredient.GetString());
                if (ingredient != ingredientsArr.Last())
                {
                    sb.Append(' ');
                }
            }
            this.ingredients = sb.ToString();
            this.dish_type = json.GetNamedString(Consts.JSON_DISH_TYPE) ?? "";
            this.date = DateTime.ParseExact(json.GetNamedString(Consts.JSON_DATE), "yyyy-MM-dd", CULTURE_INFO);
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
