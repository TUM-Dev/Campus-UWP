using System;
using System.IO;
using System.Text;
using Logging.Classes;

namespace Storage.Classes
{
    public static class TokenReader
    {
        /// <summary>
        /// The path to the Bing Maps API key.
        /// <para/>
        /// Online portal: https://www.bingmapsportal.com
        /// </summary>
        public const string MAP_TOKEN_PATH = @"Storage/Resources/MapToken.txt";

        /// <summary>
        /// Tries to load an access token from the given <paramref name="fileTokenPath"/> and returns it.
        /// In case an error occures an empty string will be returned.
        /// </summary>
        /// <param name="fileTokenPath">The path to the file containing the aaccess token.</param>
        /// <returns>The access token or an empty string in case an error occurred.</returns>
        public static string LoadTokenFromFile(string fileTokenPath)
        {
            try
            {
                return File.ReadAllText(fileTokenPath, Encoding.ASCII).Trim();
            }
            catch (FileNotFoundException e)
            {
                Logger.Error($"Failed to read token from '{e.FileName}' with:", e);
                Logger.Error("Please try creating a file containing the token.");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to read token from text file with:", e);
            }
            return "";
        }
    }
}
