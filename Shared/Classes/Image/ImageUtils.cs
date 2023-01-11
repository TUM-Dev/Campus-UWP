using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Logging.Classes;
using SixLabors.ImageSharp;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Shared.Classes.Image
{
    public enum MediaType
    {
        None,
        Jpeg,
        Png
    }
    public static class ImageUtils
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public const string IANA_MEDIA_TYPE_PNG = "image/png";
        public const string IANA_MEDIA_TYPE_JPG = "image/jpg";
        public const string IANA_MEDIA_TYPE_JPEG = "image/jpeg";

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Converts the given data to a <see cref="SoftwareBitmap"/> and returns it.
        /// </summary>
        /// <param name="data">A valid <see cref="SoftwareBitmap"/> in binary representation.</param>
        /// <returns>The resulting <see cref="SoftwareBitmap"/> from the given <paramref name="data"/>.</returns>
        public static async Task<SoftwareBitmap> ToSoftwareBitmapImageAsync(byte[] data)
        {
            Debug.Assert(!(data is null));
            try
            {
                IRandomAccessStream stream = data.AsBuffer().AsStream().AsRandomAccessStream();
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                return await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to convert binary to SoftwareBitmap.", e);
                // Debug output for now while this leads to crashes:
                AppCenter.AppCenterCrashHelper.INSTANCE.TrackError(e, $"Converting bytes to a bitmap image failed in {nameof(ToSoftwareBitmapImageAsync)}.", new Dictionary<string, string> { { "null", (data is null).ToString() }, { "length", data is null ? "-1" : data.Length.ToString() } });
            }
            return null;
        }

        /// <summary>
        /// Converts the given Base64 string to a <see cref="BitmapImage"/> and returns it.
        /// </summary>
        /// <param name="base64">A valid <see cref="BitmapImage"/> in Base64 representation.</param>
        /// <returns>The resulting <see cref="BitmapImage"/> from the given <paramref name="base64"/>.</returns>
        public static async Task<SoftwareBitmap> ToSoftwareBitmapImageAsync(string base64)
        {
            Debug.Assert(!(base64 is null));
            return await ToSoftwareBitmapImageAsync(Convert.FromBase64String(base64));
        }

        /// <summary>
        /// Parses the given string as a IANA Media Type.
        /// </summary>
        /// <returns>Returns <see cref="MediaType.None"/> if the type is not known.</returns>
        public static MediaType ParseMediaType(string s)
        {
            switch (s)
            {
                case IANA_MEDIA_TYPE_PNG:
                    return MediaType.Png;

                case IANA_MEDIA_TYPE_JPEG:
                case IANA_MEDIA_TYPE_JPG:
                    return MediaType.Jpeg;

                default:
                    return MediaType.None;
            }
        }

        /// <summary>
        /// Downloads a WebP image from the given URL and returns it as a <see cref="WriteableBitmap"/> in the PNG format.
        /// </summary>
        /// <param name="url">Target URL for downloading a WebP image.</param>
        /// <returns><see cref="WriteableBitmap"/> in the PNG format.</returns>
        public static async Task<WriteableBitmap> DownloadWebPAsync(string url)
        {
            byte[] data = await new HttpClient().GetByteArrayAsync(url);
            return await DecodeWebPAsync(data);
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Decodes the given WebP data byte array and stores it as a PNG inside the returned <see cref="WriteableBitmap"/>.
        /// </summary>
        /// <param name="data">Byte array for a WebP image.</param>
        /// <returns><see cref="WriteableBitmap"/> in the PNG format.</returns>
        public static async Task<WriteableBitmap> DecodeWebPAsync(byte[] data)
        {
            SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(data, new SixLabors.ImageSharp.Formats.Webp.WebpDecoder());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await image.SaveAsPngAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                WriteableBitmap writeableBitmap = new WriteableBitmap(image.Width, image.Height);
                await writeableBitmap.SetSourceAsync(memoryStream.AsRandomAccessStream());
                return writeableBitmap;
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
