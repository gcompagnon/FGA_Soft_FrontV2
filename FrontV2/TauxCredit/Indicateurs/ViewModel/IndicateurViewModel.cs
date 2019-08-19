using FrontV2.TauxCredit.Indicateurs.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Data;

namespace FrontV2.TauxCredit.Indicateurs.ViewModel
{
    class IndicateurViewModel : ViewModelBase
    {
        public IndicateurViewModel()
        {
            _model = new IndicateurModel();

            _isin = _model.getIsin();

            _dateD = _model.getDateD();

            _dateF = _model.getDateF();

            _source = _model.getSource();
            _selectedSource = _source[0];

            _categroriesSelector = new GenericDataPointBinding<KeyValuePair<String, float>, String>();
            _categroriesSelector.ValueSelector = pair => pair.Key;
            _valuesSelector = new GenericDataPointBinding<KeyValuePair<String, float>, float>();
            _valuesSelector.ValueSelector = pair => pair.Value;

        }

        #region Fields

        IndicateurModel _model;

        //Combobox
        private RadObservableCollection<String> _isin = new RadObservableCollection<String>();
        private RadObservableCollection<String> _selectedIsin = new RadObservableCollection<string>();
        private RadObservableCollection<String> _dateD = new RadObservableCollection<String>();
        private RadObservableCollection<String> _selectedDateD = new RadObservableCollection<string>();
        private RadObservableCollection<String> _dateF = new RadObservableCollection<String>();
        private RadObservableCollection<String> _selectedDateF = new RadObservableCollection<string>();
        private RadObservableCollection<String> _source = new RadObservableCollection<String>();
        private String _selectedSource;

        List<string> param = new List<string>();

        //Checkbox        

        private bool _isSelected1;
        private bool _isSelected2;
        private bool _isSelected3;
        private bool _isSelected4;
        private bool _isSelected5;
        private bool _isSelected6;
        private bool _isSelected7;
        private bool _isSelected8;
        private bool _isSelected9;
        private bool _isSelected10;
        private bool _isSelected11;
        private bool _isSelected12;


        private bool containsNegative = false;
        //DataTables

        public DataTable _ratesDataSource1 = new DataTable();
        public DataTable _ratesDataSource2 = new DataTable();
        public DataTable _ratesDataSource3 = new DataTable();
        public DataTable _ratesDataSource4 = new DataTable();
        public DataTable _ratesDataSource5 = new DataTable();
        public DataTable _ratesDataSource6 = new DataTable();
        public DataTable _ratesDataSource7 = new DataTable();
        public DataTable _ratesDataSource8 = new DataTable();
        public DataTable _ratesDataSource9 = new DataTable();
        public DataTable _ratesDataSource10 = new DataTable();
        public DataTable _ratesDataSource11 = new DataTable();
        public DataTable _ratesDataSource12 = new DataTable();

        //Datalist Courbe

        private RadObservableCollection<KeyValuePair<string, float>> _dataList;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList2;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList3;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList4;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList5;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList6;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList7;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList8;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList9;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList10;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList11;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList12;
        private RadObservableCollection<KeyValuePair<string, float>> _zeroLine;

        public GenericDataPointBinding<KeyValuePair<string, float>, string> _categroriesSelector;
        public GenericDataPointBinding<KeyValuePair<string, float>, float> _valuesSelector;

        private DataTable _indicateursDataSource;

        #endregion

        #region DataList

        public RadObservableCollection<KeyValuePair<string, float>> DataList
        {
            get { return _dataList; }
            set
            {
                if (Equals(value, _dataList)) return;
                _dataList = value;
                OnPropertyChanged("DataList");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList2
        {
            get { return _dataList2; }
            set
            {
                if (Equals(value, _dataList2)) return;
                _dataList2 = value;
                OnPropertyChanged("DataList2");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList3
        {
            get { return _dataList3; }
            set
            {
                if (Equals(value, _dataList3)) return;
                _dataList3 = value;
                OnPropertyChanged("DataList3");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList4
        {
            get { return _dataList4; }
            set
            {
                if (Equals(value, _dataList4)) return;
                _dataList4 = value;
                OnPropertyChanged("DataList4");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList5
        {
            get { return _dataList5; }
            set
            {
                if (Equals(value, _dataList5)) return;
                _dataList5 = value;
                OnPropertyChanged("DataList5");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList6
        {
            get { return _dataList6; }
            set
            {
                if (Equals(value, _dataList6)) return;
                _dataList6 = value;
                OnPropertyChanged("DataList6");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList7
        {
            get { return _dataList7; }
            set
            {
                if (Equals(value, _dataList7)) return;
                _dataList7 = value;
                OnPropertyChanged("DataList7");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList8
        {
            get { return _dataList8; }
            set
            {
                if (Equals(value, _dataList8)) return;
                _dataList8 = value;
                OnPropertyChanged("DataList8");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList9
        {
            get { return _dataList9; }
            set
            {
                if (Equals(value, _dataList9)) return;
                _dataList9 = value;
                OnPropertyChanged("DataList9");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList10
        {
            get { return _dataList10; }
            set
            {
                if (Equals(value, _dataList10)) return;
                _dataList10 = value;
                OnPropertyChanged("DataList10");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList11
        {
            get { return _dataList11; }
            set
            {
                if (Equals(value, _dataList11)) return;
                _dataList11 = value;
                OnPropertyChanged("DataList11");
            }
        }
        public RadObservableCollection<KeyValuePair<string, float>> DataList12
        {
            get { return _dataList12; }
            set
            {
                if (Equals(value, _dataList12)) return;
                _dataList12 = value;
                OnPropertyChanged("DataList12");
            }
        }

        // ZeroLine

        public RadObservableCollection<KeyValuePair<string, float>> ZeroLine
        {
            get { return _zeroLine; }
            set
            {
                if (Equals(value, _zeroLine)) return;
                _zeroLine = value;
                OnPropertyChanged("ZeroLine");
            }
        }


        public GenericDataPointBinding<KeyValuePair<String, float>, String> CategoriesSelector
        {
            get
            {
                return _categroriesSelector;
            }
            private set
            {
            }
        }
        public GenericDataPointBinding<KeyValuePair<String, float>, float> ValuesSelector
        {
            get
            {
                return _valuesSelector;
            }
            private set
            {
            }
        }

        #endregion

        #region Datables
  
        public DataTable RatesDataSource1
        {
            get { return _ratesDataSource1; }
            set
            {
                _ratesDataSource1 = value;
                OnPropertyChanged("RatesDataSource1");
            }
        }
        public DataTable RatesDataSource2
        {
            get { return _ratesDataSource2; }
            set
            {
                _ratesDataSource2 = value;
                OnPropertyChanged("RatesDataSource2");
            }
        }
        public DataTable RatesDataSource3
        {
            get { return _ratesDataSource3; }
            set
            {
                _ratesDataSource3 = value;
                OnPropertyChanged("RatesDataSource3");
            }
        }
        public DataTable RatesDataSource4
        {
            get { return _ratesDataSource4; }
            set
            {
                _ratesDataSource4 = value;
                OnPropertyChanged("RatesDataSource4");
            }
        }
        public DataTable RatesDataSource5
        {
            get { return _ratesDataSource5; }
            set
            {
                _ratesDataSource5 = value;
                OnPropertyChanged("RatesDataSource5");
            }
        }
        public DataTable RatesDataSource6
        {
            get { return _ratesDataSource6; }
            set
            {
                _ratesDataSource6 = value;
                OnPropertyChanged("RatesDataSource6");
            }
        }
        public DataTable RatesDataSource7
        {
            get { return _ratesDataSource7; }
            set
            {
                _ratesDataSource7 = value;
                OnPropertyChanged("RatesDataSource7");
            }
        }
        public DataTable RatesDataSource8
        {
            get { return _ratesDataSource8; }
            set
            {
                _ratesDataSource8 = value;
                OnPropertyChanged("RatesDataSource8");
            }
        }
        public DataTable RatesDataSource9
        {
            get { return _ratesDataSource9; }
            set
            {
                _ratesDataSource9 = value;
                OnPropertyChanged("RatesDataSource9");
            }
        }
        public DataTable RatesDataSource10
        {
            get { return _ratesDataSource10; }
            set
            {
                _ratesDataSource10 = value;
                OnPropertyChanged("RatesDataSource10");
            }
        }
        public DataTable RatesDataSource11
        {
            get { return _ratesDataSource11; }
            set
            {
                _ratesDataSource11 = value;
                OnPropertyChanged("RatesDataSource11");
            }
        }
        public DataTable RatesDataSource12
        {
            get { return _ratesDataSource12; }
            set
            {
                _ratesDataSource12 = value;
                OnPropertyChanged("RatesDataSource12");
            }
        }

        public DataTable IndicateursDataSource
        {
            get
            {
                return _indicateursDataSource;
            }

            set
            {
                _indicateursDataSource = value;
                OnPropertyChanged("IndicateursDataSource");
            }
        }

        #endregion

        #region Combox

        public RadObservableCollection<String> Isin
        {
            get { return _isin; }
            set
            {
                _isin = value;
                OnPropertyChanged("Isin");
            }
        }
        public RadObservableCollection<String> SelectedIsins
        {
            get { return _selectedIsin; }
            set
            {
                _selectedIsin = value;
                //   Load();
                OnPropertyChanged("SelectedIsins");
            }
        }
        public RadObservableCollection<String> DateD
        {
            get { return _dateD; }
            set
            {
                _dateD = value;
                OnPropertyChanged("DateD");
            }
        }
        public RadObservableCollection<String> SelectedDateD
        {
            get { return _selectedDateD; }
            set
            {
                _selectedDateD = value;
                Load();
                OnPropertyChanged("SelectedDateD");
            }
        }
        public RadObservableCollection<String> DateF
        {
            get { return _dateF; }
            set
            {
                _dateF = value;
                OnPropertyChanged("DateF");
            }
        }
        public RadObservableCollection<String> SelectedDateF
        {
            get { return _selectedDateF; }
            set
            {
                _selectedDateF = value;
                Load();
                OnPropertyChanged("SelectedDateF");
            }
        }

        public RadObservableCollection<String> Source1
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged("Source1");
            }
        }
        public String SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                _selectedSource = value;
                OnPropertyChanged("SelectedSource");
            }
        }

        #endregion

        #region CheckBox

        public bool IsSelected1
        {
            get { return _isSelected1; }
            set
            {
                _isSelected1 = value;
                if (value == true)
                    param.Add("Mobile Average");
                else if (param.Contains("Mobile Average"))
                {
                    DataList = null;
                    param.Remove("Mobile Average");
                    RatesDataSource1 = new DataTable();
                }
                OnPropertyChanged("IsSelected1");
            }
        }

        public bool IsSelected2
        {
            get { return _isSelected2; }
            set
            {
                _isSelected2 = value;
                if (value == true)
                    param.Add("Historical Average");
                else if (param.Contains("Historical Average"))
                {
                    param.Remove("Historical Average");
                    DataList2 = null;
                    RatesDataSource2 = new DataTable();
                }
                OnPropertyChanged("IsSelected2");
            }
        }
        public bool IsSelected3
        {
            get { return _isSelected3; }
            set
            {
                _isSelected3 = value;
                if (value == true)
                    param.Add("Volatility");
                else if (param.Contains("Volatility"))
                {
                    param.Remove("Volatility");
                    DataList3 = null;
                    RatesDataSource3 = new DataTable();
                }
                OnPropertyChanged("IsSelected3");
            }
        }
        public bool IsSelected4
        {
            get { return _isSelected4; }
            set
            {
                _isSelected4 = value;
                if (value == true)
                    param.Add("Mobile Volatility");
                else if (param.Contains("Mobile Volatility"))
                {
                    param.Remove("Mobile Volatility");
                    DataList4 = null;
                    RatesDataSource4 = new DataTable();
                }
                OnPropertyChanged("IsSelected4");
            }
        }
        public bool IsSelected5
        {
            get { return _isSelected5; }
            set
            {
                _isSelected5 = value;
                if (value == true)
                    param.Add("Z Score");
                else if (param.Contains("Z Score"))
                {
                    param.Remove("Z Score");
                    DataList5 = null;
                    RatesDataSource5 = new DataTable();
                }
                OnPropertyChanged("IsSelected5");
            }
        }
        public bool IsSelected6
        {
            get { return _isSelected6; }
            set
            {
                _isSelected6 = value;
                if (value == true)
                    param.Add("Mobile Z Score");
                else if (param.Contains("Mobile Z Score"))
                {
                    param.Remove("Mobile Z Score");
                    DataList6 = null;
                    RatesDataSource6 = new DataTable();
                }
                OnPropertyChanged("IsSelected6");
            }
        }
        public bool IsSelected7
        {
            get { return _isSelected7; }
            set
            {
                _isSelected7 = value;
                if (value == true)
                    param.Add("Maximum");
                else if (param.Contains("Maximum"))
                {
                    param.Remove("Maximum");
                    DataList7 = null;
                    RatesDataSource7 = new DataTable();
                }
                OnPropertyChanged("IsSelected7");
            }
        }
        public bool IsSelected8
        {
            get { return _isSelected8; }
            set
            {
                _isSelected8 = value;
                if (value == true)
                    param.Add("Maximun 5%");
                else if (param.Contains("Maximun 5%"))
                {
                    param.Remove("Maximun 5%");
                    DataList8 = null;
                    RatesDataSource8 = new DataTable();
                }
                OnPropertyChanged("IsSelected8");
            }
        }
        public bool IsSelected9
        {
            get { return _isSelected9; }
            set
            {
                _isSelected9 = value;
                if (value == true)
                    param.Add("Minimum");
                else if (param.Contains("Minimum"))
                {
                    param.Remove("Minimum");
                    DataList9 = null;
                    RatesDataSource9 = new DataTable();
                }
                OnPropertyChanged("IsSelected9");
            }
        }
        public bool IsSelected10
        {
            get { return _isSelected10; }
            set
            {
                _isSelected10 = value;
                if (value == true)
                    param.Add("Minimum 5%");
                else if (param.Contains("Minimum 5%"))
                {
                    param.Remove("Minimum 5%");
                    DataList10 = null;
                    RatesDataSource10 = new DataTable();


                }
                OnPropertyChanged("IsSelected10");
            }
        }

        public bool IsSelected11
        {
            get { return _isSelected11; }
            set
            {
                _isSelected11 = value;
                if (value == true)
                    param.Add("Closing Price");
                else if (param.Contains("Closing Price"))
                {
                    param.Remove("Closing Price");
                    DataList11 = null;
                    RatesDataSource11 = new DataTable();
                }
                OnPropertyChanged("IsSelected11");
            }
        }

        public bool IsSelected12
        {
            get { return _isSelected12; }
            set
            {
                _isSelected12 = value;
                if (value == true)
                    param.Add("Average");
                else if (param.Contains("Average"))
                {
                    param.Remove("Average");
                    DataList12 = null;
                    RatesDataSource12 = new DataTable();
                }
                OnPropertyChanged("IsSelected12");
            }
        }

        #endregion       

        public void Load()
        {
            string toto = SelectedIsins.First();
            DataTable tmp = new DataTable();
            if (SelectedIsins.Count == 0 || SelectedDateD.Count == 0 || SelectedDateF.Count == 0 || SelectedSource == null)
                return;
            else
            {
                _indicateursDataSource = null;
                foreach (string s in SelectedIsins)
                {
                    tmp = _model.GetIndicateurs(s, SelectedDateD.First(), SelectedDateF.First(), SelectedSource);
                    if (_indicateursDataSource == null)
                        _indicateursDataSource = tmp;
                    else
                        _indicateursDataSource.Merge(tmp);
                }
                IndicateursDataSource = _indicateursDataSource;
            }
        }

        public void FillRatesTableColumns()
        {
            _ratesDataSource1 = new DataTable();
            _ratesDataSource1.Columns.Add("Date");
            _ratesDataSource1.Columns.Add("Valeur");

            _ratesDataSource2 = new DataTable();
            _ratesDataSource2.Columns.Add("Date");
            _ratesDataSource2.Columns.Add("Valeur");

            _ratesDataSource3 = new DataTable();
            _ratesDataSource3.Columns.Add("Date");
            _ratesDataSource3.Columns.Add("Valeur");

            _ratesDataSource4 = new DataTable();
            _ratesDataSource4.Columns.Add("Date");
            _ratesDataSource4.Columns.Add("Valeur");

            _ratesDataSource5 = new DataTable();
            _ratesDataSource5.Columns.Add("Date");
            _ratesDataSource5.Columns.Add("Valeur");

            _ratesDataSource6 = new DataTable();
            _ratesDataSource6.Columns.Add("Date");
            _ratesDataSource6.Columns.Add("Valeur");

            _ratesDataSource7 = new DataTable();
            _ratesDataSource7.Columns.Add("Date");
            _ratesDataSource7.Columns.Add("Valeur");

            _ratesDataSource8 = new DataTable();
            _ratesDataSource8.Columns.Add("Date");
            _ratesDataSource8.Columns.Add("Valeur");

            _ratesDataSource9 = new DataTable();
            _ratesDataSource9.Columns.Add("Date");
            _ratesDataSource9.Columns.Add("Valeur");

            _ratesDataSource10 = new DataTable();
            _ratesDataSource10.Columns.Add("Date");
            _ratesDataSource10.Columns.Add("Valeur");

            _ratesDataSource11 = new DataTable();
            _ratesDataSource11.Columns.Add("Date");
            _ratesDataSource11.Columns.Add("Valeur");

            _ratesDataSource12 = new DataTable();
            _ratesDataSource12.Columns.Add("Date");
            _ratesDataSource12.Columns.Add("Valeur");

            OnPropertyChanged("RatesDataSource1");
            OnPropertyChanged("RatesDataSource2");
            OnPropertyChanged("RatesDataSource3");
            OnPropertyChanged("RatesDataSource4");
            OnPropertyChanged("RatesDataSource5");
            OnPropertyChanged("RatesDataSource6");
            OnPropertyChanged("RatesDataSource7");
            OnPropertyChanged("RatesDataSource8");
            OnPropertyChanged("RatesDataSource9");
            OnPropertyChanged("RatesDataSource10");
            OnPropertyChanged("RatesDataSource11");
            OnPropertyChanged("RatesDataSource12");
        }

        public void IndicateurExcute()
        {
            Load();
        }

        public void fillDataGrid()
        {
            if (param.Contains("Mobile Average"))
                RatesDataSource1 = _model.GetGraphProcedure(SelectedIsins.First(), 
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "MobileAvg3M");
            if (param.Contains("Historical Average"))
                RatesDataSource2 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "HistAvg");
            if (param.Contains("Average"))
                RatesDataSource12 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "PeriodAvg");
            if (param.Contains("Volatility"))
                RatesDataSource3 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "PeriodVol");
            if (param.Contains("Mobile Volatility"))
                RatesDataSource4 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "MobileVol3M");
            if (param.Contains("Z Score"))
                RatesDataSource5 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "Zscore");
            if (param.Contains("Mobile Z Score"))
                RatesDataSource6 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "MobileZscore3M");
            if (param.Contains("Maximum"))
                RatesDataSource7 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "YTM_max");
            if (param.Contains("Maximum 5%"))
                RatesDataSource8 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "YTM_max5pc");
            if (param.Contains("Minimum"))
                RatesDataSource9 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "YTM_min");
            if (param.Contains("Minimum 5%"))
                RatesDataSource10 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "YTM_min5pc");
            if (param.Contains("Closing Price"))
                RatesDataSource11 = _model.GetGraphProcedure(SelectedIsins.First(),
                    SelectedDateD.First(), SelectedDateF.First(), SelectedSource, "YTM_close");
        }
        
        public void showChart()
        {
            DataList = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource1.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                String ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp);
                DataList.Add(new KeyValuePair<String, float>(date, rate));
            }
        }

        public void showChart2()
        {
            DataList2 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource2.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string rateTmp = row["Valeur"].ToString();
                float rate = float.Parse(rateTmp);
                DataList2.Add(new KeyValuePair<string, float>(date, rate));
            }
        }

        public void showChart3()
        {
            DataList3 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource3.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp) * 10;
                DataList3.Add(new KeyValuePair<string, float>(date, rate));
            }
        }
     
        public void showChart4()
        {
            DataList4 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource4.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp) * 10;
                DataList4.Add(new KeyValuePair<string, float>(date, rate));
            }
        }
  
        public void showChart5()
        {
            DataList5 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource5.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp) * 10;
                if (rate < 0)
                    containsNegative = true;
                DataList5.Add(new KeyValuePair<string, float>(date, rate));
            }
        }
   
        public void showChart6()
        {
            DataList6 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource6.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp) * 10;
                if (rate < 0)
                    containsNegative = true;
                DataList6.Add(new KeyValuePair<string, float>(date, rate));
            }
        }
       
        public void showChart7()
        {
            DataList7 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource7.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp);
                DataList7.Add(new KeyValuePair<string, float>(date, rate));
            }
        }
    
        public void showChart8()
        {
            DataList8 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource8.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp);
                DataList8.Add(new KeyValuePair<string, float>(date, rate));

            }
        }
   
        public void showChart9()
        {
            DataList9 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource9.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp);
                DataList9.Add(new KeyValuePair<string, float>(date, rate));

            }
        }
      
        public void showChart10()
        {
            DataList10 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource10.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp);
                DataList10.Add(new KeyValuePair<string, float>(date, rate));
            }



        }
      
        public void showChart11()
        {
            DataList11 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource11.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp);
                DataList11.Add(new KeyValuePair<string, float>(date, rate));
            }
        }
     
        public void showChart12()
        {
            DataList12 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource12.Rows)
            {
                string date = row["Date"].ToString();
                date = date.Substring(0, 10);
                string ratetmp = row["Valeur"].ToString();
                float rate = float.Parse(ratetmp);
                DataList12.Add(new KeyValuePair<string, float>(date, rate));
            }
        }
      
        public void showZero()
        {
            if (containsNegative)
            {
                ZeroLine = new RadObservableCollection<KeyValuePair<string, float>>();
                foreach (var v in DataList5)
                {
                    ZeroLine.Add(new KeyValuePair<string, float>(v.Key, 0));
                }
                foreach (var u in DataList6)
                {
                    ZeroLine.Add(new KeyValuePair<string, float>(u.Key, 0));
                }
                containsNegative = false;
            }
            else
            {
                ZeroLine = null;
                containsNegative = false;
            }
        }

    }
}
