using System;
using System.Threading.Tasks;
using Storage.Classes.Models.Canteens;

namespace ExternalData.Classes.Manager
{
    public class CanteenRatingManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly CanteenRatingManager INSTANCE = new CanteenRatingManager();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Returns the rating for the given dish if available locally.
        /// In case no rating is available locally, it will try to fetch it from online and caches it locally.
        /// </summary>
        /// <param name="dish">The <see cref="Dish"/> you want to get a rating for.</param>
        /// <returns>On success a valid rating or null in case something went wrong or no rating was found.</returns>
        public async Task<Rating> GetRatingAsync(Dish dish)
        {
            Random r = new Random();
            await Task.Delay(1000 * dish.Labels.Count);
            if (r.NextDouble() < 0.5)
            {
                return null;
            }
            return new Rating
            {
                AveragePoints = r.NextDouble() * 5,
                CanteenId = dish.CanteenId,
                DishId = dish.Id,
                RatingCount = r.Next(10000),
            };
        }

        /// <summary>
        /// Returns the rating for the given canteen if available locally.
        /// In case no rating is available locally, it will try to fetch it from online and caches it locally.
        /// </summary>
        /// <param name="canteen">The <see cref="Canteen"/> you want to get a rating for.</param>
        /// <returns>On success a valid rating or null in case something went wrong or no rating was found.</returns>
        public async Task<Rating> GetRatingAsync(Canteen canteen)
        {
            await Task.Delay(1000 * canteen.Name.Length);
            return null;
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
