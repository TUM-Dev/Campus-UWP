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
using Windows.UI.Xaml.Controls.Maps;

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
            Load();
            MODEL.PropertyChanged += OnModelPropertyChanged;
            LoadSettings();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void RefreshCanteens()
        {
            Task.Run(async () =>
            {
                if (!MODEL.IsLoadingCanteens)
                {
                    await LoadCanteensAsync(true);
                }
            });
        }

        public void RefreshDishes()
        {
            Task.Run(async () =>
            {
                if (!MODEL.IsLoadingCanteens)
                {
                    await LoadDishesForCanteenAsync(MODEL.SelectedCanteen, true);
                }
            });
        }

        public void RefreshLanguages()
        {
            Task.Run(async () =>
            {
                if (!MODEL.IsLoadingLanguages)
                {
                    await LoadLanguagesAsync(true);
                }
            });
        }

        public void RefreshLabels()
        {
            Task.Run(async () =>
            {
                if (!MODEL.IsLoadingLabels)
                {
                    await LoadLabelsAsync(true);
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

        public async Task ShowEatApiBugAsync()
        {
            await UiUtils.LaunchUriAsync(new Uri(Localisation.GetLocalizedString("EatApiBugUrl")));
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
            canteens = await CanteenManager.INSTANCE.UpdateCanteensAsync(refresh).ConfAwaitFalse();
            MODEL.CANTEENS.Replace(canteens);
            LoadLastSelectedCanteen();
            MODEL.IsLoadingCanteens = false;
        }

        private async Task LoadLanguagesAsync(bool refresh)
        {
            MODEL.IsLoadingLanguages = true;
            IEnumerable<Language> languages;
            languages = await CanteenManager.INSTANCE.UpdateLanguagesAsync(refresh).ConfAwaitFalse();
            MODEL.LANGUAGES.Replace(languages);
            MODEL.IsLoadingLanguages = false;
        }

        private async Task LoadLabelsAsync(bool refresh)
        {
            MODEL.IsLoadingLabels = true;
            await CanteenManager.INSTANCE.UpdateLabelsAsync(refresh).ConfAwaitFalse();
            MODEL.IsLoadingLabels = false;
        }

        private async Task LoadDishesForCanteenAsync(Canteen canteen, bool refresh)
        {
            MODEL.IsLoadingDishes = true;
            MODEL.ShowDishesError = false;
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
                MODEL.DISHES.Replace(dishes);
            }
            else
            {
                MODEL.DISHES.Clear();
            }
            MODEL.IsLoadingDishes = false;
        }

        private void LoadSettings()
        {
            int mapStyle = Storage.Classes.Settings.GetSettingInt(SettingsConsts.CANTEENS_MAP_STYLE, (int)MapStyle.Terrain);
            try
            {
                MODEL.MapStyle = (MapStyle)mapStyle;
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to parse MapStyle from {mapStyle}.", e);
                MODEL.MapStyle = MapStyle.Terrain;
            }
        }

        private void Load()
        {
            Task.Run(async () =>
            {
                await LoadLabelsAsync(false);
                await LoadLanguagesAsync(false);

                if (MODEL.LANGUAGES.IsEmpty())
                {
                    MODEL.ShowCanteensError = true;
                    MODEL.CANTEENS.Clear();
                    return;
                }

                await LoadCanteensAsync(false);
            });
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
