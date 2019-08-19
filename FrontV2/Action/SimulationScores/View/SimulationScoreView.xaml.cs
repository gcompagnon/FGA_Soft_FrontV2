using FrontV2.Action.ScoreChange.View;
using FrontV2.Action.SimulationScore.ViewModel;
using FrontV2.Utilities;
using System;
using System.Data;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.Action.SimulationScore.View
{
    /// <summary>
    /// Logique d'interaction pour Open.xaml
    /// </summary>
    public partial class SimulationScoreView : Window
    {
        SimulationScoreViewModel vm;

        public SimulationScoreView()
        {

            InitializeComponent();

            vm = new SimulationScoreViewModel();
            vm.StartCheck();
            DataContext = vm;
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
            e.Column.CellStyleSelector = Application.Current.Resources["simulationScoreStyleSelector"] as StyleSelector;

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

        private void OpenCoef_Click(object sender, RoutedEventArgs e)
        {
            WindowsApplication1.Action.Coefficient.BaseActionCoefSecteur coefs = new WindowsApplication1.Action.Coefficient.BaseActionCoefSecteur(true);
            coefs.ShowDialog();
        }

        private void CalculateScores_Click(object sender, RoutedEventArgs e)
        {
            vm.CalculateScores();
        }

        private void LoadScore_Click(object sender, RoutedEventArgs e)
        {
            vm.LoadValuesExecute();
        }

    }
}