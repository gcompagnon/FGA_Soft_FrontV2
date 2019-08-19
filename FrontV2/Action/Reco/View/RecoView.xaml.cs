using FrontV2.Action.Reco.ViewModel;
using FrontV2.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Documents.FormatProviders.Rtf;
using Telerik.Windows.Documents.Model;

namespace FrontV2.Action.Reco.View
{
    /// <summary>
    /// Logique d'interaction pour RecoView.xaml
    /// </summary>
    public partial class RecoView : Window
    {

        private RecoViewModel _viewModel;
        private String _prevDateSelected = "";
        private String _prevTypeSelected = "";

        public RecoView()
        {
            InitializeComponent();
            _viewModel = new RecoViewModel();
            DataContext = _viewModel;

            HistoMenu.Visibility = Visibility.Hidden;
        }

        public RecoView(bool b)
        {
            InitializeComponent();
            _viewModel = new RecoViewModel();
            DataContext = _viewModel;
            HistoGrid.IsReadOnly = b;
            HistoMenu.Visibility = Visibility.Visible;
        }

        private void Clear_SelectedSector(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SuperSectors != null)
                _viewModel.SelectedSuperSector = _viewModel.SuperSectors[0];
        }

        private void Clear_SelectedIndustry(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Sectors != null && _viewModel.Sectors.Count > 0 && _viewModel.SuperSectors != null)
                _viewModel.SelectedSector = _viewModel.Sectors[0];
        }

        private void ValuesGrid_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = System.Windows.Application.Current.Resources["recommandationStyleSelector"] as StyleSelector;

            GridViewDataColumn column = e.Column as GridViewDataColumn;
            if (column.Header.ToString() == "Date")
                column.DataFormatString = "dd/MM/yyyy";

            if (e.Column.Header.ToString() == "ISIN")
                e.Column.IsVisible = false;
            if (e.Column.Header.ToString() == "ID_ICB")
                e.Column.IsVisible = false;
            if (e.Column.Header.ToString() == "ID_FGA")
                e.Column.IsVisible = false;
        }

        private void HistoGrid_AutoGeneratingColumn(object sender,
            GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = System.Windows.Application.Current.Resources["recommandationStyleSelector2"] as StyleSelector;

            GridViewDataColumn column = e.Column as GridViewDataColumn;

            if (column.Header.ToString() == "Date")
                column.DataFormatString = "dd/MM/yyyy";

            if (column.Header.ToString() == "id_comment")
                column.IsVisible = false;
            if (column.Header.ToString() == "id_comment_change")
                column.IsVisible = false;
            if (column.Header.ToString() == "type")
                column.IsVisible = false;
            if (column.Header.ToString() == "ISIN")
                column.IsVisible = false;
            if (e.Column.Header.ToString() == "ID")
                e.Column.IsVisible = false;
        }

        private void LoadReco_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadExecute();
        }

        private void Values_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            System.Data.DataRow row = ValuesGrid.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;
            if (row["ISIN"] == null || row["ISIN"].ToString() == "")
            {
                String sectorFGA = row["Label_Industry"].ToString();
                String sectorICB = row["Label_Secteur"].ToString();

                String id_fga = row["ID_FGA"].ToString();
                String id_icb = row["ID_ICB"].ToString();

                _viewModel.FillHistoDataSourceSector(sectorFGA, sectorICB, id_fga, id_icb);
            }
            else
            {
                String isin = row["ISIN"].ToString();
                String id_sector_fga = row["Label_Industry"].ToString();
                String sector_fga = row["TICKER"].ToString();
                String libelle = row["Company_Name"].ToString();
                _viewModel.FillHistoDataSource(isin, id_sector_fga, sector_fga, libelle);
            }

            CommentReader.Document = new RadDocument();

            if (_viewModel.HistoDataSource != null && _viewModel.HistoDataSource.Rows.Count >= 1)
                Writecoment(_viewModel.HistoDataSource.Rows[0]);
        }

        private void Historique_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            Writecoment(HistoGrid.SelectedItem as System.Data.DataRow);
        }

        public void Writecoment(DataRow row)
        {
            if (row == null)
                return;

            String id = row["id_comment"].ToString();
            RtfFormatProvider rtf = new RtfFormatProvider();
            String comment = _viewModel.FillRecoTextBox(id).ToString();

            if (comment == "")
            {
                CommentReader.Document = new RadDocument();
                return;
            }
            CommentReader.Document = rtf.Import(comment);
        }

        private void Nouvelle_Recommandation_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = ValuesGrid.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            String id_fga = row["ID_FGA"].ToString();
            String id_icb = row["ID_ICB"].ToString();
            String prevMXEU = "";
            String prevMXEUM = "";
            String prevMXEM = "";
            String prevMXUSLC = "";
            if (GlobalInfos.isEurope)
            {
                prevMXEU = row["RecoMXEU"].ToString();
                prevMXEUM = row["RecoMXEUM"].ToString();
                prevMXEM = row["RecoMXEM"].ToString();
            }
            else
                prevMXUSLC = row["RecoMXUSLC"].ToString();

            if (row["ISIN"].ToString() == "")
            {

                if (id_fga == "")
                {
                    String sector_name = row["Label_Secteur"].ToString(); ;

                    NewRecommandationSectorsView newReco = new NewRecommandationSectorsView();
                    newReco.Load(id_icb, sector_name, prevMXEU, prevMXEUM, prevMXEM, prevMXUSLC, "ICB");
                    newReco.ShowDialog();
                    if (newReco.validated)
                        _viewModel.LoadExecute();
                }
                else
                {
                    String sector_name = row["Label_Industry"].ToString(); ;

                    NewRecommandationSectorsView newReco = new NewRecommandationSectorsView();
                    newReco.Load(id_fga, sector_name, prevMXEU, prevMXEUM, prevMXEM, prevMXUSLC, "FGA");
                    newReco.ShowDialog();
                    if (newReco.validated)
                        _viewModel.LoadExecute();

                }
            }
            else
            {
                String isin = row["ISIN"].ToString();
                String libelle = row["Company_Name"].ToString();
                String ticker = row["TICKER"].ToString();

                NewRecommandationValeurView newReco = new NewRecommandationValeurView();
                newReco.Load(isin, libelle, prevMXEU, prevMXEUM, prevMXEM, prevMXUSLC, ticker);
                newReco.ShowDialog();
                if (newReco.validated)
                    _viewModel.LoadExecute();
            }
        }

        private void ReLoad_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            _viewModel.LoadExecute();
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
                    this.ValuesGrid.SelectedItems.Clear();
                    this.ValuesGrid.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void EnlageReco_Click(object sender, RoutedEventArgs e)
        {
            LargeRecommandation largeReco = new LargeRecommandation();
            largeReco.load(CommentReader.Document);
            largeReco.ShowDialog();
        }
              
        private void CellBeginEditHisto(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            DataRow row = HistoGrid.SelectedItem as DataRow;
            if (row == null)
                return;

            _prevDateSelected = row["Date"].ToString();
            _prevTypeSelected = row["type"].ToString();
        }

        private void EndEditionHisto(object sender, GridViewCellEditEndedEventArgs e)
        {
            DataRow row = HistoGrid.SelectedItem as DataRow;

            String ISIN = row["ISIN"].ToString();
            String newDate = row["Date"].ToString();
            String newRecoMXEU = row["RecoMXEU"].ToString();
            String newRecoMXEUM = row["RecoMXEUM"].ToString();
            String newRecoMXEM = row["RecoMXEM"].ToString();
            String newRecoMXUSLC = row["RecoMXUSLC"].ToString();
            int id = int.Parse(row["ID"].ToString());

            if (ISIN != "") //reco valeur
                _viewModel.UpdateRecoValue(id, newDate, newRecoMXEU, newRecoMXEUM, newRecoMXEM, newRecoMXUSLC);
            else //reco secteur
                _viewModel.UpdateRecoSector(_prevDateSelected, _prevTypeSelected, id, newDate, newRecoMXEU, newRecoMXEUM, newRecoMXEM, newRecoMXUSLC);
        }

        private void RadContextMenuHisto_Opened(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.HistoGrid.SelectedItems.Clear();
                    this.HistoGrid.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void SuppressLine_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
          
            DataRow row = HistoGrid.SelectedItem as DataRow;

            String ISIN = row["ISIN"].ToString();
            String date = row["Date"].ToString();
            String type = row["type"].ToString();
            int id = int.Parse(row["ID"].ToString());
            int id_comment = int.Parse(row["id_comment"].ToString());
            int id_comment_change = int.Parse(row["id_comment_change"].ToString());

            if (ISIN != "") //reco valeur
                _viewModel.DeleteRecoValeur(id, id_comment, id_comment_change);
            else //reco secteur
                _viewModel.DeleteRecoSector(date, type, id, id_comment, id_comment_change);

            _viewModel.LoadExecute();
        }

        private void ExtractReco_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            _viewModel.ExtractRecoMethod(this.ValuesGrid);
        }

        private void ExtractRecoForPrint_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            _viewModel.ExtractForPrintRecoMethod(this.ValuesGrid);
        }

        /******************
             Click SXCEL
        *******************/
        private void OpenExcel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = ValuesGrid.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["Company_Name"].ToString() == "")
                return;

            String libelle = row["Company_Name"].ToString();
            String industry = row["Label_Secteur"].ToString();
            String sector = row["Label_Industry"].ToString();

            Helpers.OpenExcel(sector, industry, libelle);
        }

        /******************
              Click PDF
         *******************/
        private void OpenPDF_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = ValuesGrid.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["Company_Name"].ToString() == "")
                return;

            String libelle = row["Company_Name"].ToString();
            String industry = row["Label_Secteur"].ToString();
            String sector = row["Label_Industry"].ToString();

            Helpers.OpenPDF(sector, industry, libelle);
        }

        /******************
            Go To Bloom
        *******************/
        private void DES_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "DES");
        }

        private void BQ_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BQ");
        }

        private void HCPI_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "HCPI");
        }

        private void BRC_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BRC");
        }

        private void EVT_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EVT");
        }

        private void ICN_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ICN");
        }

        private void EEG_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EEG");
        }

        private void EQRV_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EQRV");
        }

        private void PBAR_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "PBAR");
        }

        private void ANR_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ANR");
        }

        private void GIP5_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP5");
        }

        private void GIP30_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP30");
        }

        private void GR_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
            {
                if (_viewModel.SelectedUniverse == "USA")
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_USA");
                else
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_EUROPE");
            }
        }

        private void WGT_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = ValuesGrid.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "WGT");
        }

        private void ExpandAll_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ValuesGrid.ExpandAllGroups();
        }

        private void CollapseAll_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ValuesGrid.CollapseAllGroups();
        }

    }
}
