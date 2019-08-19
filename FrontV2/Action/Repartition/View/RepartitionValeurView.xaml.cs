using FrontV2.Action.Repartition.ViewModel;
using FrontV2.Utilities;
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
using Telerik.Windows.Controls.GridView;

namespace FrontV2.Action.Repartition.View
{
    /// <summary>
    /// Logique d'interaction pour RepartitionValeurView.xaml
    /// </summary>
    public partial class RepartitionValeurView : Window
    {
        private RepartitionValeurViewModel _viewmodel = new RepartitionValeurViewModel();

        public RepartitionValeurView()
        {
            _viewmodel = new RepartitionValeurViewModel();

            InitializeComponent();
            DataContext = _viewmodel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewmodel.LoadGrids();
        }

        private void RadGridValues_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            e.Column.CellStyleSelector = Application.Current.Resources["repartitionStyleSelector"] as StyleSelector;

            if (e.Column.Header.ToString() != "Ticker"
                && e.Column.Header.ToString() != "Company")
                e.Column.TextAlignment = TextAlignment.Right;

            if (_viewmodel.ShowGap)
            {
                if (e.Column.Header.ToString().Contains("Ecart"))
                    e.Column.IsVisible = true;
                else
                    e.Column.IsVisible = false;

                if (e.Column.Header.ToString() == "Ticker"
                || e.Column.Header.ToString() == "Company")
                    e.Column.IsVisible = true;
            }
            else
            {
                if (e.Column.Header.ToString().Contains("Ecart"))
                    e.Column.IsVisible = false;
                else
                    e.Column.IsVisible = true;

                if (e.Column.Header.ToString() == "Ticker"
                    || e.Column.Header.ToString() == "Company")
                    e.Column.IsVisible = true;
            }

            Helpers.AddToolTips(e.Column as GridViewDataColumn);
        }


        private void RadGridPositions_AutoGeneratingColumn(object sender,
            Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() != "Ticker"
                && e.Column.Header.ToString() != "Company")
                e.Column.TextAlignment = TextAlignment.Right;
            Helpers.AddToolTips(e.Column as GridViewDataColumn);
        }

        private void Gaps_Checked(object sender, RoutedEventArgs e)
        {
            _viewmodel.LoadGrids();
        }
    }
}
