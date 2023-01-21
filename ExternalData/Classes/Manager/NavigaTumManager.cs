using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using ExternalData.Classes.NavigaTum;
using Logging.Classes;
using Windows.Data.Json;
using Windows.Devices.Geolocation;

namespace ExternalData.Classes.Manager
{
    public class NavigaTumManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private const string BASE_SEARCH_URI = "https://nav.tum.de/api/search";
        private const string BASE_GET_URI = "https://nav.tum.de/api/get";
        private const string BASE_IMAGE_URI = "https://nav.tum.de/cdn";
        private const string BASE_MAP_URI = "https://nav.tum.de/cdn/maps";

        private const string JSON_SECTIONS = "sections";
        private const string JSON_ENTITIES = "entries";
        private const string JSON_TIME_MS = "time_ms";
        private const string JSON_FACET = "facet";
        private const string JSON_ID = "id";
        private const string JSON_NAME = "name";
        private const string JSON_SUBTEXT = "subtext";
        private const string JSON_SUBTEXT_BOLD = "subtext_bold";
        private const string JSON_ARCH_NAME = "arch_name";
        private const string JSON_PROPS = "props";
        private const string JSON_COMPUTED = "computed";
        private const string JSON_COORDS = "coords";
        private const string JSON_LAT = "lat";
        private const string JSON_LON = "lon";
        private const string JSON_TEXT = "text";
        private const string JSON_TYPE = "type";
        private const string JSON_TYPE_COMMON_NAME = "type_common_name";
        private const string JSON_IMGS = "imgs";
        private const string JSON_AUTHOR = "author";
        private const string JSON_SOURCE = "source";
        private const string JSON_LICENSE = "license";
        private const string JSON_URL = "url";
        private const string JSON_MAPS = "maps";
        private const string JSON_ROOMFINDER = "roomfinder";
        private const string JSON_AVAILABLE = "available";
        private const string JSON_DEFAULT = "default";
        private const string JSON_X = "x";
        private const string JSON_Y = "y";

        public const string PRE_HIGHLIGHT = "/u0019";
        public const string POST_HIGHLIGHT = "/u0017";

        public const string IMAGE_TYPE = "sm";

        public const string MAP_SOURCE = "roomfinder";

        public static readonly NavigaTumManager INSTANCE = new NavigaTumManager();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public static string GetImageUrl(string imageName)
        {
            return $"{BASE_IMAGE_URI}/{IMAGE_TYPE}/{imageName}";
        }

        public static string GetMapUrl(string mapId)
        {
            return $"{BASE_MAP_URI}/{MAP_SOURCE}/{mapId}.webp";
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<List<AbstractSearchResultItem>> FindRoomsAsync(string query)
        {
            Logger.Info($"NavigaTUM searching for '{query}' ...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(BuildSearchQueryUri(query));
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error($"NavigaTUM search failed for '{query}'.", e);
                    return new List<AbstractSearchResultItem>();
                }
            }

            JsonObject json;
            try
            {
                json = JsonObject.Parse(jsonString);
                List<AbstractSearchResultItem> results = ParseSearchResults(json);
                Logger.Info($"Found {results.Count} search results in {json.GetNamedNumber(JSON_TIME_MS)}ms.");
                return results;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error($"Failed to parse NavigaTUM results for '{query}'.", e);
                return new List<AbstractSearchResultItem>();
            }
        }

        public async Task<Location> GetLocationInfoAsync(string id)
        {
            Debug.Assert(!(id is null));

            Logger.Info($"NavigaTUM querying location info for '{id}' ...");
            string jsonString;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsonString = await wc.DownloadStringTaskAsync(BuildLocationInfoQueryUri(id));
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error($"NavigaTUM querying location failed for '{id}'.", e);
                    return null;
                }
            }

            JsonObject json;
            try
            {
                json = JsonObject.Parse(jsonString);
                Location location = ParseLocationResult(json);
                if (location is null)
                {
                    Logger.Error($"Failed to parse NavigaTUM location query result for '{id}'. No valid location found in response JSON: {jsonString}");
                }
                else
                {
                    Logger.Info($"Found valid NavigaTUM location for: {id}");
                }
                return location;
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error($"Failed to parse NavigaTUM location query result for '{id}'.", e);
                return null;
            }
        }

        #endregion

        #region --Misc Methods (Private)--
        private static Uri BuildSearchQueryUri(string query)
        {
            return new Uri($"{BASE_SEARCH_URI}?q={query}&limit_all=15&limit_rooms=7&limit_buildings=7&pre_highlight={PRE_HIGHLIGHT}&post_highlight={POST_HIGHLIGHT}");
        }

        private static Uri BuildLocationInfoQueryUri(string id)
        {
            return new Uri($"{BASE_GET_URI}/{id}?lang=en");
        }

        private static List<AbstractSearchResultItem> ParseSearchResults(JsonObject json)
        {
            JsonArray sectionsArr = json.GetObject().GetNamedArray(JSON_SECTIONS);
            List<AbstractSearchResultItem> results = new List<AbstractSearchResultItem>();

            foreach (IJsonValue sectionItem in sectionsArr)
            {
                JsonArray entitiesArr = sectionItem.GetObject().GetNamedArray(JSON_ENTITIES);
                string facet = sectionItem.GetObject().GetNamedString(JSON_FACET);

                switch (facet)
                {
                    case "sites_buildings":
                        ParseBuildings(entitiesArr, results);
                        break;

                    case "rooms":
                        ParseRooms(entitiesArr, results);
                        break;

                    default:
                        Logger.Warn($"Unknown NavigaTUM facet type: {facet}");
                        break;
                }
            }
            return results;
        }

        private static void ParseBuildings(JsonArray entitiesArr, List<AbstractSearchResultItem> results)
        {
            foreach (IJsonValue entityItem in entitiesArr)
            {
                results.Add(new BuildingSearchResultItem
                {
                    id = entityItem.GetObject().GetNamedString(JSON_ID),
                    name = entityItem.GetObject().GetNamedString(JSON_NAME),
                    subtext = entityItem.GetObject().GetNamedString(JSON_SUBTEXT),
                });
            }
        }

        private static void ParseRooms(JsonArray entitiesArr, List<AbstractSearchResultItem> results)
        {
            foreach (IJsonValue entityItem in entitiesArr)
            {
                results.Add(new RoomSearchResultItem
                {
                    id = entityItem.GetObject().GetNamedString(JSON_ID),
                    name = entityItem.GetObject().GetNamedString(JSON_NAME),
                    subtext = entityItem.GetObject().GetNamedString(JSON_SUBTEXT),
                    subtextBold = entityItem.GetObject().GetNamedString(JSON_SUBTEXT_BOLD),
                });
            }
        }

        private static Location ParseLocationResult(JsonObject json)
        {
            // Position:
            JsonObject coordsJson = json.GetNamedObject(JSON_COORDS);
            Geopoint pos = new Geopoint(new BasicGeoposition
            {
                Latitude = coordsJson.GetNamedNumber(JSON_LAT),
                Longitude = coordsJson.GetNamedNumber(JSON_LON),
            });

            // Properties:
            List<LocationProperty> properties = new List<LocationProperty>();
            if (json.ContainsKey(JSON_PROPS))
            {
                JsonObject propsJson = json.GetNamedObject(JSON_PROPS);
                JsonArray propsComputedArr = propsJson.GetNamedArray(JSON_COMPUTED);
                foreach (IJsonValue prop in propsComputedArr)
                {
                    JsonObject propJson = prop.GetObject();
                    if (propJson.ContainsKey(JSON_NAME) && propJson.ContainsKey(JSON_TEXT))
                    {
                        properties.Add(new LocationProperty
                        {
                            name = propJson.GetNamedString(JSON_NAME),
                            text = propJson.GetNamedString(JSON_TEXT)
                        });
                    }
                }
            }

            // Images:
            List<LocationImage> images = new List<LocationImage>();
            if (json.ContainsKey(JSON_IMGS))
            {
                JsonArray imagesJsonArr = json.GetNamedArray(JSON_IMGS);
                foreach (IJsonValue image in imagesJsonArr)
                {
                    JsonObject imageJson = image.GetObject();
                    string name = imageJson.GetNamedString(JSON_NAME);
                    images.Add(new LocationImage
                    {
                        name = name,
                        url = GetImageUrl(name),
                        authorName = imageJson.GetNamedObject(JSON_AUTHOR).GetNamedString(JSON_TEXT),
                        authorUrl = TryGetString(imageJson.GetNamedObject(JSON_AUTHOR), JSON_URL),
                        sourceName = imageJson.GetNamedObject(JSON_SOURCE).GetNamedString(JSON_TEXT),
                        sourceUrl = TryGetString(imageJson.GetNamedObject(JSON_SOURCE), JSON_URL),
                        licenseName = imageJson.GetNamedObject(JSON_LICENSE).GetNamedString(JSON_TEXT),
                        licenseUrl = TryGetString(imageJson.GetNamedObject(JSON_LICENSE), JSON_URL),
                    });
                }
            }

            // Maps:
            string defaultMap = null;
            List<LocationMap> maps = new List<LocationMap>();
            JsonObject mapsJson = json.GetNamedObject(JSON_MAPS);
            if (mapsJson.ContainsKey(JSON_ROOMFINDER))
            {
                JsonObject roomfinderJson = mapsJson.GetNamedObject(JSON_ROOMFINDER);
                defaultMap = roomfinderJson.GetNamedString(JSON_DEFAULT);
                Debug.Assert(!string.IsNullOrEmpty(defaultMap));

                JsonArray availableJsonArr = roomfinderJson.GetNamedArray(JSON_AVAILABLE);
                foreach (IJsonValue map in availableJsonArr)
                {
                    JsonObject mapJson = map.GetObject();
                    string id = mapJson.GetNamedString(JSON_ID);
                    maps.Add(new LocationMap
                    {
                        id = id,
                        name = mapJson.GetNamedString(JSON_NAME),
                        url = GetMapUrl(id),
                        x = mapJson.GetNamedNumber(JSON_X),
                        y = mapJson.GetNamedNumber(JSON_Y)
                    });
                }
            }

            // Combine:
            return new Location
            {
                id = json.GetNamedString(JSON_ID),
                archName = TryGetString(json, JSON_ARCH_NAME),
                name = json.GetNamedString(JSON_NAME),
                type = json.GetNamedString(JSON_TYPE),
                typeCommonName = json.GetNamedString(JSON_TYPE_COMMON_NAME),
                pos = pos,
                properties = properties,
                images = images,
                defaultMap = defaultMap,
                maps = maps
            };
        }

        private static string TryGetString(JsonObject json, string key)
        {
            if (json.ContainsKey(key) && json.TryGetValue(key, out IJsonValue value) && value.ValueType == JsonValueType.String)
            {
                return value.GetString();
            }
            return null;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
