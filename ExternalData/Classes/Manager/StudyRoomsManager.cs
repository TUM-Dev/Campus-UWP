using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExternalData.Classes.Events;
using Logging.Classes;
using Shared.Classes;
using Storage.Classes.Contexts;
using Storage.Classes.Models.External;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace ExternalData.Classes.Manager
{
    public class StudyRoomsManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly Uri STUDY_ROOMS_URI = new Uri("https://www.devapp.it.tum.de/iris/ris_api.php?format=json");
        public static readonly TimeSpan MAX_TIME_IN_CACHE = TimeSpan.FromMinutes(5);

        public static readonly StudyRoomsManager INSTANCE = new StudyRoomsManager();
        private Task<IEnumerable<StudyRoomGroup>> updateTask;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<IEnumerable<StudyRoomGroup>> UpdateAsync(bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                return await updateTask.ConfAwaitFalse();
            }

            updateTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(STUDY_ROOMS_URI.ToString()))
                {
                    Logger.Info("No need to fetch study rooms. Cache is still valid.");
                    using (StudyRoomDbContext ctx = new StudyRoomDbContext())
                    {
                        return ctx.StudyRoomGroups.Include(ctx.GetIncludePaths(typeof(StudyRoomGroup))).ToList();
                    }
                }
                IEnumerable<StudyRoomGroup> groups = null;
                try
                {
                    groups = await DownloadRoomsAsync(force);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to request study rooms with:", e);
                }
                if (!(groups is null))
                {
                    using (StudyRoomDbContext ctx = new StudyRoomDbContext())
                    {
                        ctx.RemoveRange(ctx.StudyRoomGroups);
                        ctx.RemoveRange(ctx.StudyRooms);
                        ctx.RemoveRange(ctx.StudyRoomAttributes);
                        ctx.AddRange(groups);
                    }
                    CacheDbContext.UpdateCacheEntry(STUDY_ROOMS_URI.ToString(), DateTime.Now.Add(MAX_TIME_IN_CACHE));
                }
                else
                {
                    Logger.Info("Loading study rooms from DB.");
                    using (StudyRoomDbContext ctx = new StudyRoomDbContext())
                    {
                        return ctx.StudyRoomGroups.Include(ctx.GetIncludePaths(typeof(StudyRoomGroup))).ToList();
                    }
                }
                return groups;
            });
            return await updateTask.ConfAwaitFalse();
        }

        #endregion

        #region --Misc Methods (Private)--
        private static async Task<string> DownloadStringAsync(Uri uri)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            IHttpContent content = response.Content;
            IBuffer buffer = await content.ReadAsBufferAsync();
            using (DataReader dataReader = DataReader.FromBuffer(buffer))
            {
                string result = dataReader.ReadString(buffer.Length);
                return result;
            }
        }

        private async Task<List<StudyRoomGroup>> DownloadRoomsAsync(bool force)
        {
            JsonObject json;
            try
            {
                string data = await DownloadStringAsync(STUDY_ROOMS_URI);
                json = JsonObject.Parse(data);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to download and parse study rooms.", e);
                throw e;
            }
            return ParseStudyRooms(json);
        }

        private static List<StudyRoomGroup> ParseStudyRooms(JsonObject json)
        {
            if (json is null)
            {
                return null;
            }

            // Study Rooms:
            Dictionary<int, StudyRoom> rooms = new Dictionary<int, StudyRoom>();
            foreach (JsonValue val in json.GetNamedArray("raeume"))
            {
                try
                {
                    StudyRoom room = ParseStudyRoom(val.GetObject());
                    rooms[room.Id] = room;
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to parse study room.", e);
                }
            }

            // Room Groups:
            List<StudyRoomGroup> groups = new List<StudyRoomGroup>();
            foreach (JsonValue val in json.GetNamedArray("gruppen"))
            {
                try
                {
                    groups.Add(ParseStudyRoomGroup(val.GetObject(), rooms));
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to parse study room group.", e);
                }
            }

            if (rooms.Count > 0)
            {
                Logger.Warn($"Found {rooms.Count} rooms without a group.");
            }
            return groups;
        }

        private static DateTime ParseOccupiedDate(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return DateTime.MinValue;
            }
            return DateTime.ParseExact(s, "yyyy-MM-dd HH:mm:ss", new CultureInfo("de-DE"), DateTimeStyles.None);
        }

        private static List<StudyRoomAttribute> ParseStudyRoomAttributes(JsonArray arr)
        {
            List<StudyRoomAttribute> attributes = new List<StudyRoomAttribute>();
            foreach (JsonValue val in arr)
            {
                JsonObject json = val.GetObject();
                attributes.Add(new StudyRoomAttribute
                {
                    Name = json.GetNamedString("name"),
                    Details = json.GetNamedString("detail"),
                });
            }
            return attributes;
        }

        private static StudyRoom ParseStudyRoom(JsonObject json)
        {
            StudyRoomStatus status;
            switch (json.GetNamedString("status"))
            {
                case "frei":
                    status = StudyRoomStatus.FREE;
                    break;

                case "belegt":
                    status = StudyRoomStatus.OCCUPIED;
                    break;

                default:
                    status = StudyRoomStatus.UNKNOWN;
                    break;
            }

            return new StudyRoom
            {
                Id = (int)json.GetNamedNumber("raum_nr"),
                Code = json.GetNamedString("raum_code"),
                Number = json.GetNamedString("raum_nummer"),
                ArchitectNumber = json.GetNamedString("raum_nr_architekt"),
                BookedFor = (int)json.GetNamedNumber("belegung_fuer"),
                BookedIn = (int)json.GetNamedNumber("belegung_in"),
                BookedFrom = ParseOccupiedDate(json.GetNamedString("belegung_ab")),
                BookedUntil = ParseOccupiedDate(json.GetNamedString("belegung_bis")),
                OccupiedBy = json.GetNamedString("belegung_durch"),
                Name = json.GetNamedString("raum_name"),
                BuildingNumber = (int)json.GetNamedNumber("gebaeude_nr"),
                BuildingCode = json.GetNamedString("gebaeude_code"),
                BuildingName = json.GetNamedString("gebaeude_name"),
                Status = status,
                ResNumber = (int)json.GetNamedNumber("res_nr"),
                Attributes = ParseStudyRoomAttributes(json.GetNamedArray("attribute"))
            };
        }

        private static StudyRoomGroup ParseStudyRoomGroup(JsonObject json, Dictionary<int, StudyRoom> rooms)
        {
            List<StudyRoom> localRooms = new List<StudyRoom>();
            foreach (JsonValue val in json.GetNamedArray("raeume"))
            {
                int roomId = (int)val.GetNumber();
                if (!rooms.ContainsKey(roomId))
                {
                    Logger.Warn($"Failed to add room {roomId} to group. Room not found.");
                    continue;
                }
                localRooms.Add(rooms[roomId]);
                rooms.Remove(roomId);
            }
            return new StudyRoomGroup
            {
                Id = (int)json.GetNamedNumber("nr"),
                Sorting = (int)json.GetNamedNumber("sortierung"),
                Details = json.GetNamedString("detail"),
                Name = json.GetNamedString("name"),
                Rooms = localRooms
            };
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
