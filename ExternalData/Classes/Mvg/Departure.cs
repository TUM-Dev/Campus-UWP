using System;

namespace ExternalData.Classes.Mvg
{
    public class Departure: IComparable<Departure>
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public DateTime departureTime;
        public Product productType;
        public string label;
        public string destination;
        public int delay;
        public bool canceled;
        public string lineBackgroundColor;
        public string platform;
        public string infoMessage;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public int CompareTo(Departure other)
        {
            return departureTime.AddMinutes(delay).CompareTo(other.departureTime.AddMinutes(other.delay));
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
