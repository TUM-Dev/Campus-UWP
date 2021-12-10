using System;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Storage.Classes.Models.External;
using UI_Context.Classes.Templates.Controls.StudyRooms;

namespace UI_Context.Classes.Context.Controls.StudyRooms
{
    public class StudyRoomControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly StudyRoomControlDataTemplate MODEL = new StudyRoomControlDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void UpdateView(StudyRoom room, DelegateCommand<object> command)
        {
            if (!(room is null))
            {
                MODEL.ATTRIBUTES.Replace(room.Attributes.Select(a =>
                {
                    MetadataItem item = new MetadataItem { Label = a.Name };
                    if (!string.IsNullOrEmpty(a.Details))
                    {
                        item.Command = command;
                        item.CommandParameter = a;
                    }
                    return item;
                }));
                if (room.Status == StudyRoomStatus.OCCUPIED || room.IsSoonOccupied())
                {
                    string occupiedInfo = "Occupied";
                    if (room.IsSoonOccupied())
                    {
                        if (room.BookedIn < 120 * 60)
                        {
                            occupiedInfo += $" in {room.BookedIn / 60} minutes";
                        }
                        else if (room.BookedIn / 3600 == 0)
                        {
                            occupiedInfo += $" in {room.BookedIn / 3600} hours";
                        }
                        else
                        {
                            occupiedInfo += $" in {room.BookedIn / 3600} hours and {room.BookedIn / 60 % 60} minutes";
                        }
                    }

                    if (room.BookedFor <= 60)
                    { }
                    else if (room.BookedFor < 120 * 60)
                    {
                        occupiedInfo += $" for {room.BookedFor / 60} minutes";
                    }
                    else if (room.BookedFor / 3600 == 0)
                    {
                        occupiedInfo += $" for {room.BookedFor / 3600} hours";
                    }
                    else
                    {
                        occupiedInfo += $" for {room.BookedFor / 3600} hours and {room.BookedFor / 60 % 60} minutes";
                    }

                    if (room.BookedUntil == DateTime.MinValue)
                    { }
                    else if (room.BookedUntil <= DateTime.Now)
                    {
                        occupiedInfo += $" until NOW";
                    }
                    else if (room.BookedUntil.Date == DateTime.Now.Date)
                    {
                        occupiedInfo += $" until {room.BookedUntil.ToString("HH:mm")}";
                    }
                    else
                    {
                        occupiedInfo += $" until {room.BookedUntil.ToString("d HH:mm")}";
                    }

                    if (string.IsNullOrEmpty(room.OccupiedBy))
                    {
                        occupiedInfo += '.';
                    }
                    else
                    {
                        occupiedInfo += $" by {room.OccupiedBy}.";
                    }
                    MODEL.OccupiedInfo = occupiedInfo;
                }
                else
                {
                    MODEL.OccupiedInfo = "";
                }
            }
            else
            {
                MODEL.ATTRIBUTES.Clear();
            }
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
