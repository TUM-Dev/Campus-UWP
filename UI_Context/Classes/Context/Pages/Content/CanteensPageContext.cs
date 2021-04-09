using System.Collections.Generic;
using System.Threading.Tasks;
using Canteens.Classes.Manager;
using Shared.Classes;
using Storage.Classes;
using Storage.Classes.Models.Canteens;
using UI_Context.Classes.Templates.Pages.Content;

namespace UI_Context.Classes.Context.Pages.Content
{
    public class CanteensPageContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CanteensPageTemplate MODEL = new CanteensPageTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public CanteensPageContext()
        {
            _ = LoadCanteensAsync();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh(bool canteens, bool dishes)
        {
            if (canteens && !MODEL.IsLoadingCanteens)
            {
                Task.Run(async () =>
                {
                    await LoadCanteensAsync();
                });
            }

            if (dishes && !MODEL.IsLoadingDishes)
            {
                Task.Run(async () =>
                {
                    MODEL.IsLoadingDishes = true;
                    IEnumerable<Dish> tmp = await DishManager.INSTANCE.UpdateAsync().ConfAwaitFalse();
                    MODEL.DISHES.Clear();
                    MODEL.DISHES.AddRange(tmp);
                    MODEL.IsLoadingDishes = false;
                });
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        private void LoadLastSelectedCanteen()
        {
            string canteenId = Storage.Classes.Settings.GetSettingString(SettingsConsts.LAST_SELECTED_CANTEEN_ID);
            if(string.Equals(MODEL.SelectedCanteen?.Id, canteenId))
            {
                return;
            }

            foreach (Canteen canteen in MODEL.CANTEENS)
            {
                if(string.Equals(canteen.Id, canteenId))
                {
                    MODEL.SelectedCanteen = canteen;
                }
            }
        }

        private async Task LoadCanteensAsync()
        {
            MODEL.IsLoadingCanteens = true;
            IEnumerable<Canteen> tmp = await CanteenManager.INSTANCE.UpdateAsync().ConfAwaitFalse();
            MODEL.CANTEENS.Clear();
            MODEL.CANTEENS.AddRange(tmp);
            LoadLastSelectedCanteen();
            MODEL.IsLoadingCanteens = false;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
