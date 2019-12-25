using System;

namespace TumOnline.Classes
{
    public class TumOnlineService
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        /// <summary>
        /// This services will not be cached.
        /// </summary>
        public static readonly TimeSpan VALIDITY_NONE = TimeSpan.Zero;
        public static readonly TimeSpan VALIDITY_ONE_DAY = TimeSpan.FromDays(1);
        public static readonly TimeSpan VALIDITY_TWO_DAYS = TimeSpan.FromDays(2);
        public static readonly TimeSpan VALIDITY_FIVE_DAYS = TimeSpan.FromDays(5);
        public static readonly TimeSpan VALIDITY_TEN_DAYS = TimeSpan.FromDays(10);

        // A Set of predefined services for TUMonline with their validity:
        public static readonly TumOnlineService REQUEST_TOKEN = new TumOnlineService("requestToken", VALIDITY_NONE);
        public static readonly TumOnlineService TOKEN_ACTIVATED = new TumOnlineService("isTokenConfirmed", VALIDITY_NONE);

        public static readonly TumOnlineService CALENDAR = new TumOnlineService("kalender", VALIDITY_FIVE_DAYS);
        public static readonly TumOnlineService TUITION_FEE_STATUS = new TumOnlineService("studienbeitragsstatus", VALIDITY_TWO_DAYS);
        public static readonly TumOnlineService LECTURES_PERSONAL = new TumOnlineService("veranstaltungenEigene", VALIDITY_FIVE_DAYS);
        public static readonly TumOnlineService LECTURES_DETAILS = new TumOnlineService("veranstaltungenDetails", VALIDITY_TEN_DAYS);
        public static readonly TumOnlineService LECTURES_APPOINTMENTS = new TumOnlineService("veranstaltungenTermine", VALIDITY_TEN_DAYS);
        public static readonly TumOnlineService LECTURES_SEARCH = new TumOnlineService("veranstaltungenSuche", VALIDITY_NONE);
        public static readonly TumOnlineService ORG_TREE = new TumOnlineService("orgBaum", VALIDITY_TEN_DAYS);
        public static readonly TumOnlineService ORG_DETAILS = new TumOnlineService("orgDetails", VALIDITY_TEN_DAYS);
        public static readonly TumOnlineService PERSON_DETAILS = new TumOnlineService("personenDetails", VALIDITY_FIVE_DAYS);
        public static readonly TumOnlineService PERSON_SEARCH = new TumOnlineService("personenSuche", VALIDITY_NONE);
        public static readonly TumOnlineService GRADES = new TumOnlineService("noten", VALIDITY_FIVE_DAYS);

        public readonly string NAME;
        public readonly TimeSpan VALIDITY;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumOnlineService(string name, TimeSpan validity)
        {
            NAME = name;
            VALIDITY = validity;
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
