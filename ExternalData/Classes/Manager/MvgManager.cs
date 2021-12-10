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
        private static readonly string BASE_DEPARTURE_URI = "https://www.mvg.de/api/fahrinfo/departure/";
        private static readonly string BASE_STATION_QUERY_URI = "https://www.mvg.de/api/fahrinfo/location/queryWeb";

        private const string JSON_DEPARTURES = "departures";
        private const string JSON_DEPARTURE_TIME = "departureTime";
        private const string JSON_DEPARTURE_PRODUCT = "product";
        private const string JSON_DEPARTURE_LABEL = "label";
        private const string JSON_DEPARTURE_DESTINATION = "destination";
        private const string JSON_DEPARTURE_DELAY = "delay";
        private const string JSON_DEPARTURE_CANCELED = "cancelled";
        private const string JSON_DEPARTURE_LINE_BACKGROUND_COLOR = "lineBackgroundColor";
        private const string JSON_DEPARTURE_PLATFORM = "platform";

        private const string JSON_STATION_ID = "id";
        private const string JSON_STATION_NAME = "name";
        private const string JSON_STATION_LOCATIONS = "locations";
        private const string JSON_STATION_TYPE = "type";
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
                    jsonString = await wc.DownloadStringTaskAsync(BuildDepartureUri(stationId, bus, ubahn, sbahn, tram));
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error($"Failed to download departures string for '{stationId}'.", e);
                    return new List<Departure>();
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

        public async Task<IEnumerable<Station>> FindStationAsync(string query)
        {
            Logger.Info($"Finding stations for '{query}' ...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(BuildStationQueryUri(query));
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error($"Failed to download stations string for '{query}'.", e);
                    return new List<Station>();
                }
            }

            JsonObject json;
            try
            {
                json = JsonObject.Parse(jsonString);
                JsonArray stationsArr = json.GetObject().GetNamedArray(JSON_STATION_LOCATIONS);
                List<Station> stations = new List<Station>();
                foreach (IJsonValue item in stationsArr)
                {
                    // Ignore everything that is not a station:
                    if (string.Equals(item.GetObject().GetNamedString(JSON_STATION_TYPE), "station"))
                    {
                        stations.Add(ParseStation(item.GetObject()));
                    }
                }
                Logger.Info($"Successfully downloaded {stations.Count()} stations for '{query}'.");
                return stations;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error($"Failed to parse downloaded stations for '{query}'.", e);
                return new List<Station>();
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        private static Uri BuildDepartureUri(string stationId, bool bus, bool ubahn, bool sbahn, bool tram)
        {
            return new Uri(BASE_DEPARTURE_URI + stationId + "?footway=0" + "&bus=" + (bus ? "true" : "false") + "&ubahn=" + (ubahn ? "true" : "false") + "&sbahn=" + (sbahn ? "true" : "false") + "&tram=" + (tram ? "true" : "false"));
        }

        private static Uri BuildStationQueryUri(string query)
        {
            return new Uri(BASE_STATION_QUERY_URI + "?limit=10" + "&q=" + query);
        }

        private Departure ParseDeparture(JsonObject json)
        {
            int delay = json.ContainsKey(JSON_DEPARTURE_DELAY) ? (int)json.GetNamedNumber(JSON_DEPARTURE_DELAY) : 0;
            return new Departure
            {
                canceled = json.GetNamedBoolean(JSON_DEPARTURE_CANCELED),
                delay = delay,
                destination = json.GetNamedString(JSON_DEPARTURE_DESTINATION),
                infoMessage = "",
                label = json.GetNamedString(JSON_DEPARTURE_LABEL),
                platform = json.GetNamedString(JSON_DEPARTURE_PLATFORM),
                lineBackgroundColor = json.GetNamedString(JSON_DEPARTURE_LINE_BACKGROUND_COLOR),
                productType = ParseProduct(json.GetNamedString(JSON_DEPARTURE_PRODUCT)),
                departureTime = ParseDepartureTime(json.GetNamedNumber(JSON_DEPARTURE_TIME), delay)
            };
        }

        private Station ParseStation(JsonObject json)
        {
            return new Station
            {
                id = json.GetNamedString(JSON_STATION_ID),
                name = json.GetNamedString(JSON_STATION_NAME)
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

                case "REGIONAL_BUS":
                    return Product.REGIONAL_BUS;

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
        /// <param name="delay">The delay in minutes that will be added to the departure time.</param>
        /// <returns>A <see cref="DateTime"/> object representing the local departure time.</returns>
        private DateTime ParseDepartureTime(double departureTime, int delay)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(departureTime).AddMinutes(delay).ToLocalTime();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
