using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes.cache;
using TUMCampusApp.classes.managers;
using TUMCampusApp.classes.userData;
using Windows.Data.Xml.Dom;

namespace TUMCampusApp.classes.tum
{
    class TUMOnlineRequest
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private string addition;
        private List<string> parameterArguments;
        private List<string> parameters;
        private int validity;

        private static readonly string SERVICE_BASE_URL = "https://campus.tum.de/tumonline/wbservicesbasic.";

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
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
        public async Task<string> doRequestAsync()
        {
            //Debug.WriteLine(buildUrl());
            Uri url = buildUrl();
            if(validity > 0)
            {
                string result = CacheManager.INSTANCE.isCached(url.ToString());
                if (result != null)
                {
                    return result;
                }
                else
                {
                    result = await NetUtils.downloadStringAsync(buildUrl());
                    CacheManager.INSTANCE.cache(new Cache(url.ToString(), CacheManager.encodeString(result), validity.ToString(), validity, CacheManager.CACHE_TYP_DATA));
                    return result;
                }
            }
            return await NetUtils.downloadStringAsync(url);
        }

        public async Task<XmlDocument> doRequestDocumentAsync()
        {
            Uri url = buildUrl();
            if (validity > 0)
            {
                string result = CacheManager.INSTANCE.isCached(url.ToString());
                if (result != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    return doc;
                }
                else
                {
                    XmlDocument doc = await XmlDocument.LoadFromUriAsync(url);
                    if (doc != null)
                    {
                        CacheManager.INSTANCE.cache(new Cache(url.ToString(), CacheManager.encodeString(doc.GetXml()), validity.ToString(), validity, CacheManager.CACHE_TYP_DATA));
                    }
                    return doc;
                }
            }
            return await XmlDocument.LoadFromUriAsync(buildUrl());
        }

        public void addParameter(string param, string arg)
        {
            this.parameters.Add(param);
            this.parameterArguments.Add(arg);

        }

        public void addToken()
        {
            addParameter(Const.P_TOKEN, TumManager.getToken());
        }

        public override string ToString()
        {
            return buildUrl().ToString();
        }

        #endregion

        #region --Misc Methods (Private)--
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
