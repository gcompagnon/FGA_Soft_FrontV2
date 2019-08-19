using FrontV2.TauxCredit.BaseInterpolation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Data;

namespace FrontV2.TauxCredit.BaseInterpolation.ViewModel
{
    class BaseInterpolationViewModel : ViewModelBase
    {

        public BaseInterpolationViewModel()
        {
            Init();

            _categroriesSelector = new GenericDataPointBinding<KeyValuePair<String, float>, String>();
            _categroriesSelector.ValueSelector = pair => pair.Key;

            _valuesSelector = new GenericDataPointBinding<KeyValuePair<String, float>, float>();
            _valuesSelector.ValueSelector = pair => pair.Value;

            FillRatesTableColumns();

        }

        #region Fields
        Connection co = new Connection();
        private BaseInterpolationModel _model = new BaseInterpolationModel();


        //Comboboxs
        private GenericDataPointBinding<KeyValuePair<string, float>, string> _categroriesSelector;
        private GenericDataPointBinding<KeyValuePair<string, float>, float> _valuesSelector;

        private RadObservableCollection<String> _dates = new RadObservableCollection<String>();
        private String _selectedDate;
        private String _selectedDate2;

        private RadObservableCollection<int> _maturities = new RadObservableCollection<int>();
        private int _selectedMaturity;
        private int _selectedMaturity2;

        private RadObservableCollection<String> _pays = new RadObservableCollection<string>();
        private String _selectedPays1;
        private String _selectedPays2;

        private RadObservableCollection<String> _source = new RadObservableCollection<String>();
        private String _selectedSource;

        private RadObservableCollection<KeyValuePair<string, float>> _dataList;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList2;
        private RadObservableCollection<KeyValuePair<string, float>> _dataList3;
        private RadObservableCollection<KeyValuePair<string, float>> _zeroLine;

        private bool containsNegative = false;

        #region DataTables
        //Graph
        public DataTable _ratesDataSource = new DataTable();
        public DataTable _ratesDataSource2 = new DataTable();
        //Tab
        private DataTable _indicateurDataSource1;
        private DataTable _indicateurDataSource2;

        public DataTable _ratesData1 = new DataTable();
        public DataTable _ratesData2 = new DataTable();
        public DataTable _ratesData3 = new DataTable();
        public DataTable _ratesData4 = new DataTable();
        public DataTable _ratesData5 = new DataTable();
        public DataTable _ratesData6 = new DataTable();
        public DataTable _ratesData7 = new DataTable();
        public DataTable _ratesData8 = new DataTable();
        public DataTable _ratesData9 = new DataTable();
        public DataTable _ratesData10 = new DataTable();
        public DataTable _ratesData11 = new DataTable();

        #endregion
        #endregion

        #region Properties

        #region Tableau

        public DataTable IndicateursDataSource1
        {
            get
            {
                return _indicateurDataSource1;
            }

            set
            {
                _indicateurDataSource1 = value;
                OnPropertyChanged("IndicateursDataSource1");
            }
        }
        public DataTable IndicateursDataSource2
        {
            get
            {
                return _indicateurDataSource2;
            }

            set
            {
                _indicateurDataSource2 = value;
                OnPropertyChanged("IndicateursDataSource2");
            }
        }

        public DataTable RatesData1
        {
            get { return _ratesData1; }
            set
            {
                _ratesData1 = value;
                OnPropertyChanged("RatesData1");
            }
        }
        public DataTable RatesData2
        {
            get { return _ratesData2; }
            set
            {
                _ratesData2 = value;
                OnPropertyChanged("RatesData2");
            }
        }
        public DataTable RatesData3
        {
            get { return _ratesData3; }
            set
            {
                _ratesData3 = value;
                OnPropertyChanged("RatesData3");
            }
        }
        public DataTable RatesData4
        {
            get { return _ratesData4; }
            set
            {
                _ratesData4 = value;
                OnPropertyChanged("RatesData4");
            }
        }
        public DataTable RatesData5
        {
            get { return _ratesData5; }
            set
            {
                _ratesData5 = value;
                OnPropertyChanged("RatesData5");
            }
        }
        public DataTable RatesData6
        {
            get { return _ratesData6; }
            set
            {
                _ratesData6 = value;
                OnPropertyChanged("RatesData6");
            }
        }
        public DataTable RatesData7
        {
            get { return _ratesData7; }
            set
            {
                _ratesData7 = value;
                OnPropertyChanged("RatesData7");
            }
        }
        public DataTable RatesData8
        {
            get { return _ratesData8; }
            set
            {
                _ratesData8 = value;
                OnPropertyChanged("RatesData8");
            }
        }
        public DataTable RatesData9
        {
            get { return _ratesData9; }
            set
            {
                _ratesData9 = value;
                OnPropertyChanged("RatesData9");
            }
        }
        public DataTable RatesData10
        {
            get { return _ratesData10; }
            set
            {
                _ratesData10 = value;
                OnPropertyChanged("RatesData10");
            }
        }
        public DataTable RatesData11
        {
            get { return _ratesData11; }
            set
            {
                _ratesData11 = value;
                OnPropertyChanged("RatesData11");
            }
        }

        #endregion

        #region DataTables

        public DataTable RatesDataSource
        {
            get { return _ratesDataSource; }
            set
            {
                _ratesDataSource = value;
                OnPropertyChanged("RatesDataSource");
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

        #endregion

        #region ComBox

        public RadObservableCollection<String> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged("Dates");
            }
        }
        public String SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                Load();
                Load2();
                OnPropertyChanged("SelectedDate");

            }
        }
        public String SelectedDate2
        {
            get { return _selectedDate2; }
            set
            {
                _selectedDate2 = value;
                Load();
                Load2();
                OnPropertyChanged("SelectedDate2");

            }
        }
        
        public RadObservableCollection<int> Maturities
        {
            get { return _maturities; }
            set
            {
                _maturities = value;
                OnPropertyChanged("Maturity");
            }
        }
        public int SelectedMaturity
        {
            get { return _selectedMaturity; }
            set
            {
                _selectedMaturity = value;
                Load();
                Load2();
                OnPropertyChanged("SelectedMaturity");

            }
        }
        public int SelectedMaturity2
        {
            get { return _selectedMaturity2; }
            set
            {
                _selectedMaturity2 = value;
                OnPropertyChanged("SelectedMaturity2");

            }
        }

        public RadObservableCollection<String> Pays
        {
            get { return _pays; }
            set
            {
                _pays = value;
                OnPropertyChanged("Pays1");
            }
        }
        public String SelectedPays1
        {
            get { return _selectedPays1; }
            set
            {
                _selectedPays1 = value;
                OnPropertyChanged("SelectedPays1");
            }
        }
        public String SelectedPays2
        {
            get { return _selectedPays2; }
            set
            {
                _selectedPays2 = value;
                OnPropertyChanged("SelectedPays2");
            }
        }

        public RadObservableCollection<String> Source
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

        #region Courbes
        //Country 1
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

        //Country 2

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
        //Courbe of Spread 
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

        #endregion

        #region Methods

        private void Init()
        {
            #region Dates

            _dates = _model.GetDates();
            _selectedDate = _dates[0];
            _selectedDate2 = _dates[0];

            #endregion

            #region Pays

            _pays = _model.GetPays();
            _selectedPays1 = _pays[0];
            _selectedPays2 = _pays[0];

            #endregion

            #region Maturity

            _maturities = _model.GetMaturity();
            _selectedMaturity = _maturities[0];
            _selectedMaturity2 = _maturities[0];

            #endregion

            #region Source

            _source = _model.GetSource();
            _selectedSource = _source[0];

            #endregion
        }

        public void Load()
        {
            if (SelectedDate == null || SelectedDate2 == null || SelectedMaturity == 0 || SelectedPays1 == null)
                return;
            else
                IndicateursDataSource1 = _model.gettableau(_selectedDate, _selectedDate2, _selectedPays1, _selectedMaturity);
        }

        public void Load2()
        {
            if (SelectedDate == null || SelectedDate2 == null || SelectedMaturity2 == 0 || SelectedPays2 == null)
                return;
            else
                IndicateursDataSource2 = _model.getproceduretab2(_selectedDate, _selectedDate2, _selectedPays2, _selectedMaturity2);
        }

        #endregion
       
        public void InterpolationExceute()
        {
            //RatesDataSource = _model.getprocedure(SelectedDate, SelectedDate2, SelectedPays1, SelectedMaturity);
            //RatesDataSource2 = _model.getprocedure2(SelectedDate, SelectedDate2, SelectedPays2, SelectedMaturity2);
        }

        public void fillDataGrid()
        {
            RatesDataSource = _model.getinterpol(SelectedDate, SelectedDate2, SelectedPays1, SelectedMaturity, SelectedSource);
            RatesDataSource2 = _model.getinterpol(SelectedDate, SelectedDate2, SelectedPays2, SelectedMaturity2, SelectedSource);
        }

        #region Fill Columns for Tab

        public void FillRatesTableColumns()
        {
            _ratesDataSource = new DataTable();
            _ratesDataSource.Columns.Add("Date");
            _ratesDataSource.Columns.Add("Maturity");
            _ratesDataSource.Columns.Add("Rate");

            _ratesDataSource2 = new DataTable();
            _ratesDataSource2.Columns.Add("Date");
            _ratesDataSource2.Columns.Add("Maturity");
            _ratesDataSource2.Columns.Add("Rate");

            OnPropertyChanged("RatesDataSource");
            OnPropertyChanged("RatesDataSource2");
        }

        public void FillRatesColumns()
        {
            _ratesData1 = new DataTable();
            _ratesData1.Columns.Add("Date");
            _ratesData1.Columns.Add("Valeur");

            _ratesData2 = new DataTable();
            _ratesData2.Columns.Add("Date");
            _ratesData2.Columns.Add("Valeur");

            _ratesData3 = new DataTable();
            _ratesData3.Columns.Add("Date");
            _ratesData3.Columns.Add("Valeur");

            _ratesData4 = new DataTable();
            _ratesData4.Columns.Add("Date");
            _ratesData4.Columns.Add("Valeur");

            _ratesData5 = new DataTable();
            _ratesData5.Columns.Add("Date");
            _ratesData5.Columns.Add("Valeur");

            _ratesData6 = new DataTable();
            _ratesData6.Columns.Add("Date");
            _ratesData6.Columns.Add("Valeur");

            _ratesData7 = new DataTable();
            _ratesData7.Columns.Add("Date");
            _ratesData7.Columns.Add("Valeur");

            _ratesData8 = new DataTable();
            _ratesData8.Columns.Add("Date");
            _ratesData8.Columns.Add("Valeur");

            _ratesData9 = new DataTable();
            _ratesData9.Columns.Add("Date");
            _ratesData9.Columns.Add("Valeur");

            _ratesData10 = new DataTable();
            _ratesData10.Columns.Add("Date");
            _ratesData10.Columns.Add("Valeur");

            _ratesData11 = new DataTable();
            _ratesData11.Columns.Add("Date");
            _ratesData11.Columns.Add("Valeur");

            OnPropertyChanged("RatesData1");
            OnPropertyChanged("RatesData2");
            OnPropertyChanged("RatesData3");
            OnPropertyChanged("RatesData4");
            OnPropertyChanged("RatesData5");
            OnPropertyChanged("RatesData6");
            OnPropertyChanged("RatesData7");
            OnPropertyChanged("RatesData8");
            OnPropertyChanged("RatesData9");
            OnPropertyChanged("RatesData10");
            OnPropertyChanged("RatesData11");
        }

        #endregion

        #region ShowChart


        public void showChart()
        {

            DataList = new RadObservableCollection<KeyValuePair<string, float>>();


            foreach (DataRow row in _ratesDataSource.Rows)
            {
                string date = row["Date"].ToString();
                int year = int.Parse(date.Substring(6, 4));
                date = date.Substring(0, 6) + year.ToString();

                String rateTmp = row["Rate"].ToString();
                float rate = float.Parse(rateTmp);
                DataList.Add(new KeyValuePair<String, float>(date, rate));
            }

        }

        public void showChart2()
        {
            DataList2 = new RadObservableCollection<KeyValuePair<string, float>>();

            foreach (DataRow row in _ratesDataSource2.Rows)
            {
                string date = row["Date"].ToString();
                int year = int.Parse(date.Substring(6, 4));
                date = date.Substring(0, 6) + year.ToString();

                String rateTmp = row["Rate"].ToString();
                float rate = float.Parse(rateTmp);
                DataList2.Add(new KeyValuePair<String, float>(date, rate));
            }



        }

        public void showChart3(ObservableCollection<KeyValuePair<string, float>> Datalist, RadObservableCollection<KeyValuePair<string, float>> Datalist2)
        {
            DataList3 = new RadObservableCollection<KeyValuePair<string, float>>();

            for (int i = 0; i < ((Datalist.Count < Datalist2.Count) ? DataList.Count : Datalist2.Count); i++)
            {
                if (Datalist[i].Key == Datalist2[i].Key)
                {
                    float val = Datalist[i].Value - Datalist2[i].Value;
                    if (val < 0)
                        containsNegative = true;
                    DataList3.Add(new KeyValuePair<string, float>(Datalist[i].Key, val));
                }
            }

        }

        public void showZero()
        {
            if (containsNegative)
            {
                ZeroLine = new RadObservableCollection<KeyValuePair<string, float>>();
                foreach (var v in DataList3)
                    ZeroLine.Add(new KeyValuePair<string, float>(v.Key, 0));
                containsNegative = false;
            }
            else
            {
                ZeroLine = null;
                containsNegative = false;
            }
        }

        #endregion
        
    }
}


