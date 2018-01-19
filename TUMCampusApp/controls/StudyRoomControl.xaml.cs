using System;
using TUMCampusAppAPI.DBTables;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TUMCampusApp.Controls
{
    public sealed partial class StudyRoomControl : UserControl
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private StudyRoomTable room;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 24/02/2017 Created [Fabian Sauter]
        /// </history>
        public StudyRoomControl(StudyRoomTable room)
        {
            this.room = room;
            this.InitializeComponent();
            this.name_tbx.Text = room.name;
            this.location_tbx.Text = room.code;
            if(room.occupied_till == null || room.occupied_till.CompareTo(DateTime.Now) <= 0)
            {
                main_grid.Background = new SolidColorBrush(Colors.DarkGreen);
                status_tbx.Text = "Free";
            }
            else
            {
                TimeSpan timeSpan = room.occupied_till.Subtract(DateTime.Now);
                string statusText = "Occupied for ";
                if (timeSpan.Hours > 0)
                {
                    if(timeSpan.Hours == 1)
                    {
                        statusText += timeSpan.Hours + " hours and ";
                    }
                    else
                    {
                        statusText += timeSpan.Hours + " hour and ";
                    }
                }
                if(timeSpan.Minutes == 1)
                {
                    statusText += timeSpan.Minutes + " minute, until: ";
                }
                else
                {
                    statusText += timeSpan.Minutes + " minutes, until: ";
                }
                statusText += room.occupied_till.ToString("dd.MM.yyyy HH:mm");
                status_tbx.Text = statusText;
            }
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


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
