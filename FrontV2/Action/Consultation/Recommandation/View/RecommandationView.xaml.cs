using FrontV2.Action.Consultation.Recommandation.ViewModel;
using FrontV2.Action.Reco;
using FrontV2.Utilities;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace FrontV2.Action.Consultation.Recommandation.View
{
    /// <summary>
    /// Logique d'interaction pour RecommandationView.xaml
    /// </summary>
    public partial class RecommandationView : Window, IView
    {
        RecommandationViewModel _viewModel;
        public RecommandationView()
        {
            InitializeComponent();
            _viewModel = new RecommandationViewModel();
            DataContext = _viewModel;

                
            (DataContext as RecommandationViewModel).View = this;
        }

        /// <summary>
        /// Add a textblock in the working grid
        /// </summary>
        /// <param name="b"></param>
        public void AddTextBlockToGrid(TextBlock b)
        {
            _window.Title = "Recommandations" + " : {" +  (DataContext as RecommandationViewModel).Univers
                + ", " +  (DataContext as RecommandationViewModel).Sector.Libelle + "}";

            // Overrided method from ViewModel Interface
            RowDefinition RowGrid1 = new RowDefinition();
            RowDefinition RowGrid2 = new RowDefinition();
            _grid.RowDefinitions.Add(RowGrid1);
            _grid.RowDefinitions.Add(RowGrid2);
            _grid.Children.Add(b);
        }

        /// <summary>
        /// Add a rich text box in the working grid
        /// </summary>
        /// <param name="b"></param>
        public void AddRichTextBoxToGrid(RichTextBox b)
        {
            // Overrided method from ViewModel Interface
            _grid.Children.Add(b);
        }

        /// <summary>
        /// Create a PDF File with the existing recommandations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BPrint_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ShowDialog();

            Helpers.ExportDataTableToPDFTableRecommandation(dialog, _viewModel.Univers, _viewModel.Sector.Libelle, _viewModel.Datas);
        }
    }
}
