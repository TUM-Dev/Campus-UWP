using Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using TUMCampusAppAPI.TUMOnline.Exceptions;
using Windows.Data.Xml.Dom;

namespace TUMCampusAppAPI.TUMOnline
{
    public class TUMOnlineRequest
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private string addition;
        private List<string> parameterArguments;
        private List<string> parameters;
        private int validity;

        private static readonly string SERVICE_BASE_URL = "https://campus.tum.de/tumonline/wbservicesbasic.";

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public TUMOnlineRequest(TUMOnlineConst tumOC)
        {
            this.addition = tumOC.ToString();
            this.parameterArguments = new List<string>();
            this.parameters = new List<string>();
            this.validity = -1;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public void setValidity(int validity)
        {
            this.validity = validity;
        }

        public int getValidity()
        {
            return validity;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        [Obsolete("doRequestAsync() is deprecated, please use doRequestDocumentAsync() instead.")]
        public async Task<string> doRequestAsync()
        {
            Uri url = buildUrl();
            string result = null;
            if (validity > 0)
            {
                result = CacheManager.INSTANCE.isCached(url.ToString());
                if (result != null)
                {
                    return result;
                }
            }
            if (!DeviceInfo.isConnectedToInternet())
            {
                return null;
            }
            result = await NetUtils.downloadStringAsync(url);
            if (validity > 0)
            {
                if (result != null)
                {
                    cacheResult(url.ToString(), result);
                }
            }
            return result;
        }

        public async Task<XmlDocument> doRequestDocumentAsync()
        {
            Uri url = buildUrl();
            XmlDocument doc = null;
            if (validity > 0)
            {
                string result = CacheManager.INSTANCE.isCached(url.ToString());
                if (result != null)
                {
                    doc = new XmlDocument();
                    doc.LoadXml(result);
                    return doc;
                }
            }
            if (!DeviceInfo.isConnectedToInternet())
            {
                return null;
            }
            doc = await XmlDocument.LoadFromUriAsync(url);
            if (validity > 0)
            {
                if (doc != null)
                {
                    cacheResult(url.ToString(), doc.GetXml());
                }
            }

            if (doc != null && doc.SelectSingleNode("/error") != null)
            {
                string innerText = doc.SelectSingleNode("/error").InnerText;
                Logger.Warn("Thrown an error during a TUM Online request: " + innerText);
                if (innerText.Contains("Token"))
                {
                    throw new InvalidTokenTUMOnlineException(buildUrl().ToString(), innerText);
                }
                else
                {
                    throw new NoAccessTUMOnlineException(buildUrl().ToString(), innerText);
                }
            }

            return doc;
        }

        public void addParameter(string param, string arg)
        {
            this.parameters.Add(param);
            this.parameterArguments.Add(arg);

        }

        public void addToken()
        {
            addParameter(Consts.P_TOKEN, TumManager.getToken());
        }

        public override string ToString()
        {
            return buildUrl().ToString();
        }

        #endregion

        #region --Misc Methods (Private)--
        private void cacheResult(string url, string result)
        {
            CacheManager.INSTANCE.cache(new CacheTable(url, CacheManager.encodeString(result), validity, validity, CacheManager.CACHE_TYP_DATA));
        }

        private Uri buildUrl()
        {
            string s = SERVICE_BASE_URL + addition;
            for(int i = 0; i < parameters.Count; i++)
            {
                if(i == 0)
                {
                    s += "?";
                }
                else
                {
                    s += "&";
                }
                s += parameters[i] + "=" + parameterArguments[i];
            }
            return new Uri(s);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
