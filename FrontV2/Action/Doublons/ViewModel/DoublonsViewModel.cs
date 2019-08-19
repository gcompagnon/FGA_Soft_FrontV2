using FrontV2.Action.Doublons.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace FrontV2.Action.Doublons.ViewModel
{
    class DoublonsViewModel : ViewModelBase,INotifyPropertyChanged
    {
        #region Constructor | Inner Class

        public DoublonsViewModel()
        {
            _model = new DoublonsModel();

            FillStocks();
        }

        /// <summary>
        /// Inner Class to represent the Object displayed in the datatable and for an easier way to correct the database.
        /// </summary>
        public class S_Stock
        {
            public String Date { get; set; }
            public String Name { get; set; }
            public String Isin { get; set; }
            public String Country { get; set; }
            public String Ticker { get; set; }
            public bool IsSelected { get; set; }
            public bool Done { get; set; }

            public S_Stock(String date, String name, String isin, String country, String ticker)
            {
                Date = date;
                Name = name;
                Isin = isin;
                Country = country;
                Ticker = ticker;
                IsSelected = false;
                Done = false;
            }

            /// <summary>
            /// Overloading of the equal operator
            /// </summary>
            /// <param name="s1"></param>
            /// <param name="s2"></param>
            /// <returns></returns>
            public static bool operator ==(S_Stock s1, S_Stock s2)
            {
                return s1.Name == s2.Name
                    && s1.Isin == s2.Isin
                    && s1.Ticker == s2.Ticker
                    && s1.Country == s2.Country;
            }

            /// <summary>
            /// Overloading of the different operator
            /// </summary>
            /// <param name="s1"></param>
            /// <param name="s2"></param>
            /// <returns></returns>
            public static bool operator !=(S_Stock s1, S_Stock s2)
            {
                return !(s1 == s2);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if( obj is S_Stock )
                {
                    return this == ((S_Stock)obj);
                }
                else
                    return false;
            }
        }

        #endregion

        #region Fields

        readonly DoublonsModel _model;

        #region String FIND_DOUBLE_EQUITY

        public const String FIND_DOUBLE_EQUITY_SQL =
                " DECLARE @lastdate AS DATETIME" +
                " SET @lastdate = (SELECT MAX(DATE) FROM DATA_FACTSET) - 3" +
                " DECLARE @sql AS	VARCHAR(max)" +
                " SELECT distinct x.COMPANY_NAME, x.ISIN, x.TICKER" +
                " INTO ##tmpdoubles" +
                " FROM" +
                " (SELECT distinct a.TICKER, a.COMPANY_NAME, a.ISIN" +
                " FROM DATA_FACTSET a" +
                " INNER JOIN DATA_FACTSET b on (a.ISIN = b.ISIN AND" +
                " (a.TICKER<>b.TICKER OR a.COMPANY_NAME<>b.COMPANY_NAME))" +
                " OR" +
                " (a.TICKER = b.TICKER AND " +
                " (a.ISIN<>b.ISIN OR a.COMPANY_NAME<>b.COMPANY_NAME))" +
                " OR" +
                " (a.COMPANY_NAME=b.COMPANY_NAME AND" +
                " (a.ISIN<>b.ISIN OR a.TICKER<>b.TICKER))" +
                " WHERE a.ISIN Is Not null AND a.date >= @lastdate AND b.date >= @lastdate )AS x" +
                " ORDER BY x.TICKER"+
                " SELECT (SELECT MAX(h.DATE) FROM DATA_FACTSET h " +
                " WHERE j.TICKER=h.TICKER AND j.ISIN=h.ISIN AND j.COMPANY_NAME=h.COMPANY_NAME) as date, " +
                " j.COMPANY_NAME as name, " +
                " j.ISIN as isin, " +
                " (SELECT TOP(1) h.COUNTRY FROM DATA_FACTSET h " +
                " WHERE j.TICKER=h.TICKER AND j.ISIN=h.ISIN AND j.COMPANY_NAME=h.COMPANY_NAME) as country, " +
                " j.TICKER as ticker " +
                " FROM ##tmpdoubles as j " +
                " ORDER BY country, j.TICKER " +
                " DROP TABLE ##tmpdoubles";        

        #endregion

        public RadObservableCollection<S_Stock> _stockList;
        S_Stock _gridSelectedItem;

        #endregion

        #region Properties

        public RadObservableCollection<S_Stock> StockList
        {
            get { return _stockList; }
            set
            {
                _stockList = value;
                OnPropertyChanged("StockList");
            }
        }

        public S_Stock GridSelectedItem
        {
            get { return _gridSelectedItem; }
            set
            {
                _gridSelectedItem = value;
                OnPropertyChanged("GridSelectedItem");
            }
        }

        #endregion

        #region Commands


        #endregion

        /// <summary>
        /// Fill the List used for the datagrid
        /// </summary>
        public void FillStocks()
        {
            List<Dictionary<String, object>> tmp = _model.GetDoublonsData(FIND_DOUBLE_EQUITY_SQL);

            StockList = new RadObservableCollection<S_Stock>();


            foreach (Dictionary<String, object> stock in tmp)
                StockList.Add(new S_Stock(stock["date"].ToString(),
                        stock["name"].ToString(),
                        stock["isin"].ToString(),
                        stock["country"].ToString(),
                        stock["ticker"].ToString())
                    );
        }

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);    
        }

        #endregion
    }
}
