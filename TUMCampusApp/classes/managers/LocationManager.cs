﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusApp.classes.canteen;
using Windows.Devices.Geolocation;

namespace TUMCampusApp.classes.managers
{
    class LocationManager : AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static LocationManager INSTANCE;
        private static readonly double[][] CAMPUS_LOCATIONS = {
            new double[] {48.2648424, 11.6709511}, // Garching Forschungszentrum
            new double[] {48.249432, 11.633905}, // Garching HochbrÃ¼ck
            new double[] {48.397990, 11.722727}, // Weihenstephan
            new double[] {48.149436, 11.567635}, // StammgelÃ¤nde
            new double[] {48.110847, 11.4703001}, // Klinikum GroÃŸhadern
            new double[] {48.137539, 11.601119}, // Klinikum rechts der Isar
            new double[] {48.155916, 11.583095}, // LeopoldstraÃŸe
            new double[] {48.150244, 11.580665} // Geschwister Schollplatz/AdalbertstraÃŸe
    };
        private static readonly String[] CAMPUS_SHORT = {
            "G", // Garching Forschungszentrum
            "H", // Garching HochbrÃ¼ck
            "W", // Weihenstephan
            "C", // StammgelÃ¤nde
            "K", // Klinikum GroÃŸhadern
            "I", // Klinikum rechts der Isar
            "L", // LeopoldstraÃŸe
            "S" // Geschwister Schollplatz/AdalbertstraÃŸe
    };
    private static readonly String[] DEFAULT_CAMPUS_STATION = {
            "Garching-Forschungszentrum",
            "Garching-HochbrÃ¼ck",
            "Weihenstephan",
            "TheresienstraÃŸe",//TODO need to use id instead of name, otherwise it does not work = 1000120
            "Klinikum GroÃŸhadern",
            "Max-Weber-Platz",
            "GiselastraÃŸe",
            "UniversitÃ¤t"
    };

    private static readonly String[] DEFAULT_CAMPUS_CAFETERIA = {"422", null, "423", "421", "414", null, "411", null};

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--
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
            try
            {
                var accessStatus = await Geolocator.RequestAccessAsync();
                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Denied:
                    case GeolocationAccessStatus.Unspecified:
                        Debug.WriteLine("No acces to GeoLocation");
                        return null;
                }
                Geolocator geoLocator = new Geolocator();
                geoLocator.DesiredAccuracy = PositionAccuracy.Default;
                Geoposition pos = await geoLocator.GetGeopositionAsync();
                UserDataManager.INSTANCE.setLastKnownDevicePosition(pos.Coordinate.Point);
                return pos.Coordinate.Point;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
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
            if(pos == null)
            {
                pos = await getCurrentLocationAsync();
            }
            if (pos == null)
            {
                return 0;
            }
            return getCampusFromLocation(pos);
        }

        private static int getCampusFromLocation(Geopoint pos)
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

        /// <summary>
        /// Returns the cafeteria's identifier which is near the given location
        /// The used radius around the cafeteria is 1km.
        /// </summary>
        /// <history>
        /// 14/12/2016  Created [Fabian Sauter]
        /// </history>
        public async Task<List<Canteen>> getCanteensAsync()
        {
            List<Canteen> list = CanteenManager.INSTANCE.getCanteens();
            Geopoint pos = UserDataManager.INSTANCE.getLastKnownDevicePosition();
            if (pos == null)
            {
                pos = await getCurrentLocationAsync();
            }
            if (pos == null)
            {
                return list;
            }
            foreach (Canteen c in list)
            {
                c.distance = (float) calcDistance(c.latitude, c.longitude, pos.Position.Latitude, pos.Position.Longitude);
            }
            list.Sort();
            return list;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>  
        /// Returns the distance in miles or kilometers of any two  
        /// latitude / longitude points.  
        /// </summary>  
        public static double calcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double dLat = toRadian(lat2 - lat1);
            double dLon = toRadian(lng2 - lng1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(toRadian(lat1)) * Math.Cos(toRadian(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            return c * 6371000;
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