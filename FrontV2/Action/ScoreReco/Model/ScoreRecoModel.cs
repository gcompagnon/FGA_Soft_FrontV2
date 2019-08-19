using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Telerik.Windows.Data;

namespace FrontV2.Action.ScoreReco.Model
{
    class ScoreRecoModel
    {
        readonly Connection connection = new Connection();

        public ScoreRecoModel()
        {
        }

        /// <summary>
        /// Return all date from database
        /// </summary>
        /// <returns></returns>
        public RadObservableCollection<String> GetDates()
        {
            return connection.GetDates();
        }

        //Gics Sectors
        public List<object> GetSectorsICB()
        {
            return connection.GetSectorsICB();
        }

        public List<object> GetSectorsFGA()
        {
            return connection.GetSectorsFGA();
        }

        public List<object> GetEnterprises()
        {
            return connection.GetEnterprises();
        }

        public List<object> GetFGAIndustries(String sector, String FGA_classname)
        {
            return connection.GetFGAIndustries(sector, FGA_classname);
        }
        
        public List<String> GetCompanies(String date, String superSector, String sector)
        {
            return connection.GetCompanies(date, superSector, sector);
        }

        /// <summary>
        /// Fill a DataTable used as the display of the grid
        /// </summary>
        /// <param name="date"></param>
        /// <param name="univers"></param>
        /// <param name="superSector"></param>
        /// <param name="sector"></param>
        /// <returns></returns>
        public DataTable FillValeursAnalyse(String date, String univers,
            String superSector, String sector)
        {
            if (univers == null || univers == "" || superSector == null || superSector == "")
                return new DataTable();

            String FGA_Classname = "";
            if (univers == "Europe")
                FGA_Classname = "FGA_EU";
            else if (univers == "USA")
                FGA_Classname = "FGA_US";

            if (sector != null && sector != "")
            {
                int id = int.Parse(connection.SelectDistinctWheres("ref_security.SECTOR", "code",
                    new List<String> { "class_name", "level", "label" },
                    new List<object> { FGA_Classname, 0, sector })[0].ToString());

                return connection.ProcedureStockeeForDataGrid("ACT_DataGridBlendValeur",
                    new List<String> { "@date", "@id_fga", "@FGA" },
                    new List<object> { date, id, univers });
            }
            else
            {
                DataTable tmp = new DataTable();
                foreach (String s in GetFGAIndustries(superSector, univers))
                {
                    if (s == "")
                        continue;

                    int id = int.Parse(connection.SelectDistinctWheres("ref_security.SECTOR", "code",
                        new List<String> { "class_name", "level", "label" },
                        new List<object> { FGA_Classname, 0, s })[0].ToString());

                    DataTable t = connection.ProcedureStockeeForDataGrid("ACT_DataGridBlendValeur",
                         new List<String> { "@date", "@id_fga", "@FGA" },
                         new List<object> { date, id, univers });
                    tmp.Merge(t);
                }
                return tmp;
            }
        }

        /// <summary>
        /// Call ACT_Radar_Growth
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ticker"></param>
        /// <param name="sector"></param>
        /// <returns></returns>
        public Dictionary<object, object> GetGrowthChartValues(String date, String ticker, String sector)
        {
            String id_fga = connection.GetIdFGA(sector);

            return connection.ProcedureStockeeDico("ACT_Radar_Growth",
                new List<String> { "@date", "@Pres", "@TCIKER", "@FGA", "@MM" },
                new List<object> { date, "Growth", ticker, id_fga, "" });
        }

        /// <summary>
        /// Call ACT_Radar_Value fill the second datagrid to compare the values
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="ticker1"></param>
        /// <param name="ticker2"></param>
        /// <returns></returns>
        public DataTable GetValuesChart(String date, String ticker1, String ticker2)
        {
            int sector1 = -1;
            int sector2 = -1;

            if (ticker1 != null)
                sector1 = GetSectorFromTicker(ticker1);
            else
                ticker1 = ticker2;

            if (ticker2 != null)
                sector2 = GetSectorFromTicker(ticker2);
            else
                ticker2 = ticker1;

            return connection.ProcedureStockeeForDataGrid("ACT_Radar_Value",
                new List<String> { "@date", "@date2", "@TICKER1", "@TICKER2", "@FGA" },
                new List<Object> { date, date, ticker1, ticker2, sector1 });
        }
        public DataTable GetValuesChart(String date, String date2, String ticker1, String ticker2)
        {
            int sector1 = -1;
            int sector2 = -1;

            if (ticker1 != null)
                sector1 = GetSectorFromTicker(ticker1);
            else
                ticker1 = ticker2;

            if (ticker2 != null)
                sector2 = GetSectorFromTicker(ticker2);
            else
                ticker2 = ticker1;

            return connection.ProcedureStockeeForDataGrid("ACT_Radar_Value",
                new List<String> { "@date", "@date2", "@TICKER1", "@TICKER2", "@FGA" },
                new List<Object> { date, date2, ticker1, ticker2, sector1 });
        }

        /// <summary>
        /// Get values for the bar series chart (1)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public RadObservableCollection<KeyValuePair<String, double>> GetHistoData(String date, String ticker)
        {
            int id_fga = GetSectorFromTicker(ticker);

            Dictionary<Object, Object> values = connection.ProcedureStockeeDico("ACT_Radar_Growth",
                new List<String> { "@date", "@Pres", "@TICKER", "@FGA", "@MM" },
                new List<object> { date, "Total", ticker, id_fga.ToString(), "" });

            RadObservableCollection<KeyValuePair<String, double>> res = new RadObservableCollection<KeyValuePair<string, double>>();
            foreach (var o in values)
            {
                if (o.Value == null || o.Value.ToString() == "")
                    res.Add(new KeyValuePair<String, double>(o.Key.ToString(), 0));
                else
                    res.Add(new KeyValuePair<String, double>(o.Key.ToString(), (double)o.Value));
            }

            return res;
        }

        /// <summary>
        /// Get values for the growth chart
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public RadObservableCollection<KeyValuePair<String, double>> GetRadarGrowthData(String date, String ticker)
        {
            int id_fga = GetSectorFromTicker(ticker);

            Dictionary<object, object> values =
                connection.ProcedureStockeeDico("ACT_Radar_Growth", new List<String> { "@date", "@Pres", "@TICKER", "@FGA", "@MM" },
                    new List<Object> { date, "Growth", ticker, id_fga.ToString(), "" });

            RadObservableCollection<KeyValuePair<String, double>> res = new RadObservableCollection<KeyValuePair<string, double>>();
            foreach (var o in values)
            {
                if (o.Value == null || o.Value.ToString() == "")
                    res.Add(new KeyValuePair<String, double>(o.Key.ToString(), 0));
                else
                    res.Add(new KeyValuePair<String, double>(o.Key.ToString(), (double)o.Value));
            }

            return res;
        }

        /// <summary>
        /// Get values for the values chart
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public RadObservableCollection<KeyValuePair<String, double>> GetRadarValuesData(String date, String ticker)
        {
            int id_fga = GetSectorFromTicker(ticker);

            Dictionary<object, object> values =
                connection.ProcedureStockeeDico("ACT_Radar_Growth", new List<String> { "@date", "@Pres", "@TICKER", "@FGA", "@MM" },
                    new List<Object> { date, "Value", ticker, id_fga.ToString(), "" });

            RadObservableCollection<KeyValuePair<String, double>> res = new RadObservableCollection<KeyValuePair<string, double>>();
            foreach (var o in values)
            {
                if (o.Value == null || o.Value.ToString() == "")
                    res.Add(new KeyValuePair<String, double>(o.Key.ToString(), 0));
                else
                    res.Add(new KeyValuePair<String, double>(o.Key.ToString(), (double)o.Value));
            }

            return res;
        }

        /// <summary>
        /// Get values for the profit chart
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public RadObservableCollection<KeyValuePair<String, double>> GetRadarProfitData(String date, String ticker)
        {

            int id_fga = GetSectorFromTicker(ticker);

            Dictionary<object, object> values =
                connection.ProcedureStockeeDico("ACT_Radar_Growth", new List<String> { "@date", "@Pres", "@TICKER", "@FGA", "@MM" },
                    new List<Object> { date, "Qualite", ticker, id_fga.ToString(), "" });

            RadObservableCollection<KeyValuePair<String, double>> res = new RadObservableCollection<KeyValuePair<string, double>>();
            foreach (var o in values)
            {
                if (o.Value == null || o.Value.ToString() == "")
                    res.Add(new KeyValuePair<String, double>(o.Key.ToString(), 0));
                else
                    res.Add(new KeyValuePair<String, double>(o.Key.ToString(), (double)o.Value));
            }

            return res;
        }

        /// <summary>
        /// You shall not need an explanation
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public int GetSectorFromTicker(String ticker)
        {
            return int.Parse(connection.SelectDistinctWheres("DATA_FACTSET", "SECTOR",
               new List<String> { "TICKER" },
               new List<object> { ticker })[0].ToString());
        }

        public RadObservableCollection<KeyValuePair<string, int>> GetQuintile(String ticker)
        {
            RadObservableCollection<KeyValuePair<string, int>> res = new RadObservableCollection<KeyValuePair<string, int>>();

            String request = "SELECT CONVERT(DATE, date, 103) as Date, GARPN_QUINTILE_S as Quint FROM DATA_FACTSET WHERE TICKER='" + ticker + "' AND GARPN_QUINTILE_S IS NOT NULL ORDER BY DATE";
            List<KeyValuePair<string, int>> values = connection.sqlToListKeyValuePair(request);
            
            // VOIR DANS LE CAHIER L'ALGO.
            values = GetfinDeMois(values);
            values = GetLastDates(values, 12);

            foreach (var v in values)
                res.Add(new KeyValuePair<string, int>(v.Key.Substring(0, 10), v.Value));

            return res;
        }

        public List<KeyValuePair<String, int>> GetfinDeMois(List<KeyValuePair<String, int>> dates)
        {
            List<KeyValuePair<String, int>> res = new List<KeyValuePair<String, int>>();
            int prevMonth = -1;

            String oldKey = "";
            int oldValue = 0;

            foreach (var date in dates)
            {
                int curMonth = GetMonth(date.Key);

                if (curMonth == prevMonth)
                {
                    prevMonth = curMonth;
                    oldKey = date.Key;
                    oldValue = date.Value;
                    continue;
                }
                else
                {
                    if (prevMonth == -1)
                    {
                        prevMonth = curMonth;
                        oldKey = date.Key;
                        oldValue = date.Value;
                        continue;
                    }
                    res.Add(new KeyValuePair<String, int>(oldKey, oldValue));
                }
                prevMonth = curMonth;
                oldKey = date.Key;
                oldValue = date.Value;
            }

            res.Add(new KeyValuePair<String, int>(oldKey, oldValue));

            return res;
        }

        public List<KeyValuePair<String, int>> GetLastDates(List<KeyValuePair<String, int>> dates, int interval)
        {
            List<KeyValuePair<String, int>> res = new List<KeyValuePair<string, int>>();
            if (interval > dates.Count)
                interval = dates.Count;

            for (int i = interval; i > 0; i--)
            {
                String s = dates[dates.Count - i].Key;
                int n = dates[dates.Count - i].Value;
                res.Add(new KeyValuePair<String, int>(s, n));
            }
            return res;
        }

        public static int GetMonth(String date)
        {
            return int.Parse(date.Substring(3, 2));
        }
        
        /*********************************
                    ScoreChanges
         *********************************/

        public DataTable GetUniversIndustries(String univers)
        {
            String requete = "";

            requete += GetTmpAllSectorsInUnivers(univers);
            requete += GetTmpAllIndustriesInUnivers(univers);

            requete += GetAllSectorsInUnivers(univers);
            requete += " ";
            requete += " UNION";
            requete += " ";
            requete += GetAllIndustriesInUnivers(univers);

            requete += " ";
            requete += " DROP TABLE #sectors";
            requete += " DROP TABLE #industries";

            return connection.LoadDataGridByString(requete);
        }

        public String GetTmpAllSectorsInUnivers(String univers)
        {
            String secteurs = "";
            //SECTORS
            #region Sectors
            secteurs += " SELECT * ";
            secteurs += " into #sectors ";
            secteurs += " FROM ref_security.SECTOR as ss";
            secteurs += " inner join DATA_FACTSET as fac on fac.GICS_SECTOR = ss.code";
            secteurs += " where ";
            secteurs += " level = 0";
            secteurs += " and ss.class_name =  'GICS' and fac.GICS_SUBINDUSTRY is null ";
            secteurs += " and fac.DATE = (Select MAX(date) from DATA_FACTSET)";
            if (univers == "Europe")
                secteurs += " and fac.MXEU is not null and fac.MXUSLC is null ";
            else
                secteurs += " and fac.MXEU is null and fac.MXUSLC is not null";
            #endregion

            return secteurs;
        }

        public String GetTmpAllIndustriesInUnivers(String univers)
        {
            String industries = "";
            // INDUSTRIES
            #region Industries
            industries += " SELECT ";
            industries += " ss.label As 'SECTOR', ";
            industries += " ss.code As 'SuperSecteurId', ";
            industries += " s.label As 'INDUSTRY', ";
            industries += " s.code As 'SecteurId',";
            industries += " fac.MXEU,";
            industries += " fac.MXEUM,";
            industries += " fac.MXEM,";
            industries += " fac.MXUSLC";
            industries += " into #industries";
            industries += " FROM ref_security.SECTOR s ";
            industries += " INNER JOIN ref_security.SECTOR_TRANSCO st on";
            industries += " st.id_sector1 = s.id ";
            industries += " INNER JOIN ref_security.SECTOR fils on ";
            industries += " fils.id = st.id_sector2 ";
            industries += " LEFT OUTER JOIN ref_security.SECTOR ss ON ";
            industries += " fils.id_parent = ss.id ";
            industries += " INNER JOIN DATA_FACTSET fac on ";
            industries += " fac.FGA_SECTOR = s.code ";
            industries += " WHERE fac.GICS_SECTOR is null ";
            industries += " AND ss.class_name = 'GICS' ";
            industries += " AND fac.DATE=(Select MAX(date) from DATA_FACTSET)";
            if (univers == "Europe")
            {
                industries += " and fac.MXEU is not null and fac.MXUSLC is null ";
                industries += " AND s.class_name = 'FGA_EU'";
            }
            else
            {
                industries += " and fac.MXEU is null and fac.MXUSLC is not null";
                industries += " AND s.class_name = 'FGA_US'";
            }
            #endregion
            return industries;
        }

        public String GetAllSectorsInUnivers(String univers)
        {
            String secteurs = "";

            secteurs += " Select ";
            secteurs += " distinct s.label as Label_Secteur,";
            secteurs += " NULL as Label_Industry,";
            secteurs += " NULL as TICKER,";
            secteurs += " NULL as Company_Name,";
            secteurs += " NULL as Q,";
            secteurs += " CONVERT(DATE, rs.date ,103) as Date,";
            if (univers == "Europe")
            {
                secteurs += " rs.reco_MXEM as RecoMXEM,";
                secteurs += " s.MXEM as PoidsMXEM,";
                secteurs += " rs.recommandation as RecoMXEU,";
                secteurs += " s.MXEU as PoidsMXEU,";
                secteurs += " rs.reco_MXEUM as RecoMXEUM,";
                secteurs += " s.MXEUM as PoidsMXEUM,";
            }
            else
            {
                secteurs += " rs.reco_MXUSLC as RecoMXUSLC,";
                secteurs += " s.MXUSLC as PoidsMXUSLC,";
            }
            secteurs += " NULL as Liquidity,";
            secteurs += " NULL as Exclu,";
            secteurs += " NULL as ISIN,";
            secteurs += " Cast(s.code as VARCHAR(32)) as ID_ICB,";
            secteurs += " NULL as ID_FGA";
            secteurs += " from ACT_RECO_SECTOR as rs";
            secteurs += " right join #sectors as s";
            secteurs += " on rs.id_secteur = s.code AND rs.date = (SELECT MAX(date) from ACT_RECO_SECTOR as rv where rv.id_secteur = s.code)";
            if (univers == "Europe")
                secteurs += " AND (rs.reco_MXUSLC IS NULL OR rs.recommandation = '')";
            else
                secteurs += " AND (rs.recommandation IS NULL OR rs.recommandation = '' )";

            return secteurs;
        }

        public String GetAllIndustriesInUnivers(String univers)
        {
            String industries = "";

            industries += " Select";
            industries += " distinct s.SECTOR  as Label_Secteur, ";
            industries += " s.INDUSTRY as Label_Industry,";
            industries += " NULL as TICKER,";
            industries += " NULL as Company_Name,";
            industries += " NULL as Q,";
            industries += " CONVERT(DATE, rs.date ,103) as Date,";
            if (univers == "Europe")
            {
                industries += " rs.reco_MXEM as RecoMXEM,";
                industries += " s.MXEM as PoidsMXEM,";
                industries += " rs.recommandation as RecoMXEU,";
                industries += " s.MXEU as PoidsMXEU,";
                industries += " rs.reco_MXEUM as RecoMXEUM,";
                industries += " s.MXEUM as PoidsMXEUM,";
            }
            else
            {
                industries += " rs.reco_MXUSLC as RecoMXUSLC,";
                industries += " s.MXUSLC as PoidsMXUSLC,";
            }
            industries += " NULL as Liquidity,";
            industries += " NULL as Exclu,";
            industries += " NULL as ISIN,";
            industries += " Cast(s.SuperSecteurId as VARCHAR(32)) as ID_ICB,";
            industries += " Cast(s.SecteurId as VARCHAR(32)) as ID_FGA";
            industries += " from ACT_RECO_SECTOR as rs";
            industries += " right join #industries as s";
            industries += " on rs.id_secteur = s.SecteurId AND rs.date = (SELECT MAX(date) from ACT_RECO_SECTOR as rv where rv.id_secteur = s.SecteurId)";
            if (univers == "Europe")
                industries += " AND (rs.reco_MXUSLC IS NULL OR rs.recommandation = '')";
            else
                industries += " AND (rs.recommandation IS NULL OR rs.recommandation = '' )";

            return industries;
        }
    }
}