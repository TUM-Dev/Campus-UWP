namespace TUMCampusApp.Controls.Widgets
{
    interface IHideableWidget
    {
        /// <summary>
        /// Should return the settings token for disabling the widget.
        /// Return null to disable disabling the widget.
        /// </summary>
        /// <returns></returns>
        string getSettingsToken();

        /// <summary>
        /// Gets called before the visibility gets changed.
        /// </summary>
        void onHiding();
    }
}
