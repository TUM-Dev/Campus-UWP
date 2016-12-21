using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace TUMCampusApp.classes
{
    class CustomAccelerometer
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        // The minimum acceleration value to trigger the Shaken event
        private const double AccelerationThreshold = 2.5;
        // The minimum interval in milliseconds between two consecutive calls for the Shaken event
        private const int ShakenInterval = 500;
        private static bool _AccelerometerLoaded;
        private static Accelerometer _DefaultAccelerometer;
        private static DateTime _ShakenTimespan = DateTime.Now;
        private static bool _Enabled;

        /// <summary>
        /// Gets or sets whether or not the Accelerometer is currently enabled and can raise the Shaken event
        /// </summary>
        /// /// </summary>
        /// <history>
        /// 20/12/2016  Created [Fabian Sauter]
        /// </history>
        public static bool Enabled
        {
            get { return _Enabled; }
            set
            {
                if (_Enabled != value && DefaultAccelerometer != null)
                {
                    if (value) DefaultAccelerometer.ReadingChanged += _DefaultAccelerometer_ReadingChanged;
                    else DefaultAccelerometer.ReadingChanged -= _DefaultAccelerometer_ReadingChanged;
                }
                _Enabled = value;
            }
        }

        /// <summary>
        /// Gets the current Accelerometer in use
        /// </summary>
        /// /// <history>
        /// 20/12/2016  Created [Fabian Sauter]
        /// </history>
        private static Accelerometer DefaultAccelerometer
        {
            get
            {
                if (!_AccelerometerLoaded)
                {
                    _AccelerometerLoaded = true;
                    _DefaultAccelerometer = Accelerometer.GetDefault();
                }
                return _DefaultAccelerometer;
            }
        }

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Handles the ReadingChanged event and raises the Shaken event when necessary
        /// </summary>
        /// <history>
        /// 20/12/2016  Created [Fabian Sauter]
        /// </history>
        private static void _DefaultAccelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            double g = Math.Abs(args.Reading.AccelerationX + args.Reading.AccelerationY + args.Reading.AccelerationZ);
            if (g > AccelerationThreshold && DateTime.Now.Subtract(_ShakenTimespan).Milliseconds > ShakenInterval)
            {
                _ShakenTimespan = DateTime.Now;
                Shaken?.Invoke(null, EventArgs.Empty);
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--
        /// <summary>
        /// Raised whenever the Accelerometer detects a shaking gesture
        /// </summary>
        /// <history>
        /// 20/12/2016  Created [Fabian Sauter]
        /// </history>
        public static event EventHandler Shaken;

        #endregion
    }
}
