using FrontV2.Action.Consultation.Model;
using FrontV2.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace FrontV2.Action.Consultation.ViewModel
{
    class BaseActionConsultationViewModel : ViewModelBase
    {
        #region Constructor

        public BaseActionConsultationViewModel()
        {
            GlobalInfos.rowESGExclu = new List<string>();

            load();
            _dates = _model.getDates();
            
            FillGeneralTableColumns();
            FillCroissanceTableComuns();
            FillQualityTableColumns();
            FillValorisationTableColumns();
            FillMomentumTableColumns();
            FillSyntheseTableColumns();
        }

        #endregion

        #region Fields

        readonly Connection co = new Connection();

        private readonly BaseActionConsultationModel _model = new BaseActionConsultationModel();

        // Datatables
        public DataTable _generalDataSource = new DataTable();
        public DataTable _croissanceDataSource = new DataTable();
        public DataTable _qualityDataSource = new DataTable();
        public DataTable _valorisationDataSource = new DataTable();
        public DataTable _momentumDataSource = new DataTable();
        public DataTable _syntheseDataSource = new DataTable();

        // ComboBoxes
        private RadObservableCollection<String> _dates = new RadObservableCollection<String>();
        private String _selectedDate;
        private RadObservableCollection<String> _univers = new RadObservableCollection<String>();
        private String _selectedUniverse;
        private RadObservableCollection<Sector> _supersectors = new RadObservableCollection<Sector>();
        private Sector _selectedSuperSector;
        private RadObservableCollection<Sector> _sectors = new RadObservableCollection<Sector>();
        private Sector _selectedSector;

        //Agr
        public static StringBuilder GeneralAgr = new StringBuilder();
        public static StringBuilder CroissanceAgr = new StringBuilder();
        public static StringBuilder QualityAgr = new StringBuilder();
        public static StringBuilder ValorisationAgr = new StringBuilder();
        public static StringBuilder MomentumAgr = new StringBuilder();
        public static StringBuilder SyntheseAgr = new StringBuilder();

        private bool _isbusy;
        private String _busyContent;
        #endregion

        #region Properties

        // Datatables
        public DataTable GeneralDataSource
        {
            get
            {
                return _generalDataSource;
            }
            set
            {
                _generalDataSource = value;
                OnPropertyChanged("GeneralDataSource");
            }
        }
        public DataTable CroissanceDataSource
        {
            get
            {
                return _croissanceDataSource;
            }
            set
            {
                _croissanceDataSource = value;
                OnPropertyChanged("CroissanceDataSource");
            }
        }
        public DataTable QualityDataSource
        {
            get
            {
                return _qualityDataSource;
            }
            set
            {
                _qualityDataSource = value;
                OnPropertyChanged("QualityDataSource");
            }
        }
        public DataTable ValorisationDataSource
        {
            get
            {
                return _valorisationDataSource;
            }
            set
            {
                _valorisationDataSource = value;
                OnPropertyChanged("ValorisationDataSource");
            }
        }
        public DataTable MomentumDataSource
        {
            get
            {
                return _momentumDataSource;
            }
            set
            {
                _momentumDataSource = value;
                OnPropertyChanged("MomentumDataSource");
            }
        }
        public DataTable SyntheseDataSource
        {
            get
            {
                return _syntheseDataSource;
            }
            set
            {
                _syntheseDataSource = value;
                OnPropertyChanged("SyntheseDataSource");
            }
        }

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
                SuperSectors = _model.getSuperSectors();
                Sectors = _model.getSectors(_selectedUniverse, _selectedSuperSector);
                //SelectedSuperSector = _supersectors[1];
                FillGeneralTableColumns();
                FillSyntheseTableColumns();
            }
        }
        public RadObservableCollection<Sector> SuperSectors
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
        public Sector SelectedSuperSector
        {
            get
            {
                return _selectedSuperSector;
            }
            set
            {
                _selectedSuperSector = value;
                OnPropertyChanged("SelectedSuperSector");
                Sectors = _model.getSectors(_selectedUniverse, _selectedSuperSector);
                //SelectedSector = _sectors[0];
                FillQualityTableColumns();
                FillSyntheseTableColumns();
            }
        }
        public RadObservableCollection<Sector> Sectors
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
        public Sector SelectedSector
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

        /// <summary>
        /// Load default properties used in the view
        /// </summary>
        private void load()
        {
            _dates = _model.getDates();
            _selectedDate = _dates[0];

            _univers = _model.getUnivers();
            _selectedUniverse = _univers[0];
        }

        #endregion

        #region Commands

        public void LoadDataExecute()
        {
            IsBusy = true;
        }

        #endregion

        #region Fill Columns Per Tab

        /// <summary>
        /// Fill the columns for the general datagrid tab
        /// </summary>
        public void FillGeneralTableColumns()
        {
            _generalDataSource = new DataTable();

            _generalDataSource.Columns.Add("TICKER");
            _generalDataSource.Columns.Add("COMPANY_NAME");
            _generalDataSource.Columns.Add("SECTOR");
            _generalDataSource.Columns.Add("SuperSecteurId");
            _generalDataSource.Columns.Add("INDUSTRY");
            _generalDataSource.Columns.Add("SecteurId");
            _generalDataSource.Columns.Add("Isin");
            _generalDataSource.Columns.Add("SUIVI");
            _generalDataSource.Columns.Add("COUNTRY");
            _generalDataSource.Columns.Add("CURRENCY");
            _generalDataSource.Columns.Add("PRICE");
            _generalDataSource.Columns.Add("PRICE_EUR");
            if (SelectedUniverse.CompareTo("ALL") == 0)
            {
                _generalDataSource.Columns.Add("MXEU");
                _generalDataSource.Columns.Add("MXUSLC");
            }
            else if (SelectedUniverse.CompareTo("EUROPE") == 0)
                _generalDataSource.Columns.Add("MXEU");
            else if (SelectedUniverse.CompareTo("USA") == 0)
                _generalDataSource.Columns.Add("MXUSLC");
            else if (SelectedUniverse.CompareTo("EMU") == 0)
                _generalDataSource.Columns.Add("MXEM");
            else if (SelectedUniverse.CompareTo("EUROPE EX EMU") == 0)
                _generalDataSource.Columns.Add("MXEUM");
            else if (SelectedUniverse.CompareTo("FRANCE") == 0)
                _generalDataSource.Columns.Add("MXFR");
            else if (SelectedUniverse.CompareTo("FEDERIS ACTIONS") == 0)
            {
                _generalDataSource.Columns.Add("MXEM");
                _generalDataSource.Columns.Add("6100001");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
            {
                _generalDataSource.Columns.Add("MXFR");
                _generalDataSource.Columns.Add("6100002");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS ISR EURO") == 0)
            {
                _generalDataSource.Columns.Add("MXEM");
                _generalDataSource.Columns.Add("6100004");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS NORTH AMERICA") == 0)
            {
                _generalDataSource.Columns.Add("MXUSLC");
                _generalDataSource.Columns.Add("6100024");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
            {
                _generalDataSource.Columns.Add("MXEU");
                _generalDataSource.Columns.Add("6100026");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EURO ACTIONS") == 0)
            {
                _generalDataSource.Columns.Add("MXEM");
                _generalDataSource.Columns.Add("6100030");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS IRC ACTIONS") == 0)
            {
                _generalDataSource.Columns.Add("MXEM");
                _generalDataSource.Columns.Add("6100033");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EX EURO") == 0)
            {
                _generalDataSource.Columns.Add("MXEUM");
                _generalDataSource.Columns.Add("6100062");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS CROISSANCE EURO") == 0)
            {
                _generalDataSource.Columns.Add("MXEM");
                _generalDataSource.Columns.Add("6100063");
            }
            else if (SelectedUniverse.CompareTo("AVENIR EURO") == 0)
            {
                _generalDataSource.Columns.Add("MXEM");
                _generalDataSource.Columns.Add("AVEURO");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS VALUE EURO") == 0)
            {
                _generalDataSource.Columns.Add("MXEM");
                _generalDataSource.Columns.Add("AVEUROPE");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS ISR AMERIQUE") == 0)
            {
                _generalDataSource.Columns.Add("MXUSLC");
                _generalDataSource.Columns.Add("6100066");
            }
            _generalDataSource.Columns.Add("MARKET_CAP_EUR");
            _generalDataSource.Columns.Add("EV_NTM_EUR");
            _generalDataSource.Columns.Add("SALES_NTM_EUR");
            _generalDataSource.Columns.Add("ESG");
            _generalDataSource.Columns.Add("LIQUIDITY_TEST");
            _generalDataSource.Columns.Add("LIQUIDITY");

            _generalDataSource.DefaultView.Sort = "SECTOR, INDUSTRY";

            OnPropertyChanged("GeneralDataSource");
            ////////////////
            //    Agr
            ///////////////
            GeneralAgr.Clear();
            GeneralAgr.Append(" fac.COUNTRY,");
            GeneralAgr.Append(" fac.CURRENCY,");
            GeneralAgr.Append(" fac.PRICE,");
            GeneralAgr.Append(" fac.PRICE_EUR,");
            if (SelectedUniverse.CompareTo("ALL") == 0)
            {
                GeneralAgr.Append(" fac.MXEU,");
                GeneralAgr.Append(" fac.MXUSLC,");
            }
            else if (SelectedUniverse.CompareTo("EUROPE") == 0)
                GeneralAgr.Append(" fac.MXEU,");
            else if (SelectedUniverse.CompareTo("USA") == 0)
                GeneralAgr.Append(" fac.MXUSLC,");
            else if (SelectedUniverse.CompareTo("EMU") == 0)
                GeneralAgr.Append(" fac.MXEM,");
            else if (SelectedUniverse.CompareTo("EUROPE EX EMU") == 0)
                GeneralAgr.Append(" fac.MXEUM,");
            else if (SelectedUniverse.CompareTo("FRANCE") == 0)
                GeneralAgr.Append(" fac.MXFR,");
            else if (SelectedUniverse.CompareTo("FEDERIS ACTIONS") == 0)
            {
                GeneralAgr.Append(" fac.MXEM,");
                GeneralAgr.Append(" fac.[6100001],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
            {
                GeneralAgr.Append(" fac.MXFR,");
                GeneralAgr.Append(" fac.[6100002],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS ISR EURO") == 0)
            {
                GeneralAgr.Append(" fac.MXEM,");
                GeneralAgr.Append(" fac.[6100004],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS NORTH AMERICA") == 0)
            {
                GeneralAgr.Append(" fac.MXUSLC,");
                GeneralAgr.Append(" fac.[6100024],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
            {
                GeneralAgr.Append(" fac.MXEU,");
                GeneralAgr.Append(" fac.[6100026],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EURO ACTIONS") == 0)
            {
                GeneralAgr.Append(" fac.MXEM,");
                GeneralAgr.Append(" fac.[6100030],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS IRC ACTIONS") == 0)
            {
                GeneralAgr.Append(" fac.MXEM,");
                GeneralAgr.Append(" fac.[6100033],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EX EURO") == 0)
            {
                GeneralAgr.Append(" fac.MXEUM,");
                GeneralAgr.Append(" fac.[6100062],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS CROISSANCE EURO") == 0)
            {
                GeneralAgr.Append(" fac.MXEM,");
                GeneralAgr.Append(" fac.[6100063],");
            }
            else if (SelectedUniverse.CompareTo("AVENIR EURO") == 0)
            {
                GeneralAgr.Append(" fac.MXEM,");
                GeneralAgr.Append(" fac.AVEURO,");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS VALUE EURO") == 0)
            {
                GeneralAgr.Append(" fac.MXEM,");
                GeneralAgr.Append(" fac.AVEUROPE,");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS ISR AMERIQUE") == 0)
            {
                GeneralAgr.Append(" fac.MXUSLC,");
                GeneralAgr.Append(" fac.[6100066],");
            }
            GeneralAgr.Append(" fac.MARKET_CAP_EUR,");
            GeneralAgr.Append(" fac.EV_NTM_EUR,");
            GeneralAgr.Append(" fac.SALES_NTM_EUR,");
            GeneralAgr.Append(" fac.ESG,");
            GeneralAgr.Append(" fac.LIQUIDITY_TEST,");
            GeneralAgr.Append(" fac.LIQUIDITY ");
        }

        /// <summary>
        /// Fill the columns for the croissance datagrid tab
        /// </summary>
        public void FillCroissanceTableComuns()
        {
            _croissanceDataSource = new DataTable();

            _croissanceDataSource.Columns.Add("TICKER");
            _croissanceDataSource.Columns.Add("COMPANY_NAME");
            _croissanceDataSource.Columns.Add("SECTOR");
            _croissanceDataSource.Columns.Add("SuperSecteurId");
            _croissanceDataSource.Columns.Add("INDUSTRY");
            _croissanceDataSource.Columns.Add("SecteurId");
            _croissanceDataSource.Columns.Add("Isin");
            _croissanceDataSource.Columns.Add("SUIVI");
            _croissanceDataSource.Columns.Add("COUNTRY");
            _croissanceDataSource.Columns.Add("CURRENCY");

            _croissanceDataSource.Columns.Add("IGROWTH_NTM");
            _croissanceDataSource.Columns.Add("EPS_CHG_NTM");
            _croissanceDataSource.Columns.Add("EPS_CHG_STM");
            _croissanceDataSource.Columns.Add("EBIT_CHG_NTM");
            _croissanceDataSource.Columns.Add("EBIT_CHG_STM");
            _croissanceDataSource.Columns.Add("EBIT_MARGIN_DIFF_NTM");
            _croissanceDataSource.Columns.Add("EBIT_MARGIN_DIFF_STM");
            _croissanceDataSource.Columns.Add("SALES_CHG_NTM");
            _croissanceDataSource.Columns.Add("SALES_CHG_STM");
            _croissanceDataSource.Columns.Add("CAPEX_CHG_NTM");
            _croissanceDataSource.Columns.Add("CAPEX_CHG_STM");
            _croissanceDataSource.Columns.Add("FCF_CHG_NTM");
            _croissanceDataSource.Columns.Add("FCF_CHG_STM");

            _croissanceDataSource.DefaultView.Sort = "SECTOR, INDUSTRY";

            OnPropertyChanged("CroissanceDataSource");

            ////////////////
            //    Agr
            ///////////////
            CroissanceAgr.Clear();
            CroissanceAgr.Append(" fac.COUNTRY,");
            CroissanceAgr.Append(" fac.CURRENCY,");
            CroissanceAgr.Append(" fac.IGROWTH_NTM, ");
            CroissanceAgr.Append(" fac.EPS_CHG_NTM, ");
            CroissanceAgr.Append(" fac.EPS_CHG_STM, ");
            CroissanceAgr.Append(" fac.EBIT_CHG_NTM, ");
            CroissanceAgr.Append(" fac.EBIT_CHG_STM, ");
            CroissanceAgr.Append(" fac.EBIT_MARGIN_DIFF_NTM, ");
            CroissanceAgr.Append(" fac.EBIT_MARGIN_DIFF_STM, ");
            CroissanceAgr.Append(" fac.SALES_CHG_NTM, ");
            CroissanceAgr.Append(" fac.SALES_CHG_STM, ");
            CroissanceAgr.Append(" fac.CAPEX_CHG_NTM, ");
            CroissanceAgr.Append(" fac.CAPEX_CHG_STM, ");
            CroissanceAgr.Append(" fac.FCF_CHG_NTM, ");
            CroissanceAgr.Append(" fac.FCF_CHG_STM ");
        }

        /// <summary>
        /// Fill the columns for the quality datagrid tab
        /// </summary>
        public void FillQualityTableColumns()
        {
            _qualityDataSource = new DataTable();

            _qualityDataSource.Columns.Add("TICKER");
            _qualityDataSource.Columns.Add("COMPANY_NAME");
            _qualityDataSource.Columns.Add("SECTOR");
            _qualityDataSource.Columns.Add("SuperSecteurId");
            _qualityDataSource.Columns.Add("INDUSTRY");
            _qualityDataSource.Columns.Add("SecteurId");
            _qualityDataSource.Columns.Add("Isin");
            _qualityDataSource.Columns.Add("SUIVI");
            _qualityDataSource.Columns.Add("COUNTRY");
            _qualityDataSource.Columns.Add("CURRENCY");

            _qualityDataSource.Columns.Add("EBIT_MARGIN_LTM");
            _qualityDataSource.Columns.Add("EBIT_MARGIN_NTM");
            _qualityDataSource.Columns.Add("EBIT_MARGIN_STM");
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") == 0)
                {
                    _qualityDataSource.Columns.Add("PBT_RWA_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_RWA_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_RWA_STM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_RWA_DIFF_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_RWA_DIFF_STM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_SALES_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_SALES_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_SALES_STM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_SALES_DIFF_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("PBT_SALES_DIFF_STM"); // FINANCIAL
                }
            }
            _qualityDataSource.Columns.Add("ROE_LTM");
            _qualityDataSource.Columns.Add("ROE_NTM");
            _qualityDataSource.Columns.Add("ROE_STM");
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") == 0)
                {
                    _qualityDataSource.Columns.Add("ROTE_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("ROTE_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("ROTE_STM"); // FINANCIAL
                }
                else
                {
                    _qualityDataSource.Columns.Add("FCF_YLD_LTM");
                    _qualityDataSource.Columns.Add("FCF_YLD_NTM");
                    _qualityDataSource.Columns.Add("FCF_YLD_STM");
                    _qualityDataSource.Columns.Add("FCF_YLD_DIFF_NTM");
                    _qualityDataSource.Columns.Add("FCF_YLD_DIFF_STM");
                }
            }
            else
            {
                _qualityDataSource.Columns.Add("FCF_YLD_LTM");
                _qualityDataSource.Columns.Add("FCF_YLD_NTM");
                _qualityDataSource.Columns.Add("FCF_YLD_STM");
                _qualityDataSource.Columns.Add("FCF_YLD_DIFF_NTM");
                _qualityDataSource.Columns.Add("FCF_YLD_DIFF_STM");
            }
            _qualityDataSource.Columns.Add("PAYOUT_LTM");
            _qualityDataSource.Columns.Add("PAYOUT_NTM");
            _qualityDataSource.Columns.Add("PAYOUT_STM");
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") == 0)
                {
                    _qualityDataSource.Columns.Add("COST_INCOME_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("COST_INCOME_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("COST_INCOME_STM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("TIER_1_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("TIER_1_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("TIER_1_STM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("RORWA_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("RORWA_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("RORWA_STM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("COMBINED_RATIO_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("COMBINED_RATIO_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("COMBINED_RATIO_STM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("LOSS_RATIO_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("LOSS_RATIO_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("LOSS_RATIO_STM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("EXPENSE_RATIO_LTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("EXPENSE_RATIO_NTM"); // FINANCIAL
                    _qualityDataSource.Columns.Add("EXPENSE_RATIO_STM"); // FINANCIAL
                }
            }
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") != 0)
                {
                    _qualityDataSource.Columns.Add("NET_DEBT_EBITDA_LTM");
                    _qualityDataSource.Columns.Add("NET_DEBT_EBITDA_NTM");
                    _qualityDataSource.Columns.Add("NET_DEBT_EBITDA_STM");
                    _qualityDataSource.Columns.Add("GEARING_LTM");
                    _qualityDataSource.Columns.Add("GEARING_NTM");
                    _qualityDataSource.Columns.Add("GEARING_STM");
                    _qualityDataSource.Columns.Add("CAPEX_SALES_LTM");
                    _qualityDataSource.Columns.Add("CAPEX_SALES_NTM");
                    _qualityDataSource.Columns.Add("CAPEX_SALES_STM");
                }
            }
            else
            {
                _qualityDataSource.Columns.Add("NET_DEBT_EBITDA_LTM");
                _qualityDataSource.Columns.Add("NET_DEBT_EBITDA_NTM");
                _qualityDataSource.Columns.Add("NET_DEBT_EBITDA_STM");
                _qualityDataSource.Columns.Add("GEARING_LTM");
                _qualityDataSource.Columns.Add("GEARING_NTM");
                _qualityDataSource.Columns.Add("GEARING_STM");
                _qualityDataSource.Columns.Add("CAPEX_SALES_LTM");
                _qualityDataSource.Columns.Add("CAPEX_SALES_NTM");
                _qualityDataSource.Columns.Add("CAPEX_SALES_STM");
            }

            _qualityDataSource.DefaultView.Sort = "SECTOR, INDUSTRY";
            OnPropertyChanged("QualityDataSource");

            ////////////////
            //    Agr
            ///////////////
            QualityAgr.Clear();

            QualityAgr.Append(" fac.COUNTRY,");
            QualityAgr.Append(" fac.CURRENCY,");
            QualityAgr.Append(" fac.EBIT_MARGIN_LTM,");
            QualityAgr.Append(" fac.EBIT_MARGIN_NTM,");
            QualityAgr.Append(" fac.EBIT_MARGIN_STM,");
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") == 0)
                {
                    QualityAgr.Append(" fac.PBT_RWA_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_RWA_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_RWA_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_RWA_DIFF_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_RWA_DIFF_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_SALES_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_SALES_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_SALES_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_SALES_DIFF_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.PBT_SALES_DIFF_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.ROTE_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.ROTE_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.ROTE_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.COST_INCOME_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.COST_INCOME_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.COST_INCOME_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.TIER_1_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.TIER_1_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.TIER_1_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.RORWA_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.RORWA_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.RORWA_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.COMBINED_RATIO_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.COMBINED_RATIO_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.COMBINED_RATIO_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.LOSS_RATIO_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.LOSS_RATIO_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.LOSS_RATIO_STM,"); // FINANCIAL
                    QualityAgr.Append(" fac.EXPENSE_RATIO_LTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.EXPENSE_RATIO_NTM,"); // FINANCIAL
                    QualityAgr.Append(" fac.EXPENSE_RATIO_STM,"); // FINANCIAL
                }
            }
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") != 0)
                {
                    QualityAgr.Append(" fac.FCF_YLD_LTM,");
                    QualityAgr.Append(" fac.FCF_YLD_NTM,");
                    QualityAgr.Append(" fac.FCF_YLD_STM,");
                    QualityAgr.Append(" fac.FCF_YLD_DIFF_NTM,");
                    QualityAgr.Append(" fac.FCF_YLD_DIFF_STM,");
                    QualityAgr.Append(" fac.NET_DEBT_EBITDA_LTM,");
                    QualityAgr.Append(" fac.NET_DEBT_EBITDA_NTM,");
                    QualityAgr.Append(" fac.NET_DEBT_EBITDA_STM,");
                    QualityAgr.Append(" fac.GEARING_LTM,");
                    QualityAgr.Append(" fac.GEARING_NTM,");
                    QualityAgr.Append(" fac.GEARING_STM,");
                    QualityAgr.Append(" fac.CAPEX_SALES_LTM,");
                    QualityAgr.Append(" fac.CAPEX_SALES_NTM,");
                    QualityAgr.Append(" fac.CAPEX_SALES_STM,");
                }
            }
            else
            {
                QualityAgr.Append(" fac.FCF_YLD_LTM,");
                QualityAgr.Append(" fac.FCF_YLD_NTM,");
                QualityAgr.Append(" fac.FCF_YLD_STM,");
                QualityAgr.Append(" fac.FCF_YLD_DIFF_NTM,");
                QualityAgr.Append(" fac.FCF_YLD_DIFF_STM,");
                QualityAgr.Append(" fac.NET_DEBT_EBITDA_LTM,");
                QualityAgr.Append(" fac.NET_DEBT_EBITDA_NTM,");
                QualityAgr.Append(" fac.NET_DEBT_EBITDA_STM,");
                QualityAgr.Append(" fac.GEARING_LTM,");
                QualityAgr.Append(" fac.GEARING_NTM,");
                QualityAgr.Append(" fac.GEARING_STM,");
                QualityAgr.Append(" fac.CAPEX_SALES_LTM,");
                QualityAgr.Append(" fac.CAPEX_SALES_NTM,");
                QualityAgr.Append(" fac.CAPEX_SALES_STM,");
            }
            QualityAgr.Append(" fac.ROE_LTM,");
            QualityAgr.Append(" fac.ROE_NTM,");
            QualityAgr.Append(" fac.ROE_STM,");
            QualityAgr.Append(" fac.PAYOUT_LTM,");
            QualityAgr.Append(" fac.PAYOUT_NTM,");
            QualityAgr.Append(" fac.PAYOUT_STM ");
        }

        /// <summary>
        /// Fill the columns for the valorisation datagrid tab
        /// </summary>
        public void FillValorisationTableColumns()
        {
            _valorisationDataSource = new DataTable();

            _valorisationDataSource.Columns.Add("TICKER");
            _valorisationDataSource.Columns.Add("COMPANY_NAME");
            _valorisationDataSource.Columns.Add("SECTOR");
            _valorisationDataSource.Columns.Add("SuperSecteurId");
            _valorisationDataSource.Columns.Add("INDUSTRY");
            _valorisationDataSource.Columns.Add("SecteurId");
            _valorisationDataSource.Columns.Add("Isin");
            _valorisationDataSource.Columns.Add("SUIVI");
            _valorisationDataSource.Columns.Add("COUNTRY");
            _valorisationDataSource.Columns.Add("CURRENCY");

            _valorisationDataSource.Columns.Add("DIV_YLD_NTM");
            _valorisationDataSource.Columns.Add("PE_LTM");
            _valorisationDataSource.Columns.Add("PE_NTM");
            _valorisationDataSource.Columns.Add("PE_STM");
            _valorisationDataSource.Columns.Add("PB_LTM");
            _valorisationDataSource.Columns.Add("PB_NTM");
            _valorisationDataSource.Columns.Add("PB_STM");
            _valorisationDataSource.Columns.Add("EV_EBIT_LTM");
            _valorisationDataSource.Columns.Add("EV_EBIT_NTM");
            _valorisationDataSource.Columns.Add("EV_EBIT_STM");
            _valorisationDataSource.Columns.Add("EV_EBITA_LTM");
            _valorisationDataSource.Columns.Add("EV_EBITA_NTM");
            _valorisationDataSource.Columns.Add("EV_EBITA_STM");
            _valorisationDataSource.Columns.Add("EV_EBITDA_LTM");
            _valorisationDataSource.Columns.Add("EV_EBITDA_NTM");
            _valorisationDataSource.Columns.Add("EV_EBITDA_STM");
            _valorisationDataSource.Columns.Add("EV_SALES_LTM");
            _valorisationDataSource.Columns.Add("EV_SALES_NTM");
            _valorisationDataSource.Columns.Add("EV_SALES_STM");
            _valorisationDataSource.Columns.Add("P_TBV_LTM");
            _valorisationDataSource.Columns.Add("P_TBV_NTM");
            _valorisationDataSource.Columns.Add("P_TBV_STM");
            _valorisationDataSource.Columns.Add("P_EMB_VALUE_LTM");
            _valorisationDataSource.Columns.Add("P_EMB_VALUE_NTM");
            _valorisationDataSource.Columns.Add("P_EMB_VALUE_STM");
            _valorisationDataSource.Columns.Add("PEG_NTM");
            _valorisationDataSource.Columns.Add("EV_EBIT_TO_G_NTM");
            _valorisationDataSource.Columns.Add("EV_EBITA_TO_G_NTM");
            _valorisationDataSource.Columns.Add("EV_EBITDA_TO_G_NTM");

            _valorisationDataSource.DefaultView.Sort = "SECTOR, INDUSTRY";

            OnPropertyChanged("ValorisationDataSource");

            ////////////////
            //    Agr
            ///////////////
            ValorisationAgr.Clear();
            ValorisationAgr.Append(" fac.COUNTRY,");
            ValorisationAgr.Append(" fac.CURRENCY,");
            ValorisationAgr.Append(" fac.DIV_YLD_NTM,");
            ValorisationAgr.Append(" fac.PE_LTM,");
            ValorisationAgr.Append(" fac.PE_NTM,");
            ValorisationAgr.Append(" fac.PE_STM,");
            ValorisationAgr.Append(" fac.PB_LTM,");
            ValorisationAgr.Append(" fac.P_TBV_NTM,");
            ValorisationAgr.Append(" fac.PB_NTM,");
            ValorisationAgr.Append(" fac.PB_STM,");
            ValorisationAgr.Append(" fac.EV_EBIT_LTM,");
            ValorisationAgr.Append(" fac.EV_EBIT_NTM,");
            ValorisationAgr.Append(" fac.EV_EBIT_STM,");
            ValorisationAgr.Append(" fac.EV_EBITA_LTM,");
            ValorisationAgr.Append(" fac.EV_EBITA_NTM,");
            ValorisationAgr.Append(" fac.EV_EBITA_STM,");
            ValorisationAgr.Append(" fac.EV_EBITDA_LTM,");
            ValorisationAgr.Append(" fac.EV_EBITDA_NTM,");
            ValorisationAgr.Append(" fac.EV_EBITDA_STM,");
            ValorisationAgr.Append(" fac.EV_SALES_LTM,");
            ValorisationAgr.Append(" fac.EV_SALES_NTM,");
            ValorisationAgr.Append(" fac.EV_SALES_STM,");
            ValorisationAgr.Append(" fac.P_TBV_LTM,");
            ValorisationAgr.Append(" fac.P_TBV_STM,");
            ValorisationAgr.Append(" fac.P_EMB_VALUE_LTM,");
            ValorisationAgr.Append(" fac.P_EMB_VALUE_NTM,");
            ValorisationAgr.Append(" fac.P_EMB_VALUE_STM,");
            ValorisationAgr.Append(" fac.PEG_NTM,");
            ValorisationAgr.Append(" fac.EV_EBIT_TO_G_NTM,");
            ValorisationAgr.Append(" fac.EV_EBITA_TO_G_NTM,");
            ValorisationAgr.Append(" fac.EV_EBITDA_TO_G_NTM ");
        }

        /// <summary>
        /// Fill the columns for the momentum datagrid tab
        /// </summary>
        public void FillMomentumTableColumns()
        {
            _momentumDataSource = new DataTable();

            _momentumDataSource.Columns.Add("TICKER");
            _momentumDataSource.Columns.Add("COMPANY_NAME");
            _momentumDataSource.Columns.Add("SECTOR");
            _momentumDataSource.Columns.Add("SuperSecteurId");
            _momentumDataSource.Columns.Add("INDUSTRY");
            _momentumDataSource.Columns.Add("SecteurId");
            _momentumDataSource.Columns.Add("Isin");
            _momentumDataSource.Columns.Add("SUIVI");
            _momentumDataSource.Columns.Add("CURRENCY");

            _momentumDataSource.Columns.Add("PERF_MTD");
            _momentumDataSource.Columns.Add("PERF_MTD_EUR");
            _momentumDataSource.Columns.Add("PERF_YTD");
            _momentumDataSource.Columns.Add("PERF_YTD_EUR");
            _momentumDataSource.Columns.Add("PERF_1M");
            _momentumDataSource.Columns.Add("PERF_1M_EUR");
            _momentumDataSource.Columns.Add("PERF_1YR");
            _momentumDataSource.Columns.Add("PERF_1YR_EUR");
            _momentumDataSource.Columns.Add("EPS_CHG_1M");
            _momentumDataSource.Columns.Add("EPS_CHG_3M");
            _momentumDataSource.Columns.Add("EPS_CHG_1YR");
            _momentumDataSource.Columns.Add("EPS_CHG_YTD");
            _momentumDataSource.Columns.Add("BETA_1YR");
            _momentumDataSource.Columns.Add("EPS_BROKER_UP_REV");
            _momentumDataSource.Columns.Add("PRICE_BROKER_UP_REV");
            _momentumDataSource.Columns.Add("PRICE");
            _momentumDataSource.Columns.Add("TARGET");
            _momentumDataSource.Columns.Add("UPSIDE");
            _momentumDataSource.Columns.Add("RATING_POS_PCT");
            _momentumDataSource.Columns.Add("RATING_TOT");
            _momentumDataSource.Columns.Add("PRICE_52_HIGH");
            _momentumDataSource.Columns.Add("PRICE_52_LOW");
            _momentumDataSource.Columns.Add("VOL_1M");
            _momentumDataSource.Columns.Add("VOL_3M");
            _momentumDataSource.Columns.Add("VOL_1YR");
            _momentumDataSource.Columns.Add("PRICE_PCTIL_1M");
            _momentumDataSource.Columns.Add("PRICE_PCTIL_1YR");
            _momentumDataSource.Columns.Add("PRICE_PCTIL_5YR");
            _momentumDataSource.Columns.Add("COUNTRY");

            _momentumDataSource.DefaultView.Sort = "SECTOR, INDUSTRY";

            OnPropertyChanged("MomentumDataSource");

            //////////////////
            ///    Agr
            //////////////////
            MomentumAgr.Clear();
            MomentumAgr.Append(" fac.CURRENCY,");
            MomentumAgr.Append(" fac.PERF_MTD,");
            //MomentumAgr.Append(" fac.PERF_MTD_EUR,");
            MomentumAgr.Append(" fac.PERF_YTD,");
            //MomentumAgr.Append(" fac.PERF_YTD_EUR,");
            MomentumAgr.Append(" fac.PERF_1M,");
            //MomentumAgr.Append(" fac.PERF_1M_EUR,");
            MomentumAgr.Append(" fac.PERF_1YR,");
            //MomentumAgr.Append(" fac.PERF_1YR_EUR,");
            MomentumAgr.Append(" fac.EPS_CHG_1M,");
            MomentumAgr.Append(" fac.EPS_CHG_3M,");
            MomentumAgr.Append(" fac.EPS_CHG_1YR,");
            MomentumAgr.Append(" fac.EPS_CHG_YTD,");
            MomentumAgr.Append(" fac.BETA_1YR,");
            MomentumAgr.Append(" fac.EPS_BROKER_UP_REV,");
            MomentumAgr.Append(" fac.PRICE_BROKER_UP_REV,");
            MomentumAgr.Append(" fac.PRICE,");
            MomentumAgr.Append(" fac.TARGET,");
            MomentumAgr.Append(" fac.UPSIDE,");
            MomentumAgr.Append(" fac.RATING_POS_PCT,");
            MomentumAgr.Append(" fac.RATING_TOT,");
            MomentumAgr.Append(" fac.PRICE_52_HIGH,");
            MomentumAgr.Append(" fac.PRICE_52_LOW,");
            MomentumAgr.Append(" fac.VOL_1M,");
            MomentumAgr.Append(" fac.VOL_3M,");
            MomentumAgr.Append(" fac.VOL_1YR,");
            MomentumAgr.Append(" fac.PRICE_PCTIL_1M,");
            MomentumAgr.Append(" fac.PRICE_PCTIL_1YR,");
            MomentumAgr.Append(" fac.PRICE_PCTIL_5YR,");
            MomentumAgr.Append(" fac.COUNTRY ");
        }

        /// <summary>
        /// Fill the columns for the synthese datagrid tab
        /// </summary>
        public void FillSyntheseTableColumns()
        {
            _syntheseDataSource = new DataTable();

            _syntheseDataSource.Columns.Add("TICKER");
            _syntheseDataSource.Columns.Add("COMPANY_NAME");
            _syntheseDataSource.Columns.Add("SECTOR");
            _syntheseDataSource.Columns.Add("SuperSecteurId");
            _syntheseDataSource.Columns.Add("INDUSTRY");
            _syntheseDataSource.Columns.Add("SecteurId");
            _syntheseDataSource.Columns.Add("Isin");
            _syntheseDataSource.Columns.Add("SUIVI");
            _syntheseDataSource.Columns.Add("COUNTRY");
            _syntheseDataSource.Columns.Add("CURRENCY");

            if (SelectedUniverse.CompareTo("ALL") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEU");
                _syntheseDataSource.Columns.Add("MXUSLC");
            }
            else if (SelectedUniverse.CompareTo("EUROPE") == 0)
                _syntheseDataSource.Columns.Add("MXEU");
            else if (SelectedUniverse.CompareTo("USA") == 0)
                _syntheseDataSource.Columns.Add("MXUSLC");
            else if (SelectedUniverse.CompareTo("EMU") == 0)
                _syntheseDataSource.Columns.Add("MXEM");
            else if (SelectedUniverse.CompareTo("EUROPE EX EMU") == 0)
                _syntheseDataSource.Columns.Add("MXEUM");
            else if (SelectedUniverse.CompareTo("FRANCE") == 0)
                _syntheseDataSource.Columns.Add("MXFR");
            else if (SelectedUniverse.CompareTo("FEDERIS ACTIONS") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEM");
                _syntheseDataSource.Columns.Add("6100001");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
            {
                _syntheseDataSource.Columns.Add("MXFR");
                _syntheseDataSource.Columns.Add("6100002");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS ISR EURO") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEM");
                _syntheseDataSource.Columns.Add("6100004");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS NORTH AMERICA") == 0)
            {
                _syntheseDataSource.Columns.Add("MXUSLC");
                _syntheseDataSource.Columns.Add("6100024");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEU");
                _syntheseDataSource.Columns.Add("6100026");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EURO ACTIONS") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEM");
                _syntheseDataSource.Columns.Add("6100030");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS IRC ACTIONS") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEM");
                _syntheseDataSource.Columns.Add("6100033");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EX EURO") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEUM");
                _syntheseDataSource.Columns.Add("6100062");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS CROISSANCE EURO") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEM");
                _syntheseDataSource.Columns.Add("6100063");
            }
            else if (SelectedUniverse.CompareTo("AVENIR EURO") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEM");
                _syntheseDataSource.Columns.Add("AVEURO");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS VALUE EURO") == 0)
            {
                _syntheseDataSource.Columns.Add("MXEM");
                _syntheseDataSource.Columns.Add("AVEUROPE");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS ISR AMERIQUE") == 0)
            {
                _syntheseDataSource.Columns.Add("MXUSLC");
                _syntheseDataSource.Columns.Add("6100066");
            }
            _syntheseDataSource.Columns.Add("EPS_CHG_NTM");
            _syntheseDataSource.Columns.Add("DIV_YLD_NTM");
            if (SelectedSuperSector == null || SelectedSuperSector.Libelle.CompareTo("Financials") != 0)
                _syntheseDataSource.Columns.Add("FCF_YLD_NTM"); // NORMAL
            _syntheseDataSource.Columns.Add("EBIT_MARGIN_NTM");
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") == 0)
                {
                    _syntheseDataSource.Columns.Add("PBT_RWA_NTM"); // -FINANCIAL
                    _syntheseDataSource.Columns.Add("PBT_SALES_NTM"); // -FINANCIAL
                }
            }
            _syntheseDataSource.Columns.Add("ROE_NTM");
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") == 0)
                    _syntheseDataSource.Columns.Add("ROTE_NTM"); // -FINANCIAL
            }
            _syntheseDataSource.Columns.Add("PE_NTM");
            _syntheseDataSource.Columns.Add("PB_NTM");
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") == 0)
                {
                    _syntheseDataSource.Columns.Add("P_TBV_NTM"); // -FINANCIAL
                    _syntheseDataSource.Columns.Add("RORWA_NTM"); // -FINANCIAL
                    _syntheseDataSource.Columns.Add("COST_INCOME_NTM"); // -FINANCIAL
                    _syntheseDataSource.Columns.Add("TIER_1_NTM"); // -FINANCIAL
                    _syntheseDataSource.Columns.Add("P_EMB_VALUE_NTM"); // -FINANCIAL
                    _syntheseDataSource.Columns.Add("COMBINED_RATIO_NTM"); // -FINANCIAL
                }
            }
            if (SelectedSuperSector == null || SelectedSuperSector.Libelle.CompareTo("Financials") != 0)
            {
                _syntheseDataSource.Columns.Add("EV_EBIT_NTM"); // NORMAL
                _syntheseDataSource.Columns.Add("EV_EBITDA_NTM"); // NORMAL
                _syntheseDataSource.Columns.Add("EV_SALES_NTM"); // NORMAL
                _syntheseDataSource.Columns.Add("PEG_NTM"); // NORMAL
                _syntheseDataSource.Columns.Add("EV_EBIT_TO_G_NTM"); // NORMAL
                _syntheseDataSource.Columns.Add("NET_DEBT_EBITDA_NTM"); // NORMAL
            }
            _syntheseDataSource.Columns.Add("PRICE");
            _syntheseDataSource.Columns.Add("TARGET");
            _syntheseDataSource.Columns.Add("UPSIDE");
            _syntheseDataSource.Columns.Add("ESG");
            _syntheseDataSource.Columns.Add("GARPN_TOTAL_S");
            _syntheseDataSource.Columns.Add("GARPN_GROWTH_S");
            _syntheseDataSource.Columns.Add("GARPN_VALUE_S");
            _syntheseDataSource.Columns.Add("GARPN_YIELD_S");
            _syntheseDataSource.Columns.Add("GARPN_ISR_S");
            _syntheseDataSource.Columns.Add("GARPN_TOTAL_NO_ISR_S");

            _syntheseDataSource.DefaultView.Sort = "SECTOR, INDUSTRY";

            OnPropertyChanged("SyntheseDataSource");

            //////////////////
            ///    Agr
            //////////////////
            SyntheseAgr.Clear();
            SyntheseAgr.Append(" fac.COUNTRY,");
            SyntheseAgr.Append(" fac.CURRENCY,");
            if (SelectedUniverse.CompareTo("ALL") == 0)
            {
                SyntheseAgr.Append(" fac.MXEU,");
                SyntheseAgr.Append(" fac.MXUSLC,");
            }
            else if (SelectedUniverse.CompareTo("EUROPE") == 0)
                SyntheseAgr.Append(" fac.MXEU,");
            else if (SelectedUniverse.CompareTo("USA") == 0)
                SyntheseAgr.Append(" fac.MXUSLC,");
            else if (SelectedUniverse.CompareTo("EMU") == 0)
                SyntheseAgr.Append(" fac.MXEM,");
            else if (SelectedUniverse.CompareTo("EUROPE EX EMU") == 0)
                SyntheseAgr.Append(" fac.MXEUM,");
            else if (SelectedUniverse.CompareTo("FRANCE") == 0)
                SyntheseAgr.Append(" fac.MXFR,");
            else if (SelectedUniverse.CompareTo("FEDERIS ACTIONS") == 0)
            {
                SyntheseAgr.Append(" fac.MXEM,");
                SyntheseAgr.Append(" fac.[6100001],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
            {
                SyntheseAgr.Append(" fac.MXFR,");
                SyntheseAgr.Append(" fac.[6100002],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS ISR EURO") == 0)
            {
                SyntheseAgr.Append(" fac.MXEM,");
                SyntheseAgr.Append(" fac.[6100004],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS NORTH AMERICA") == 0)
            {
                SyntheseAgr.Append(" fac.MXUSLC,");
                SyntheseAgr.Append(" fac.[6100024],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
            {
                SyntheseAgr.Append(" fac.MXEU,");
                SyntheseAgr.Append(" fac.[6100026],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EURO ACTIONS") == 0)
            {
                SyntheseAgr.Append(" fac.MXEM,");
                SyntheseAgr.Append(" fac.[6100030],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS IRC ACTIONS") == 0)
            {
                SyntheseAgr.Append(" fac.MXEM,");
                SyntheseAgr.Append(" fac.[6100033],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS EX EURO") == 0)
            {
                SyntheseAgr.Append(" fac.MXEUM,");
                SyntheseAgr.Append(" fac.[6100062],");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS CROISSANCE EURO") == 0)
            {
                SyntheseAgr.Append(" fac.MXEM,");
                SyntheseAgr.Append(" fac.[6100063],");
            }
            else if (SelectedUniverse.CompareTo("AVENIR EURO") == 0)
            {
                SyntheseAgr.Append(" fac.MXEM,");
                SyntheseAgr.Append(" fac.AVEURO,");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS VALUE EURO") == 0)
            {
                SyntheseAgr.Append(" fac.MXEM,");
                SyntheseAgr.Append(" fac.AVEUROPE,");
            }
            else if (SelectedUniverse.CompareTo("FEDERIS ISR AMERIQUE") == 0)
            {
                SyntheseAgr.Append(" fac.MXUSLC,");
                SyntheseAgr.Append(" fac.[6100066],");
            }
            SyntheseAgr.Append(" fac.EPS_CHG_NTM,");
            SyntheseAgr.Append(" fac.DIV_YLD_NTM,");
            if (SelectedSuperSector != null)
            {
                if (SelectedSuperSector.Libelle.CompareTo("Financials") == 0)
                {
                    SyntheseAgr.Append(" fac.PBT_RWA_NTM,"); // -FINANCIAL
                    SyntheseAgr.Append(" fac.PBT_SALES_NTM,"); // -FINANCIAL
                    SyntheseAgr.Append(" fac.ROTE_NTM,"); // -FINANCIAL
                    SyntheseAgr.Append(" fac.P_TBV_NTM,"); // -FINANCIAL
                    SyntheseAgr.Append(" fac.RORWA_NTM,"); // -FINANCIAL
                    SyntheseAgr.Append(" fac.COST_INCOME_NTM,"); // -FINANCIAL
                    SyntheseAgr.Append(" fac.TIER_1_NTM,"); // -FINANCIAL
                    SyntheseAgr.Append(" fac.P_EMB_VALUE_NTM,"); // -FINANCIAL
                    SyntheseAgr.Append(" fac.COMBINED_RATIO_NTM,"); // -FINANCIAL
                }
            }
            if (SelectedSuperSector == null || SelectedSuperSector.Libelle.CompareTo("Financials") != 0)
            {
                SyntheseAgr.Append(" fac.FCF_YLD_NTM,"); // NORMAL
                SyntheseAgr.Append(" fac.EV_EBIT_NTM,"); // NORMAL
                SyntheseAgr.Append(" fac.EV_EBITDA_NTM,"); // NORMAL
                SyntheseAgr.Append(" fac.EV_SALES_NTM,"); // NORMAL
                SyntheseAgr.Append(" fac.PEG_NTM,"); // NORMAL
                SyntheseAgr.Append(" fac.EV_EBIT_TO_G_NTM,"); // NORMAL
                SyntheseAgr.Append(" fac.NET_DEBT_EBITDA_NTM,"); // NORMAL
                SyntheseAgr.Append(" fac.PRICE,");
            }
            SyntheseAgr.Append(" fac.EBIT_MARGIN_NTM,");
            SyntheseAgr.Append(" fac.ROE_NTM,");
            SyntheseAgr.Append(" fac.PE_NTM,");
            SyntheseAgr.Append(" fac.PB_NTM,");
            SyntheseAgr.Append(" fac.TARGET,");
            SyntheseAgr.Append(" fac.UPSIDE,");
            SyntheseAgr.Append(" fac.ESG,");
            SyntheseAgr.Append(" fac.GARPN_TOTAL_S,");
            SyntheseAgr.Append(" fac.GARPN_GROWTH_S,");
            SyntheseAgr.Append(" fac.GARPN_VALUE_S,");
            SyntheseAgr.Append(" fac.GARPN_YIELD_S,");
            SyntheseAgr.Append(" fac.GARPN_ISR_S,");
            SyntheseAgr.Append(" fac.GARPN_TOTAL_NO_ISR_S ");

        }

        #endregion

        #region Formats

        /// <summary>
        /// Formats columns of the datagrids based on the result of the table ACT_AGR_FORMAT from the database
        /// </summary>
        public void setFormat()
        {
            string sql = "SELECT Champs_FACTSET AS nom, format, precision, general, growth, value, quality, momentum, synthese  FROM ACT_AGR_FORMAT";
            string format = null;
            double d = 0;
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            String prev = "";
            List<Dictionary<string, object>> listDico = co.sqlToListDico(sql);
            foreach (Dictionary<string, object> dico in listDico)
            {
                String field = dico["nom"].ToString().TrimEnd();

                if ((int)dico["format"] != 4 && field.CompareTo("ESG") != 0
                    && field.CompareTo("UPSIDE") != 0)
                {
                    #region Select Format

                    switch (((int)dico["format"]))
                    {
                        case 0:
                            switch ((int)(dico["precision"]))
                            {
                                case 0:
                                    format = "# ##0;(##0)";
                                    break;
                                case 1:
                                    format = "# ##0.0;(##0.0)";
                                    break;
                                case 2:
                                    format = "# ##0.00;(##0.00)";
                                    break;
                                case 3:
                                    format = "# ##0.000;(##0.000)";
                                    break;
                            }
                            break;
                        case 1:
                            switch ((int)(dico["precision"]))
                            {
                                case 0:
                                    format = "# ##0x;(##0x)";
                                    break;
                                case 1:
                                    format = "# ##0.0x;(##0.0x)";
                                    break;
                                case 2:
                                    format = "# ##0.00x;(##0.00x)";
                                    break;
                            }
                            break;
                        case 2:
                            switch ((int)(dico["precision"]))
                            {
                                case 0:
                                    format = "# ##0\\%;(##0\\%)";
                                    break;
                                case 1:
                                    format = "# ##0.0\\%;(##0.0\\%)";
                                    break;
                                case 2:
                                    format = "# ##0.00\\%;(##0.00\\%)";
                                    break;
                            }
                            break;
                    }

                    #endregion

                    #region Format

                    if (((String)dico["general"]).CompareTo("X") == 0)
                    {
                        foreach (DataRow row in GeneralDataSource.Rows)
                        {
                            if (GeneralDataSource.Columns.Contains(field))
                            {
                                if (!object.ReferenceEquals(row[field], DBNull.Value))
                                {
                                    d = Double.Parse(row[field].ToString());
                                    row[field] = d.ToString(format, nfi);
                                }
                            }
                        }
                    }
                    if (((String)dico["growth"]).CompareTo("X") == 0)
                    {
                        foreach (DataRow row in CroissanceDataSource.Rows)
                        {
                            if (CroissanceDataSource.Columns.Contains(field))
                            {
                                if (!object.ReferenceEquals(row[field], DBNull.Value))
                                {
                                    //d = (double)row[field];
                                    d = Double.Parse(row[field].ToString());
                                    row[field] = d.ToString(format, nfi);
                                }
                            }
                        }
                    }
                    if (((String)dico["quality"]).CompareTo("X") == 0)
                    {
                        foreach (DataRow row in QualityDataSource.Rows)
                        {
                            if (QualityDataSource.Columns.Contains(field))
                            {
                                if (!object.ReferenceEquals(row[field], DBNull.Value))
                                {
                                    //d = (double)row[field];
                                    d = Double.Parse(row[field].ToString());
                                    row[field] = d.ToString(format, nfi);
                                }
                            }
                        }
                    }
                    if (((String)dico["value"]).CompareTo("X") == 0)
                    {
                        foreach (DataRow row in ValorisationDataSource.Rows)
                        {
                            if (ValorisationDataSource.Columns.Contains(field))
                            {
                                if (!object.ReferenceEquals(row[field], DBNull.Value))
                                {
                                    //d = (double)row[field];
                                    d = Double.Parse(row[field].ToString());
                                    row[field] = d.ToString(format, nfi);
                                }
                            }
                        }
                    }
                    if (((String)dico["momentum"]).CompareTo("X") == 0)
                    {
                        foreach (DataRow row in MomentumDataSource.Rows)
                        {
                            if (MomentumDataSource.Columns.Contains(field))
                            {
                                if (!object.ReferenceEquals(row[field], DBNull.Value))
                                {
                                    //d = (double)row[field];
                                    d = Double.Parse(row[field].ToString());
                                    row[field] = d.ToString(format, nfi);
                                }
                            }
                        }
                    }
                    if (((String)dico["synthese"]).CompareTo("X") == 0)
                    {
                        foreach (DataRow row in SyntheseDataSource.Rows)
                        {
                            if (SyntheseDataSource.Columns.Contains(field))
                            {
                                try
                                {
                                    if (!object.ReferenceEquals(row[field], DBNull.Value))
                                    {
                                        //d = (double)row[field];
                                        if (row[field].GetType().ToString() == "System.Double")
                                            continue;
                                        d = Double.Parse(row[field].ToString());
                                        row[field] = d.ToString(format, nfi);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }

                    #endregion
                }
                else if (field.CompareTo("ESG") == 0)
                {
                    #region ESG

                    foreach (DataRow row in GeneralDataSource.Rows)
                    {
                        if (GeneralDataSource.Columns.Contains(field))
                        {
                            if (!object.ReferenceEquals(row[(String)dico["nom"]], DBNull.Value))
                            {
                                string tmp = (String)row[(String)dico["nom"]];
                                if (tmp != "EXCLU")
                                {
                                    row[(String)dico["nom"]] = Helpers.DisplayRoundedNumber(tmp, 2);
                                }
                                else
                                {
                                    row[(String)dico["nom"]] = "EXCLU";
                                }
                            }
                        }
                    }

                    foreach (DataRow row in SyntheseDataSource.Rows)
                    {


                        if (SyntheseDataSource.Columns.Contains(field))
                        {
                            if (!object.ReferenceEquals(row[(String)dico["nom"]], DBNull.Value))
                            {
                                string tmp = (String)row[(String)dico["nom"]];
                                if (tmp != "EXCLU")
                                {
                                    row[(String)dico["nom"]] = Helpers.DisplayRoundedNumber(tmp, 2);
                                }
                                else
                                {
                                    row[(String)dico["nom"]] = "EXCLU";
                                }
                            }
                        }
                    }

                    #endregion
                }
                else if (field.CompareTo("UPSIDE") == 0)
                {
                    #region UPSIDE

                    foreach (DataRow row in MomentumDataSource.Rows)
                    {
                        if (MomentumDataSource.Columns.Contains(field))
                        {
                            if (!object.ReferenceEquals(row[(String)dico["nom"]], DBNull.Value))
                            {
                                d = Double.Parse(row[field].ToString());
                                if (d >= 0)
                                    row[field] = (d * 100).ToString(format, nfi) + "%";
                                else
                                {
                                    d *= -1;
                                    row[field] = "(" + (d * 100).ToString(format, nfi) + ")%";
                                }
                            }
                        }
                    }

                    foreach (DataRow row in SyntheseDataSource.Rows)
                    {
                        if (SyntheseDataSource.Columns.Contains(field))
                        {
                            if (!object.ReferenceEquals(row[(String)dico["nom"]], DBNull.Value))
                            {
                                d = Double.Parse(row[field].ToString());
                                if (d >= 0)
                                    row[field] = (d * 100).ToString(format, nfi) + "%";
                                else
                                {
                                    d *= -1;
                                    row[field] = "(" + (d * 100).ToString(format, nfi) + ")%";
                                }
                            }
                        }
                    }
                    #endregion
                }
                prev = dico["nom"].ToString();
            }
        }

        #endregion

        #region Background worker

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
            CompanyNameCleaner cleaner = new CompanyNameCleaner();
            this.BusyContent = "";

            GeneralDataSource = cleaner.CleanCompanyName(_model.FillTableWithData(_generalDataSource, SelectedDate, SelectedUniverse,
                SelectedSuperSector, SelectedSector, "Gen"));
            InvokeOnUIThread(() =>
            {
                this.BusyContent = string.Concat("Loaded GeneralDataSource\n");
            });

            CroissanceDataSource = cleaner.CleanCompanyName(_model.FillTableWithData(_croissanceDataSource, SelectedDate, SelectedUniverse,
                SelectedSuperSector, SelectedSector, "Cro"));
            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Loaded CroissanceDataSource\n");
            });

            QualityDataSource = cleaner.CleanCompanyName(_model.FillTableWithData(_qualityDataSource, SelectedDate, SelectedUniverse,
                 SelectedSuperSector, SelectedSector, "Qua"));
            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Loaded QualityDataSource\n");
            });

            ValorisationDataSource = cleaner.CleanCompanyName(_model.FillTableWithData(_valorisationDataSource, SelectedDate, SelectedUniverse,
                SelectedSuperSector, SelectedSector, "Val"));
            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Loaded ValorisationDataSource\n");
            });

            MomentumDataSource = cleaner.CleanCompanyName(_model.FillTableWithData(_momentumDataSource, SelectedDate, SelectedUniverse,
                SelectedSuperSector, SelectedSector, "Mom"));
            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Loaded MomentumDataSource\n");
            });

            SyntheseDataSource = cleaner.CleanCompanyName(_model.FillTableWithData(_syntheseDataSource, SelectedDate, SelectedUniverse,
                SelectedSuperSector, SelectedSector, "Syn"));
            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Loaded SyntheseDataSource\n");
            });

            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Formatting\n");
            });

            setFormat();

            InvokeOnUIThread(() =>
            {
                this.BusyContent += string.Concat("Recreating all tables for filters\n");
            });

            FillColsValid();

            GeneralDataSource = RecreateTables(GeneralDataSource);
            CroissanceDataSource = RecreateTables(CroissanceDataSource);
            QualityDataSource = RecreateTables(QualityDataSource);
            ValorisationDataSource = RecreateTables(ValorisationDataSource);
            MomentumDataSource = RecreateTables(MomentumDataSource);
            SyntheseDataSource = RecreateTables(SyntheseDataSource);
        }

        #endregion

        List<String> colsValid = new List<string>();

        public void FillColsValid()
        {
            colsValid.Add("TICKER");
            colsValid.Add("COMPANY_NAME");
            colsValid.Add("SECTOR");
            colsValid.Add("INDUSTRY");
            colsValid.Add("SUIVI");
            colsValid.Add("COUNTRY");
            colsValid.Add("CURRENCY");
            colsValid.Add("ESG");
            colsValid.Add("SuperSecteurId");
            colsValid.Add("SecteurId");
            colsValid.Add("Isin");
            colsValid.Add("LIQUIDITY_TEST");
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

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

        #endregion
    }
}