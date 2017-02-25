using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.StudyRooms;
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
        #region --Construktoren--
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
        public List<StudyRoom> getRooms(int groupID)
        {
            return dB.Query<StudyRoom>("SELECT * FROM StudyRoom WHERE group_id = ?", groupID);
        }

        /// <summary>
        /// Returns all room groups that are found in the db.
        /// </summary>
        public List<StudyRoomGroup> getRoomGroups()
        {
            return dB.Query<StudyRoomGroup>("SELECT * FROM StudyRoomGroup");
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
            dB.CreateTable<StudyRoom>();
            dB.CreateTable<StudyRoomGroup>();
        }

        /// <summary>
        /// Downloads the status of all study rooms and study room groups from an external source.
        /// </summary>
        public async Task downloadStudyRoomsAndGroups()
        {
            JsonObject jsonO = await NetUtils.downloadJsonObjectAsync(new Uri(STUDYROOM_URL));
            if(jsonO == null)
            {
                return;
            }

            List<StudyRoom> rooms = new List<StudyRoom>();
            foreach (JsonValue val in jsonO.GetNamedArray("raeume"))
            {
                rooms.Add(new StudyRoom(val.GetObject()));
            }

            List<StudyRoomGroup> roomGroups = new List<StudyRoomGroup>();
            foreach (JsonValue val in jsonO.GetNamedArray("gruppen"))
            {
                StudyRoomGroup g = new StudyRoomGroup(val.GetObject());
                foreach (JsonValue v in val.GetObject().GetNamedArray("raeume"))
                {
                    int nr = (int)v.GetNumber();
                    for (int i = 0; i < rooms.Count; i++)
                    {
                        if(rooms[i].id == nr)
                        {
                            rooms[i].group_id = g.id;
                        }
                    }
                }
                roomGroups.Add(g);
            }

            dB.DeleteAll<StudyRoom>();
            dB.InsertAll(rooms);
            dB.DeleteAll<StudyRoomGroup>();
            dB.InsertAll(roomGroups);
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
