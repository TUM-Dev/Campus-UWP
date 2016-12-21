using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using TUMCampusApp.Pages;
using Windows.Storage;
using Windows.Storage.Pickers;
using static TUMCampusApp.classes.Utillities;

namespace TUMCampusApp.classes.Data
{
    class DataStorage
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static DataStorage INSTANCE = new DataStorage();

        public UserData userData;
        public SettingsData settingsData;

        public bool dataInitiallyLoaded;
        private MainPage mainPage;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        private DataStorage()
        {
            this.userData = null;
            this.settingsData = null;

            this.dataInitiallyLoaded = false;
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        public void setMainPage(MainPage page)
        {
            this.mainPage = page;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        #region --User Data--
        /// <summary>
        /// Saves the user data to disk
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        private async Task saveUserDataTaskAsync()
        {
            if (userData == null)
            {
                return;
            }

            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserData));
                using (Stream stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(UserData.JSON_FILE_NAME, CreationCollisionOption.ReplaceExisting))
                {
                    serializer.WriteObject(stream, userData);
                }
            }
            catch (Exception)
            {

            }
        }

        public async void saveUserDataAsync()
        {
            await saveUserDataTaskAsync();
        }

        public void saveUserData()
        {
            Task t = new Task(new Action(saveUserDataAsync));
            t.Start();
        }

        /// <summary>
        /// Loads the user data from disk
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        private async Task loadUserDataTaskAsync()
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UserData));
                Stream stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(UserData.JSON_FILE_NAME);
                userData = (UserData)serializer.ReadObject(stream);
            }
            catch (Exception)
            {

            }

            if (userData == null)
            {
                userData = new UserData();
            }
        }

        public async void loadUserDataAsync()
        {
            await saveUserDataTaskAsync();
        }

        public void loadUserData()
        {
            Task t = new Task(new Action(loadUserDataAsync));
            t.Start();
        }
        #endregion

        #region --Settings Data--
        /// <summary>
        /// Saves the settings data to disk
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        private async Task saveSettingsDataTaskAsync()
        {
            if (settingsData == null)
            {
                return;
            }

            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SettingsData));
                using (Stream stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(SettingsData.JSON_FILE_NAME, CreationCollisionOption.ReplaceExisting))
                {
                    serializer.WriteObject(stream, settingsData);
                }
            }
            catch (Exception)
            {

            }
        }

        public async void saveSettingsDataAsync()
        {
            await saveSettingsDataTaskAsync();
        }

        public void saveSettingsData()
        {
            Task t = new Task(new Action(saveSettingsDataAsync));
            t.Start();
        }

        /// <summary>
        /// Loads the settings data from disk
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        private async Task loadSettingsDataTaskAsync()
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SettingsData));
                Stream stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(SettingsData.JSON_FILE_NAME);
                settingsData = (SettingsData)serializer.ReadObject(stream);
            }
            catch (Exception)
            {

            }

            if (settingsData == null)
            {
                settingsData = new SettingsData();
            }
        }

        public async void loadSettingsDataAsync()
        {
            await loadSettingsDataTaskAsync();
        }

        public void loadSettingsData()
        {
            Task t = new Task(new Action(loadSettingsDataAsync));
            t.Start();
        }
        #endregion

        #region --All Data--
        public async void loadAllDataAsync()
        {
            await loadUserDataTaskAsync();
            await loadSettingsDataTaskAsync();
        }

        public async void saveAllDataAsync()
        {
            await saveUserDataTaskAsync();
            await saveSettingsDataTaskAsync();
        }

        public async Task loadAllDataTaskAsync()
        {
            await loadUserDataTaskAsync();
            await loadSettingsDataTaskAsync();
        }

        public async Task saveAllDataTaskAsync()
        {
            await saveUserDataTaskAsync();
            await saveSettingsDataTaskAsync();
        }

        /// <summary>
        /// Opens a FileSavePicker and trys to create a backup of the selected data
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public async Task backupData()
        {
            saveAllData();
            try
            {
                FileSavePicker picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeChoices.Add("Ausgaben", new List<string>() { ".ujson" });
                picker.FileTypeChoices.Add("Einstellungen", new List<string>() { ".ejson" });
                DateTime now = DateTime.Now.ToLocalTime();
                string name = "" + now.Day + '.' + now.Month + '.' + now.Year + ' ' + now.Hour + '.' + now.Minute;
                picker.SuggestedFileName = name;
                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    IStorageItem storageItem = null;
                    if (file.FileType.Equals(".ujson"))
                    {
                        storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(UserData.JSON_FILE_NAME);
                    }
                    else
                    {
                        storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(SettingsData.JSON_FILE_NAME);
                    }
                    if (storageItem != null && storageItem is StorageFile)
                    {
                        StorageFile sF = storageItem as StorageFile;
                        await sF.CopyAndReplaceAsync(file);
                        await showMessageBoxAsync("Erfolgreich gespeichert als:\n" + file.Name);
                    }
                    else
                    {
                        await showMessageBoxAsync("Zieldatei existiert nicht! \nBitte erneut versuchen.");
                    }
                }
                else
                {
                    await showMessageBoxAsync("Datei konnte nicht erstellt werden.");
                }
            }
            catch (Exception e)
            {
                await showMessageBoxAsync(e.ToString());
            }

        }

        /// <summary>
        /// Opens a FileOpenPicker and trys to import the selected data
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public async Task loadExtenalData()
        {
            saveAllData();
            try
            {
                FileOpenPicker picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeFilter.Add(".ujson");
                picker.FileTypeFilter.Add(".ejson");
                StorageFile file = await picker.PickSingleFileAsync();

                if (file != null)
                {
                    IStorageItem storageItem = null;
                    if (file.FileType.Equals(".ujson"))
                    {
                        storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(UserData.JSON_FILE_NAME);
                    }
                    else
                    {
                        storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(SettingsData.JSON_FILE_NAME);
                    }
                    if (storageItem != null && storageItem is StorageFile)
                    {
                        StorageFile sF = storageItem as StorageFile;
                        await file.CopyAndReplaceAsync(sF);
                        await showMessageBoxAsync("Erfolgreich gespeichert als:\n" + file.Name);
                        loadAllDataAsync();
                    }
                    else
                    {
                        await showMessageBoxAsync("Zieldatei existiert nicht! \nBitte erneut versuchen.");
                    }
                }
                else
                {
                    await showMessageBoxAsync("Datei konnte nicht erstellt werden.");
                }
            }
            catch (Exception e)
            {
                await showMessageBoxAsync(e.ToString());
            }
        }
        #endregion

        /// <summary>
        /// Loads the settings and user data in a seperat Task
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public void loadAllData()
        {
            Task t = new Task(new Action(loadAllDataAsync));
            t.Start();
        }

        /// <summary>
        /// Saves the settings and user data in a seperat Task
        /// </summary>
        /// <history>
        /// 10/12/2016  Created [Fabian Sauter]
        /// </history>
        public void saveAllData()
        {
            Task t = new Task(new Action(saveAllDataAsync));
            t.Start();
        }

        public void navigateToPage(EnumPage page)
        {
            mainPage.navigateToPage(page);
        }

        public void enableBurgerMenue()
        {
            mainPage.enableBurgerMenue();
        }

        public void disableBurgerMenue()
        {
            mainPage.disableBurgerMenue();
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
