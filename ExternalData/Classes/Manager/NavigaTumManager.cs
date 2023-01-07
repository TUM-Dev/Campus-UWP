using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using ExternalData.Classes.NavigaTum;
using Logging.Classes;
using Windows.Data.Json;

namespace ExternalData.Classes.Manager
{
    public class NavigaTumManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private const string BASE_SEARCH_URI = "https://nav.tum.de/api/search";

        private const string JSON_SECTIONS = "sections";
        private const string JSON_ENTITIES = "entries";
        private const string JSON_TIME_MS = "time_ms";
        private const string JSON_FACET = "facet";
        private const string JSON_ID = "id";
        private const string JSON_TYPE = "type";
        private const string JSON_NAME = "name";
        private const string JSON_SUBTEXT = "subtext";

        public static readonly NavigaTumManager INSTANCE = new NavigaTumManager();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


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

        #endregion

        #region --Misc Methods (Private)--
        private static Uri BuildSearchQueryUri(string query)
        {
            return new Uri(BASE_SEARCH_URI + "?q=" + query + "&limit_all=15&limit_rooms=15&limit_buildings=15");
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
                });
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
