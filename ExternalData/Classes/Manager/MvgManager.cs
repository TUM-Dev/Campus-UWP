using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using ExternalData.Classes.Mvg;
using Logging.Classes;
using Windows.Data.Json;

namespace ExternalData.Classes.Manager
{
    public class MvgManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly string BASE_URI = "https://www.mvg.de/api/fahrinfo/departure/";

        private const string JSON_DEPARTURES = "departures";
        private const string JSON_DEPARTURE_TIME = "departureTime";
        private const string JSON_DEPARTURE_PRODUCT = "product";
        private const string JSON_DEPARTURE_LABEL = "label";
        private const string JSON_DEPARTURE_DESTINATION = "destination";
        private const string JSON_DEPARTURE_DELAY = "delay";
        private const string JSON_DEPARTURE_CANCELED = "cancelled";
        private const string JSON_DEPARTURE_LINE_BACKGROUND_COLOR = "lineBackgroundColor";
        private const string JSON_DEPARTURE_PLATFORM = "platform";
        // private const string JSON_DEPARTURE_LINE_BACKGROUND_INFO_MESSAGE = "infoMessages";

        public static readonly MvgManager INSTANCE = new MvgManager();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<IEnumerable<Departure>> RequestDeparturesAsync(string stationId, bool bus, bool ubahn, bool sbahn, bool tram)
        {
            Logger.Info($"Downloading departures for '{stationId}' ...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(BuildUri(stationId, bus, ubahn, sbahn, tram));
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error($"Failed to download departures string for '{stationId}'.", e);
                    return null;
                }
            }

            JsonObject json;
            try
            {
                json = JsonObject.Parse(jsonString);
                JsonArray departuresArr = json.GetObject().GetNamedArray(JSON_DEPARTURES);
                List<Departure> departures = new List<Departure>();
                foreach (IJsonValue item in departuresArr)
                {
                    departures.Add(ParseDeparture(item.GetObject()));
                }
                Logger.Info($"Successfully downloaded {departures.Count()} departures for '{stationId}'.");
                return departures;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error($"Failed to parse downloaded departures for '{stationId}'.", e);
                return new List<Departure>();
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        private static Uri BuildUri(string stationId, bool bus, bool ubahn, bool sbahn, bool tram)
        {
            return new Uri(BASE_URI + stationId + "?footway=0" + "&bus=" + (bus ? "true" : "false") + "&ubahn=" + (ubahn ? "true" : "false") + "&sbahn=" + (sbahn ? "true" : "false") + "&tram=" + (tram ? "true" : "false"));
        }

        private Departure ParseDeparture(JsonObject json)
        {
            return new Departure
            {
                canceled = json.GetNamedBoolean(JSON_DEPARTURE_CANCELED),
                delay = (int)json.GetNamedNumber(JSON_DEPARTURE_DELAY),
                destination = json.GetNamedString(JSON_DEPARTURE_DESTINATION),
                infoMessage = "",
                label = json.GetNamedString(JSON_DEPARTURE_LABEL),
                platform = json.GetNamedString(JSON_DEPARTURE_PLATFORM),
                lineBackgroundColor = json.GetNamedString(JSON_DEPARTURE_LINE_BACKGROUND_COLOR),
                productType = ParseProduct(json.GetNamedString(JSON_DEPARTURE_PRODUCT)),
                departureTime = ParseDepartureTime(json.GetNamedNumber(JSON_DEPARTURE_TIME))
            };
        }

        private static Product ParseProduct(string productType)
        {
            switch (productType)
            {
                case "UBAHN":
                    return Product.U_BAHN;

                case "BUS":
                    return Product.BUS;

                case "TRAM":
                    return Product.TRAM;

                case "SBAHN":
                    return Product.S_BAHN;

                default:
                    Logger.Error($"Unknown MVG product '{productType}'.");
                    return Product.UNKNOWN;
            }
        }

        /// <summary>
        /// Parses the given UNIX time stamp to a <see cref="DateTime"/> object in local time.
        /// </summary>
        /// <param name="departureTime">The UNIX time of the departure in ms.</param>
        /// <returns>A <see cref="DateTime"/> object representing the local departure time.</returns>
        private DateTime ParseDepartureTime(double departureTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(departureTime).ToLocalTime();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
