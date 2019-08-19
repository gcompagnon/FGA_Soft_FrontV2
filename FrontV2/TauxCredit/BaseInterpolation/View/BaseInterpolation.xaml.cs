using FrontV2.TauxCredit.BaseInterpolation.ViewModel;
using System;
using System.Collections.Generic;
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

namespace FrontV2.TauxCredit.BaseInterpolation.View
{
    /// <summary>
    /// Logique d'interaction pour BaseInterpolation.xaml
    /// </summary>
    public partial class BaseInterpolation : Window
    {
        private BaseInterpolationViewModel _viewModel;

        public BaseInterpolation()
        {
            InitializeComponent();
            _viewModel = new BaseInterpolationViewModel();

            this.DataContext = _viewModel;

        }


        //Affiche de la date sql to c#
        private void RadGridView_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = Application.Current.Resources["indicateurStyleSelector"] as StyleSelector;
            GridViewDataColumn dd = (GridViewDataColumn)e.Column;
            if (dd.Header.ToString() == "Date")
                dd.DataFormatString = "dd/MM/yyyy";
        }

        private void Interpolation_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.InterpolationExceute();

            _viewModel.fillDataGrid();
            _viewModel.showChart();
            _viewModel.showChart2();
            _viewModel.showChart3(_viewModel.DataList, _viewModel.DataList2);
            _viewModel.showZero();
            // CategoricalAxis Intervalle pour les dates
            if (_viewModel.DataList.Count() > 35)
                telerik.HorizontalAxis.LabelInterval = _viewModel.DataList.Count() / 35;
            else
                telerik.HorizontalAxis.LabelInterval = 1;
            if (_viewModel.DataList2.Count() > 35)
                telerik.HorizontalAxis.LabelInterval = _viewModel.DataList2.Count() / 35;
            else
                telerik.HorizontalAxis.LabelInterval = 1;
            if (_viewModel.DataList3.Count() > 35)
                Spread.HorizontalAxis.LabelInterval = _viewModel.DataList3.Count() / 35;
            else
                Spread.HorizontalAxis.LabelInterval = 1;
        }

    }
}
