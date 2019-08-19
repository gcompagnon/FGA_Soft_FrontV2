using System.Windows;
using Telerik.Windows.Documents.Model;

namespace FrontV2.Action.Reco.View
{
    /// <summary>
    /// Logique d'interaction pour LargeRecommandation.xaml
    /// </summary>
    public partial class LargeRecommandation : Window
    {
        public LargeRecommandation()
        {
            InitializeComponent();
        }

        public void load(RadDocument doc)
        {
            EnlargedReco.Document = doc;
        }
    }
}
