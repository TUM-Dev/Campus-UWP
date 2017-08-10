using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TUMCampusAppAPI.TUMOnline;
using TUMCampusAppAPI.UserDatas;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.Managers
{
    public class TumManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static TumManager INSTANCE;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public TumManager()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the current TUMOnline token or null if none exists.
        /// </summary>
        /// <returns>Returns the current TUMOnline token or null if none exists.</returns>
        public static string getToken()
        {
            return (string)Util.getSetting(Const.ACCESS_TOKEN);
        }

        /// <summary>
        /// Returns a specific node from the given xml string.
        /// </summary>
        /// <param name="xml">The xml string.</param>
        /// <param name="node">The nodes name.</param>
        /// <returns>Returns a specific node from the given xml string.</returns>
        private string getNodeFromXML(string xml, string node)
        {
            if (xml == null)
            {
                return null;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc.SelectSingleNode(node).InnerText;
        }

        /// <summary>
        /// Checks if the current token is still active and enabled.
        /// </summary>
        /// <returns>Returns true if yes.</returns>
        public async Task<bool> isTokenConfirmedAsync()
        {
            string token = getToken();
            if (token == null)
            {
                return false;
            }
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.TOKEN_CONFIRMED);
            req.addParameter(Const.P_TOKEN, token);
            try
            {
                XmlDocument doc = await req.doRequestDocumentAsync();
                IXmlNode node = doc.SelectSingleNode("confirmed");
                if (node == null)
                {
                    return false;
                }
                return bool.Parse(node.InnerText);
            }
            catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Searches online for a given persons and returns them.
        /// </summary>
        /// <param name="name">The name of the persons.</param>
        /// <returns>The found persons</returns>
        public async Task<List<TUMUser>> getUser(string name)
        {
            //https://campus.tum.de/tumonline/wbservicesbasic.personenSuche?pToken=F7F0E6F2AA6EA4FB1BFC71FC0AC24ACD&pSuche=Fabian%20Sauter
            //https://campus.tum.de/tumonline/wbservicesbasic.personenDetails?pToken=F7F0E6F2AA6EA4FB1BFC71FC0AC24ACD&pIdentNr=-1844547
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.PERSON_SEARCH);
            req.addToken();
            req.setValidity(CacheManager.VALIDITY_FIFE_DAYS);
            req.addParameter(Const.P_SEARCH, name);

            XmlDocument doc = await req.doRequestDocumentAsync();
            List<TUMUser> list = new List<TUMUser>();
            foreach (var element in doc.SelectNodes("/rowset/row"))
            {
                list.Add(new TUMUser(element));
            }

            return list;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async override Task InitManagerAsync()
        {
        }

        /// <summary>
        /// Requests and returns a new token.
        /// </summary>
        /// <param name="userId">The id of the user in form of:xx00xxx</param>
        /// <returns>The requested token.</returns>
        public async Task<string> reqestNewTokenAsync(string userId)
        {
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.REQUEST_TOKEN);
            req.addParameter(Const.P_TOKEN_NAME, "TUM UWP App " + DeviceInfo.INSTANCE.Name);
            req.addParameter(Const.P_USER_NAME, userId);
            string result = await req.doRequestAsync();
            analyseAndSaveToken(result);
            return result;
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Checks and saves the given token.
        /// </summary>
        /// <param name="result">The xml document that contains the token.</param>
        private void analyseAndSaveToken(string result)
        {
            if (result != null && result != "" && result.Contains(TUMOnlineConst.SERVICE_REQEST_TOKEN_ANSWER_INACTIV))
            {
                string token = getNodeFromXML(result, "token");
                if (token == null)
                {
                    return;
                }
                saveToken(token);
            }
        }

        /// <summary>
        /// Saves the given token.
        /// </summary>
        /// <param name="token">The token that should get saved.</param>
        public void saveToken(string token)
        {
            Util.setSetting(Const.ACCESS_TOKEN, token);
        }

        /// <summary>
        /// Checks if the given token is in a valid format.
        /// Does not check if the token is still activated.
        /// </summary>
        /// <param name="token">The token that should get checked.</param>
        /// <returns>Whether the token is valid.</returns>
        public bool isTokenValid(String token)
        {
            Regex r = new Regex(@"(\d|[A-Z]){32}");
            return r.IsMatch(token) && token.Length == 32;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
