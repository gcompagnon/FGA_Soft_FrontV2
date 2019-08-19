using FrontV2.Action.SimulationScore.Model;
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

namespace FrontV2.Action.SimulationScore.ViewModel
{
    class SimulationScoreViewModel : ViewModelBase
    {
        #region Constructor

        public SimulationScoreViewModel()
        {
            foreach (String s in _model.GetSectorsICB())
                _supersectors.Add(s);

            _dates = _model.GetDates();

            _selectedDate = _dates[0];
            _selectedSecondDate = _dates[0];
        }

        public void StartCheck()
        {
            if (_model.IsActCoefCritereEmpty())
                _model.CopyActCoefCritere();
        }

        #endregion

        #region Fields

        private bool _isBusy;
        private string _busyContent;

        private readonly SimulationScoreModel _model = new SimulationScoreModel();
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

        //DataGrids
        private DataTable _valuesDataSource = new DataTable();

       
        #endregion

        #region Properties

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

        #endregion

        #region Commands

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

            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Recreating all tables for filters\n");
            });

            FillColsValid();
            ValuesDataSource = RecreateTables(ValuesDataSource);
        }
        #endregion

        public void CalculateScores()
        {
            InvokeOnUIThread(() =>
            {
                this.BusyContent = string.Concat("Clear simulation at Date = " + SelectedDate + "\n");
            });
            _model.ClearDataFactsetSimulation(SelectedDate);

            InvokeOnUIThread(() =>
            {
                this.BusyContent = string.Concat("Copying Data_Factset at Date = " + SelectedDate + "\n");
            });
            _model.CopyDataFactSet(SelectedDate);


            InvokeOnUIThread(() =>
            {
                this.BusyContent = string.Concat("Calculating Scores\n");
            });
            _model.CalculateNewScores(SelectedDate);
        }

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