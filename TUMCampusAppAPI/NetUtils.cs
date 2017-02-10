using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace TUMCampusAppAPI
{
    public class NetUtils
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Downloads and returns the result string from the given url.
        /// </summary>
        /// <param name="url">The url for downloading.</param>
        /// <returns>Returns the result string.</returns>
        public static async Task<string> downloadStringAsync(Uri url)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAppendWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.DefaultRequestHeaders.TryAppendWithoutValidation("Connection", "keep-alive");
                client.DefaultRequestHeaders.TryAppendWithoutValidation("Cookie", "TCAP=nr86sbfkrvv22b7i72gj0mq1lp5durn9dd53pm9p2v3rd6sko2ssjjk0gtvitam9ijv855bkt2de5k1gvf4rt3j6u03j92cuopkqe40");
                client.DefaultRequestHeaders.TryAppendWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36");
                HttpResponseMessage response = await client.GetAsync(url);
                //response.EnsureSuccessStatusCode();
                IHttpContent content = response.Content;
                IBuffer buffer = await content.ReadAsBufferAsync();
                using (DataReader dataReader = DataReader.FromBuffer(buffer))
                {
                    return dataReader.ReadString(buffer.Length);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return null;
        }

        /// <summary>
        /// Downloads and returns the result JsonObject from the given url.
        /// </summary>
        /// <param name="url">The url for downloading.</param>
        /// <returns>Returns the result JsonObject from the given url.</returns>
        public static async Task<JsonObject> downloadJsonObjectAsync(Uri url)
        {
            try
            {
                return JsonObject.Parse(await downloadStringAsync(url));
            }
            catch(Exception e)
            {
                Logger.Error("Unable to parse the downloaded string to a JsonObject", e);
            }
            return null;
        }

        /// <summary>
        /// Downloads and returns the result JsonArray from the given url.
        /// </summary>
        /// <param name="url">The url for downloading.</param>
        /// <returns>Returns the result JsonArray from the given url.</returns>
        public static async Task<JsonArray> downloadJsonArrayAsync(Uri url)
        {
            try
            {
                return JsonArray.Parse(await downloadStringAsync(url));
            }
            catch (Exception e)
            {
                Logger.Error("Unable to parse the downloaded string to a JsonArray", e);
            }
            return null;
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
