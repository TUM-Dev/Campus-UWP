using System.Collections.Generic;
using System.Threading.Tasks;
using ExternalData.Classes.Manager;
using ExternalData.Classes.Mvg;
using UI_Context.Classes.Templates.Controls.Mvg;

namespace UI_Context.Classes.Context.Controls.Mvg
{
    public class MvgWidgetControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly MvgWidgetControlDataTemplate MODEL = new MvgWidgetControlDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public MvgWidgetControlContext()
        {
            // Ensure we are initially loading to prevent the user from performing multiple requests at once:
            MODEL.IsLoading = true;
            Refresh();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void Refresh()
        {
            Task.Run(async () => await LoadDeparturesAsync());
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task LoadDeparturesAsync()
        {
            MODEL.IsLoading = true;
            IEnumerable<Departure> departures = await MvgManager.INSTANCE.RequestDeparturesAsync("de:09162:530", true, true, true, true);
            MODEL.DEPARTURES.Clear();
            MODEL.DEPARTURES.AddRange(departures);
            MODEL.IsLoading = false;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
