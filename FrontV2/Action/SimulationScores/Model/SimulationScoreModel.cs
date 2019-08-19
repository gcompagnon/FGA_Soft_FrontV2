using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Telerik.Windows.Data;

namespace FrontV2.Action.SimulationScore.Model
{
    class SimulationScoreModel
    {
        readonly Connection connection = new Connection();

        public SimulationScoreModel()
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

                return connection.ProcedureStockeeForDataGrid("ACT_DataGridBlendValeur_Simulation",
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

                    DataTable t = connection.ProcedureStockeeForDataGrid("ACT_DataGridBlendValeur_Simulation",
                         new List<String> { "@date", "@id_fga", "@FGA" },
                         new List<object> { date, id, univers });
                    tmp.Merge(t);
                }
                return tmp;
            }
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

        public void CalculateNewScores(String selectedDate)
        {
            connection.ProcedureStockee("ACT_BlendScore_Valeur_Simulation",
                new List<String> { "@date", "@Winsor_coef", "@Class_name" },
            new List<object> { selectedDate, 90, "FGA_EU" });
            connection.ProcedureStockee("ACT_BlendScore_Valeur_Simulation",
                new List<String> { "@date", "@Winsor_coef", "@Class_name" },
            new List<object> { selectedDate, 90, "FGA_US" });
        }

        public void CopyDataFactSet(String selectedDate)
        {
            String sql = "INSERT INTO DATA_FACTSET_SIMULATION "
                + " SELECT * FROM DATA_FACTSET WHERE DATE = '" + selectedDate + "'";
            connection.RequeteSql(sql);
        }

        public bool IsActCoefCritereEmpty()
        {
            return int.Parse((connection.
                SqlWithReturn("SELECT COUNT(*) FROM ACT_COEF_CRITERE_SIMULATION"))[0].ToString()) == 0;
        }

        public void CopyActCoefCritere()
        {
            String sql = 
                " SET IDENTITY_INSERT ACT_COEF_CRITERE_SIMULATION ON "
                + " INSERT INTO ACT_COEF_CRITERE_SIMULATION (id_critere, id_parent, nom, position, description, CAP_min, CAP_max, format, precision, groupe, inverse, is_sector) "
                + " SELECT id_critere, id_parent, nom, position, description, CAP_min, CAP_max, format, precision, groupe, inverse, is_sector FROM ACT_COEF_CRITERE "
                + " "
                + " INSERT INTO ACT_COEF_SECTEUR_SIMULATION "
                + " SELECT * FROM ACT_COEF_SECTEUR ";

            connection.RequeteSql(sql);
        }
              
        public void CleanCriteres()
        {
            connection.RequeteSql("DELETE FROM ACT_COEF_CRITERE_SIMULATION");
            connection.RequeteSql("DELETE FROM ACT_COEF_SECTEUR_SIMULATION");

        }

        public void ClearDataFactsetSimulation(String date)
        {
            connection.RequeteSql("DELETE FROM DATA_FACTSET_SIMULATION WHERE DATE ='" + date + "'");
        }
    }
}