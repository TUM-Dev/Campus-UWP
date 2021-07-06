using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Storage.Classes;

namespace TumOnline.Classes.Managers
{
    public class CalendarManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly CalendarManager INSTANCE = new CalendarManager();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        private async Task<IEnumerable<int>> DownloadCalendarAsync(TumOnlineCredentials credentials, bool force)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.CALENDAR);
            AccessManager.AddToken(request, credentials);
            request.AddQuery("pMonateVor", "2");
            request.AddQuery("pMonateNach", "5");
            XmlDocument doc = await request.RequestDocumentAsync(!force);
            return new List<int>();
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
