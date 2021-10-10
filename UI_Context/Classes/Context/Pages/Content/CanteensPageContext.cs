using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using ExternalData.Classes.Manager;
using Logging.Classes;
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
            DishManager.INSTANCE.OnRequestError += OnRequestDishesError;
            CanteenManager.INSTANCE.OnRequestError += OnRequestCanteensError;
            Task.Run(async () => await LoadCanteensAsync(false));
            MODEL.PropertyChanged += OnModelPropertyChanged;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh(bool canteens, bool dishes)
        {
            Task.Run(async () =>
            {
                if (canteens && !MODEL.IsLoadingCanteens)
                {
                    await LoadCanteensAsync(true);
                }

                if (dishes && !MODEL.IsLoadingDishes)
                {
                    await LoadDishesForCanteenAsync(MODEL.SelectedCanteen, true);
                }
            });
        }

        public void NextDate()
        {
            DateTime newDate = DateTime.MaxValue;
            if (MODEL.DishDate >= DateTime.MaxValue || MODEL.DishDate < DateTime.Now.Date)
            {
                newDate = DishManager.INSTANCE.GetNextDate(MODEL.SelectedCanteen.Id, DateTime.Now.AddDays(-1));
            }
            else
            {
                newDate = DishManager.INSTANCE.GetNextDate(MODEL.SelectedCanteen.Id, MODEL.DishDate);
                if (newDate == DateTime.MaxValue)
                {
                    newDate = DishManager.INSTANCE.GetNextDate(MODEL.SelectedCanteen.Id, DateTime.Now.AddDays(-1));
                }
            }
            MODEL.DishDate = newDate;
            Task.Run(async () => await LoadDishesForCanteenAsync(MODEL.SelectedCanteen, false));
        }

        public void PrevDate()
        {
            DateTime newDate = DateTime.MinValue;
            if (MODEL.DishDate <= DateTime.Now.Date)
            {
                newDate = DishManager.INSTANCE.GetPrevDate(MODEL.SelectedCanteen.Id, DateTime.MaxValue);
            }
            else
            {
                newDate = DishManager.INSTANCE.GetPrevDate(MODEL.SelectedCanteen.Id, MODEL.DishDate);
                if (newDate == DateTime.MinValue || newDate < DateTime.Now.Date)
                {
                    newDate = DishManager.INSTANCE.GetPrevDate(MODEL.SelectedCanteen.Id, DateTime.MaxValue);
                }
            }
            MODEL.DishDate = newDate;
            Task.Run(async () => await LoadDishesForCanteenAsync(MODEL.SelectedCanteen, false));
        }

        #endregion

        #region --Misc Methods (Private)--
        private void LoadLastSelectedCanteen()
        {
            string canteenId = Storage.Classes.Settings.GetSettingString(SettingsConsts.LAST_SELECTED_CANTEEN_ID);
            if (!(canteenId is null))
            {
                if (string.Equals(MODEL.SelectedCanteen?.Id, canteenId))
                {
                    return;
                }

                foreach (Canteen canteen in MODEL.CANTEENS)
                {
                    if (string.Equals(canteen.Id, canteenId))
                    {
                        MODEL.SelectedCanteen = canteen;
                        return;
                    }
                }
            }

            // By default load the first canteen:
            if (MODEL.CANTEENS.Count > 0)
            {
                MODEL.SelectedCanteen = MODEL.CANTEENS[0];
            }
        }

        private async Task LoadCanteensAsync(bool refresh)
        {
            MODEL.IsLoadingCanteens = true;
            MODEL.ShowCanteensError = false;
            IEnumerable<Canteen> canteens;
            canteens = await CanteenManager.INSTANCE.UpdateAsync(refresh).ConfAwaitFalse();
            MODEL.CANTEENS.Clear();
            MODEL.CANTEENS.AddRange(canteens);
            LoadLastSelectedCanteen();
            MODEL.IsLoadingCanteens = false;
        }

        private async Task LoadDishesForCanteenAsync(Canteen canteen, bool refresh)
        {
            MODEL.IsLoadingDishes = true;
            MODEL.ShowDishesError = false;
            MODEL.DISHES.Clear();
            if (!(canteen is null))
            {
                IEnumerable<Dish> dishes;
                await DishManager.INSTANCE.UpdateAsync(refresh).ConfAwaitFalse();
                if (MODEL.DishDate == DateTime.MaxValue)
                {
                    // Load dishes for the next day when canteens are closed already anyway:
                    DateTime date = DateTime.Now.Hour >= 15 ? DateTime.Now : DateTime.Now.AddDays(-1);
                    MODEL.DishDate = DishManager.INSTANCE.GetNextDate(canteen.Id, date);
                    if (MODEL.DishDate == DateTime.MaxValue)
                    {
                        Logger.Info($"No next dishes found for canteen '{canteen.Id}'.");
                        MODEL.IsLoadingDishes = false;
                        return;
                    }
                }
                dishes = await DishManager.INSTANCE.LoadDishesAsync(canteen.Id, MODEL.DishDate).ConfAwaitFalse();
                MODEL.DISHES.AddRange(dishes);
            }
            MODEL.IsLoadingDishes = false;
        }

        public async Task ShowEatApiBugAsync()
        {
            await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("EatApiBugUrl")));
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, nameof(MODEL.SelectedCanteen)))
            {
                Task.Run(async () => await LoadDishesForCanteenAsync(MODEL.SelectedCanteen, false));
            }
        }

        private void OnRequestCanteensError(AbstractManager sender, RequestErrorEventArgs e)
        {
            MODEL.ShowCanteensError = true;
        }

        private void OnRequestDishesError(AbstractManager sender, RequestErrorEventArgs e)
        {
            MODEL.ShowDishesError = true;
        }

        #endregion
    }
}
