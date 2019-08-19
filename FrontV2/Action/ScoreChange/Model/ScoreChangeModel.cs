using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Telerik.Windows.Data;

namespace FrontV2.Action.ScoreChange.Model
{
    class ScoreChangeModel
    {
        private Connection _connection;

        public ScoreChangeModel()
        {
            _connection = new Connection();
        }

        public RadObservableCollection<String> GetDates()
        {
            return _connection.GetDates();
        }

        public DataTable GetChanges(String dateMin, String dateMax, bool filterRank = true)
        {
            String fullRequest = GetChangedValues(dateMin, dateMax, filterRank);

            DataTable tmp = _connection.LoadDataGridByString(fullRequest);

            String clear = "DROP TABLE #qrvalues1 DROP TABLE #qrvalues2 DROP TABLE #sectors";
            _connection.RequeteSql(clear);

            return tmp;
        }

        public String GetChangedValues(String dateMin, String dateMax, bool filterRank = true)
        {
            String nsql = "SELECT * " +
            "into #qrvalues1 " +
            "from DATA_FACTSET " +
            "where DATE ='" + dateMin + "' AND ISIN IS NOT NULL " +
            " " +
            "SELECT * " +
            "into #qrvalues2 " +
            "from DATA_FACTSET " +
            "where DATE = '" + dateMax + "'  And ISIN IS NOT NULL " +
            " " +
            "SELECT * " +
            "into #sectors " +
            "from DATA_FACTSET " +
            "WHERE DATE = '" + dateMax +"' AND ISIN IS NULL AND GICS_SUBINDUSTRY IS NULL " +
                " " +
            "SELECT distinct sect3.label as SECTOR, sect2.label as INDUSTRY, facs.SUIVI as SUIVI, fac1.TICKER as TICKER, fac1.COMPANY_NAME AS COMPANY, " +
            "CONVERT(DATE, fac1.DATE, 103) as Date1, fac1.GARPN_RANKING_S as Rang1, fac1.GARPN_QUINTILE_S as Quint1, " +
            "CONVERT(DATE, fac2.DATE, 103) as Date2, fac2.GARPN_RANKING_S as Rang2, fac2.GARPN_QUINTILE_S as Quint2 ," +
            "fac1.COUNTRY as Pays " +
            "from #qrvalues1 fac1 " +
            "inner join #qrvalues2 as fac2 on fac2.ISIN = fac1.ISIN " +
            "inner join ref_security.SECTOR as sect on fac1.SECTOR = sect.code " +
            "inner join ref_security.SECTOR_TRANSCO as transco on transco.id_sector1 = sect.id " +
            "inner join ref_security.SECTOR as sect2 on sect2.id = transco.id_sector2 " +
            "inner join ref_security.SECTOR as sect3 on sect3.id = sect.id_parent " +
            "inner join #sectors as facs on facs.FGA_SECTOR = sect2.code " +
            "WHERE fac1.GARPN_QUINTILE_S <> fac2.GARPN_QUINTILE_S AND fac1.ISIN = fac2.ISIN ";
            if (filterRank)
                nsql += "and ABS(fac1.GARPN_RANKING_S - fac2.GARPN_RANKING_S) > 1 ";

            return nsql;
        }

        public RadObservableCollection<KeyValuePair<string, int>> GetQuintiles(String ticker, String dateMin, String dateMax)
        {
            RadObservableCollection<KeyValuePair<string, int>> res = new RadObservableCollection<KeyValuePair<string, int>>();

            String request = "SELECT CONVERT(DATE, date, 103) as Date, GARPN_QUINTILE_S as Quint FROM DATA_FACTSET WHERE TICKER='" + ticker + "' AND GARPN_QUINTILE_S IS NOT NULL AND DATE <= '" + dateMin + "' AND DATE >= '" + dateMax + "' ORDER BY DATE";
            List<KeyValuePair<string, int>> values = _connection.sqlToListKeyValuePair(request);

            foreach (var v in values)
                res.Add(new KeyValuePair<string, int>(v.Key.Substring(0, 10), v.Value));

            return res;
        }

    }
}
