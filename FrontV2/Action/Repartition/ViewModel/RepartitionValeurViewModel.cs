using FrontV2.Action.Repartition.Model;
using FrontV2.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace FrontV2.Action.Repartition.ViewModel
{
    class RepartitionValeurViewModel : ViewModelBase, INotifyPropertyChanged
    {
        RepartitionValeurModel _model;

        public RepartitionValeurViewModel()
        {
            _model = new RepartitionValeurModel();

            Dates = _model.GetDates();
            SelectedDate = _dates[0];
            AvailableTickers = _model.GetAllTickers();
            SelectedTickers = new RadObservableCollection<string>();
        }

        #region Fields

        private RadObservableCollection<String> _dates;
        private String _selectedDate;

        private bool _showGap;

        private RadObservableCollection<String> _availableTickers;
        private RadObservableCollection<String> _selectedTickers;

        private DataTable _valuesDataSource;
        private DataTable _positionsDataSource;

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
            }
        }

        public bool ShowGap
        {
            get { return _showGap; }
            set
            {
                _showGap = value;
                OnPropertyChanged("ShowGap");
            }
        }

        public RadObservableCollection<String> AvailableTickers
        {
            get { return _availableTickers; }
            set
            {
                _availableTickers = value;
                OnPropertyChanged("AvailableTickers");
            }
        }
        public RadObservableCollection<String> SelectedTickers
        {
            get { return _selectedTickers; }
            set
            {
                _selectedTickers = value;
                OnPropertyChanged("SelectedTickers");
            }
        }

        public DataTable ValuesDataSource
        {
            get { return _valuesDataSource; }
            set
            {
                _valuesDataSource = value;
                OnPropertyChanged("ValuesDataSource");
            }
        }
        public DataTable PositionsDataSource
        {
            get { return _positionsDataSource; }
            set
            {
                _positionsDataSource = value;
                OnPropertyChanged("PositionsDataSource");
            }
        }

        #endregion

        #region Methods
        public void LoadGrids()
        {
            DataTable tmpV = new DataTable();
            DataTable tmpP = new DataTable();

            bool first = true;
            foreach (String str in _selectedTickers)
            {
                if (first)
                {
                    tmpV = _model.GetValueDataSource(SelectedDate, str);
                    tmpP = _model.GetPositionsDataSource(SelectedDate, str);
                    first = false;
                }
                else
                {
                    tmpV.Merge(_model.GetValueDataSource(SelectedDate, str));
                    tmpP.Merge(_model.GetPositionsDataSource(SelectedDate, str));
                }
            }
            CompanyNameCleaner cleaner = new CompanyNameCleaner();

            ValuesDataSource = cleaner.CleanCompanyName(tmpV, "Ticker", "Company");
            PositionsDataSource = cleaner.CleanCompanyName(tmpP, "Ticker", "Company");
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
