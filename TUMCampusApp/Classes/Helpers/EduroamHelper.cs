using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace TUMCampusApp.Classes.Helpers
{
    class EduroamHelper
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 21/03/2018 Created [Fabian Sauter]
        /// </history>
        public EduroamHelper()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        public static async Task connectAsync(string id, string password)
        {
            WiFiAdapter adapter = null;
            await adapter.ConnectAsync(null, WiFiReconnectionKind.Automatic, new Windows.Security.Credentials.PasswordCredential("", null, null), "eduroam");
        }

        public static async Task installCertificateAsync()
        {

        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
