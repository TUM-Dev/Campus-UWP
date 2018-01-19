using TUMCampusAppAPI;
using Windows.ApplicationModel;

namespace TUMCampusApp.Classes
{
    class AppUpdateHandler
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--


        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <history>
        /// 19/01/2018 Created [Fabian Sauter]
        /// </history>
        public AppUpdateHandler()
        {

        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--
        /// <summary>
        /// Returns the current package version.
        /// </summary>
        /// <returns>The current package version.</returns>
        private PackageVersion getPackageVersion()
        {
            return Package.Current.Id.Version;
        }

        /// <summary>
        /// Saves the given package version in the application storage.
        /// </summary>
        /// <param name="version">The package version, that should get saved.</param>
        private void setVersion(PackageVersion version)
        {
            Util.setSetting(Const.VERSION_MAJOR, version.Major);
            Util.setSetting(Const.VERSION_MINOR, version.Minor);
            Util.setSetting(Const.VERSION_BUILD, version.Build);
            Util.setSetting(Const.VERSION_REVISION, version.Revision);
        }

        /// <summary>
        /// Returns the package version from the application storage.
        /// Default: major = 0, minor = 0, build = 0, revision = 0.
        /// </summary>
        /// <returns>A not null package version.</returns>
        private PackageVersion getLastStartedVersion()
        {
            return new PackageVersion()
            {
                Major = Util.getSettingUshort(Const.VERSION_MAJOR),
                Minor = Util.getSettingUshort(Const.VERSION_MINOR),
                Build = Util.getSettingUshort(Const.VERSION_BUILD),
                Revision = Util.getSettingUshort(Const.VERSION_REVISION),
            };
        }

        /// <summary>
        /// Checks if PackageVersion a is equal to PackageVersion b.
        /// </summary>
        /// <param name="a">The PackageVersion of the last app start.</param> 0.1.0.0
        /// <param name="b">The current PackageVersion.</param> 0.2.0.0
        /// <returns>Returns true, if the current PackageVersion equals the last app start PackageVersion.</returns>
        private bool compare(PackageVersion a, PackageVersion b)
        {
            return a.Major == b.Major && a.Minor == b.Minor && a.Build == b.Build && a.Revision == b.Revision;
        }

        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Gets called on App start and performs update task e.g. migrate the DB to a new format.
        /// </summary>
        public static void onAppStart()
        {
            AppUpdateHandler handler = new AppUpdateHandler();
            PackageVersion versionLastStart = handler.getLastStartedVersion();

            // Check if version != 0.0.0.0 => first ever start of the app:
            if (!(versionLastStart.Major == versionLastStart.Minor && versionLastStart.Build == versionLastStart.Revision && versionLastStart.Minor == versionLastStart.Build && versionLastStart.Major == 0) || Util.getSettingBoolean(Const.INITIALLY_STARTED))
            {
                if (!handler.compare(versionLastStart, handler.getPackageVersion()))
                {
                    // Insert code here for update routines
                }
            }
            handler.setVersion(handler.getPackageVersion());
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
