using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TUMCampusAppAPI.Managers;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
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
        /// <summary>
        /// Returns the filename from the given url.
        /// </summary>
        /// <param name="url">The image url.</param>
        private static string getImageNameFromUrl(string url)
        {
            string name = url.Substring(url.LastIndexOf('/') + 1);
            name = name.Replace(".thumb.", "");
            return name.Replace(' ', '_');
        }

        /// <summary>
        /// Returns the Cache folder as a StorageFolder.
        /// If it does not exist, a new one will get created.
        /// </summary>
        public static async Task<StorageFolder> getCacheFolder()
        {
            return await ApplicationData.Current.LocalFolder.CreateFolderAsync("Cache", CreationCollisionOption.OpenIfExists);
        }

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
                IHttpContent content = response.Content;
                IBuffer buffer = await content.ReadAsBufferAsync();
                using (DataReader dataReader = DataReader.FromBuffer(buffer))
                {
                    string result = dataReader.ReadString(buffer.Length);
                    return result;
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

        public static async Task<BitmapImage> downloadImageAsync(Uri url)
        {
            string imagePath = CacheManager.INSTANCE.isCached(url.ToString());
            if(imagePath == null)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var buffer = await client.GetBufferAsync(url);
                        imagePath = await saveImageToFile(buffer, url.ToString());
                        CacheManager.INSTANCE.cache(new Caches.Cache(url.ToString(), CacheManager.encodeString(imagePath), CacheManager.VALIDITY_ONE_MONTH, CacheManager.VALIDITY_ONE_MONTH, CacheManager.CACHE_TYP_IMAGE));
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Unable to download image from: " + url.ToString(), e);
                    return null;
                }
            }

            return new BitmapImage(new Uri(imagePath));
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Saves the given buffer as a file.
        /// The name and type get specified by the given url (e.g. http://example.com/image.jpg will result in image.jpg).
        /// </summary>
        /// <param name="buffer">The buffer containing the file.</param>
        /// <param name="url">The download url.</param>
        /// <returns></returns>
        private static async Task<string> saveImageToFile(IBuffer buffer, string url)
        {
            StorageFolder cacheFolder = await getCacheFolder();
            if(cacheFolder == null)
            {
                Logger.Error("Unable to open or create Cache folder.");
                return null;
            }
            string name = getImageNameFromUrl(url);
            StorageFile imageFile = await cacheFolder.CreateFileAsync(getImageNameFromUrl(name), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBufferAsync(imageFile, buffer);
            return imageFile.Path;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
