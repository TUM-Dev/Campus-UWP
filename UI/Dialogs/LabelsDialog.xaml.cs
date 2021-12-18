using System.Collections.Generic;
using System.Text;
using ExternalData.Classes.Manager;
using Shared.Classes;
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
            AddLabels(sb, DishManager.LABELS_EMOJI_MISC_LOOKUP);
            sb.Append('\n');
            AddLabels(sb, DishManager.LABELS_EMOJI_ADDITIONALS_LOOKUP);
            sb.Append('\n');
            AddLabels(sb, DishManager.LABELS_EMOJI_ALLERGENS_LOOKUP);
            Labels = sb.ToString();
        }

        private void AddLabels(StringBuilder sb, Dictionary<string, string> labels)
        {
            foreach (KeyValuePair<string, string> pair in labels)
            {
                sb.Append(pair.Value);
                sb.Append('\t');
                sb.Append(Localisation.GetLocalizedString("Label_" + pair.Key));
                sb.Append('\n');
            }
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
