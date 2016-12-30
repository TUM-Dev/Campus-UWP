using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TUMCampusApp.classes;
using TUMCampusApp.classes.managers;
using TUMCampusApp.classes.userData;
using TUMCampusApp.pages.setup;
using TUMCampusApp.Pages;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static TUMCampusApp.classes.Utillities;

namespace TUMCampusApp.pages
{
    public sealed partial class SplashScreenPage : Page
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        internal Rect splashImageRect;
        private SplashScreen splash;
        internal bool dismissed = false;
        private string tileID;

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 18/12/2016  Created [Fabian Sauter]
        /// </history>
        public SplashScreenPage(LaunchActivatedEventArgs args, bool loadState)
        {
            this.InitializeComponent();
            this.tileID = args.TileId;
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
            splash = args.SplashScreen;
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;
                positionElements();
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);
            }
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void positionElements()
        {
            positionImage();
        }

        private void positionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            extendedSplashImage.Height = splashImageRect.Height;
            extendedSplashImage.Width = splashImageRect.Width;
        }

        private async Task initAppAsync()
        {
            await invokeTbxAsync("Gathering device infos...");
            DeviceInfo.INSTANCE = new DeviceInfo();
            await incProgressAsync();

            await invokeTbxAsync("Loading cache manager...");
            CacheManager.INSTANCE = new CacheManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading canteen manager...");
            CanteenManager.INSTANCE = new CanteenManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading canteenmenu manager...");
            CanteenMenueManager.INSTANCE = new CanteenMenueManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading location manager...");
            LocationManager.INSTANCE = new LocationManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading sync manager...");
            SyncManager.INSTANCE = new SyncManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading userdata manager...");
            UserDataManager.INSTANCE = new UserDataManager();
            await incProgressAsync();

            await invokeTbxAsync("Loading TumManager...");
            TumManager.INSTANCE = new TumManager();
            await incProgressAsync();

            await invokeTbxAsync("Initializing cache manager...");
            await CacheManager.INSTANCE.initManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing canteen manager...");
            await CanteenManager.INSTANCE.initManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing canteenmenu manager...");
            await CanteenMenueManager.INSTANCE.initManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing location manager...");
            await LocationManager.INSTANCE.initManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing sync manager...");
            await SyncManager.INSTANCE.initManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing device position...");
            await UserDataManager.INSTANCE.initManagerAsync();
            await incProgressAsync();

            await invokeTbxAsync("Initializing TumManager...");
            await TumManager.INSTANCE.initManagerAsync();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                splashProgressBar.Value = 100.0;
            });

        }

        private async Task invokeTbxAsync(string s)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                task_tbx.Text = s;
            });
        }

        private async Task incProgressAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                splashProgressBar.Value += 7;
            });
        }

        private void initAppTask()
        {
            initAppAsync().Wait();
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                DismissExtendedSplashAsync();
            }).AsTask().Wait();
        }

        private async void DismissExtendedSplashAsync()
        {
            if (tileID.Equals(Const.TILE_ID_CANTEEN))
            {
                Window.Current.Content = new MainPage(EnumPage.CanteensPage);
            }
            else
            {
                Frame f = new Frame();
                if (!UserDataManager.INSTANCE.shouldHideWizardOnStartup())
                {
                    bool wifiOnly = UserDataManager.INSTANCE.onlyUpdateWhileConnectedToWifi();
                    if ((!wifiOnly && DeviceInfo.isConnectedToInternet()) || (wifiOnly && DeviceInfo.isConnectedToWifi()))
                    {
                        if(TumManager.getToken() == null || TumManager.getToken() == "")
                        {
                            f.Navigate(typeof(SetupPageStep1));
                        }
                        else if (!await TumManager.INSTANCE.isTokenConfirmedAsync())
                        {
                            f.Navigate(typeof(SetupPageStep2));
                        }
                        else
                        {
                            TumManager.INSTANCE.setTUMOnlineEnabled(true);
                            f.Navigate(typeof(MainPage));
                        }
                    }
                }
                else
                {
                    TumManager.INSTANCE.setTUMOnlineEnabled(false);
                    f.Navigate(typeof(MainPage));
                }
                Window.Current.Content = f;
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        private void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
            Task.Factory.StartNew(() => initAppTask());
        }

        private void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;
                positionElements();
            }
        }

        #endregion
    }
}
