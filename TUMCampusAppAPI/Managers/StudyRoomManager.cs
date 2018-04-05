using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.DBTables;
using Windows.Data.Json;

namespace TUMCampusAppAPI.Managers
{
    public class StudyRoomManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static StudyRoomManager INSTANCE;
        public static readonly string STUDYROOM_URL = "https://www.devapp.it.tum.de/iris/ris_api.php?format=json";

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 24/02/2017 Created [Fabian Sauter]
        /// </history>
        public StudyRoomManager()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns all rooms that are contained in the db and match the given groupID.
        /// </summary>
        /// <param name="groupID">Specifies a group of rooms.</param>
        public List<StudyRoomTable> getRooms(int groupID)
        {
            waitForSyncToFinish();
            return dB.Query<StudyRoomTable>(true, "SELECT * FROM StudyRoomTable WHERE group_id = ?", groupID);
        }

        /// <summary>
        /// Returns all room groups that are found in the db.
        /// </summary>
        public List<StudyRoomGroupTable> getRoomGroups()
        {
            waitForSyncToFinish();
            return dB.Query<StudyRoomGroupTable>(true, "SELECT * FROM StudyRoomGroupTable");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<StudyRoomTable>();
            dB.CreateTable<StudyRoomGroupTable>();
        }

        /// <summary>
        /// Downloads the status of all study rooms and study room groups from an external source.
        /// </summary>
        /// <returns>Returns the syncing task or null if did not sync.</returns>
        public Task downloadStudyRoomsAndGroups()
        {
            waitForSyncToFinish();
            REFRESHING_TASK_SEMA.Wait();
            refreshingTask = Task.Run(async () =>
            {
                JsonObject jsonO = await NetUtils.downloadJsonObjectAsync(new Uri(STUDYROOM_URL));
                if (jsonO == null)
                {
                    return;
                }

                List<StudyRoomTable> rooms = new List<StudyRoomTable>();
                foreach (JsonValue val in jsonO.GetNamedArray("raeume"))
                {
                    rooms.Add(new StudyRoomTable(val.GetObject()));
                }

                List<StudyRoomGroupTable> roomGroups = new List<StudyRoomGroupTable>();
                foreach (JsonValue val in jsonO.GetNamedArray("gruppen"))
                {
                    StudyRoomGroupTable g = new StudyRoomGroupTable(val.GetObject());
                    foreach (JsonValue v in val.GetObject().GetNamedArray("raeume"))
                    {
                        int nr = (int)v.GetNumber();
                        for (int i = 0; i < rooms.Count; i++)
                        {
                            if (rooms[i].id == nr)
                            {
                                rooms[i].group_id = g.id;
                            }
                        }
                    }
                    roomGroups.Add(g);
                }

                dB.DeleteAll<StudyRoomTable>();
                dB.InsertAll(rooms);
                dB.DeleteAll<StudyRoomGroupTable>();
                dB.InsertAll(roomGroups);
            });
            REFRESHING_TASK_SEMA.Release();

            return refreshingTask;
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
