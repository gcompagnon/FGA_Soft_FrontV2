using FrontV2.Action.Consultation.ViewModel;
using FrontV2.Utilities;
using System;
using System.Data;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.Action.Consultation.View
{
    /// <summary>
    /// Logique d'interaction pour BaseActionConsultation.xaml
    /// </summary>
    public partial class BaseActionConsultation : Window
    {
        private BaseActionConsultationViewModel _viewModel;

        public BaseActionConsultation()
        {
            InitializeComponent();
            _viewModel = new BaseActionConsultationViewModel();
            this.DataContext = _viewModel;
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadDataExecute();
        }

        /// <summary>
        /// Print the current window ass best fit for a page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintScreen_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
            printDlg.PrintTicket.PageOrientation = PageOrientation.Landscape;

            if (printDlg.ShowDialog() == true)
            {
                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / this.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                                                                                                       this.ActualHeight);

                //Transform the Visual to scale
                this.LayoutTransform = new ScaleTransform(scale, scale);

                //get the size of the printer page
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                this.Measure(sz);
                this.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                //now print the visual to printer to fit on the one page.
                printDlg.PrintVisual(this, "Wpf controller");
            }
        }

        private void RadGridViewType1_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = Application.Current.Resources["consultationStyleSelector"] as StyleSelector;

            if (e.Column.Header.ToString() == "SuperSecteurId")
                e.Column.IsVisible = false;

            if (e.Column.Header.ToString() == "SecteurId")
                e.Column.IsVisible = false;

            if (e.Column.Header.ToString() == "Isin")
                e.Column.IsVisible = false;

            GridViewDataColumn col = e.Column as GridViewDataColumn;
            if (col.DataType == typeof(DoubleFormatted))
                col.TextAlignment = TextAlignment.Right;

            Helpers.AddToolTips(e.Column as GridViewDataColumn);
        }

        private void RadGridViewType2_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = Application.Current.Resources["consultationStyleSelector2"] as StyleSelector;

            if (e.Column.Header.ToString() == "SuperSecteurId")
                e.Column.IsVisible = false;

            if (e.Column.Header.ToString() == "SecteurId")
                e.Column.IsVisible = false;

            if (e.Column.Header.ToString() == "Isin")
                e.Column.IsVisible = false;

            GridViewDataColumn col = e.Column as GridViewDataColumn;
            if (col.DataType == typeof(DoubleFormatted))
                col.TextAlignment = TextAlignment.Right;

            Helpers.AddToolTips(e.Column as GridViewDataColumn);
        }

        /******************
             ContextMenu
        *******************/
        private void RadContextMenu_OpenedGeneral(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.General.SelectedItems.Clear();
                    this.General.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void RadContextMenu_OpenedCroissance(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.Croissance.SelectedItems.Clear();
                    this.Croissance.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void RadContextMenu_OpenedQuality(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.Quality.SelectedItems.Clear();
                    this.Quality.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void RadContextMenu_OpenedValorisation(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.Valorisation.SelectedItems.Clear();
                    this.Valorisation.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void RadContextMenu_OpenedMomentum(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.Momentum.SelectedItems.Clear();
                    this.Momentum.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void RadContextMenu_OpenedSynthese(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.Synthese.SelectedItems.Clear();
                    this.Synthese.SelectedItem = row;
                }
            }
            catch
            { }
        }

        /******************
             Click EXCEL
        *******************/
        private void OpenExcel_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = General.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenExcel(sector, industry, libelle);
        }

        private void OpenExcel_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Quality.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenExcel(sector, industry, libelle);
        }

        private void OpenExcel_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Croissance.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenExcel(sector, industry, libelle);
        }

        private void OpenExcel_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Valorisation.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenExcel(sector, industry, libelle);
        }

        private void OpenExcel_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Momentum.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenExcel(sector, industry, libelle);
        }

        private void OpenExcel_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Synthese.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenExcel(sector, industry, libelle);
        }

        /******************
              Click PDF
         *******************/
        private void OpenPDF_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = General.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenPDF(sector, industry, libelle);
        }

        private void OpenPDF_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Quality.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenPDF(sector, industry, libelle);
        }

        private void OpenPDF_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Croissance.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenPDF(sector, industry, libelle);
        }

        private void OpenPDF_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Valorisation.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenPDF(sector, industry, libelle);
        }

        private void OpenPDF_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Momentum.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenPDF(sector, industry, libelle);
        }

        private void OpenPDF_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Data.DataRow row = Synthese.SelectedItem as System.Data.DataRow;

            if (row == null)
                return;

            if (row["ISIN"].ToString() == "" || row["INDUSTRY"].ToString() == "")
                return;

            String libelle = row["COMPANY_NAME"].ToString();
            String industry = row["INDUSTRY"].ToString();
            String sector = row["SECTOR"].ToString();

            Helpers.OpenPDF(sector, industry, libelle);
        }

        /******************
            Go To Bloom
        *******************/

        #region General
        private void DES_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "DES");
        }

        private void BQ_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BQ");
        }

        private void HCPI_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "HCPI");
        }

        private void BRC_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BRC");
        }

        private void EVT_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EVT");
        }

        private void ICN_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ICN");
        }

        private void EEG_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EEG");
        }

        private void EQRV_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EQRV");
        }

        private void PBAR_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "PBAR");
        }

        private void ANR_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ANR");
        }

        private void GIP5_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP5");
        }

        private void GIP30_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP30");
        }

        private void GR_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
            {
                if (_viewModel.SelectedUniverse == "USA")
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_USA");
                else
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_EUROPE");
            }
        }

        private void WGT_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = General.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "WGT");
        }
        #endregion

        #region Croissance
        private void DES_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "DES");
        }

        private void BQ_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BQ");
        }

        private void HCPI_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "HCPI");
        }

        private void BRC_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BRC");
        }

        private void EVT_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EVT");
        }

        private void ICN_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ICN");
        }

        private void EEG_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EEG");
        }

        private void EQRV_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EQRV");
        }

        private void PBAR_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "PBAR");
        }

        private void ANR_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ANR");
        }

        private void GIP5_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP5");
        }

        private void GIP30_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP30");
        }

        private void GR_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
            {
                if (_viewModel.SelectedUniverse == "USA")
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_USA");
                else
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_EUROPE");
            }
        }

        private void WGT_ClickCroissance(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Croissance.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "WGT");
        }
        #endregion

        #region Quality
        private void DES_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "DES");
        }

        private void BQ_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BQ");
        }

        private void HCPI_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "HCPI");
        }

        private void BRC_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BRC");
        }

        private void EVT_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EVT");
        }

        private void ICN_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ICN");
        }

        private void EEG_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EEG");
        }

        private void EQRV_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EQRV");
        }

        private void PBAR_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "PBAR");
        }

        private void ANR_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ANR");
        }

        private void GIP5_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP5");
        }

        private void GIP30_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP30");
        }

        private void GR_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
            {
                if (_viewModel.SelectedUniverse == "USA")
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_USA");
                else
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_EUROPE");
            }
        }

        private void WGT_ClickQuality(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Quality.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "WGT");
        }
        #endregion

        #region Valorisation
        private void DES_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "DES");
        }

        private void BQ_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BQ");
        }

        private void HCPI_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "HCPI");
        }

        private void BRC_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BRC");
        }

        private void EVT_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EVT");
        }

        private void ICN_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ICN");
        }

        private void EEG_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EEG");
        }

        private void EQRV_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EQRV");
        }

        private void PBAR_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "PBAR");
        }

        private void ANR_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ANR");
        }

        private void GIP5_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP5");
        }

        private void GIP30_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP30");
        }

        private void GR_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
            {
                if (_viewModel.SelectedUniverse == "USA")
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_USA");
                else
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_EUROPE");
            }
        }

        private void WGT_ClickValorisation(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Valorisation.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "WGT");
        }
        #endregion

        #region Momentum
        private void DES_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "DES");
        }

        private void BQ_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BQ");
        }

        private void HCPI_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "HCPI");
        }

        private void BRC_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BRC");
        }

        private void EVT_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EVT");
        }

        private void ICN_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ICN");
        }

        private void EEG_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EEG");
        }

        private void EQRV_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EQRV");
        }

        private void PBAR_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "PBAR");
        }

        private void ANR_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ANR");
        }

        private void GIP5_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP5");
        }

        private void GIP30_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP30");
        }

        private void GR_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
            {
                if (_viewModel.SelectedUniverse == "USA")
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_USA");
                else
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_EUROPE");
            }
        }

        private void WGT_ClickMomentum(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Momentum.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "WGT");
        }
        #endregion

        #region Synthese
        private void DES_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "DES");
        }

        private void BQ_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BQ");
        }

        private void HCPI_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "HCPI");
        }

        private void BRC_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "BRC");
        }

        private void EVT_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EVT");
        }

        private void ICN_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ICN");
        }

        private void EEG_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EEG");
        }

        private void EQRV_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "EQRV");
        }

        private void PBAR_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "PBAR");
        }

        private void ANR_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "ANR");
        }

        private void GIP5_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP5");
        }

        private void GIP30_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "GIP30");
        }

        private void GR_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
            {
                if (_viewModel.SelectedUniverse == "USA")
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_USA");
                else
                    Helpers.ToBloomberg(row["TICkER"].ToString(), "GR_EUROPE");
            }
        }

        private void WGT_ClickSynthese(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DataRow row = Synthese.SelectedItem as DataRow;
            if (row != null)
                Helpers.ToBloomberg(row["TICkER"].ToString(), "WGT");
        }
        #endregion

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            General.ExpandAllGroups();
        }

        private void Collapse_Click(object sender, RoutedEventArgs e)
        {
            General.CollapseAllGroups();
        }

        private void ExpandAll_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TabGeneral.IsSelected)
                General.ExpandAllGroups();
            else if (TabCroissance.IsSelected)
                Croissance.ExpandAllGroups();
            else if (TabValorisation.IsSelected)
                Valorisation.ExpandAllGroups();
            else if (TabMomentum.IsSelected)
                Momentum.ExpandAllGroups();
            else if (TabQuality.IsSelected)
                Quality.ExpandAllGroups();
            else if (TabSynthese.IsSelected)
                Synthese.ExpandAllGroups();
        }

        private void CollapseAll_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TabGeneral.IsSelected)
                General.CollapseAllGroups();
            else if (TabCroissance.IsSelected)
                Croissance.CollapseAllGroups();
            else if (TabValorisation.IsSelected)
                Valorisation.CollapseAllGroups();
            else if (TabMomentum.IsSelected)
                Momentum.CollapseAllGroups();
            else if (TabQuality.IsSelected)
                Quality.CollapseAllGroups();
            else if (TabSynthese.IsSelected)
                Synthese.CollapseAllGroups();
        }
    }
}