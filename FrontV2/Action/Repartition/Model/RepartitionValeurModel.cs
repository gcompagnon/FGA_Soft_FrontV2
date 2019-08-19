using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Data;

namespace FrontV2.Action.Repartition.Model
{
    class RepartitionValeurModel
    {
        private Connection _connection;

        public RepartitionValeurModel()
        {
            _connection = new Connection();
        }

        public RadObservableCollection<String> GetDates()
        {
            return _connection.GetDates();
        }

        public RadObservableCollection<String> GetAllTickers()
        {
            List<object> tmp = _connection.SqlWithReturn("SELECT TICKER FROM DATA_FACTSET WHERE DATE = (SELECT MAX(DATE) FROM DATA_FACTSET)");

            RadObservableCollection<String> res = new RadObservableCollection<string>();
            foreach (var v in tmp)
                res.Add(v.ToString());

            return res;
        }

        public String GetISINFromTicker(String ticker)
        {
            return _connection.GetISINFromTicker(ticker);
        }

        public DataTable GetValueDataSource(String date, String ticker)
        {
            return _connection.ProcedureStockeeForDataGrid("ACT_RepartitionValeur",
                new List<String> { "@date", "@ticker" },
                new List<object> { date, ticker });
        }

        public DataTable GetPositionsDataSource(String date, String ticker)
        {
             return _connection.ProcedureStockeeForDataGrid("ACT_RepartitionValeurPosition",
                new List<String> { "@date", "@ticker", "@isin" },
                new List<object> { date, ticker, GetISINFromTicker(ticker) });
        }
    }
}
