using System.Threading.Tasks;
using ExternalData.Classes.Manager;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes.Models.Canteens;
using UI_Context.Classes.Templates.Controls.Canteens;

namespace UI_Context.Classes.Context.Controls.Canteens
{
    public class DishRatingControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly DishRatingControlDataTemplate MODEL = new DishRatingControlDataTemplate();

        private Task updateTask;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void UpdateView(Dish dish)
        {
            if (dish is null)
            {
                return;
            }
            Task.Run(async () => await UpdateRatingAsync(dish));
        }

        public async Task UpdateRatingAsync(Dish dish)
        {
            // Wait for the old update to finish first:
            if (updateTask is null || updateTask.IsCompleted)
            {
                updateTask = Task.Run(async () =>
                {
                    MODEL.HasValidRating = false;
                    MODEL.IsUnavailable = false;
                    MODEL.IsLoading = true;
                    Logger.Debug($"Loading rating for '{dish.Name}' ({dish.CanteenId}).");
                    MODEL.Rating = await CanteenRatingManager.INSTANCE.GetRatingAsync(dish);
                    MODEL.IsLoading = false;
                    MODEL.HasValidRating = !(MODEL.Rating is null);
                    MODEL.IsUnavailable = MODEL.Rating is null;
                });
            }
            await updateTask.ConfAwaitFalse();
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
