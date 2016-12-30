using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes.tum;
using TUMCampusApp.classes.userData;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.classes.managers
{
    class TumManager : AbstractManager
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
        public static string getToken()
        {
            return (string)Utillities.getSetting(Const.ACCESS_TOKEN);
        }

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

        public async Task<bool> isTokenConfirmedAsync()
        {
            string token = getToken();
            if (token == null)
            {
                return false;
            }
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.TOKEN_CONFIRMED);
            req.addParameter(Const.P_TOKEN, token);
            string result = await req.doRequestAsync();

            string status = getNodeFromXML(result, "confirmed");
            if (status == null)
            {
                return false;
            }
            return bool.Parse(status);
        }

        public async Task<List<TUMUser>> getUser(string name)
        {
            //https://campus.tum.de/tumonline/wbservicesbasic.personenSuche?pToken=F7F0E6F2AA6EA4FB1BFC71FC0AC24ACD&pSuche=Fabian%20Sauter
            //https://campus.tum.de/tumonline/wbservicesbasic.personenDetails?pToken=F7F0E6F2AA6EA4FB1BFC71FC0AC24ACD&pIdentNr=-1844547
            TUMOnlineRequest req = new TUMOnlineRequest(TUMOnlineConst.PERSON_SEARCH);
            req.addToken();
            req.addParameter(Const.P_SEARCH, name);

            XmlDocument doc = await req.doRequestDocumentAsync();
            List<TUMUser> list = new List<TUMUser>();
            foreach (var element in doc.SelectNodes("/rowset/row"))
            {
                list.Add(new TUMUser(element));
            }

            return list;
        }

        public bool isTUMOnlineEnabled()
        {
            var res = Utillities.getSetting(Const.TUMO_DISABLED);
            if (res == null)
            {
                return false;
            }
            return (bool)res;
        }

        public void setTUMOnlineEnabled(bool enabled)
        {
            Utillities.setSetting(Const.TUMO_DISABLED, enabled);
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public override Task initManagerAsync()
        {
            return base.initManagerAsync();
        }

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
        private void analyseAndSaveToken(string result)
        {
            if (result != null && result != "" && result.Contains(TUMOnlineConst.SERVICE_REQEST_TOKEN_ANSWER_INACTIV))
            {
                string token = getNodeFromXML(result, "token");
                if (token == null)
                {
                    return;
                }
                Utillities.setSetting(Const.ACCESS_TOKEN, token);
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
