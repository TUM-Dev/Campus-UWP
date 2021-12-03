using System;
using System.Collections.Generic;
using Logging.Classes;
using Windows.Security.Credentials;

namespace Storage.Classes
{
    public static class Vault
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private const string VAULT_NAME = "TUM_ONLINE_VAULT";
        private static readonly PasswordVault PASSWORD_VAULT = new PasswordVault();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the corresponding PasswordCredential object for the given TUM ID.
        /// Will return null if an error occurs or none exists.
        /// </summary>
        /// <param name="tumId">The TUM ID you want to retrieve the PasswordCredential for.</param>
        private static PasswordCredential GetCredentials(string tumId)
        {
            try
            {
                return PASSWORD_VAULT.Retrieve(VAULT_NAME, tumId);
            }
            catch (Exception) { }

            return null;
        }

        /// <summary>
        /// Returns all PasswordCredential objects stored in the vault.
        /// </summary>
        /// <returns>A read only list of PasswordCredential objects.</returns>
        public static IReadOnlyList<PasswordCredential> GetAll()
        {
            try
            {
                return PASSWORD_VAULT.RetrieveAll();
            }
            catch (Exception)
            {
                return new List<PasswordCredential>();
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Returns the TumOnlineCredentials object for the given TUM ID.
        /// If no password got found, an empty string will get set as the TOKEN property.
        /// </summary>
        /// <param name="tumId">The TUM ID, the token should get loaded for.</param>
        public static TumOnlineCredentials LoadCredentials(string tumId)
        {
            PasswordCredential passwordCredential = GetCredentials(tumId);
            if (passwordCredential is null)
            {
                Logger.Warn("No token found for: " + (string.IsNullOrEmpty(tumId) ? "Not setup" : tumId));
                return new TumOnlineCredentials(tumId);
            }
            passwordCredential.RetrievePassword();
            return new TumOnlineCredentials(tumId, passwordCredential.Password);
        }

        /// <summary>
        /// Creates a secure password vault for the given credentials and stores the token in it.
        /// </summary>
        /// <param name="credentials">The TumOnlineCredentials a password vault should get created for.</param>
        public static void StoreCredentials(TumOnlineCredentials credentials)
        {
            // Delete existing password vaults:
            DeleteToken(credentials.TUM_ID);

            //removeAll();

            // Store the new password:
            if (!string.IsNullOrEmpty(credentials.TOKEN))
            {
                PASSWORD_VAULT.Add(new PasswordCredential(VAULT_NAME, credentials.TUM_ID, credentials.TOKEN));
            }
        }

        /// <summary>
        /// Deletes the password vault for the given TUM ID, if one exists.
        /// </summary>
        /// <param name="tumId">The TUM ID for the corresponding vault that should get deleted.</param>
        public static void DeleteToken(string tumId)
        {
            PasswordCredential passwordCredential = GetCredentials(tumId);
            DeleteToken(passwordCredential);
        }

        /// <summary>
        /// Deletes the given password vault.
        /// </summary>
        /// <param name="passwordCredential">The PasswordCredential that should get deleted.</param>
        public static void DeleteToken(PasswordCredential passwordCredential)
        {
            if (passwordCredential != null)
            {
                PASSWORD_VAULT.Remove(passwordCredential);
            }
        }

        /// <summary>
        /// Deletes all vaults.
        /// </summary>
        public static void DeleteAllVaults()
        {
            foreach (PasswordCredential item in PASSWORD_VAULT.RetrieveAll())
            {
                PASSWORD_VAULT.Remove(item);
            }
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
