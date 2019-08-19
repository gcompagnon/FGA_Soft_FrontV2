using FrontV2.Action.ScoreReco.Model;
using FrontV2.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Data;

namespace FrontV2.Action.ScoreReco.ViewModel
{
    class ScoreRecoViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Constructor

        public ScoreRecoViewModel()
        {
            foreach (String s in _model.GetSectorsICB())
                _supersectors.Add(s);

            _dates = _model.GetDates();

            _categroriesSelector = new GenericDataPointBinding<KeyValuePair<String, double>, String>();
            _categroriesSelector.ValueSelector = pair => pair.Key;
            _valuesSelector = new GenericDataPointBinding<KeyValuePair<String, double>, double>();
            _valuesSelector.ValueSelector = pair => pair.Value;

            _categroriesSelectorQuint = new GenericDataPointBinding<KeyValuePair<String, int>, String>();
            _categroriesSelectorQuint.ValueSelector = pair => pair.Key;
            _valuesSelectorQuint = new GenericDataPointBinding<KeyValuePair<String, int>, int>();
            _valuesSelectorQuint.ValueSelector = pair => pair.Value;

            _selectedDate = _dates[0];
            _selectedSecondDate = _dates[0];
        }

        #endregion

        #region Fields

        private bool _isBusy;
        private string _busyContent;
        private bool _isBusy2;
        private string _busyContent2;

        private int origin = 0;

        private readonly ScoreRecoModel _model = new ScoreRecoModel();
        private CompanyNameCleaner cleaner = new CompanyNameCleaner();

        private readonly List<object> _sectorsICB = new List<object>();

        // ComboBoxes
        private RadObservableCollection<String> _dates = new RadObservableCollection<String>();
        private String _selectedDate;
        private String _selectedSecondDate;
        private RadObservableCollection<String> _univers = new RadObservableCollection<String> { "Europe", "USA" };
        private String _selectedUniverse;
        private RadObservableCollection<String> _supersectors = new RadObservableCollection<String>();
        private String _selectedSuperSector;
        private RadObservableCollection<String> _sectors = new RadObservableCollection<String>();
        private String _selectedSector;

        private RadObservableCollection<String> _companies = new RadObservableCollection<string>();
        private String _selectedCompany1;
        private String _selectedCompany2;

        //DataGrids
        private DataTable _valuesDataSource = new DataTable();

        private DataTable _metriqueChartDataSource = new DataTable();
        private DataTable _scoreChartDataSource = new DataTable();

        //Charts
        private readonly GenericDataPointBinding<KeyValuePair<String, double>, String> _categroriesSelector;
        private readonly GenericDataPointBinding<KeyValuePair<String, double>, double> _valuesSelector;
        private readonly GenericDataPointBinding<KeyValuePair<String, int>, String> _categroriesSelectorQuint;
        private readonly GenericDataPointBinding<KeyValuePair<String, int>, int> _valuesSelectorQuint;

        private RadObservableCollection<KeyValuePair<string, double>> _columnsSeriesHisto1 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _columnsSeriesHisto2 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _columnsSeriesHisto3 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _columnsSeriesHisto4 = new RadObservableCollection<KeyValuePair<string, double>>();

        private RadObservableCollection<KeyValuePair<string, double>> _radarGrowthSeries1 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarGrowthSeries2 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarGrowthSeries3 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarGrowthSeries4 = new RadObservableCollection<KeyValuePair<string, double>>();

        private RadObservableCollection<KeyValuePair<string, double>> _radarValuesSeries1 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarValuesSeries2 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarValuesSeries3 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarValuesSeries4 = new RadObservableCollection<KeyValuePair<string, double>>();

        private RadObservableCollection<KeyValuePair<string, double>> _radarProfitSeries1 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarProfitSeries2 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarProfitSeries3 = new RadObservableCollection<KeyValuePair<string, double>>();
        private RadObservableCollection<KeyValuePair<string, double>> _radarProfitSeries4 = new RadObservableCollection<KeyValuePair<string, double>>();

        private RadObservableCollection<KeyValuePair<string, int>> _quintileChart1 = new RadObservableCollection<KeyValuePair<string, int>>();
        private RadObservableCollection<KeyValuePair<string, int>> _quintileChart2 = new RadObservableCollection<KeyValuePair<string, int>>();

        #endregion

        #region Properties


        public String Ticker1 { get; set; }
        public String Ticker2 { get; set; }

        // ComboBoxes
        public RadObservableCollection<String> Dates
        {
            get
            {
                return _dates;
            }
            set
            {
                _dates = value;
                OnPropertyChanged("Dates");
            }
        }
        public String SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                _selectedSecondDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }
        public String SelectedSecondDate
        {
            get
            {
                return _selectedSecondDate;
            }
            set
            {
                _selectedSecondDate = value;
                OnPropertyChanged("SelectedSecondDate");
                origin = 34;
                IsBusy2 = true;

            }
        }

        public RadObservableCollection<String> Univers
        {
            get
            {
                return _univers;
            }
            set
            {
                _univers = value;
                OnPropertyChanged("Univers");
            }
        }
        public String SelectedUniverse
        {
            get
            {
                return _selectedUniverse;
            }
            set
            {
                _selectedUniverse = value;
                OnPropertyChanged("SelectedUniverse");
                _sectors.Clear();
                foreach (String s in _model.GetFGAIndustries(_selectedSuperSector, _selectedUniverse))
                    _sectors.Add(s);
            }
        }

        public RadObservableCollection<String> SuperSectors
        {
            get
            {
                return _supersectors;
            }
            set
            {
                _supersectors = value;
                OnPropertyChanged("SuperSectors");
            }
        }
        public String SelectedSuperSector
        {
            get
            {
                return _selectedSuperSector;
            }
            set
            {
                _selectedSuperSector = value;
                OnPropertyChanged("SelectedSuperSector");
                _sectors.Clear();
                foreach (String s in _model.GetFGAIndustries(_selectedSuperSector, _selectedUniverse))
                    _sectors.Add(s);
            }
        }

        public RadObservableCollection<String> Sectors
        {
            get
            {
                return _sectors;
            }
            set
            {
                _sectors = value;
                OnPropertyChanged("Sectors");
            }
        }
        public String SelectedSector
        {
            get
            {
                return _selectedSector;
            }
            set
            {
                _selectedSector = value;
                OnPropertyChanged("SelectedSector");
            }
        }

        public RadObservableCollection<String> Companies
        {
            get
            {
                return _companies;
            }
            set
            {
                _companies = value;
                OnPropertyChanged("Companies");
            }
        }

        public String SelectedCompany1
        {
            get
            {
                return _selectedCompany1;
            }
            set
            {
                _selectedCompany1 = value;
                OnPropertyChanged("SelectedCompany1");
                origin = 1;
                IsBusy2 = true;
            }
        }
        public String SelectedCompany2
        {
            get
            {
                return _selectedCompany2;
            }
            set
            {
                _selectedCompany2 = value;
                OnPropertyChanged("SelectedCompany2");
                origin = 2;
                IsBusy2 = true;
            }
        }

        //DataGrids
        public DataTable ValuesDataSource
        {
            get
            {
                return _valuesDataSource;
            }
            set
            {
                _valuesDataSource = value;
                OnPropertyChanged("ValuesDataSource");
            }
        }

        public DataTable MetriqueChartDataSource
        {
            get
            {
                return _metriqueChartDataSource;
            }
            set
            {
                _metriqueChartDataSource = value;
                OnPropertyChanged("MetriqueChartDataSource");
            }
        }
        public DataTable ScoreChartDataSource
        {
            get
            {
                return _scoreChartDataSource;
            }
            set
            {
                _scoreChartDataSource = value;
                OnPropertyChanged("ScoreChartDataSource");
            }
        }

        //Charts 

        public RadObservableCollection<KeyValuePair<string, double>> ColumnsSeriesHisto1
        {
            get
            {
                return _columnsSeriesHisto1;
            }
            set
            {
                _columnsSeriesHisto1 = value;
                OnPropertyChanged("ColumnsSeriesHisto1");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> ColumnsSeriesHisto2
        {
            get
            {
                return _columnsSeriesHisto2;
            }
            set
            {
                _columnsSeriesHisto2 = value;
                OnPropertyChanged("ColumnsSeriesHisto2");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> ColumnsSeriesHisto3
        {
            get
            {
                return _columnsSeriesHisto3;
            }
            set
            {
                _columnsSeriesHisto3 = value;
                OnPropertyChanged("ColumnsSeriesHisto3");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> ColumnsSeriesHisto4
        {
            get
            {
                return _columnsSeriesHisto4;
            }
            set
            {
                _columnsSeriesHisto4 = value;
                OnPropertyChanged("ColumnsSeriesHisto4");
            }
        }

        public GenericDataPointBinding<KeyValuePair<String, double>, String> CategoriesSelector
        {
            get
            {
                return _categroriesSelector;
            }
            private set
            {
            }
        }
        public GenericDataPointBinding<KeyValuePair<String, double>, double> ValuesSelector
        {
            get
            {
                return _valuesSelector;
            }
            private set
            {
            }
        }

        public GenericDataPointBinding<KeyValuePair<String, int>, String> CategoriesSelectorQuint
        {
            get
            {
                return _categroriesSelectorQuint;
            }
            private set
            {
            }
        }
        public GenericDataPointBinding<KeyValuePair<String, int>, int> ValuesSelectorQuint
        {
            get
            {
                return _valuesSelectorQuint;
            }
            private set
            {
            }
        }

        public RadObservableCollection<KeyValuePair<string, double>> RadarGrowthSeries1
        {
            get
            {
                return _radarGrowthSeries1;
            }
            set
            {
                _radarGrowthSeries1 = value;
                OnPropertyChanged("RadarGrowthSeries1");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarGrowthSeries2
        {
            get
            {
                return _radarGrowthSeries2;
            }
            set
            {
                _radarGrowthSeries2 = value;
                OnPropertyChanged("RadarGrowthSeries2");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarGrowthSeries3
        {
            get
            {
                return _radarGrowthSeries3;
            }
            set
            {
                _radarGrowthSeries3 = value;
                OnPropertyChanged("RadarGrowthSeries3");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarGrowthSeries4
        {
            get
            {
                return _radarGrowthSeries4;
            }
            set
            {
                _radarGrowthSeries4 = value;
                OnPropertyChanged("RadarGrowthSeries4");
            }
        }

        public RadObservableCollection<KeyValuePair<string, double>> RadarValuesSeries1
        {
            get
            {
                return _radarValuesSeries1;
            }
            set
            {
                _radarValuesSeries1 = value;
                OnPropertyChanged("RadarValuesSeries1");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarValuesSeries2
        {
            get
            {
                return _radarValuesSeries2;
            }
            set
            {
                _radarValuesSeries2 = value;
                OnPropertyChanged("RadarValuesSeries2");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarValuesSeries3
        {
            get
            {
                return _radarValuesSeries3;
            }
            set
            {
                _radarValuesSeries3 = value;
                OnPropertyChanged("RadarValuesSeries3");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarValuesSeries4
        {
            get
            {
                return _radarValuesSeries4;
            }
            set
            {
                _radarValuesSeries4 = value;
                OnPropertyChanged("RadarValuesSeries4");
            }
        }

        public RadObservableCollection<KeyValuePair<string, double>> RadarProfitSeries1
        {
            get
            {
                return _radarProfitSeries1;
            }
            set
            {
                _radarProfitSeries1 = value;
                OnPropertyChanged("RadarProfitSeries1");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarProfitSeries2
        {
            get
            {
                return _radarProfitSeries2;
            }
            set
            {
                _radarProfitSeries2 = value;
                OnPropertyChanged("RadarProfitSeries2");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarProfitSeries3
        {
            get
            {
                return _radarProfitSeries3;
            }
            set
            {
                _radarProfitSeries3 = value;
                OnPropertyChanged("RadarProfitSeries3");
            }
        }
        public RadObservableCollection<KeyValuePair<string, double>> RadarProfitSeries4
        {
            get
            {
                return _radarProfitSeries4;
            }
            set
            {
                _radarProfitSeries4 = value;
                OnPropertyChanged("RadarProfitSeries4");
            }
        }

        public RadObservableCollection<KeyValuePair<string, int>> QuintileChart1
        {
            get
            {
                return _quintileChart1;
            }
            set
            {
                _quintileChart1 = value;
                OnPropertyChanged("QuintileChart1");
            }
        }
        public RadObservableCollection<KeyValuePair<string, int>> QuintileChart2
        {
            get
            {
                return _quintileChart2;
            }
            set
            {
                _quintileChart2 = value;
                OnPropertyChanged("QuintileChart2");
            }
        }

        public bool IsBusy
        {
            get { return this._isBusy; }
            set
            {
                if (this._isBusy != value)
                {
                    this._isBusy = value;
                    this.OnPropertyChanged("IsBusy");

                    if (this._isBusy)
                    {
                        var backgroundWorker = new BackgroundWorker();
                        backgroundWorker.DoWork += this.OnBackgroundWorkerDoWorkValues;
                        backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompletedValues;
                        backgroundWorker.RunWorkerAsync();

                    }
                }
            }
        }
        public string BusyContent
        {
            get { return this._busyContent; }
            set
            {
                if (this._busyContent != value)
                {
                    this._busyContent = value;
                    this.OnPropertyChanged("BusyContent");
                }
            }
        }

        public bool IsBusy2
        {
            get { return this._isBusy2; }
            set
            {
                if (this._isBusy2 != value)
                {
                    this._isBusy2 = value;
                    this.OnPropertyChanged("IsBusy2");

                    if (this._isBusy2)
                    {
                        var backgroundWorker = new BackgroundWorker();
                        backgroundWorker.DoWork += this.OnBackgroundWorkerDoWorkCharts;
                        backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompletedCharts;
                        backgroundWorker.RunWorkerAsync();
                    }
                }
            }
        }
        public string BusyContent2
        {
            get { return this._busyContent2; }
            set
            {
                if (this._busyContent2 != value)
                {
                    this._busyContent2 = value;
                    this.OnPropertyChanged("BusyContent2");
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Please, don't touch if you don't know
        /// </summary>

        public void LoadValuesExecute()
        {
            IsBusy = true;

        }

        #endregion

        /// <summary>
        /// Set a format for the columns
        /// </summary>
        /// <param name="table"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public DataTable FillAndSetFormat(DataTable table, DataTable output)
        {
            foreach (DataRow r in table.Rows)
            {
                DataRow nr = output.NewRow();
                for (int i = 0; i < r.ItemArray.Length; i++)
                {
                    String formatTwoDecimalsPercent = "{0:0.00%}";

                    String formatTwoDecimals = "{0:0.00}";

                    switch (GetColumnFromIndex(i))
                    {
                        case ("Poids"):
                            nr[i] = String.Format(formatTwoDecimalsPercent, double.Parse(r[i].ToString()) / 100);
                            break;
                        case ("Note"):
                            nr[i] = String.Format(formatTwoDecimals, r[i]);
                            break;
                        case ("Note Fi"):
                            nr[i] = String.Format(formatTwoDecimals, r[i]);
                            break;
                        case ("Growth"):
                            nr[i] = String.Format(formatTwoDecimals, r[i]);
                            break;
                        case ("Profit"):
                            nr[i] = String.Format(formatTwoDecimals, r[i]);
                            break;
                        case ("Value"):
                            nr[i] = String.Format(formatTwoDecimals, r[i]);
                            break;
                        case ("ISR"):
                            nr[i] = String.Format(formatTwoDecimals, r[i]);
                            break;
                        case ("note ISR"):
                            nr[i] = String.Format(formatTwoDecimals, r[i]);
                            break;
                        default:
                            nr[i] = r[i];
                            break;
                    }
                }
                output.Rows.Add(nr);
            }
            return output;
        }

        /// <summary>
        /// Gives you the columns header from an index if index is out, it will crash you program so be careful
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public String GetColumnFromIndex(int index)
        {
            return _valuesDataSource.Columns[index].ToString();
        }

        /// <summary>
        /// Fill the combo boxes used for the charts to select the companies and the second date
        /// </summary>
        private void FillChartsBoxes()
        {
            if (SelectedDate == null || SelectedUniverse == null
                || SelectedSuperSector == null || SelectedSuperSector == "")
                return;

            _companies = new RadObservableCollection<string>();
            List<String> v = cleaner.CleanCompanyName(_model.GetCompanies(SelectedDate, SelectedSuperSector, SelectedSector));
            v.Sort();
            _companies.Add("");
            foreach (String s in v)
                _companies.Add(s);
            Companies = _companies;
        }

        /// <summary>
        /// Call every methods for all the charts depending on the current company
        /// </summary>
        /// <param name="i"></param>
        private void FillCharts(int i)
        {

            if (i == 1 || i == 3)
            {
                if (SelectedCompany1 == "")
                {
                    ResetGraph(1);
                    return;
                }
            }
            else if (i == 2 || i == 4)
            {
                if (SelectedCompany2 == "")
                {
                    ResetGraph(2);
                    return;
                }
            }

            if (SelectedCompany1 != null && SelectedCompany1 != "")
                Ticker1 = SelectedCompany1.Split('|')[1].TrimStart();
            else Ticker1 = null;
            if (SelectedCompany2 != null && SelectedCompany2 != "")
                Ticker2 = SelectedCompany2.Split('|')[1].TrimStart();
            else Ticker2 = null;



            FillHisto(i);
            FillRadarGrowth(i);
            FillRadarValues(i);
            FillRadarProfit(i);
            FillQuintileChart(i);

            if (SelectedDate != null)
            {
                if ((Ticker2 == null || Ticker2 == "") && SelectedDate != SelectedSecondDate)
                    FillComparatif2Dates();
                else
                    FillComparatif();
            }
        }

        private void FillComparatif()
        {
            DataTable _tempsSummaryDataSource = new DataTable();

            _tempsSummaryDataSource = _model.GetValuesChart(SelectedDate, Ticker1, Ticker2);

            #region Create sub datatables
            _metriqueChartDataSource = new DataTable();
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[0].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[1].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[3].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[5].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[6].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[8].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[10].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[11].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[13].ToString());

            _scoreChartDataSource = new DataTable();
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[0].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[2].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[4].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[5].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[7].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[9].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[10].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[12].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[14].ToString());

            #endregion

            foreach (DataRow row in _tempsSummaryDataSource.Rows)
            {
                #region Round for Summary

                char separator = '.';
                if (CultureInfo.CurrentCulture.Name.ToLower().Contains("fr")
                       && CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                    separator = ',';

                if (row[1].ToString() != "")
                    row[1] = Helpers.DisplayRoundedNumber(row[1].ToString(), 2, separator);
                if (row[2].ToString() != "")
                    row[2] = Helpers.DisplayRoundedNumber(row[2].ToString(), 2, separator);
                if (row[6].ToString() != "")
                    row[6] = Helpers.DisplayRoundedNumber(row[6].ToString(), 2, separator);
                if (row[7].ToString() != "")
                    row[7] = Helpers.DisplayRoundedNumber(row[7].ToString(), 2, separator);
                if (row[11].ToString() != "")
                    row[11] = Helpers.DisplayRoundedNumber(row[11].ToString(), 2, separator);
                if (row[12].ToString() != "")
                    row[12] = Helpers.DisplayRoundedNumber(row[12].ToString(), 2, separator);
                if (row[3].ToString() != "")
                    row[3] = Helpers.DisplayRoundedNumber(row[3].ToString(), 2, separator);
                if (row[4].ToString() != "")
                    row[4] = Helpers.DisplayRoundedNumber(row[4].ToString(), 2, separator);
                if (row[8].ToString() != "")
                    row[8] = Helpers.DisplayRoundedNumber(row[8].ToString(), 2, separator);
                if (row[9].ToString() != "")
                    row[9] = Helpers.DisplayRoundedNumber(row[9].ToString(), 2, separator);
                if (row[13].ToString() != "")
                    row[13] = Helpers.DisplayRoundedNumber(row[13].ToString(), 2, separator);
                if (row[14].ToString() != "")
                    row[14] = Helpers.DisplayRoundedNumber(row[14].ToString(), 2, separator);

                #endregion

                #region Assign to new DataTable
                DataRow rowS = _scoreChartDataSource.NewRow();
                DataRow rowM = _metriqueChartDataSource.NewRow();

                for (int j = 0; j < _tempsSummaryDataSource.Columns.Count; j++)
                {
                    switch (j)
                    {
                        case (0):
                            rowS[0] = row[j];
                            rowM[0] = row[j];
                            break;
                        case (1):
                            rowM[1] = row[j];
                            break;
                        case (2):
                            rowS[1] = row[j];
                            break;
                        case (3):
                            rowM[2] = row[j];
                            break;
                        case (4):
                            rowS[2] = row[j];
                            break;
                        case (5):
                            rowS[3] = row[j];
                            rowM[3] = row[j];
                            break;
                        case (6):
                            rowM[4] = row[j];
                            break;
                        case (7):
                            rowS[4] = row[j];
                            break;
                        case (8):
                            rowM[5] = row[j];
                            break;
                        case (9):
                            rowS[5] = row[j];
                            break;
                        case (10):
                            rowS[6] = row[j];
                            rowM[6] = row[j];
                            break;
                        case (11):
                            rowM[7] = row[j];
                            break;
                        case (12):
                            rowS[7] = row[j];
                            break;
                        case (13):
                            rowM[8] = row[j];
                            break;
                        case (14):
                            rowS[8] = row[j];
                            break;
                    }
                }
                _scoreChartDataSource.Rows.Add(rowS);
                _metriqueChartDataSource.Rows.Add(rowM);
                #endregion
            }
            ScoreChartDataSource = _scoreChartDataSource;
            MetriqueChartDataSource = _metriqueChartDataSource;
        }

        private void FillComparatif2Dates()
        {
            DataTable _tempsSummaryDataSource = _model.GetValuesChart(SelectedDate, SelectedSecondDate, Ticker1, Ticker2);

            #region Create sub datatables
            _metriqueChartDataSource = new DataTable();
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[0].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[1].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[3].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[5].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[6].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[8].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[10].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[11].ToString());
            _metriqueChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[13].ToString());

            _scoreChartDataSource = new DataTable();
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[0].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[2].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[4].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[5].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[7].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[9].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[10].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[12].ToString());
            _scoreChartDataSource.Columns.Add(_tempsSummaryDataSource.Columns[14].ToString());

            #endregion

            foreach (DataRow row in _tempsSummaryDataSource.Rows)
            {
                #region Round for Summary

                char separator = '.';
                if (CultureInfo.CurrentCulture.Name.ToLower().Contains("fr")
                       && CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                    separator = ',';

                if (row[1].ToString() != "")
                    row[1] = Helpers.DisplayRoundedNumber(row[1].ToString(), 2, separator);
                if (row[2].ToString() != "")
                    row[2] = Helpers.DisplayRoundedNumber(row[2].ToString(), 2, separator);
                if (row[6].ToString() != "")
                    row[6] = Helpers.DisplayRoundedNumber(row[6].ToString(), 2, separator);
                if (row[7].ToString() != "")
                    row[7] = Helpers.DisplayRoundedNumber(row[7].ToString(), 2, separator);
                if (row[11].ToString() != "")
                    row[11] = Helpers.DisplayRoundedNumber(row[11].ToString(), 2, separator);
                if (row[12].ToString() != "")
                    row[12] = Helpers.DisplayRoundedNumber(row[12].ToString(), 2, separator);
                if (row[3].ToString() != "")
                    row[3] = Helpers.DisplayRoundedNumber(row[3].ToString(), 2, separator);
                if (row[4].ToString() != "")
                    row[4] = Helpers.DisplayRoundedNumber(row[4].ToString(), 2, separator);
                if (row[8].ToString() != "")
                    row[8] = Helpers.DisplayRoundedNumber(row[8].ToString(), 2, separator);
                if (row[9].ToString() != "")
                    row[9] = Helpers.DisplayRoundedNumber(row[9].ToString(), 2, separator);
                if (row[13].ToString() != "")
                    row[13] = Helpers.DisplayRoundedNumber(row[13].ToString(), 2, separator);
                if (row[14].ToString() != "")
                    row[14] = Helpers.DisplayRoundedNumber(row[14].ToString(), 2, separator);

                #endregion

                #region Assign to new DataTable
                DataRow rowS = _scoreChartDataSource.NewRow();
                DataRow rowM = _metriqueChartDataSource.NewRow();

                for (int j = 0; j < _tempsSummaryDataSource.Columns.Count; j++)
                {
                    switch (j)
                    {
                        case (0):
                            rowS[0] = row[j];
                            rowM[0] = row[j];
                            break;
                        case (1):
                            rowM[1] = row[j];
                            break;
                        case (2):
                            rowS[1] = row[j];
                            break;
                        case (3):
                            rowM[2] = row[j];
                            break;
                        case (4):
                            rowS[2] = row[j];
                            break;
                        case (5):
                            rowS[3] = row[j];
                            rowM[3] = row[j];
                            break;
                        case (6):
                            rowM[4] = row[j];
                            break;
                        case (7):
                            rowS[4] = row[j];
                            break;
                        case (8):
                            rowM[5] = row[j];
                            break;
                        case (9):
                            rowS[5] = row[j];
                            break;
                        case (10):
                            rowS[6] = row[j];
                            rowM[6] = row[j];
                            break;
                        case (11):
                            rowM[7] = row[j];
                            break;
                        case (12):
                            rowS[7] = row[j];
                            break;
                        case (13):
                            rowM[8] = row[j];
                            break;
                        case (14):
                            rowS[8] = row[j];
                            break;
                    }
                }
                _scoreChartDataSource.Rows.Add(rowS);
                _metriqueChartDataSource.Rows.Add(rowM);
                #endregion
            }
            ScoreChartDataSource = _scoreChartDataSource;
            MetriqueChartDataSource = _metriqueChartDataSource;
        }

        /// <summary>
        /// Self explanatory
        /// </summary>
        /// <param name="i"></param>
        private void FillHisto(int i)
        {
            if (i == 1 && SelectedCompany1 != null)
            {
                ColumnsSeriesHisto1 = _model.GetHistoData(SelectedDate,
                    SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 2 && SelectedCompany2 != null)
            {
                ColumnsSeriesHisto2 = _model.GetHistoData(SelectedDate,
                    SelectedCompany2.Split('|')[1].TrimStart());
            }
            if (i == 3 && SelectedCompany1 != null)
            {
                ColumnsSeriesHisto3 = _model.GetHistoData(SelectedSecondDate,
                    SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 4 && SelectedCompany2 != null)
            {
                ColumnsSeriesHisto4 = _model.GetHistoData(SelectedSecondDate,
                    SelectedCompany2.Split('|')[1].TrimStart());
            }
        }

        /// <summary>
        /// Self explanatory
        /// </summary>
        /// <param name="i"></param>
        private void FillRadarGrowth(int i)
        {
            if (i == 1 && SelectedCompany1 != null)
            {
                RadarGrowthSeries1 = _model.GetRadarGrowthData(SelectedDate,
                    SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 2 && SelectedCompany2 != null)
            {
                RadarGrowthSeries2 = _model.GetRadarGrowthData(SelectedDate,
                    SelectedCompany2.Split('|')[1].TrimStart());
            }
            if (i == 3 && SelectedCompany1 != null)
            {
                RadarGrowthSeries3 = _model.GetRadarGrowthData(SelectedSecondDate,
                    SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 4 && SelectedCompany2 != null)
            {
                RadarGrowthSeries4 = _model.GetRadarGrowthData(SelectedSecondDate,
                    SelectedCompany2.Split('|')[1].TrimStart());
            }
        }

        /// <summary>
        /// Self explanatory
        /// </summary>
        /// <param name="i"></param>
        private void FillRadarValues(int i)
        {
            if (i == 1 && SelectedCompany1 != null)
            {
                RadarValuesSeries1 = _model.GetRadarValuesData(SelectedDate,
                    SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 2 && SelectedCompany2 != null)
            {
                RadarValuesSeries2 = _model.GetRadarValuesData(SelectedDate,
                    SelectedCompany2.Split('|')[1].TrimStart());
            }
            if (i == 3 && SelectedCompany1 != null)
            {
                RadarValuesSeries3 = _model.GetRadarValuesData(SelectedSecondDate,
                    SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 4 && SelectedCompany2 != null)
            {
                RadarValuesSeries4 = _model.GetRadarValuesData(SelectedSecondDate,
                    SelectedCompany2.Split('|')[1].TrimStart());
            }

        }

        /// <summary>
        /// Self explanatory
        /// </summary>
        /// <param name="i"></param>
        private void FillRadarProfit(int i)
        {
            if (i == 1 && SelectedCompany1 != null)
            {
                RadarProfitSeries1 = _model.GetRadarProfitData(SelectedDate,
                    SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 2 && SelectedCompany2 != null)
            {
                RadarProfitSeries2 = _model.GetRadarProfitData(SelectedDate,
                    SelectedCompany2.Split('|')[1].TrimStart());
            }
            if (i == 3 && SelectedCompany1 != null)
            {
                RadarProfitSeries3 = _model.GetRadarProfitData(SelectedSecondDate,
                    SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 4 && SelectedCompany2 != null)
            {
                RadarProfitSeries4 = _model.GetRadarProfitData(SelectedSecondDate,
                    SelectedCompany2.Split('|')[1].TrimStart());
            }
        }

        private void FillQuintileChart(int i)
        {
            if (i == 1 && SelectedCompany1 != null)
            {
                QuintileChart1 = _model.GetQuintile(SelectedCompany1.Split('|')[1].TrimStart());
            }
            if (i == 2 && SelectedCompany2 != null)
            {
                QuintileChart2 = _model.GetQuintile(SelectedCompany2.Split('|')[1].TrimStart());
            }
        }

        private void ResetGraph(int i)
        {
            if (i == 1)
            {
                ColumnsSeriesHisto1 = null;
                ColumnsSeriesHisto3 = null;

                RadarGrowthSeries1 = null;
                RadarGrowthSeries3 = null;

                RadarProfitSeries1 = null;
                RadarProfitSeries3 = null;

                RadarValuesSeries1 = null;
                RadarValuesSeries3 = null;

                QuintileChart1 = null;
            }
            else
            {
                ColumnsSeriesHisto2 = null;
                ColumnsSeriesHisto4 = null;

                RadarGrowthSeries2 = null;
                RadarGrowthSeries4 = null;

                RadarProfitSeries2 = null;
                RadarProfitSeries4 = null;

                RadarValuesSeries2 = null;
                RadarValuesSeries4 = null;

                QuintileChart2 = null;
            }

            ScoreChartDataSource = null;
            MetriqueChartDataSource = null;
        }

        #region BackgroundWorker

        #region Values
        private void OnBackgroundWorkerRunWorkerCompletedValues(object sender, RunWorkerCompletedEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWorkValues;
            backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompletedValues;

            InvokeOnUIThread(() =>
            {
                this.IsBusy = false;
            });
        }

        private void OnBackgroundWorkerDoWorkValues(object sender, DoWorkEventArgs e)
        {
            ProceedDatas();
        }

        private void ProceedDatas()
        {
            ResetGraph(1);
            ResetGraph(2);

            var dpb = new GenericDataPointBinding<KeyValuePair<int, double>, int>();
            dpb.ValueSelector = pair => pair.Key;

            if (SelectedDate == null || SelectedUniverse == null ||
                SelectedSuperSector == null || SelectedSuperSector == "")
                return;

            _valuesDataSource = new DataTable();

            InvokeOnUIThread(() =>
            {
                this.BusyContent = string.Concat("Loading Values And Cleaning Companies\n");
            });

            DataTable tmp = cleaner.CleanCompanyName(_model.FillValeursAnalyse(SelectedDate, SelectedUniverse,
                SelectedSuperSector, SelectedSector), "TICKER", "Company Name");

            for (int i = 0; i < tmp.Columns.Count; i++)
            {
                _valuesDataSource.Columns.Add(tmp.Columns[i].ToString());
                _valuesDataSource.Columns[i].DataType = typeof(String);
            }


            this.ValuesDataSource = FillAndSetFormat(tmp, this._valuesDataSource);
            this.ValuesDataSource.DefaultView.Sort = ("Note DESC");

            GlobalInfos.tickerQuintQaunt = new Dictionary<string, int>();
            foreach (DataRow dr in ValuesDataSource.Rows)
            {
                GlobalInfos.tickerQuintQaunt.Add(dr["Ticker"].ToString(), int.Parse(dr["Quint Quant"].ToString()));
            }
            this.FillChartsBoxes();

            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Recreating all tables for filters\n");
            });

            FillColsValid();
            ValuesDataSource = RecreateTables(ValuesDataSource);
        }
        #endregion

        #region Charts

        private void OnBackgroundWorkerRunWorkerCompletedCharts(object sender, RunWorkerCompletedEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWorkCharts;
            backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompletedCharts;

            InvokeOnUIThread(() =>
            {


                IsBusy2 = false;
            });
        }

        private void OnBackgroundWorkerDoWorkCharts(object sender, DoWorkEventArgs e)
        {
            ProceedFillCharts();
        }

        private void ProceedFillCharts()
        {
            if (origin == 1)
            {
                FillCharts(1);
                FillCharts(3);
            }
            else if (origin == 2)
            {
                FillCharts(2);
                FillCharts(4);
            }
            else if (origin == 34)
            {
                FillCharts(3);
                FillCharts(4);
            }
        }

        #endregion

        public DataTable RecreateTables(DataTable source)
        {

            DataTable tmp = new DataTable();

            foreach (DataColumn col in source.Columns)
            {
                DataColumn newCol = new DataColumn();
                newCol.ColumnName = col.ColumnName;

                if (colsValid.Contains(col.ColumnName))
                    newCol.DataType = typeof(String);
                else
                    newCol.DataType = typeof(DoubleFormatted);
                tmp.Columns.Add(newCol);
            }

            foreach (DataRow row in source.Rows)
            {
                DataRow newRow = tmp.NewRow();
                foreach (DataColumn col in source.Columns)
                {

                    if (colsValid.Contains(col.ColumnName))
                    {
                        String val = row[col.ColumnName].ToString();
                        newRow[col.ColumnName] = val;
                    }
                    else
                    {
                        DoubleFormatted n = new DoubleFormatted(row[col.ColumnName].ToString());
                        newRow[col.ColumnName] = n;
                    }
                }
                tmp.Rows.Add(newRow);
            }

            return tmp;
        }

        List<String> colsValid = new List<string>();

        public void FillColsValid()
        {
            colsValid.Add("Ticker");
            colsValid.Add("Company Name");
            colsValid.Add("Crncy");
            colsValid.Add("liquidity");
            colsValid.Add("Country");
        }

        #endregion

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

        #endregion
    }
}