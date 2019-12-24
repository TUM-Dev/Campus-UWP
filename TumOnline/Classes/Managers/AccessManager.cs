using System.IO;
using System.Threading.Tasks;
using System.Xml;
using TumOnline.Classes.Exceptions;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace TumOnline.Classes.Managers
{
    public class AccessManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private const string ATTRIBUTE_TOKEN_NAME = "pTokenName";
        private const string ATTRIBUTE_USER_NAME = "pUsername";

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private static string GetTokenName()
        {
            EasClientDeviceInformation info = new EasClientDeviceInformation();
            return "TCA_UWP_" + info.FriendlyName;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public static async Task<string> RequestNewTokenAsync(string tumId)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.REQUEST_TOKEN);
            request.AddQuery(ATTRIBUTE_TOKEN_NAME, GetTokenName());
            request.AddQuery(ATTRIBUTE_USER_NAME, tumId);
            XmlDocument doc = await request.RequestDocumentAsync();
            return LoadToken(doc);
        }

        #endregion

        #region --Misc Methods (Private)--
        private static string LoadToken(XmlDocument doc)
        {
            if (!(doc is null))
            {
                XmlNode tNode = doc.SelectSingleNode("token");
                if (!(tNode is null))
                {
                    return tNode.InnerText;
                }
            }
            else
            {
                throw new InvalidTumOnlineResponseException("", "No TUMonline token found in response!", "null");
            }

            using (StringWriter stringWriter = new StringWriter())
            using (XmlWriter xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                throw new InvalidTumOnlineResponseException("", "No TUMonline token found in response!", stringWriter.GetStringBuilder().ToString());
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
