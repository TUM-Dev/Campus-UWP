using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TUMCampusAppAPI.Managers
{
    public class LocationManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static LocationManager INSTANCE;
        private static readonly double[][] CAMPUS_LOCATIONS = {
            new double[] {48.2648424, 11.6709511}, // Garching Forschungszentrum
            new double[] {48.249432, 11.633905}, // Garching Hochbrück
            new double[] {48.397990, 11.722727}, // Weihenstephan
            new double[] {48.149436, 11.567635}, // Stammgelände
            new double[] {48.110847, 11.4703001}, // Klinikum Großhadern
            new double[] {48.137539, 11.601119}, // Klinikum rechts der Isar
            new double[] {48.155916, 11.583095}, // Leopoldstraße
            new double[] {48.150244, 11.580665} // Geschwister Schollplatz/Adalbertstraße
        };

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public LocationManager()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the last known location
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public async Task<Geopoint> getCurrentLocationAsync()
        {
            Geopoint p;
            if (!SyncManager.INSTANCE.needSync(this, Consts.VALIDITY_ONE_DAY).NEEDS_SYNC && (p = UserDataManager.INSTANCE.getLastKnownDevicePosition()) != null)
            {
                Logger.Info("Position is still up to date, no need to refresh it again");
                return p;
            }
            if (!DeviceInfo.isConnectedToInternet())
            {
                Logger.Info("Unable to get the devices position without a connection to the internet");
                return null;
            }
            try
            {
                var accessStatus = await Geolocator.RequestAccessAsync();
                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Denied:
                    case GeolocationAccessStatus.Unspecified:
                        Logger.Warn("No access to GeoLocation");
                        return null;
                }
                Geolocator geoLocator = new Geolocator
                {
                    DesiredAccuracy = PositionAccuracy.Default
                };
                Geoposition pos = await geoLocator.GetGeopositionAsync();
                UserDataManager.INSTANCE.setLastKnownDevicePosition(pos.Coordinate.Point);
                SyncManager.INSTANCE.replaceIntoDb(new SyncTable(this));
                return pos.Coordinate.Point;
            }
            catch (Exception e)
            {
                Logger.Warn("Unable to get the current location:\n" + e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Returns the "id" of the current campus
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public async Task<int> getCurrentCampusAsync()
        {
            Geopoint pos = UserDataManager.INSTANCE.getLastKnownDevicePosition();
            if (pos == null)
            {
                pos = await getCurrentLocationAsync();
            }
            if (pos == null)
            {
                return 0;
            }
            return getCampusFromLocation(pos);
        }

        /// <summary>
        /// Returns the nearest campus from the given Geopoint.
        /// </summary>
        /// <param name="pos">The current position.</param>
        /// <returns>Returns the nearest campus.</returns>
        private int getCampusFromLocation(Geopoint pos)
        {
            double bestDistance = double.MaxValue;
            double distanceTemp = double.MaxValue;
            int bestCampus = -1;
            for (int i = 0; i < CAMPUS_LOCATIONS.Length; i++)
            {
                distanceTemp = calcDistance(CAMPUS_LOCATIONS[i][0], CAMPUS_LOCATIONS[i][1], pos.Position.Latitude, pos.Position.Longitude);
                if (distanceTemp < bestDistance)
                {
                    bestDistance = distanceTemp;
                    bestCampus = i;
                }
            }
            if (bestDistance < 1000)
            {
                return bestCampus;
            }
            else
            {
                return -1;
            }
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        public double calcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double dLat = toRadian(lat2 - lat1);
            double dLon = toRadian(lng2 - lng1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(toRadian(lat1)) * Math.Cos(toRadian(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            return c * 6371000;
        }

        public async override Task InitManagerAsync()
        {

        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Convert to Radians.
        /// </summary>
        private static double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
