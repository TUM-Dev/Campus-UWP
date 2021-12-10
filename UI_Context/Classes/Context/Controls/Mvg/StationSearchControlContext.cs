using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExternalData.Classes.Manager;
using ExternalData.Classes.Mvg;
using UI_Context.Classes.Templates.Controls.Mvg;

namespace UI_Context.Classes.Context.Controls.Mvg
{
    public class StationSearchControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly StationSearchControlDataTemplate MODEL = new StationSearchControlDataTemplate();
        private string nextQuery = null;
        private string curQuery = null;
        private bool isSearching = false;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Search(string query)
        {
            Task.Run(async () => await SearchAsync(query));
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task SearchAsync(string query)
        {
            // Prevent multiple runs at the same time:
            lock (this)
            {
                if (isSearching)
                {
                    nextQuery = query;
                    return;
                }
                isSearching = true;
            }

            do
            {
                if (!string.Equals(curQuery, query))
                {
                    IEnumerable<Station> stations = await MvgManager.INSTANCE.FindStationAsync(query);
                    MODEL.STATIONS.Replace(stations);
                    curQuery = query;
                }

                lock (this)
                {
                    if (nextQuery is null)
                    {
                        isSearching = false;
                        return;
                    }
                    if (string.Equals(query, nextQuery))
                    {
                        nextQuery = null;
                        return;
                    }
                    query = nextQuery;
                    nextQuery = null;
                }
            } while (true);
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
