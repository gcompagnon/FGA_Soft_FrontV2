using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Data;

namespace FrontV2.TauxCredit.Indicateurs.Model
{
    class IndicateurModel
    {
        Connection _connection;

        public IndicateurModel()
        {
            _connection = new Connection();
        }

        public RadObservableCollection<String> getIsin()
        {
            String sql = "select distinct ISINId from ref_security.PRICE where Price_Source = 'IBOXX_EUR' or Price_Source = 'BARCLAYS' order by ISINId";
            RadObservableCollection<String> collection = new RadObservableCollection<String>();
            if (_connection.IsOpen())
            {
                foreach (String d in _connection.SqlWithReturn(sql))
                {
                    collection.Add(d.ToString());
                }
            }
            return collection;
        }

        public RadObservableCollection<String> getDateD()
        {
            string sql = "select distinct DATE from TX_AGGREGATE_DATA order by Date";
            RadObservableCollection<String> collection = new RadObservableCollection<String>();
            if (_connection.IsOpen())
            {
                foreach (DateTime d in _connection.SqlWithReturn(sql))
                {
                    collection.Add(d.ToShortDateString());
                }
            }
            return collection;
        }

        public RadObservableCollection<String> getDateF()
        {
            string sql = "select distinct DATE from TX_AGGREGATE_DATA order by Date";
            RadObservableCollection<String> collection = new RadObservableCollection<String>();
            if (_connection.IsOpen())
            {
                foreach (DateTime d in _connection.SqlWithReturn(sql))
                {
                    collection.Add(d.ToShortDateString());
                }
            }
            return collection;
        }

        public RadObservableCollection<String> getSource()
        {
            String sql = "select distinct key5 from TX_AGGREGATE_DATA";
            RadObservableCollection<String> collection = new RadObservableCollection<string>();
            if (_connection.IsOpen())
            {
                foreach (String s in _connection.SqlWithReturn(sql))
                {
                    collection.Add(s.ToString());
                }
            }
            return collection;
        }

        public DataTable GetIndicateurs(String isin, String dateD, String dateF, String Source)
        {
            String sql = "select * INTO #TMP from TX_YTM_Indicateurs ('" + dateD + "', '" + dateF + "' , '" + isin + "', NULL, '" + Source + "')"
                          + "SELECT * FROM #TMP " + "where  date = (SELECT MAX(DATE) FROM #TMP)" + " drop table #TMP";
            DataTable dataT = new DataTable();
            List<object> tmp = _connection.RequeteSqltoDataTab(sql);

            dataT.Columns.Add(new DataColumn(" Date"));
            dataT.Columns.Add(new DataColumn("ISIN"));
            dataT.Columns.Add(new DataColumn("Source"));
            dataT.Columns.Add(new DataColumn("Close"));
            dataT.Columns.Add(new DataColumn("MobileAvg3M"));
            dataT.Columns.Add(new DataColumn("MobileAvg6M"));
            dataT.Columns.Add(new DataColumn("HistAvg"));
            dataT.Columns.Add(new DataColumn("PeriodAvg"));
            dataT.Columns.Add(new DataColumn("MobileVol3M"));
            dataT.Columns.Add(new DataColumn("MobileVol6M"));
            dataT.Columns.Add(new DataColumn("PeriodVol"));
            dataT.Columns.Add(new DataColumn("MobileZscore3M"));
            dataT.Columns.Add(new DataColumn("MobileZscore6M"));
            dataT.Columns.Add(new DataColumn("Zscore"));
            dataT.Columns.Add(new DataColumn("Max"));
            dataT.Columns.Add(new DataColumn("Min"));
            dataT.Columns.Add(new DataColumn("Max 5%"));
            dataT.Columns.Add(new DataColumn("Max 5% Date"));
            dataT.Columns.Add(new DataColumn("min 5%"));
            dataT.Columns.Add(new DataColumn("min 5% Date"));

            foreach (object o in tmp)
            {
                DataRow tmpr = dataT.NewRow();
                tmpr[0] = ((Object[])o)[0].ToString();
                tmpr[1] = ((Object[])o)[1].ToString();
                tmpr[2] = ((Object[])o)[3].ToString();
                tmpr[3] = ((Object[])o)[4].ToString();
                tmpr[4] = ((Object[])o)[5].ToString();
                tmpr[5] = ((Object[])o)[6].ToString();
                tmpr[6] = ((Object[])o)[7].ToString();
                tmpr[7] = ((Object[])o)[8].ToString();
                tmpr[8] = ((Object[])o)[9].ToString();
                tmpr[9] = ((Object[])o)[10].ToString();
                tmpr[10] = ((Object[])o)[11].ToString();
                tmpr[11] = ((Object[])o)[12].ToString();
                tmpr[12] = ((Object[])o)[13].ToString();
                tmpr[13] = ((Object[])o)[14].ToString();
                tmpr[14] = ((Object[])o)[15].ToString();
                tmpr[15] = ((Object[])o)[16].ToString();
                tmpr[16] = ((Object[])o)[17].ToString();
                tmpr[17] = ((Object[])o)[18].ToString();
                tmpr[18] = ((Object[])o)[19].ToString();
                tmpr[19] = ((Object[])o)[20].ToString();
                dataT.Rows.Add(tmpr);
            }

            return dataT;
        }

        public DataTable GetGraphProcedure(string selectedIsin, string selectedDateD, 
            string selectedDateF, string Source, String critere)
        {
            String sql = " select date, " + critere + " from dbo.TX_YTM_Indicateurs ('" + selectedDateD + "', '" + selectedDateF + "' , '" + selectedIsin + "', NULL, '" + Source + "')";
            DataTable dataT = new DataTable();
            List<object> tmp = _connection.RequeteSqltoDataTab2(sql);
            dataT.Columns.Add(new DataColumn("Date"));
            dataT.Columns.Add(new DataColumn("Valeur"));

            foreach (object o in tmp)
            {
                DataRow tmpr = dataT.NewRow();
                tmpr[0] = ((Object[])o)[0].ToString();
                tmpr[1] = ((Object[])o)[1].ToString();

                dataT.Rows.Add(tmpr);
            }

            return dataT;
        }
       
    }
}
