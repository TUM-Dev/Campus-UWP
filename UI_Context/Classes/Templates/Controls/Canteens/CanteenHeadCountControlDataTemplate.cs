using Shared.Classes;

namespace UI_Context.Classes.Templates.Controls.Canteens
{
    public class CanteenHeadCountControlDataTemplate: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public bool HasData
        {
            get => _HasData;
            set => SetProperty(ref _HasData, value);
        }
        private bool _HasData;

        public double Percent
        {
            get => _Percent;
            set => SetProperty(ref _Percent, value);
        }
        private double _Percent;

        public string Tooltip
        {
            get => _Tooltip;
            set => SetProperty(ref _Tooltip, value);
        }
        private string _Tooltip;

        public string StatusEmoji
        {
            get => _StatusEmoji;
            set => SetProperty(ref _StatusEmoji, value);
        }
        private string _StatusEmoji;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


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
