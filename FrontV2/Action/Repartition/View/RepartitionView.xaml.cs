using FrontV2.Action.Repartition.ViewModel;
using FrontV2.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace FrontV2.Action.Repartition.View
{
    /// <summary>
    /// Logique d'interaction pour RepartitionView.xaml
    /// </summary>
    public partial class RepartitionView : Window
    {
        RepartitionViewModel _viewModel;
        String _selectedIndustry = "";

        public RepartitionView()
        {
            InitializeComponent();

            _viewModel = new RepartitionViewModel();
            DataContext = _viewModel;
        }

        private void Sectors_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {          
            e.Column.CellStyleSelector = Application.Current.Resources["repartitionStyleSelector"] as StyleSelector;

            if (e.Column.Header.ToString() != "SECTOR GICS")
                e.Column.TextAlignment = TextAlignment.Right;

            if (_viewModel.ShowGap)
            {
                if (e.Column.Header.ToString().Contains("Ecart"))
                    e.Column.IsVisible = true;
                else
                    e.Column.IsVisible = false;

                if (e.Column.Header.ToString() == "SECTOR GICS")
                    e.Column.IsVisible = true;
            }
            else
            {
                if (e.Column.Header.ToString().Contains("Ecart"))
                    e.Column.IsVisible = false;
                else
                    e.Column.IsVisible = true;

                if (e.Column.Header.ToString() == "SECTOR GICS")
                    e.Column.IsVisible = true;
            }
            Helpers.AddToolTips(e.Column as GridViewDataColumn);
        }

        private void Countries_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {

            e.Column.CellStyleSelector = Application.Current.Resources["repartitionStyleSelector"] as StyleSelector;

            if (e.Column.Header.ToString() != "COUNTRY")
                e.Column.TextAlignment = TextAlignment.Right;

            if (_viewModel.ShowGap)
            {
                if (e.Column.Header.ToString().Contains("Ecart"))
                    e.Column.IsVisible = true;
                else
                    e.Column.IsVisible = false;

                if (e.Column.Header.ToString() == "COUNTRY")
                    e.Column.IsVisible = true;
            }
            else
            {
                if (e.Column.Header.ToString().Contains("Ecart"))
                    e.Column.IsVisible = false;
                else
                    e.Column.IsVisible = true;

                if (e.Column.Header.ToString() == "COUNTRY")
                    e.Column.IsVisible = true;
            }
            Helpers.AddToolTips(e.Column as GridViewDataColumn);
        }

        private void Industries_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = Application.Current.Resources["repartitionStyleSelector"] as StyleSelector;

            if (e.Column.Header.ToString() != "INDUSTRY")
                e.Column.TextAlignment = TextAlignment.Right;

            if (_viewModel.ShowGap)
            {
                if (e.Column.Header.ToString().Contains("Ecart"))
                    e.Column.IsVisible = true;
                else
                    e.Column.IsVisible = false;

                if (e.Column.Header.ToString() == "INDUSTRY")
                    e.Column.IsVisible = true;
            }
            else
            {
                if (e.Column.Header.ToString().Contains("Ecart"))
                    e.Column.IsVisible = false;
                else
                    e.Column.IsVisible = true;

                if (e.Column.Header.ToString() == "INDUSTRY")
                    e.Column.IsVisible = true;
            }

            Helpers.AddToolTips(e.Column as GridViewDataColumn);

        }

        private void Gaps_Checked(object sender, RoutedEventArgs e)
        {
            ///////////// SectorsRadGridView
            foreach (GridViewDataColumn col in SectorsRadGridView.Columns)
            {
                if (_viewModel.ShowGap)
                {
                    if (col.Header.ToString().Contains("Ecart"))
                        col.IsVisible = true;
                    else
                        col.IsVisible = false;

                    if (col.Header.ToString() == "SECTOR GICS")
                        col.IsVisible = true;
                }
                else
                {
                    if (col.Header.ToString().Contains("Ecart"))
                        col.IsVisible = false;
                    else
                        col.IsVisible = true;

                    if (col.Header.ToString() == "SECTOR GICS")
                        col.IsVisible = true;
                }
            }

            ///////////// CountriesRadGridView
            foreach (GridViewDataColumn col in CountriesRadGridView.Columns)
            {
                if (_viewModel.ShowGap)
                {
                    if (col.Header.ToString().Contains("Ecart"))
                        col.IsVisible = true;
                    else
                        col.IsVisible = false;

                    if (col.Header.ToString() == "COUNTRY")
                        col.IsVisible = true;
                }
                else
                {
                    if (col.Header.ToString().Contains("Ecart"))
                        col.IsVisible = false;
                    else
                        col.IsVisible = true;

                    if (col.Header.ToString() == "COUNTRY")
                        col.IsVisible = true;
                }
            }

            ///////////// IndustriesRadGridView
            foreach (GridViewDataColumn col in IndustriesRadGridView.Columns)
            {
                if (_viewModel.ShowGap)
                {
                    if (col.Header.ToString().Contains("Ecart"))
                        col.IsVisible = true;
                    else
                        col.IsVisible = false;

                    if (col.Header.ToString() == "INDUSTRY")
                        col.IsVisible = true;
                }
                else
                {
                    if (col.Header.ToString().Contains("Ecart"))
                        col.IsVisible = false;
                    else
                        col.IsVisible = true;

                    if (col.Header.ToString() == "INDUSTRY")
                        col.IsVisible = true;
                }
            }
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
                    this.SectorsRadGridView.SelectedItems.Clear();
                    this.SectorsRadGridView.SelectedItem = row;
                }
            }
            catch
            { }
        }

        private void EvolIndustries_ClickGeneral(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (_viewModel.IsBusy)
            {
                MessageBox.Show("Veuillez attendre la fin du chargement des graphiques");
                return;
            }

            DataRow row = SectorsRadGridView.SelectedItem as DataRow;

            if (row != null)
            {
                _viewModel.FillIndustriesChart(row["SECTOR GICS"].ToString());
                _selectedIndustry = row["SECTOR GICS"].ToString();
            }
        }

        private void CountryCopy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = _viewModel.SelectedDate.Substring(6, 4);
            String month = _viewModel.SelectedDate.Substring(3, 2); ;
            String day = _viewModel.SelectedDate.Substring(0, 2);

            String nom = year + month + day + "_Repartition_Country_" + _viewModel.SelectedPortefeuille;

            Helpers.ExportToBMP(nom, CountryGroup);
        }
      
        private void SectorsCopy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = _viewModel.SelectedDate.Substring(6, 4);
            String month = _viewModel.SelectedDate.Substring(3, 2); ;
            String day = _viewModel.SelectedDate.Substring(0, 2);

            String nom = year + month + day + "_Repartition_Sectors_" + _viewModel.SelectedPortefeuille;

            Helpers.ExportToBMP(nom, SectorsGroup);
        }

        private void IndustriesCopy(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            String year = _viewModel.SelectedDate.Substring(6, 4);
            String month = _viewModel.SelectedDate.Substring(3, 2); ;
            String day = _viewModel.SelectedDate.Substring(0, 2);

            String nom = year + month + day + "_Repartition_Industries_" + _selectedIndustry + "_" + _viewModel.SelectedPortefeuille;

            Helpers.ExportToBMP(nom, IndustriesGroup);
        }

    }
}
