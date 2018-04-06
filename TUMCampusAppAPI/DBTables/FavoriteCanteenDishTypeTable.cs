using SQLite;

namespace TUMCampusAppAPI.DBTables
{
    [Table(DBTableConsts.FAVORITE_CANTEEN_DISH_TABLE)]
    public class FavoriteCanteenDishTypeTable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        [PrimaryKey]
        // PrimaryKey generated in generateId()
        public string id { get; set; }
        [NotNull]
        // The dish type e.g. 'Tagesgericht 4'
        public string dish_type { get; set; }
        [NotNull]
        // The id of the canteen e.g. 'mensa-martinsried'
        public string canteen_id { get; set; }

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 19/01/2018 Created [Fabian Sauter]
        /// </history>
        public FavoriteCanteenDishTypeTable()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public static string generateId(string canteen_id, string dish_type)
        {
            return dish_type + '_' + canteen_id;
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
