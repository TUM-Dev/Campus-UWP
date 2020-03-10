using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Logging.Classes;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using TumOnline.Classes.Exceptions;
using Windows.ApplicationModel;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace TumOnline.Classes
{
    public class TumOnlineRequest
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private const string SERVICE_BASE_URL = "https://campus.tum.de/tumonline/wbservicesbasic.";
        private readonly TumOnlineService SERVICE;
        private readonly Dictionary<string, string> QUERY = new Dictionary<string, string>();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TumOnlineRequest(TumOnlineService service)
        {
            SERVICE = service;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        private static string GetUserAgent()
        {
            return "TCA UWP v." + Package.Current.Id.Version.ToFormattedString();
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<XmlDocument> RequestDocumentAsync(bool checkCached = true)
        {
            Uri uri = BuildUri();
            string result = await RequestStringAsync(uri, checkCached).ConfigureAwait(false);
            XmlDocument doc;
            try
            {
                doc = new XmlDocument();
                doc.LoadXml(result);
            }
            catch (Exception e)
            {
                throw new MalformedXmlTumOnlineException(uri.ToString(), e.Message, result);
            }

            if (doc != null && doc.SelectSingleNode("/error") != null)
            {
                string innerText = doc.SelectSingleNode("/error").InnerText;
                Logger.Warn("Thrown an error during a TUM Online request: " + innerText);
                if (innerText.Contains("Token"))
                {
                    throw new InvalidTokenTumOnlineException(uri.ToString(), innerText);
                }
                else
                {
                    throw new NoAccessTumOnlineException(uri.ToString(), innerText);
                }
            }
            return doc;
        }

        public void AddQuery(string name, string attribute)
        {
            QUERY[name] = attribute;
        }

        #endregion

        #region --Misc Methods (Private)--
        private Uri BuildUri()
        {
            string query = string.Join("&", QUERY.Keys.Select(key => !string.IsNullOrWhiteSpace(QUERY[key]) ? (WebUtility.UrlEncode(key) + '=' + WebUtility.UrlEncode(QUERY[key])) : WebUtility.UrlEncode(key)));
            UriBuilder builder = new UriBuilder(SERVICE_BASE_URL + SERVICE.NAME)
            {
                Query = query
            };
            return builder.Uri;
        }

        private async Task<string> RequestStringAsync(Uri uri, bool checkCached)
        {
            Logger.Debug("[" + nameof(TumOnlineRequest) + "] Request: " + uri.ToString());
            if (checkCached)
            {
                // TODO check if the request has been cached
            }

            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                Logger.Warn("Unable to request string - no internet.");
                return null;
            }

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAppendWithoutValidation("User-Agent", GetUserAgent());
                HttpResponseMessage response = await client.GetAsync(uri);
                IHttpContent content = response.Content;
                IBuffer buffer = await content.ReadAsBufferAsync();
                using (DataReader dataReader = DataReader.FromBuffer(buffer))
                {
                    string result = dataReader.ReadString(buffer.Length);
                    if (SERVICE.VALIDITY != TumOnlineService.VALIDITY_NONE)
                    {
                        // TODO cache result
                    }
                    Logger.Debug("[" + nameof(TumOnlineRequest) + "] Response: " + result);
                    return result;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed to request string from: " + uri, e);
            }
            return null;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
