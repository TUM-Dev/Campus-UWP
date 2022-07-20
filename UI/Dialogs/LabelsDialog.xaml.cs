using System.Text;
using Storage.Classes.Contexts;
using Storage.Classes.Models.Canteens;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.Dialogs
{
    public sealed partial class LabelsDialog: ContentDialog
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public string Labels
        {
            get => (string)GetValue(LabelsProperty);
            set => SetValue(LabelsProperty, value);
        }
        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(nameof(Labels), typeof(string), typeof(LabelsDialog), new PropertyMetadata(""));

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public LabelsDialog()
        {
            InitializeComponent();
            LoadLabels();
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--


        #endregion

        #region --Misc Methods (Private)--
        private void LoadLabels()
        {
            StringBuilder sb = new StringBuilder();
            using (CanteensDbContext ctx = new CanteensDbContext())
            {
                foreach (Label label in ctx.Labels.Include(ctx.GetIncludePaths(typeof(Label))))
                {
                    sb.Append(label.Abbreviation);
                    sb.Append('\t');
                    sb.Append(label.GetTranslatedName());
                    sb.Append('\n');
                }
            }
            Labels = sb.ToString();
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
