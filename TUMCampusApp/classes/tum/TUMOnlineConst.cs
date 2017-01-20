using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes.managers;

namespace TUMCampusApp.classes.tum
{
    class TUMOnlineConst
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly TUMOnlineConst REQUEST_TOKEN = new TUMOnlineConst("requestToken", CacheManager.VALIDITY_DO_NOT_CACHE);
        public static readonly TUMOnlineConst TOKEN_CONFIRMED = new TUMOnlineConst("isTokenConfirmed", CacheManager.VALIDITY_DO_NOT_CACHE);

        public static readonly TUMOnlineConst CALENDAR = new TUMOnlineConst("kalender", CacheManager.VALIDITY_FIFE_DAYS);
        public static readonly TUMOnlineConst TUITION_FEE_STATUS = new TUMOnlineConst("studienbeitragsstatus", CacheManager.VALIDITY_TWO_DAYS);
        public static readonly TUMOnlineConst LECTURES_PERSONAL = new TUMOnlineConst("veranstaltungenEigene", CacheManager.VALIDITY_FIFE_DAYS);
        public static readonly TUMOnlineConst LECTURES_DETAILS = new TUMOnlineConst("veranstaltungenDetails", CacheManager.VALIDITY_TEN_DAYS);
        public static readonly TUMOnlineConst LECTURES_APPOINTMENTS = new TUMOnlineConst("veranstaltungenTermine", CacheManager.VALIDITY_TEN_DAYS);
        public static readonly TUMOnlineConst LECTURES_SEARCH = new TUMOnlineConst("veranstaltungenSuche", CacheManager.VALIDITY_DO_NOT_CACHE);
        public static readonly TUMOnlineConst ORG_TREE = new TUMOnlineConst("orgBaum", CacheManager.VALIDITY_ONE_MONTH);
        public static readonly TUMOnlineConst ORG_DETAILS = new TUMOnlineConst("orgDetails", CacheManager.VALIDITY_ONE_MONTH);
        public static readonly TUMOnlineConst PERSON_DETAILS = new TUMOnlineConst("personenDetails", CacheManager.VALIDITY_FIFE_DAYS);
        public static readonly TUMOnlineConst PERSON_SEARCH = new TUMOnlineConst("personenSuche", CacheManager.VALIDITY_DO_NOT_CACHE);
        public static readonly TUMOnlineConst EXAMS = new TUMOnlineConst("noten", CacheManager.VALIDITY_TEN_DAYS);

        public static readonly TUMOnlineConst IDENTITY = new TUMOnlineConst("id", CacheManager.VALIDITY_DO_NOT_CACHE);
        public static readonly TUMOnlineConst SECRET_UPLOAD = new TUMOnlineConst("secretUpload", CacheManager.VALIDITY_DO_NOT_CACHE);

        public static readonly string SERVICE_REQEST_TOKEN_ANSWER_INACTIV = "Token ist inaktiv, muss über TUMonline oder Email aktiviert werden!";

        private readonly String webservice;
        private readonly int validity;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
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
