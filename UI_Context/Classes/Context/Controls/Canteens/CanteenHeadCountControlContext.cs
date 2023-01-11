using System;
using System.Threading.Tasks;
using ExternalData.Classes.CampusBackend;
using ExternalData.Classes.Manager;
using Storage.Classes.Models.Canteens;
using UI_Context.Classes.Templates.Controls.Canteens;

namespace UI_Context.Classes.Context.Controls.Canteens
{
    public class CanteenHeadCountControlContext
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public readonly CanteenHeadCountControlDataTemplate MODEL = new CanteenHeadCountControlDataTemplate();

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public void UpdateView(Canteen canteen)
        {
            Task.Run(async () =>
            {
                CanteenHeadCount headCount = await CanteenManager.INSTANCE.GetHeadCountAsync(canteen.Id);
                if (headCount is null || headCount.percent < 0)
                {
                    MODEL.HasData = false;
                    return;
                }

                MODEL.Percent = Math.Round(headCount.percent); // Round to full numbers since precision is not required and it makes it easier displaying the number
                MODEL.HasData = true;
                MODEL.Tooltip = $"Guests: {headCount.count} - {Math.Round(headCount.percent)}%";

                if (headCount.percent > 75)
                {
                    MODEL.StatusEmoji = "❌";
                }
                else if (headCount.percent > 50)
                {
                    MODEL.StatusEmoji = "⚠️";
                }
                else
                {
                    MODEL.StatusEmoji = "✅";
                }
            });
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
