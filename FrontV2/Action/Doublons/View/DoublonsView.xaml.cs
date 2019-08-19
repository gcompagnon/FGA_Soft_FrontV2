using FrontV2.Action.Doublons.Model;
using FrontV2.Action.Doublons.ViewModel;
using System.Collections.Generic;
using System.Windows;

namespace FrontV2.Action.Doublons.View
{
    /// <summary>
    /// Logique d'interaction pour DoublonsView.xaml
    /// </summary>
    public partial class DoublonsView : Window
    {
        DoublonsModel _model;
        DoublonsViewModel _vm;
        public DoublonsView()
        {
            InitializeComponent();

            _model = new DoublonsModel();
            _vm = new DoublonsViewModel();
            DataContext = _vm;
        }

        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Correct databas with selected item, update display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BKeep_Click(object sender, RoutedEventArgs e)
        {
            DoublonsViewModel.S_Stock GridSelectedItem = (DoublonsViewModel.S_Stock)DGStock.SelectedItem;

            List<DoublonsViewModel.S_Stock> deleted_stocks = new List<DoublonsViewModel.S_Stock>();
            foreach (DoublonsViewModel.S_Stock s in DGStock.Items)
            {
                if ((s.Name == GridSelectedItem.Name
                    || s.Isin == GridSelectedItem.Isin
                    || s.Ticker == GridSelectedItem.Ticker) && (GridSelectedItem != s))
                {
                    deleted_stocks.Add(s);
                }
                else
                {
                    s.IsSelected = false;
                }
            }
            foreach (DoublonsViewModel.S_Stock s in deleted_stocks)
            {
                ReplaceWith(s, GridSelectedItem);
                _vm.StockList.Remove(s);
            }
            _vm.StockList.Remove(GridSelectedItem);
        }

        /// <summary>
        /// Replace in the datase old values with new ticker, isin and/or company name
        /// </summary>
        /// <param name="oldStock"></param>
        /// <param name="NewStock"></param>
        private void ReplaceWith(DoublonsViewModel.S_Stock oldStock, DoublonsViewModel.S_Stock NewStock)
        {
            string name = oldStock.Name.Replace("'", "''");
            string isin = oldStock.Isin.Replace("'", "''");
            string ticker = oldStock.Ticker.Replace("'", "''");
            //string country = oldStock.Country.Replace("'", "''");
            string name2 = NewStock.Name.Replace("'", "''");
            string isin2 = NewStock.Isin.Replace("'", "''");
            string country2 = NewStock.Country.Replace("'", "''");
            string ticker2 = NewStock.Ticker.Replace("'", "''");
            string sql = null;

            sql = "UPDATE DATA_FACTSET" + " SET TICKER='" + ticker2 + "', COMPANY_NAME='" +
                name2 + "', ISIN='" + isin2 + "', COUNTRY ='" + country2 + "'" +
                " WHERE COMPANY_NAME='" + name + "' and ISIN='" + isin + "' and TICKER='" + ticker + "'";

            _model.ExecuteQuery(sql);
        }
    }
}
