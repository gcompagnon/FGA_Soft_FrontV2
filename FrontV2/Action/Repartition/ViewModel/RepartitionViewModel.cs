using FrontV2.Action.Repartition.Model;
using FrontV2.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Data;

namespace FrontV2.Action.Repartition.ViewModel
{
    public class ChartPoint
    {
        public string Date { get; set; }
        public string Label { get; set; }
        public double Value { get; set; }
        public ChartPoint(String date, double value, String label)
        {
            Date = date;
            Value = value;
            Label = label;
        }
    }

    public class ChartSerie
    {
        public string Label { get; set; }
        public RadObservableCollection<ChartPoint> Data { get; set; }
    }

    class RepartitionViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public RepartitionViewModel()
        {
            _model = new RepartitionModel();

            _dates = _model.GetDates();
            SelectedDate = _dates[0];

            _tickers = _model.GetAllTickers();
            _portefeuilles = new RadObservableCollection<string> { "6100002", "6100030",
                "AVEURO", "6100004", "6100063", "AVEUROPE", "6100001",
                "6100033", "6100062", "6100026", "6100024" };
        }

        #region Fields

        private RepartitionModel _model;

        private RadObservableCollection<String> _dates;
        private String _selectedDate;

        private RadObservableCollection<String> _tickers;

        private DataTable _sectorsDataSource;
        private DataTable _countriesDataSource;
        private DataTable _industriesDataSource;

        private RadObservableCollection<String> _portefeuilles;
        private String _selectedPortefeuille;

        private bool _showGap;

        private RadObservableCollection<ChartSerie> _countryChartDataSource;
        private RadObservableCollection<ChartSerie> _sectorsChartDataSource;
        private RadObservableCollection<ChartSerie> _industriesChartDataSource;

        private bool _isbusy;
        private String _busyContent;

        #endregion

        #region Properties

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
                OnPropertyChanged("SelectedDate");
                LoadSectorsDataSource();
                LoadCountriesDataSource();
                LoadIndustriesDataSource();
            }
        }
        
        public DataTable SectorsDataSource
        {
            get { return _sectorsDataSource; }
            set
            {
                _sectorsDataSource = value;
                OnPropertyChanged("SectorsDataSource");
            }
        }
        public DataTable CountriesDataSource
        {
            get { return _countriesDataSource; }
            set
            {
                _countriesDataSource = value;
                OnPropertyChanged("CountriesDataSource");
            }
        }
        public DataTable IndustriesDataSource
        {
            get { return _industriesDataSource; }
            set
            {
                _industriesDataSource = value;
                OnPropertyChanged("IndustriesDataSource");
            }
        }

        public bool ShowGap
        {
            get { return _showGap; }
            set
            {
                _showGap = value;
                if (value == true)
                    Portefeuilles = new RadObservableCollection<string> { "Ecart_6100002", "Ecart_6100030",
                        "Ecart_AVEURO", "Ecart_6100004", "Ecart_6100063", "Ecart_AVEUROPE", "Ecart_6100001",
                        "Ecart_6100033", "Ecart_6100062", "Ecart_6100026", "Ecart_6100024" };
                else
                    Portefeuilles = new RadObservableCollection<string> { "6100002", "6100030",
                        "AVEURO", "6100004", "6100063", "AVEUROPE", "6100001",
                        "6100033", "6100062", "6100026", "6100024" };

                OnPropertyChanged("ShowGap");
            }
        }

        public RadObservableCollection<String> Portefeuilles
        {
            get { return _portefeuilles; }
            set
            {
                _portefeuilles = value;
                OnPropertyChanged("Portefeuilles");
            }
        }
        public String SelectedPortefeuille
        {
            get { return _selectedPortefeuille; }
            set
            {
                _selectedPortefeuille = value;

                FillCharts();
                
                OnPropertyChanged("SelectedPortefeuille");
            }
        }

        public RadObservableCollection<ChartSerie> CountryChartDataSource
        {
            get { return _countryChartDataSource; }
            set
            {
                _countryChartDataSource = value;
                OnPropertyChanged("CountryChartDataSource");
            }
        }
        public RadObservableCollection<ChartSerie> SectorsChartDataSource
        {
            get { return _sectorsChartDataSource; }
            set
            {
                _sectorsChartDataSource = value;
                OnPropertyChanged("SectorsChartDataSource");
            }
        }
        public RadObservableCollection<ChartSerie> IndustriesChartDataSource
        {
            get { return _industriesChartDataSource; }
            set
            {
                _industriesChartDataSource = value;
                OnPropertyChanged("IndustriesChartDataSource");
            }
        }

        public bool IsBusy
        {
            get { return _isbusy; }
            set
            {
                if (this._isbusy != value)
                {
                    this._isbusy = value;
                    this.OnPropertyChanged("IsBusy");

                    if (this._isbusy)
                    {
                        var backgroundWorker = new BackgroundWorker();
                        backgroundWorker.DoWork += this.OnBackgroundWorkerDoWork;
                        backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
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

        #endregion

        #region Methods

        public void LoadSectorsDataSource()
        {
            SectorsDataSource = _model.GetSectorsDataSource(SelectedDate);
        }

        public void LoadCountriesDataSource()
        {
            CountriesDataSource = _model.GetCountriesDataSource(SelectedDate);
        }

        public void LoadIndustriesDataSource()
        {
            IndustriesDataSource = _model.GetIndustriesDataSource(SelectedDate);
        }

        public void FillIndustriesChart(String sector)
        {
            IndustriesChartDataSource = _model.GetChartIndustriesDataSource(_selectedPortefeuille, sector);
        }

        public String PortToBench(String port)
        {
            if (port == "6100002")
                return "MXFR";
            else if (port == "6100030" || port == "AVEURO" || port == "6100004"
                || port == "6100063" || port == "AVEUROPE" || port == "6100001" || port == "6100033")
                return "MXEM";
            else if (port == "6100062")
                return "MXEUM";
            else if (port == "6100026")
                return "MXEU";
            else if (port == "6100024" || port == "6100066")
                return "MXUSLC";
            else
                return "";
        }

        public bool IsCountryInBench(String country, String bench)
        {
            if (bench == "MXFR")
            {
                if (country == "France")
                    return true;
                else
                    return false;
            }
            else if (bench == "MXEM")
            {
                if (country == "France")
                    return true;
                else if (country == "Germany")
                    return true;
                else if (country == "Spain")
                    return true;
                else if (country == "Netherlands")
                    return true;
                else if (country == "Italy")
                    return true;
                else
                    return false;
            }
            else if (bench == "MXEUM")
            {
                if (country == "United Kingdom")
                    return true;
                else if (country == "Switzerland")
                    return true;
                else if (country == "Sweden")
                    return true;
                else if (country == "Denmark")
                    return true;
                else if (country == "Norway")
                    return true;
                else
                    return false;
            }
            else if (bench == "MXEU")
                return IsCountryInBench(country, "MXEM") || IsCountryInBench(country, "MXEUM");
            else if (bench == "MXUSLC")
            {
                if (country == "USA")
                    return true;
                else
                    return false;
            }
            return false;
        }

        public void FillCountriesChart()
        {

            _countryChartDataSource = new RadObservableCollection<ChartSerie>();

            RadObservableCollection<ChartPoint> othersDataSource = null;
            String currentBench = PortToBench(SelectedPortefeuille.Replace("Ecart_", ""));

            foreach (DataRow row in CountriesDataSource.Rows)
            {
                ChartSerie tmp =
                    _model.GetChartDataForCountry(SelectedPortefeuille, row["COUNTRY"].ToString());

                if (IsCountryInBench(tmp.Label, currentBench))
                    _countryChartDataSource.Add(tmp);
                else
                {
                    if (othersDataSource == null)
                        othersDataSource = tmp.Data;
                    else
                    {
                        for (int i = 0; i < tmp.Data.Count; i++)
                        {
                            othersDataSource[i].Value += tmp.Data[i].Value;
                            othersDataSource[i].Label = "Others";
                        }
                    }
                }

            }
            ChartSerie others = new ChartSerie() { Label = "Others", Data = othersDataSource };
            _countryChartDataSource.Add(others);
            CountryChartDataSource = _countryChartDataSource;

        }

        public void FillSectorsChart()
        {
            _sectorsChartDataSource = new RadObservableCollection<ChartSerie>();

            RadObservableCollection<ChartSerie> tmpChart = new RadObservableCollection<ChartSerie>();

            foreach (DataRow row in SectorsDataSource.Rows)
            {
                ChartSerie tmp =
                    _model.GetChartDataForSector(_selectedPortefeuille, row["SECTOR GICS"].ToString());
                if (tmp.Data.Count > 0)
                    tmpChart.Add(tmp);
            }
            SectorsChartDataSource = tmpChart;
        }

        public void FillCharts()
        {
            IsBusy = true;
        }


        #endregion

        private void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;

            InvokeOnUIThread(() =>
            {
                this.IsBusy = false;
            });
        }

        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ProceedDatas();
        }

        public void ProceedDatas()
        {
            if (SelectedPortefeuille == null)
            {
                CountryChartDataSource = null;
                SectorsChartDataSource = null;
                IndustriesChartDataSource = null;

                return;
            }

            InvokeOnUIThread(() =>
            {
                this.BusyContent = "Reset charts\n";
            });                      


            InvokeOnUIThread(() =>
            {
                this.BusyContent = string.Concat("Draw Countries\n");
            });
            FillCountriesChart();

            InvokeOnUIThread(() =>
            {
                this.BusyContent = string.Concat("Draw Sectors\n");
            });
            FillSectorsChart();

            IndustriesChartDataSource = null;

        }

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

        #endregion

    }
}
