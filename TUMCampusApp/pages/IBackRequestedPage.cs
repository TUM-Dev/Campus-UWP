namespace TUMCampusApp.Pages
{
    interface IBackRequestedPage
    {
        /// <summary>
        /// Gets triggered once a back request got performed.
        /// </summary>
        /// <returns>Return whether the back request should get canceled.</returns>
        bool onBackRequest();
    }
}
