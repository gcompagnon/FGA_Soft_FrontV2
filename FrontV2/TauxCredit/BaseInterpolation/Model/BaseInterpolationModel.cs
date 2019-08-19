using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Data;

namespace FrontV2.TauxCredit.BaseInterpolation.Model
{
    class BaseInterpolationModel
    {

        #region Fields
        private Connection co;

        #endregion

        #region Constructor

        public BaseInterpolationModel()
        {
            co = new Connection();
        }

        #endregion
        
        public RadObservableCollection<String> GetDates()
        {
            String sql = "select distinct DATE from TX_AGGREGATE_DATA order by Date";
            RadObservableCollection<String> collection = new RadObservableCollection<String>();
            if (co.IsOpen())
            {
                foreach (DateTime d in co.SqlWithReturn(sql))
                {
                    collection.Add(d.ToShortDateString());
                }
            }
            return collection;
        }
       
        public RadObservableCollection<int> GetMaturity()
        {
            String sql = "select distinct key4 from TX_AGGREGATE_DATA order by key4";
            RadObservableCollection<int> collection = new RadObservableCollection<int>();
            if (co.IsOpen())
            {
                foreach (object i in co.SqlWithReturn(sql))
                {
                    if (i.ToString() != "")
                        collection.Add(int.Parse(i.ToString()));
                }
            }
            return Sort(collection);
        }
        
        public RadObservableCollection<String> GetPays()
        {
            String sql = "select distinct key2 from TX_AGGREGATE_DATA WHERE key1 = 'YTM_I' order by key2";
            RadObservableCollection<String> collection = new RadObservableCollection<string>();
            if (co.IsOpen())
            {
                foreach (String d in co.SqlWithReturn(sql))
                {
                    collection.Add(d.ToString());
                }
            }
            return collection;
        }
        
        public RadObservableCollection<String> GetSource()
        {
            String sql = "select distinct key5 from TX_AGGREGATE_DATA";
            RadObservableCollection<String> collection = new RadObservableCollection<string>();
            if (co.IsOpen())
            {
                foreach (String s in co.SqlWithReturn(sql))
                {
                    collection.Add(s.ToString());
                }
            }
            return collection;
        }

        public DataTable getinterpol(string SelectedDate, string SelectedDate2, string SelectedPays2, int SelectedMaturity2, string SelectedSource)
        {
            String sql = "select distinct Date , key4 , Value as Rate from TX_AGGREGATE_DATA  where date between '" + SelectedDate + "'  and  '" + SelectedDate2 + "' and key4 = '" + SelectedMaturity2 + "' and key5 = '" + SelectedSource + "' and key2 = '" + SelectedPays2 + "' order by Date";
            DataTable dataT = new DataTable();
            List<object> tmp = co.sqlRequesttoDataTab3(sql);
            dataT.Columns.Add(new DataColumn("Date"));
            dataT.Columns.Add(new DataColumn("Maturity"));
            dataT.Columns.Add(new DataColumn("Rate"));
            foreach (object o in tmp)
            {
                DataRow tmpr = dataT.NewRow();
                tmpr[0] = ((Object[])o)[0].ToString();
                tmpr[1] = ((Object[])o)[1].ToString();
                tmpr[2] = ((Object[])o)[2].ToString();
                dataT.Rows.Add(tmpr);
            }

            return dataT;

        }

        public DataTable gettableau(string SelectedDate, string SelectedDate2, string SelectedPays1, int SelectefMaturity)
        {

            return null;
        }

        public DataTable getproceduretab2(String SelectedDate, String SelectedDate2, String SelectedPays2, int SelectefMaturity2)
        {
            //return co.ProcedureStockeeForDataGrid("Indicateur_Interpole", new List<string> { "@InputDate", "@InputDateF", "@InputPays", "@InputMaturity" }, new List<object> { SelectedDate, SelectedDate2, SelectedPays2, SelectefMaturity2 });
            return null;
        }

        public RadObservableCollection<int> Sort(RadObservableCollection<int> collection)
        {
            for (int j = 1; j < collection.Count; j++)
            {
                for (int i = j; i < collection.Count; i++)
                {
                    int a = collection[i - 1];
                    int b = collection[i];

                    if (a > b)
                    {
                        collection[i] = a;
                        collection[i - 1] = b;
                    }
                }
            }
            return collection;
        }
    }
}
