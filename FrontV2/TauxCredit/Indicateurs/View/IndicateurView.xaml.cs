using FrontV2.TauxCredit.Indicateurs.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace FrontV2.TauxCredit.Indicateurs.View
{
    /// <summary>
    /// Logique d'interaction pour IndicateurView.xaml
    /// </summary>
    public partial class IndicateurView : Window
    {
        private IndicateurViewModel _viewModelGraph;

        public IndicateurView()
        {
            InitializeComponent();
            _viewModelGraph = new IndicateurViewModel();
            this.DataContext = _viewModelGraph;
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
      
        private void Indicateur_Click(object sender, RoutedEventArgs e)
        {
            _viewModelGraph.IndicateurExcute();
        }

        void btnExport_Click(object sender, RoutedEventArgs e)
        {
            string extension = "xls";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    RadGrid.Export(stream,
                     new GridViewExportOptions()
                     {
                         Format = ExportFormat.Html,
                         ShowColumnHeaders = true,
                         ShowColumnFooters = true,
                         ShowGroupFooters = false,
                     });
                }
            }
        }

        private void Graph_Click(object sender, RoutedEventArgs e)
        {
            int maxPoint = 0;
            _viewModelGraph.fillDataGrid();
            _viewModelGraph.showChart();
            _viewModelGraph.showChart2();
            _viewModelGraph.showChart3();
            _viewModelGraph.showChart4();
            _viewModelGraph.showChart5();
            _viewModelGraph.showChart6();
            _viewModelGraph.showChart7();
            _viewModelGraph.showChart8();
            _viewModelGraph.showChart9();
            _viewModelGraph.showChart10();
            _viewModelGraph.showChart11();
            _viewModelGraph.showChart12();
            _viewModelGraph.showZero();

            //Intervalle pour l'axe des dates

            if (_viewModelGraph.DataList.Count() > 35 && _viewModelGraph.DataList.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList.Count() / 35;
            }

            if (_viewModelGraph.DataList2.Count() > 35 && _viewModelGraph.DataList2.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList2.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList2.Count() / 35;
            }
            if (_viewModelGraph.DataList3.Count() > 35 && _viewModelGraph.DataList3.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList3.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList3.Count() / 35;
            }
            if (_viewModelGraph.DataList4.Count() > 35 && _viewModelGraph.DataList4.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList4.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList4.Count() / 35;
            }
            if (_viewModelGraph.DataList5.Count() > 35 && _viewModelGraph.DataList5.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList5.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList5.Count() / 35;
            }
            if (_viewModelGraph.DataList6.Count() > 35 && _viewModelGraph.DataList6.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList6.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList6.Count() / 35;
            }
            if (_viewModelGraph.DataList7.Count() > 35 && _viewModelGraph.DataList7.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList7.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList7.Count() / 35;
            }
            if (_viewModelGraph.DataList8.Count() > 35 && _viewModelGraph.DataList8.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList8.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList8.Count() / 35;
            }
            if (_viewModelGraph.DataList9.Count() > 35 && _viewModelGraph.DataList9.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList9.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList9.Count() / 35;
            }
            if (_viewModelGraph.DataList10.Count() > 35 && _viewModelGraph.DataList10.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList10.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList10.Count() / 35;
            }
            if (_viewModelGraph.DataList11.Count() > 35 && _viewModelGraph.DataList11.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList11.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList11.Count() / 35;
            }
            if (_viewModelGraph.DataList12.Count() > 35 && _viewModelGraph.DataList12.Count() > maxPoint)
            {
                maxPoint = _viewModelGraph.DataList12.Count();
                telerik.HorizontalAxis.LabelInterval = _viewModelGraph.DataList12.Count() / 35;
            }
        }

    }
}
