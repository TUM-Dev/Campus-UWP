using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Logging.Classes;
using Storage.Classes;
using TumOnline.Classes.Exceptions;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace TumOnline.Classes.Managers
{
    public static class AccessManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private const string ATTRIBUTE_TOKEN_NAME = "pTokenName";
        private const string ATTRIBUTE_TOKEN = "pToken";
        private const string ATTRIBUTE_USER_NAME = "pUsername";

        private static readonly Regex TUM_ID_REGEX = new Regex(@"[a-z]{2}[0-9]{2}[a-z]{3}");
        private static readonly Regex TOKEN_REGEX = new Regex(@"(\d|[A-Z]){32}");

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
        public static void AddToken(TumOnlineRequest request, TumOnlineCredentials credentials)
        {
            if (!credentials.IsValid())
            {
                throw new InvalidTokenTumOnlineException(request.GetRequestUrl(), "Please login first.");
            }
            request.AddQuery(ATTRIBUTE_TOKEN, credentials.TOKEN);
        }

        public static async Task<string> RequestNewTokenAsync(string tumId)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.REQUEST_TOKEN);
            request.AddQuery(ATTRIBUTE_TOKEN_NAME, GetTokenName());
            request.AddQuery(ATTRIBUTE_USER_NAME, tumId);
            XmlDocument doc = await request.RequestDocumentAsync();
            return LoadToken(doc);
        }

        public static async Task<bool> IsTokenActivated(string token)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.TOKEN_ACTIVATED);
            request.AddQuery(ATTRIBUTE_TOKEN, token);

            try
            {
                XmlDocument doc = await request.RequestDocumentAsync();
                XmlNode node = doc.SelectSingleNode("confirmed");
                return !(node is null) && bool.Parse(node.InnerText);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to check if token is activated!", e);
                return false;
            }
        }

        /// <summary>
        /// Checks if the given string represents a valid TUM ID.
        /// A TUM ID has to be lower case and be in the following pattern: ab12xyz
        /// </summary>
        /// <param name="s">The string that will be checked. Can be null.</param>
        /// <returns>True if the given string is a valid TUM ID.</returns>
        public static bool IsTumIdValid(string s)
        {
            return !(s is null) && TUM_ID_REGEX.IsMatch(s);
        }

        private static bool IsTokenValid(string token)
        {
            return !(token is null) && TOKEN_REGEX.IsMatch(token);
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
                    string token = tNode.InnerText;
                    if (IsTokenValid(token))
                    {
                        return token;
                    }
                    else
                    {
                        throw new InvalidTumOnlineResponseException("", "Invalid token!", token);
                    }
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
