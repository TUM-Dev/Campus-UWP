using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using ExternalData.Classes.Manager;
using ExternalData.Classes.Mvg;
using Logging.Classes;
using Storage.Classes;
using UI_Context.Classes.Templates.Controls.Mvg;

namespace UI_Context.Classes.Context.Controls.Mvg
{
    public class MvgWidgetControlContext: IDisposable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly MvgWidgetControlDataTemplate MODEL = new MvgWidgetControlDataTemplate();
        private readonly Timer TIMER = new Timer(60 * 1000); // Refresh every 60 seconds

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public MvgWidgetControlContext()
        {
            MODEL.LastUpdate = DateTime.Now;
            TIMER.AutoReset = false;
            TIMER.Elapsed += OnTimerElapsed;
            MODEL.CurStation = LoadDefaultStation();
            Refresh();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh()
        {
            Task.Run(async () => await LoadDeparturesAsync());
        }

        public void ChangeStation(Station station)
        {
            MODEL.CurStation = station;
            Storage.Classes.Settings.SetSetting(SettingsConsts.MVG_DEFAULT_STATION_ID, station.id);
            Storage.Classes.Settings.SetSetting(SettingsConsts.MVG_DEFAULT_STATION_NAME, station.name);
            Logger.Info($"MVG station changed to: {station.name}");
            Refresh();
        }

        public void Dispose()
        {
            TIMER?.Stop();
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadDeparturesAsync()
        {
            // Ensure not multiple tasks are loading at the same time:
            lock (this)
            {
                if (MODEL.IsLoading)
                {
                    return;
                }
                MODEL.IsLoading = true;
            }
            string stationId = MODEL.CurStation?.id;
            IEnumerable<Departure> departures;
            if (!string.IsNullOrEmpty(stationId))
            {
                departures = await MvgManager.INSTANCE.RequestDeparturesAsync(stationId, true, true, true, true);
            }
            else
            {
                departures = new List<Departure>();
            }

            MODEL.DEPARTURES.Replace(departures);
            MODEL.HasDepartures = !MODEL.DEPARTURES.IsEmpty();
            MODEL.LastUpdate = DateTime.Now;
            TIMER.Start();
            MODEL.IsLoading = false;
        }

        /// <summary>
        /// Returns for now only Garching-Forschungszentrum as default station. This will change when we have something like a default campus.
        /// </summary>
        /// <returns>The default station.</returns>
        private Station LoadDefaultStation()
        {
            return new Station
            {
                id = Storage.Classes.Settings.GetSettingString(SettingsConsts.MVG_DEFAULT_STATION_ID, "de:09184:460"),
                name = Storage.Classes.Settings.GetSettingString(SettingsConsts.MVG_DEFAULT_STATION_NAME, "Garching-Forschungszentrum")
            };
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Refresh();
        }

        #endregion
    }
}
