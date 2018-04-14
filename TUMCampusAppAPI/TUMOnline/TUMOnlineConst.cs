using System;

namespace TUMCampusAppAPI.TUMOnline
{
    public class TUMOnlineConst
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly TUMOnlineConst REQUEST_TOKEN = new TUMOnlineConst("requestToken", Consts.VALIDITY_DO_NOT_CACHE);
        public static readonly TUMOnlineConst TOKEN_CONFIRMED = new TUMOnlineConst("isTokenConfirmed", Consts.VALIDITY_DO_NOT_CACHE);

        public static readonly TUMOnlineConst CALENDAR = new TUMOnlineConst("kalender", Consts.VALIDITY_FIFE_DAYS);
        public static readonly TUMOnlineConst TUITION_FEE_STATUS = new TUMOnlineConst("studienbeitragsstatus", Consts.VALIDITY_TWO_DAYS);
        public static readonly TUMOnlineConst LECTURES_PERSONAL = new TUMOnlineConst("veranstaltungenEigene", Consts.VALIDITY_FIFE_DAYS);
        public static readonly TUMOnlineConst LECTURES_DETAILS = new TUMOnlineConst("veranstaltungenDetails", Consts.VALIDITY_TEN_DAYS);
        public static readonly TUMOnlineConst LECTURES_APPOINTMENTS = new TUMOnlineConst("veranstaltungenTermine", Consts.VALIDITY_TEN_DAYS);
        public static readonly TUMOnlineConst LECTURES_SEARCH = new TUMOnlineConst("veranstaltungenSuche", Consts.VALIDITY_DO_NOT_CACHE);
        public static readonly TUMOnlineConst ORG_TREE = new TUMOnlineConst("orgBaum", Consts.VALIDITY_ONE_MONTH);
        public static readonly TUMOnlineConst ORG_DETAILS = new TUMOnlineConst("orgDetails", Consts.VALIDITY_ONE_MONTH);
        public static readonly TUMOnlineConst PERSON_DETAILS = new TUMOnlineConst("personenDetails", Consts.VALIDITY_FIFE_DAYS);
        public static readonly TUMOnlineConst PERSON_SEARCH = new TUMOnlineConst("personenSuche", Consts.VALIDITY_DO_NOT_CACHE);
        public static readonly TUMOnlineConst EXAMS = new TUMOnlineConst("noten", Consts.VALIDITY_TEN_DAYS);

        private readonly String webservice;
        private readonly int validity;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        private TUMOnlineConst(string webservice, int validity)
        {
            this.webservice = webservice;
            this.validity = validity;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public int getValidity()
        {
            return validity;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override string ToString()
        {
            return webservice;
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
