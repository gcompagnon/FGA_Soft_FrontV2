using FrontV2.Action.ScoreChange.View;
using FrontV2.Action.ScoreReco.ViewModel;
using FrontV2.Utilities;
using System;
using System.Data;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.Action.ScoreReco.View
{
    /// <summary>
    /// Logique d'interaction pour Open.xaml
    /// </summary>
    public partial class ScoreRecoView : Window
    {
        ScoreRecoViewModel vm;

        public ScoreRecoView()
        {
            
            InitializeComponent();

            vm = new ScoreRecoViewModel();
            DataContext = vm;
        }
        
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ScoreChangeView scoreChange = new ScoreChangeView();
            scoreChange.Show();
        }    

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

        private void ExportToPDF_Button_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MessageBox.Show("Pas Encore Implementé");
        }

        private void Coefficient_Button_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            WindowsApplication1.Action.Coefficient.BaseActionCoefSecteur coef1 = new WindowsApplication1.Action.Coefficient.BaseActionCoefSecteur();
            coef1.Show();
            WindowsApplication1.Action.Coefficient.BaseActionCoefIndice coef2 = new WindowsApplication1.Action.Coefficient.BaseActionCoefIndice();
            coef2.Show();
        }

        private void RadGridView_AutoGeneratingColumn(object sender, 
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = Application.Current.Resources["scoreStyleSelector"] as StyleSelector;

            if (e.Column.Header.ToString() != "Ticker" 
                && e.Column.Header.ToString() != "Company Name"
                && e.Column.Header.ToString() != "Crncy"
                && e.Column.Header.ToString() != "liquidity"
                && e.Column.Header.ToString() != "Country")
                e.Column.TextAlignment = TextAlignment.Right;
                            
            if (e.Column.Header.ToString() == "EBIT_MARGIN_NTM")
                e.Column.DisplayIndex = 21;
            if (e.Column.Header.ToString() == "NET_DEBT_EBITDA_NTM")
                e.Column.DisplayIndex = 22;
            if (e.Column.Header.ToString() == "ROE_NTM")
                e.Column.DisplayIndex = 23;

            if (e.Column.Header.ToString() == "Quint Quant")
                e.Column.IsVisible = false;

            if (vm.SelectedSuperSector != "Financials")
            {
                if (e.Column.Header.ToString() == "PBT_SALES_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "PBT_RWA_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "COST_INCOME_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "P_TBV_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "P_TBV_ON_MED5Y")
                    e.Column.IsVisible = false;
            }
            else if (vm.SelectedSector == "Banks")
            {
                if (e.Column.Header.ToString() == "PBT_SALES_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "EBIT_MARGIN_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "NET_DEBT_EBITDA_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "ROE_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "PB_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "PB_ON_MEd5Y")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "FCF_TREND")
                    e.Column.IsVisible = false;
            }
            else if (vm.SelectedSector == "Insurance")
            {
                if (e.Column.Header.ToString() == "EBIT_MARGIN_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "PBT_RWA_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "COST_INCOME_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "ROE_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "NET_DEBT_EBITDA_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "P_TBV_NTM")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "P_TBV_ON_MED5Y")
                    e.Column.IsVisible = false;
                if (e.Column.Header.ToString() == "FCF_TREND")
                    e.Column.IsVisible = false;
            }
        }

        private void RadContextMenu_OpenedGeneral(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            try
            {
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {

                    System.Data.DataRow row = cell.ParentRow.DataContext as System.Data.DataRow;
                    this.ScoreGrid.SelectedItems.Clear();
                    this.ScoreGrid.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void SelectValue1_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (vm.IsBusy2)
            {
                MessageBox.Show("Attendez la fin du chargement de la première valeur");
                return;
            }
            DataRow row = ScoreGrid.SelectedItem as DataRow;
            
            if (row == null)
                return;

            String ticker = row["Ticker"].ToString();
            String company = "";

            foreach (String s in vm.Companies)
                if (s.Contains(" " + ticker))
                {
                    company = s;
                    break;
                }
            vm.SelectedCompany1 = company;

        }

        private void SelectValue2_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (vm.IsBusy2)
            {
                MessageBox.Show("Attendez la fin du chargement de la première valeur");
                return;
            }
            DataRow row = ScoreGrid.SelectedItem as DataRow;

            if (row == null)
                return;

            String ticker = row["Ticker"].ToString();
            String company = "";

            foreach (String s in vm.Companies)
                if (s.Contains(" " + ticker))
                {
                    company = s;
                    break;
                }
            vm.SelectedCompany2 = company;
        }

        private void HistoBarsCopy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = vm.SelectedDate.Substring(6, 4);
            String month = vm.SelectedDate.Substring(3, 2); ;
            String day = vm.SelectedDate.Substring(0, 2);

            String nom = year + month + day 
                + "_Scores_Histo_"
                + vm.Ticker1 + "_" + vm.Ticker2;

            Helpers.ExportToBMP(nom, HistoPanel);
        }

        private void RadChart1Copy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = vm.SelectedDate.Substring(6, 4);
            String month = vm.SelectedDate.Substring(3, 2); ;
            String day = vm.SelectedDate.Substring(0, 2);
            
            String nom = year + month + day
                + "_Scores_Growth_"
                + vm.Ticker1 + "_" + vm.Ticker2;

            Helpers.ExportToBMP(nom, GrowthPanel);
        }

        private void RadChart2Copy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = vm.SelectedDate.Substring(6, 4);
            String month = vm.SelectedDate.Substring(3, 2); ;
            String day = vm.SelectedDate.Substring(0, 2);

            String nom = year + month + day
                + "_Scores_Value_"
                + vm.Ticker1 + "_" + vm.Ticker2;

            Helpers.ExportToBMP(nom, ValuePanel);
        }

        private void RadChart3Copy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            String year = vm.SelectedDate.Substring(6, 4);
            String month = vm.SelectedDate.Substring(3, 2); ;
            String day = vm.SelectedDate.Substring(0, 2);

            String nom = year + month + day
                + "_Scores_Profit_"
                + vm.Ticker1 + "_" + vm.Ticker2;

            Helpers.ExportToBMP(nom, ProfitPanel);
        }

        private void RadChart4Copy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = vm.SelectedDate.Substring(6, 4);
            String month = vm.SelectedDate.Substring(3, 2); ;
            String day = vm.SelectedDate.Substring(0, 2);

            String nom = year + month + day
                + "_Scores_Quintiles_"
                + vm.Ticker1;

            Helpers.ExportToBMP(nom, RadChart4);
        }

        private void RadChart5Copy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = vm.SelectedDate.Substring(6, 4);
            String month = vm.SelectedDate.Substring(3, 2); ;
            String day = vm.SelectedDate.Substring(0, 2);

            String nom = year + month + day
                + "_Scores_Quintiles_"
                + vm.Ticker2;

            Helpers.ExportToBMP(nom, RadChart5);
        }

        private void LoadScores_Click(object sender, RoutedEventArgs e)
        {
            vm.LoadValuesExecute();
        }
    }
}