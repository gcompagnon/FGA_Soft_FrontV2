using FrontV2.Action.ScoreChange.ViewModel;
using FrontV2.Utilities;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.Action.ScoreChange.View
{
    /// <summary>
    /// Logique d'interaction pour ScoreChangeView.xaml
    /// </summary>
    public partial class ScoreChangeView : Window
    {
        private ScoreChangeViewModel _viewModel;
        private String _ticker;

        public ScoreChangeView()
        {
            InitializeComponent();

            _viewModel = new ScoreChangeViewModel();
            DataContext = _viewModel;
        }

        public void preLoad()
        {
            _viewModel.SelectedDate1 = _viewModel.Dates[0];
            _viewModel.SelectedDate2 = _viewModel.Dates[1];
            _viewModel.LoadChanges(!Filter1.IsChecked.Value, true);
        }

        private void LoadChanges_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadChanges(!Filter1.IsChecked.Value);
        }

        private void Changes_AutoGeneratingColumn(object sender, Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = Application.Current.Resources["scoreChangeStyleSelector"] as StyleSelector;

            GridViewDataColumn column = e.Column as GridViewDataColumn;

            if (e.Column.Header.ToString() == "Quint1"
                  || e.Column.Header.ToString() == "Quint2"
                  || e.Column.Header.ToString() == "Rang1"
                  || e.Column.Header.ToString() == "Rang2")
                e.Column.TextAlignment = TextAlignment.Right;

            if (column.Header.ToString() == "Date1")
                column.DataFormatString = "dd/MM/yyyy";
            if (column.Header.ToString() == "Date2")
                column.DataFormatString = "dd/MM/yyyy";

        }

        private void RadContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.ChangesGrid.SelectedItems.Clear();
                    this.ChangesGrid.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void Evolution_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;

            if (row == null)
            {
                MyLegend.Text = "Pas de sélection";
                return;
            }

            MyLegend.Text = row["COMPANY"].ToString() +
                "\n" + "Du " + row["Date1"].ToString().Substring(0, 10)
                + " Au " + row["Date2"].ToString().Substring(0, 10);

            String ticker = row["TICKER"].ToString();
            _ticker = ticker;

            if (ticker != "")
            {
                _viewModel.LoadChart(ticker);
            }

        }

        private void QuintileChart_SelectionChanged(object sender, Telerik.Windows.Controls.ChartView.ChartSelectionChangedEventArgs e)
        {
            bool changedInfo = false;
            //Handle selection of the current point
            if (e.AddedPoints.Count > 0)
            {
                var addedPoint = e.AddedPoints[0];
                var series = addedPoint.Presenter as LineSeries;

                //Get the Content Presenter of the series
                var pointPresenter = series.
                    ChildrenOfType<ContentPresenter>().
                    Where(cp => cp.Tag == addedPoint).FirstOrDefault();
                var ellipseElement = pointPresenter.
                    ChildrenOfType<Ellipse>().FirstOrDefault();

                //Do whatever you want with it :)
                ellipseElement.Fill = new SolidColorBrush(Colors.Red);
                CategoricalDataPoint point = pointPresenter.Content as CategoricalDataPoint;
                this.Date.Text = point.Category.ToString();
                this.Valeur.Text = point.Value.ToString();
                changedInfo = true;
            }

            //Handle de-selection of the current point
            if (e.RemovedPoints.Count > 0)
            {
                var removedPoint = e.RemovedPoints[0];
                var series = removedPoint.Presenter as LineSeries;
                if (series == null)
                    return;
                var pointPresenter = series.
                    ChildrenOfType<ContentPresenter>().
                    Where(cp => cp.Tag == removedPoint).FirstOrDefault();
                if (pointPresenter == null)
                    return;

                var ellipseElement = pointPresenter.
                    ChildrenOfType<Ellipse>().FirstOrDefault();

                //Do whatever you want with it :)
                ellipseElement.Fill = new SolidColorBrush(Colors.DarkGreen);
                if (!changedInfo)
                {
                    this.Date.Text = "";
                    this.Valeur.Text = "";
                }
            }
        }

        private void ExportToPDF_Click(object sender, RoutedEventArgs e)
        {
            String name = "scoreChange_" + _viewModel.SelectedDate1.Replace("/", "-") + "_" + _viewModel.SelectedDate2.Replace("/", "-");
            _viewModel.ExtractGridToPDF(this.ChangesGrid, name);
        }

        private void QuintilesCopy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = _viewModel.SelectedDate1.Substring(6, 4);
            String month = _viewModel.SelectedDate1.Substring(3, 2); ;
            String day = _viewModel.SelectedDate1.Substring(0, 2);

            String nom = year + month + day
                + "_ScoresChanges_" + _ticker;

            Helpers.ExportToBMP(nom, quintilesChart);
        }

        /******************
          Click SXCEL
     *******************/
        private void OpenExcel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = ChangesGrid.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["COMPANY"].ToString() == "")
                return;

            String libelle = row["COMPANY"].ToString();
            String industry = row["SECTOR"].ToString();
            String sector = row["INDUSTRY"].ToString();

            Helpers.OpenExcel(sector, industry, libelle);
        }

        /******************
              Click PDF
         *******************/
        private void OpenPDF_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = ChangesGrid.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["COMPANY"].ToString() == "")
                return;

            String libelle = row["COMPANY"].ToString();
            String industry = row["SECTOR"].ToString();
            String sector = row["INDUSTRY"].ToString();

            Helpers.OpenPDF(sector, industry, libelle);
        }

        /******************
            Go To Bloom
        *******************/
        private void DES_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "DES");
        }

        private void BQ_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BQ");
        }

        private void HCPI_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "HCPI");
        }

        private void BRC_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BRC");
        }

        private void EVT_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EVT");
        }

        private void ICN_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ICN");
        }

        private void EEG_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EEG");
        }

        private void EQRV_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EQRV");
        }

        private void PBAR_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "PBAR");
        }

        private void ANR_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ANR");
        }

        private void GIP5_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP5");
        }

        private void GIP30_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP30");
        }

        private void GR_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_EUROPE");
        }

        private void WGT_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ChangesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "WGT");
        }

        private void ExpandAll_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangesGrid.ExpandAllGroups();
        }

        private void CollapseAll_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangesGrid.CollapseAllGroups();
        }

    }
}
