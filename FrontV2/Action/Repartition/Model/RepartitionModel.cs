using FrontV2.Action.Repartition.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Telerik.Windows.Data;

namespace FrontV2.Action.Repartition.Model
{
    class RepartitionModel
    {
        private Connection _connection;

        public RepartitionModel()
        {
            _connection = new Connection();
        }

        public RadObservableCollection<String> GetDates()
        {
            return _connection.GetDates();
        }

        public RadObservableCollection<String> GetAllTickers()
        {
            List<object> tmp = _connection.SqlWithReturn("SELECT TICKER FROM DATA_FACTSET WHERE DATE = (SELECT MAX(DATE) FROM DATA_FACTSET)");

            RadObservableCollection<String> res = new RadObservableCollection<string>();
            foreach (var v in tmp)
                res.Add(v.ToString());

            return res;
        }

        /*************************
             DataTable
        *************************/
        public DataTable GetSectorsDataSource(String date)
        {
            return _connection.ProcedureStockeeForDataGrid("ACT_RepartitionSecteurs",
                new List<String> { "@date" },
                new List<object> { date });
        }

        public DataTable GetCountriesDataSource(String date)
        {
            DataTable tmp = _connection.ProcedureStockeeForDataGrid("ACT_RepartitionPays",
                new List<String> { "@date" },
                new List<object> { date });

            foreach (DataRow row in tmp.Rows)
                row["COUNTRY"] = GetFullCountry(row["COUNTRY"].ToString());

            return tmp;
        }

        public DataTable GetIndustriesDataSource(String date)
        {
            return _connection.ProcedureStockeeForDataGrid("ACT_RepartitionIndustries",
                new List<String> { "@date", "@idSector", "@ptf", "@bench" },
                new List<object> { date, "-1", "", "" });
        }

        /*************************
             Charts
        *************************/
        public ChartSerie GetChartDataForCountry(String ptf, String country)
        {
            country = GetShortCountry(country);
            if (ptf.Contains("Ecart_"))
                return GetChartDataForCountryGap(ptf, country);

            String sql = "SELECT DATE, SUM([" + ptf + "]) * 100 as ptf" +
                 " FROM DATA_FACTSET WHERE COUNTRY = '" + country + "' GROUP BY DATE ORDER BY DATE";

            List<KeyValuePair<String, double>> tmp = _connection.sqlToListKeyValuePairDouble(sql);

            ChartSerie res = new ChartSerie();
            res.Label = GetFullCountry(country);

            tmp = GetfinDeMois(tmp);
            tmp = GetLastDates(tmp, 12);

            res.Data = new RadObservableCollection<ChartPoint>();
            foreach (var item in tmp)
            {
                if (item.Key != "")
                    res.Data.Add(new ChartPoint(item.Key.Substring(0, 10), item.Value, GetFullCountry(country)));
            }

            return res;
        }

        public ChartSerie GetChartDataForCountryGap(String ptf, String country)
        {
            String bench = "";
            String ptfBase = ptf.Replace("Ecart_", "");

            #region Choix du Bench
            switch (ptfBase)
            {
                case ("6100002"):
                    bench = "MXFR";
                    break;
                case ("6100030"):
                    bench = "MXEM";
                    break;
                case ("AVEURO"):
                    bench = "MXEM";
                    break;
                case ("6100004"):
                    bench = "MXEM";
                    break;
                case ("610000263"):
                    bench = "MXEM";
                    break;
                case ("AVEUROPE"):
                    bench = "MXEM";
                    break;
                case ("6100001"):
                    bench = "MXEM";
                    break;
                case ("6100033"):
                    bench = "MXEM";
                    break;
                case ("6100062"):
                    bench = "MXEUM";
                    break;
                case ("6100026"):
                    bench = "MXEU";
                    break;
                case ("6100024"):
                    bench = "MXUSLC";
                    break;
            }
            #endregion

            #region Sql Request
            String sql = "";
            sql += " SELECT ";
            sql += " fac.COUNTRY,  ";
            sql += " fac.Date as Date, ";
            sql += " convert(decimal(10, 2),SUM(fac.MXFR)) as MXFR, ";
            sql += " convert(decimal(10, 2),SUM(fac.MXEM)) as MXEM, ";
            sql += " convert(decimal(10, 2),SUM(fac.MXEUM)) as MXEUM, ";
            sql += " convert(decimal(10, 2),SUM(fac.MXEU)) as MXEU, ";
            sql += " convert(decimal(10, 2),SUM(fac.MXUSLC)) as MXUSLC ";
            sql += " INTO #BENCH ";
            sql += " FROM DATA_FACTSET as fac  ";
            sql += " WHERE COUNTRY = '" + country + "' ";
            sql += " GROUP BY fac.Date, fac.COUNTRY ";
            sql += " ORDER BY fac.COUNTRY ";
            sql += "  ";
            sql += " SELECT  ";
            sql += " fac.DATE, ";
            sql += " convert(decimal(10, 2),SUM(fac.[" + ptfBase + "] * 100) - b." + bench + ") as ptf ";
            sql += " FROM DATA_FACTSET as fac ";
            sql += " inner join #BENCH as b on b.date = fac.date ";
            sql += " WHERE fac.COUNTRY = '" + country + "'  ";
            sql += " GROUP BY fac.DATE, b." + bench + " ORDER BY fac.DATE ";
            sql += " DROP TABLE #BENCH ";
            #endregion

            List<KeyValuePair<String, double>> tmp = _connection.sqlToListKeyValuePairDouble(sql);

            ChartSerie res = new ChartSerie();
            res.Label = GetFullCountry(country);

            tmp = GetfinDeMois(tmp);
            tmp = GetLastDates(tmp, 12);

            res.Data = new RadObservableCollection<ChartPoint>();
            foreach (var item in tmp)
            {
                if (item.Key != "")
                    res.Data.Add(new ChartPoint(item.Key.Substring(0, 10), item.Value, GetFullCountry(country)));
            }

            return res;
        }

        public ChartSerie GetChartDataForSector(String ptf, String sector)
        {
            if (ptf.Contains("Ecart_"))
                return GetChartDataForSectorGap(ptf, sector);

            String sql = "SELECT DATE, SUM([" + ptf + "]) * 100 as ptf" +
                 " FROM DATA_FACTSET WHERE SECTOR_LABEL = '" + sector + "' GROUP BY DATE ORDER BY DATE";

            List<KeyValuePair<String, double>> tmp = _connection.sqlToListKeyValuePairDouble(sql);

            ChartSerie res = new ChartSerie();
            res.Label = sector;

            tmp = GetfinDeMois(tmp);
            tmp = GetLastDates(tmp, 12);

            res.Data = new RadObservableCollection<ChartPoint>();
            foreach (var item in tmp)
            {
                if (item.Key != "")
                    res.Data.Add(new ChartPoint(item.Key.Substring(0, 10), item.Value, sector));
            }

            return res;
        }

        public ChartSerie GetChartDataForSectorGap(String ptf, String sector)
        {
            String bench = "";
            String ptfBase = ptf.Replace("Ecart_", "");

            #region Choix du Bench
            switch (ptfBase)
            {
                case ("6100002"):
                    bench = "MXFR";
                    break;
                case ("6100030"):
                    bench = "MXEM";
                    break;
                case ("AVEURO"):
                    bench = "MXEM";
                    break;
                case ("6100004"):
                    bench = "MXEM";
                    break;
                case ("610000263"):
                    bench = "MXEM";
                    break;
                case ("AVEUROPE"):
                    bench = "MXEM";
                    break;
                case ("6100001"):
                    bench = "MXEM";
                    break;
                case ("6100033"):
                    bench = "MXEM";
                    break;
                case ("6100062"):
                    bench = "MXEUM";
                    break;
                case ("6100026"):
                    bench = "MXEU";
                    break;
                case ("6100024"):
                    bench = "MXUSLC";
                    break;
            }
            #endregion

            #region Sql Request
            String sql = "";
            sql += " SELECT distinct ";
            sql += " fac.DATE, ";
            sql += " ss.label as 'SECTOR GICS', ";
            sql += " convert(decimal(10, 2),SUM(fac.MXFR)) as MXFR, ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100002]) * 100)  as [6100002],  ";
            sql += " convert(decimal(10, 2),SUM(fac.MXEM)) as MXEM, ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100030]) * 100)  as [6100030],  ";
            sql += " convert(decimal(10, 2),SUM(fac.AVEURO) * 100)  as AVEURO,  ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100004]) * 100)  as [6100004],  ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100063]) * 100)  as [6100063],  ";
            sql += " convert(decimal(10, 2),SUM(fac.AVEUROPE) * 100)  as AVEUROPE,  ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100001]) * 100)  as [6100001],  ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100033]) * 100)  as [6100033], ";
            sql += " convert(decimal(10, 2),SUM(fac.MXEUM)) as MXEUM, ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100062]) * 100)  as [6100062], ";
            sql += " convert(decimal(10, 2),SUM(fac.MXEU)) as MXEU, ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100026]) * 100)  as [6100026], ";
            sql += " convert(decimal(10, 2),SUM(fac.MXUSLC)) as MXUSLC, ";
            sql += " convert(decimal(10, 2),SUM(fac.[6100024]) * 100)  as [6100024] ";
            sql += " INTO #values ";
            sql += " FROM ref_security.SECTOR as ss ";
            sql += " inner join DATA_FACTSET as fac on fac.GICS_SECTOR = ss.code ";
            sql += " where  ";
            sql += " level = 0 ";
            sql += " and ss.class_name =  'GICS' and fac.GICS_SUBINDUSTRY is null  ";
            sql += " and ss.label = '" + sector + "' ";
            sql += " GROUP BY ss.label, fac.DATE ";
            sql += " ORDER BY ss.label, fac.DATE ";
            sql += "  ";
            sql += " SELECT date, ";
            sql += " convert(decimal(10, 2),[" + ptfBase + "] - " + bench + ") as pft ";
            sql += " FROM #values ";
            sql += "  ";
            sql += " DROP TABLE #values ";

            #endregion

            List<KeyValuePair<String, double>> tmp = _connection.sqlToListKeyValuePairDouble(sql);

            ChartSerie res = new ChartSerie();
            res.Label = sector;

            tmp = GetfinDeMois(tmp);
            tmp = GetLastDates(tmp, 12);

            res.Data = new RadObservableCollection<ChartPoint>();
            foreach (var item in tmp)
            {
                if (item.Key != "")
                    res.Data.Add(new ChartPoint(item.Key.Substring(0, 10), item.Value, sector));
            }

            return res;
        }

        public RadObservableCollection<ChartSerie> GetChartIndustriesDataSource(String ptf, String sector)
        {
            if (ptf == null)
                return null;

            String idsector = _connection.GetIdSectorFromName(sector).ToString().Trim();
            String ptff = "[" + ptf.Replace("Ecart_", "") + "]";

            List<DateIndustryValue> tmp;

            if (ptf.Contains("Ecart_"))
            {
                String bench = "";
                if (ptff == "[6100002]")
                    bench = "MXFR";
                else if (ptff == "[6100030]" || ptff == "[AVEURO]" || ptff == "[6100004]"
                    || ptff == "[6100063]" || ptff == "[AVEUROPE]" || ptff == "[6100001]" || ptff == "[6100033]")
                    bench = "MXEM";
                else if (ptff == "[6100062]")
                    bench = "MXEUM";
                else if (ptff == "[6100026]")
                    bench = "MXEU";
                else if (ptff == "[6100024]" || ptff == "[6100066]")
                    bench = "MXUSLC";

                tmp = _connection.ProckStockToDateIndustryValue("ACT_RepartitionIndustries",
                    idsector, ptff, bench);
            }
            else
                tmp = _connection.ProckStockToDateIndustryValue("ACT_RepartitionIndustries",
                    idsector, ptff, "null");

            if (tmp == null)
                return null;

            List<List<KeyValuePair<String, double>>> tmp2 = new List<List<KeyValuePair<String, double>>>();

            List<KeyValuePair<String, double>> currentList = new List<KeyValuePair<String, double>>();
            List<String> industriesName = new List<string>();

            String currentIndustry = "";
            foreach (var v in tmp)
            {
                if (currentIndustry == "")
                    currentIndustry = v.Industry;

                if (currentIndustry != v.Industry)
                {
                    industriesName.Add(currentIndustry);
                    currentIndustry = v.Industry;
                    tmp2.Add(currentList);
                    currentList = new List<KeyValuePair<String, double>>();
                    currentList.Add(new KeyValuePair<String, double>(v.Date.Substring(0, 10), v.Value));
                    currentIndustry = v.Industry;
                }
                else
                    currentList.Add(new KeyValuePair<String, double>(v.Date.Substring(0, 10), v.Value));
            }
            tmp2.Add(currentList);
            industriesName.Add(currentIndustry);

            for (int i = 0; i < tmp2.Count; i++)
            {
                tmp2[i] = GetfinDeMois(tmp2[i]);
                tmp2[i] = GetLastDates(tmp2[i], 12);
            }

            RadObservableCollection<ChartSerie> res = new RadObservableCollection<ChartSerie>();

            for (int i = 0; i < tmp2.Count; i++)
            {
                ChartSerie serie = new ChartSerie();
                serie.Label = industriesName[i];
                serie.Data = new RadObservableCollection<ChartPoint>();

                foreach (var v in tmp2[i])
                    serie.Data.Add(new ChartPoint(v.Key, v.Value, industriesName[i]));

                res.Add(serie);
            }

            return res;
            /*
            String industries = "";
            industries += " SELECT ";
            industries += "  distinct  fac.DATE as Date, ";
            industries += "  s.label, ";
            industries += "  convert(decimal(10, 2),SUM(fac.[" + ptf + "] *  100)) as '" + ptf + "'  ";
            industries += "  FROM ref_security.SECTOR s   ";
            industries += "  INNER JOIN ref_security.SECTOR_TRANSCO st on  ";
            industries += "  st.id_sector1 = s.id   ";
            industries += "  INNER JOIN ref_security.SECTOR fils on   ";
            industries += "  fils.id = st.id_sector2   ";
            industries += "  LEFT OUTER JOIN ref_security.SECTOR ss ON   ";
            industries += "  fils.id_parent = ss.id   ";
            industries += "  INNER JOIN DATA_FACTSET fac on   ";
            industries += "  fac.FGA_SECTOR = s.code   ";
            industries += "  WHERE fac.GICS_SECTOR is null  ";
            industries += "  AND ss.class_name = 'GICS'   ";
            industries += "  AND ss.label = '" + sector + "' ";
            industries += "  GROUP BY fac.DATE, s.label ORDER BY s.label, DATE";

            List<DateIndustryValue> tmp = _connection.sqlToDateIndustryValue(industries);
            List<List<KeyValuePair<String, double>>> tmp2 = new List<List<KeyValuePair<String, double>>>();

            List<KeyValuePair<String, double>> currentList = new List<KeyValuePair<String, double>>();
            List<String> industriesName = new List<string>();

            String currentIndustry = "";
            foreach (var v in tmp)
            {
                if (currentIndustry == "")
                    currentIndustry = v.Industry;

                if (currentIndustry != v.Industry)
                {
                    industriesName.Add(currentIndustry);
                    currentIndustry = v.Industry;
                    tmp2.Add(currentList);
                    currentList = new List<KeyValuePair<String, double>>();
                    currentList.Add(new KeyValuePair<String, double>(v.Date.Substring(0, 10), v.Value));
                    currentIndustry = v.Industry;
                }
                else
                    currentList.Add(new KeyValuePair<String, double>(v.Date.Substring(0, 10), v.Value));
            }
            tmp2.Add(currentList);
            industriesName.Add(currentIndustry);

            for (int i = 0; i < tmp2.Count; i++)
            {
                tmp2[i] = GetfinDeMois(tmp2[i]);
                tmp2[i] = GetLastDates(tmp2[i], 12);
            }

            RadObservableCollection<CharSerie> res = new RadObservableCollection<CharSerie>();

            for (int i = 0; i < tmp2.Count; i++)
            {
                CharSerie serie = new CharSerie();
                serie.Label = industriesName[i];
                serie.Data = new RadObservableCollection<ChartPoint>();

                foreach (var v in tmp2[i])
                    serie.Data.Add(new ChartPoint(v.Key, v.Value, industriesName[i]));

                res.Add(serie);
            }

            return res;
             */
        }

        /*************************
             Cleaners
        *************************/
        public List<KeyValuePair<String, double>> GetfinDeMois(List<KeyValuePair<String, double>> dates)
        {
            List<KeyValuePair<String, double>> res = new List<KeyValuePair<String, double>>();
            int prevMonth = -1;

            String oldKey = "";
            double oldValue = 0;

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
                    res.Add(new KeyValuePair<String, double>(oldKey, oldValue));
                }
                prevMonth = curMonth;
                oldKey = date.Key;
                oldValue = date.Value;
            }

            res.Add(new KeyValuePair<String, double>(oldKey, oldValue));

            return res;
        }

        public List<KeyValuePair<String, double>> GetLastDates(List<KeyValuePair<String, double>> dates,
            int interval)
        {
            List<KeyValuePair<String, double>> res = new List<KeyValuePair<string, double>>();
            if (interval > dates.Count)
                interval = dates.Count;

            for (int i = interval; i > 0; i--)
            {
                String s = dates[dates.Count - i].Key;
                double n = dates[dates.Count - i].Value;
                res.Add(new KeyValuePair<String, double>(s, n));
            }
            return res;
        }

        public static int GetMonth(String date)
        {
            return int.Parse(date.Substring(3, 2));
        }

        public String GetFullCountry(String country)
        {
            switch (country)
            {
                case "FR": return "France";
                case "DE": return "Germany";
                case "ES": return "Spain";
                case "NL": return "Netherlands";
                case "IT": return "Italy";
                case "CH": return "Switzerland";
                case "GB": return "United Kingdom";
                case "BE": return "Belgium";
                case "FI": return "Finland";
                case "SE": return "Sweden";
                case "IE": return "Ireland";
                case "DK": return "Denmark";
                case "AT": return "Austria";
                case "NO": return "Norway";
                case "PT": return "Portugal";
                case "US": return "USA";
                default: return "Others";
            }
        }

        public String GetShortCountry(String country)
        {
            switch (country)
            {
                case "France": return "FR";
                case "Germany": return "DE";
                case "Spain": return "ES";
                case "Netherlands": return "NL";
                case "Italy": return "IT";
                case "Switzerland": return "CH";
                case "United Kingdom": return "GB";
                case "Belgium": return "BE";
                case "Finland": return "FI";
                case "Sweden": return "SE";
                case "Ireland": return "IE";
                case "Denmark": return "DK";
                case "Austria": return "AT";
                case "Norway": return "NO";
                case "Portugal": return "PT";
                case "USA": return "US";
                default: return "Others";
            }
        }
    }
}
