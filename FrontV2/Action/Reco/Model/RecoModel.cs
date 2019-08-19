using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FrontV2.Action.Reco.Model
{
    class RecoModel
    {
        readonly Connection connection;

        public RecoModel()
        {
            connection = new Connection();
        }

        public List<object> GetFGAIndustries(String superSector, String univers)
        {
            return connection.GetFGAIndustries(superSector, univers);
        }

        public List<object> GetSectorsICB()
        {
            return connection.GetSectorsICB();
        }

        public DataTable GetValuesData(String univers, String sector, String industry, String restriction)
        {
            if (univers != "Europe")
                GlobalInfos.isEurope = false;
            else
                GlobalInfos.isEurope = true;

            if (sector == null || sector == "")
            {
                if (restriction == "All")
                    return GetUniversEnsemble(univers);
                else if (restriction == "Valeurs")
                    return GetUniversValeurs(univers);
                else
                    return GetUniversIndustries(univers);
            }
            else if (industry == null || industry == "")
                return GetUniversSector(univers, sector);
            else
                return GetUniversSectorIndustry(univers, industry);
        }

        /// <summary>
        /// Univers + Ensemble
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public DataTable GetUniversEnsemble(String univers)
        {
            String requete = "";

            requete += GetTmpAllIndustriesInUnivers(univers);
            requete += GetTmpAllSectorsInUnivers(univers);
            requete += GetTmpAllValuesInUnivers(univers);

            requete += GetAllIndustriesInUnivers(univers);
            requete += " ";
            requete += " UNION";
            requete += " ";
            requete += GetAllSectorsInUnivers(univers);
            requete += " ";
            requete += " UNION";
            requete += " ";
            requete += GetAllValuesWithRecoInUnivers(univers);

            requete += " ";
            requete += " DROP TABLE #sectors";
            requete += " DROP TABLE #industries";
            requete += " DROP TABLE #values";

            return connection.LoadDataGridByString(requete);
        }

        /// <summary>
        /// Univers + Industries
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Univers + Valeurs
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public DataTable GetUniversValeurs(String univers)
        {
            String requete = "";

            requete += GetTmpAllIndustriesInUnivers(univers);
            requete += GetTmpAllValuesInUnivers(univers);
            requete += GetAllValuesWithRecoInUnivers(univers);
            requete += " DROP TABLE #industries";
            requete += " DROP TABLE #values";

            return connection.LoadDataGridByString(requete);
        }

        /// <summary>
        /// Univers + Sector
        /// </summary>
        /// <param name="univers"></param>
        /// <param name="sector"></param>
        /// <returns></returns>
        public DataTable GetUniversSector(String univers, String sector)
        {
            String requete = "";

            requete += GetTmpAllIndustriesInSectorInUnivers(univers, sector);
            requete += GetTmpAllValuesInUnivers(univers);

            requete += GetAllIndustriesInUnivers(univers);
            requete += " UNION ";
            requete += GetAllValuesPerIndustry(univers);

            requete += " DROP TABLE #industries";
            requete += " DROP TABLE #values";

            return connection.LoadDataGridByString(requete);
        }

        /// <summary>
        /// Univers + Sector + Industry
        /// </summary>
        /// <param name="univers"></param>
        /// <param name="sector"></param>
        /// <param name="industy"></param>
        /// <returns></returns>
        public DataTable GetUniversSectorIndustry(String univers, String industry)
        {
            String requete = "";

            requete += GetTmpIndustry(univers, industry);
            requete += GetTmpAllValuesInUnivers(univers);

            requete += GetAllIndustriesInUnivers(univers);
            requete += " UNION ";
            requete += GetAllValuesPerIndustry(univers);

            requete += " DROP TABLE #industries";
            requete += " DROP TABLE #values";

            return connection.LoadDataGridByString(requete);
        }

        #region Historique et Recommandation

        public DataTable GetHistoData(String isin, String id_sector_fga, String sector_fga, String libelle)
        {
            String requete = "";

            if (isin == "")
            {
                requete = "SELECT CONVERT(DATE, date ,103) as Date, '" + sector_fga + "' As Libelle,  recommandation As RecoMXEU, reco_MXEUM As RecoMXEUM, reco_MXEM As RecoMXEM, reco_MXUSLC as 'Reco MXUSLC', id_comment, id_comment_change, loginModif as Login, NULL as type, ISIN as ISIN, id_secteur as ID";
                requete += " FROM ACT_RECO_SECTOR rec";
                requete += " WHERE rec.type = 'FGA' and rec.id_secteur = " + id_sector_fga;
                requete += " ORDER BY date DESC";
            }
            else
            {
                String valeur = "'" + libelle.Replace("'", "''") + "'";
                isin = "'" + isin.Replace("'", "''") + "'";

                requete = "SELECT CONVERT(DATE, date ,103) as Date, " + valeur + "As Libelle,reco_SXXP As RecoMXEU, reco_SXXA As RecoMXEUM, reco_SXXE As RecoMXEM, reco_MXUSLC as RecoMXUSLC, id_comment, id_comment_change, loginModif as Login, NULL as type, ISIN as ISIN, id_valeur as ID";
                requete += " FROM ACT_RECO_VALEUR";
                requete += " WHERE isin = " + isin;
                requete += " ORDER BY date DESC";
            }
            return connection.LoadDataGridByString(requete);
        }

        public DataTable GetHistoDataSector(String univers, String sectorFGA, String sectorICB, String id_fga, String id_icb)
        {
            String reco = "";
            if (sectorFGA != "")
            {
                sectorFGA = "'" + sectorFGA.Replace("'", "''") + "'";
                reco = "SELECT CONVERT(DATE, date ,103) as Date, " + sectorFGA + " As Libelle, recommandation As RecoMXEU, reco_MXEUM As RecoMXEUM, reco_MXEM As RecoMXEM, reco_MXUSLC as RecoMXUSLC, id_comment, id_comment_change, loginModif as Login, type, NULL as ISIN, id_secteur as ID";
                reco += " FROM ACT_RECO_SECTOR rec";
                reco += " WHERE rec.type = 'FGA' and rec.id_secteur = " + int.Parse(id_fga);
                if (univers == "Europe")
                    reco += " AND (rec.reco_MXUSLC IS NULL OR rec.reco_MXUSLC = '') ";
                else
                    reco += " AND (rec.recommandation IS NULL OR rec.recommandation = '') ";
                reco += " ORDER BY date DESC, type DESC";
            }
            else
            {
                sectorICB = "'" + sectorICB.Replace("'", "''") + "'";
                reco = "SELECT CONVERT(DATE, date ,103) as Date, " + sectorICB + " As Libelle, recommandation As RecoMXEU, reco_MXEUM As RecoMXEUM, reco_MXEM As RecoMXEM, reco_MXUSLC as RecoMXUSLC, id_comment, id_comment_change, loginModif as Login, type, NULL as ISIN, id_secteur as ID";
                reco += " FROM ACT_RECO_SECTOR rec";
                reco += " WHERE rec.type = 'ICB' and rec.id_secteur = " + int.Parse(id_icb);
                if (univers == "Europe")
                    reco += " AND (rec.reco_MXUSLC IS NULL OR rec.reco_MXUSLC = '') ";
                else
                    reco += " AND (rec.recommandation IS NULL OR rec.recommandation = '') ";
                reco += " UNION";
                reco += " SELECT  CONVERT(DATE, date ,103) as Date, fga.libelle As Libelle, recommandation As RecoMXEU, reco_MXEUM As RecoMXEUM, reco_MXEM As RecoMXEM, reco_MXUSLC as RecoMXUSLC, id_comment, id_comment_change, loginModif as Login, type, NULL as ISIN, id_secteur as ID";
                reco += " FROM ACT_SUPERSECTOR sup";
                reco += " INNER JOIN ACT_SECTOR sec on sec.id_supersector = sup.id";
                reco += " INNER JOIN ACT_SUBSECTOR sub on sub.id_sector = sec.id";
                reco += " INNER JOIN ACT_FGA_SECTOR fga on fga.id = sub.id_fga_sector";
                reco += " INNER JOIN ACT_RECO_SECTOR rec on rec.id_secteur = fga.id and rec.type = 'FGA'";
                reco += " WHERE rec.type = 'FGA' and sup.id_federis = " + int.Parse(id_icb);
                if (univers == "Europe")
                    reco += " AND (rec.reco_MXUSLC IS NULL OR rec.reco_MXUSLC = '') ";
                else
                    reco += " AND (rec.recommandation IS NULL OR rec.recommandation = '') ";
                reco += " GROUP BY date, type, fga.libelle, recommandation, reco_MXEUM, reco_MXEM, reco_MXUSLC, id_comment, id_comment_change, loginModif, id_secteur";
                reco += " ORDER BY date DESC, type DESC";
            }
            return connection.LoadDataGridByString(reco);
        }

        public object GetRecommandationText(String id)
        {
            return connection.SelectWhere("ACT_RECO_COMMENT", "comment", "id", id).FirstOrDefault();
        }

        #endregion

        #region Methods helpers

        /// <summary>
        /// The temps table crated is #sectors
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public String GetTmpAllSectorsInUnivers(String univers)
        {
            String secteurs = "";
            //SECTORS

            #region Sectors

            secteurs += " SELECT distinct ";
            secteurs += " ss.label, ";
            secteurs += " ss.code, ";
            secteurs += " SUM(fac.MXEU) as MXEU,";
            secteurs += " SUM(fac.MXEUM) as MXEUM,";
            secteurs += " SUM(fac.MXEM) as MXEM,";
            secteurs += " SUM(fac.MXUSLC) as MXUSLC";
            secteurs += " into #sectors ";
            secteurs += " FROM ref_security.SECTOR as ss ";
            secteurs += " inner join DATA_FACTSET as fac on fac.GICS_SECTOR = ss.code ";
            secteurs += " where ";
            secteurs += " level = 0 ";
            secteurs += " and ss.class_name =  'GICS' and fac.GICS_SUBINDUSTRY is null ";
            secteurs += " and fac.DATE = (Select MAX(date) from DATA_FACTSET) ";
            if (univers == "Europe")
                secteurs += " and fac.MXUSLC is null ";
            else
                secteurs += " and fac.MXEU is null and fac.MXUSLC is not null";
            secteurs += " GROUP BY ss.label, ss.code";

            #endregion

            return secteurs;
        }

        /// <summary>
        /// Create a temp table #industries
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public String GetTmpAllIndustriesInUnivers(String univers)
        {
            String industries = "";
            // INDUSTRIES
            #region Industries
            industries += " SELECT distinct";
            industries += " ss.label As 'SECTOR', ";
            industries += " ss.code As 'SuperSecteurId', ";
            industries += " s.label As 'INDUSTRY', ";
            industries += " s.code As 'SecteurId',";
            industries += " fac.MXEU as MXEU,";
            industries += " fac.MXEUM as MXEUM,";
            industries += " fac.MXEM as MXEM,";
            industries += " fac.MXUSLC as MXUSLC,";
            industries += " fac.SUIVI";
            industries += " into #industriestmp";
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
                industries += " and fac.MXUSLC is null ";
                industries += " AND s.class_name = 'FGA_EU'";
            }
            else
            {
                industries += " and fac.MXEU is null and fac.MXUSLC is not null";
                industries += " AND s.class_name = 'FGA_US'";
            }

            //
            industries += " SELECT distinct";
            industries += " SECTOR, ";
            industries += " SuperSecteurId, ";
            industries += " INDUSTRY, ";
            industries += " SecteurId,";
            industries += " SUM(MXEU) as MXEU,";
            industries += " SUM(MXEUM) as MXEUM,";
            industries += " SUM(MXEM) as MXEM,";
            industries += " SUM(MXUSLC) as MXUSLC,";
            industries += " SUIVI";
            industries += " into #industries";
            industries += " FROM #industriestmp ";
            industries += " GROUP BY SECTOR, SuperSecteurId, INDUSTRY, SecteurId, SUIVI";

            industries += " DROP TABLE #industriestmp";

            #endregion

            return industries;
        }

        /// <summary>
        /// Create a temp table #values
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public String GetTmpAllValuesInUnivers(String univers)
        {
            String values = "";

            values += " SELECT ";
            values += " distinct CONVERT (INT, fac.GARPN_QUINTILE_S) as Q, fac.COMPANY_NAME As 'COMPANY_NAME', ";
            values += " ss.label As 'SECTOR', ";
            values += " ss.code As 'SuperSecteurId', ";
            values += " fga.label As 'INDUSTRY', ";
            values += " fga.code As 'SecteurId', ";
            values += " fac.TICKER, ";
            values += " fac.ISIN, ";
            values += " fac.MXEU,";
            values += " fac.MXEUM,";
            values += " fac.MXEM,";
            values += " fac.MXUSLC,";
            values += " fac.LIQUIDITY_test as Liquidity,";
            values += " CASE WHEN fac.ESG is null THEN 'X' ELSE NULL END AS exclu,";
            values += " fac.COUNTRY, fac.CURRENCY, fac.SUIVI ";
            values += " INTO #values";
            values += " FROM DATA_FACTSET fac ";
            values += " INNER JOIN ref_security.SECTOR s on";
            values += " s.code = fac.SECTOR ";
            values += " INNER JOIN ref_security.SECTOR ss on";
            values += " ss.id = s.id_parent ";
            values += " INNER JOIN ref_security.SECTOR_TRANSCO st on";
            values += " st.id_sector1 = s.id ";
            values += " INNER JOIN ref_security.SECTOR fga on";
            values += " fga.id = st.id_sector2 ";
            values += " INNER JOIN DATA_factset fgafac on ";
            values += " fgafac.fga_sector = fga.code AND fgafac.DATE=fac.DATE";
            values += " WHERE ";
            values += " fgafac.gics_sector is null ";
            values += " AND fac.ISIN is not null ";
            values += " AND fac.date=(Select MAX(date) from DATA_FACTSET) ";
            values += " AND fga.label IN (SELECT INDUSTRY FROM #industries) ";
            if (univers == "Europe")
            {
                values += " and (fac.MXEU is not null OR fac.MXEUM is not null OR fac.MXEM is not null) and fac.MXUSLC is null ";
                values += " AND fga.class_name = 'FGA_EU'";
            }
            else
            {
                values += " and fac.MXEUM is null and fac.MXEU is null and fac.MXEM is null and fac.MXUSLC is not null";
                values += " AND fga.class_name = 'FGA_US'";
            }
            values += " ORDER BY fac.COMPANY_NAME";


            return values;
        }

        /// <summary>
        /// Create temp able with THE wanted industry
        /// </summary>
        /// <param name="univers"></param>
        /// <param name="industry"></param>
        /// <returns></returns>
        public String GetTmpIndustry(String univers, String industry)
        {
            String requete = "";

            requete += " SELECT distinct";
            requete += " ss.label As 'SECTOR', ";
            requete += " ss.code As 'SuperSecteurId', ";
            requete += " s.label As 'INDUSTRY', ";
            requete += " s.code As 'SecteurId',";
            requete += " fac.MXEU as MXEU,";
            requete += " fac.MXEUM as MXEUM,";
            requete += " fac.MXEM as MXEM,";
            requete += " fac.MXUSLC as MXUSLC,";
            requete += " fac.SUIVI";
            requete += " into #industriestmp";
            requete += " FROM ref_security.SECTOR s ";
            requete += " INNER JOIN ref_security.SECTOR_TRANSCO st on";
            requete += " st.id_sector1 = s.id ";
            requete += " INNER JOIN ref_security.SECTOR fils on ";
            requete += " fils.id = st.id_sector2 ";
            requete += " LEFT OUTER JOIN ref_security.SECTOR ss ON ";
            requete += " fils.id_parent = ss.id ";
            requete += " INNER JOIN DATA_FACTSET fac on ";
            requete += " fac.FGA_SECTOR = s.code ";
            requete += " WHERE fac.GICS_SECTOR is null ";
            requete += " AND ss.class_name = 'GICS' ";
            requete += " AND fac.DATE=(Select MAX(date) from DATA_FACTSET)";
            requete += " AND s.label = '" + industry + "'";
            if (univers == "Europe")
            {
                requete += " and fac.MXUSLC is null ";
                requete += " AND s.class_name = 'FGA_EU'";
            }
            else
            {
                requete += " and fac.MXEU is null and fac.MXUSLC is not null";
                requete += " AND s.class_name = 'FGA_US'";
            }

            requete += " SELECT distinct";
            requete += " SECTOR, ";
            requete += " SuperSecteurId, ";
            requete += " INDUSTRY, ";
            requete += " SecteurId,";
            requete += " SUM(MXEU) as MXEU,";
            requete += " SUM(MXEUM) as MXEUM,";
            requete += " SUM(MXEM) as MXEM,";
            requete += " SUM(MXUSLC) as MXUSLC,";
            requete += " SUIVI";
            requete += " into #industries";
            requete += " FROM #industriestmp ";
            requete += " GROUP BY SECTOR, SuperSecteurId, INDUSTRY, SecteurId, SUIVI";

            requete += " DROP TABLE #industriestmp";

            return requete;
        }

        /// <summary>
        /// Create a table with all industries in a sector in a univers
        /// </summary>
        /// <param name="univers"></param>
        /// <param name="sector"></param>
        /// <returns></returns>
        public String GetTmpAllIndustriesInSectorInUnivers(String univers, String sector)
        {
            String industries = "";

            industries += " SELECT distinct";
            industries += " ss.label As 'SECTOR', ";
            industries += " ss.code As 'SuperSecteurId', ";
            industries += " s.label As 'INDUSTRY', ";
            industries += " s.code As 'SecteurId',";
            industries += " fac.MXEU as MXEU,";
            industries += " fac.MXEUM as MXEUM,";
            industries += " fac.MXEM as MXEM,";
            industries += " fac.MXUSLC as MXUSLC,";
            industries += " fac.SUIVI";
            industries += " into #industriestmp";
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
            industries += " AND ss.label = '" + sector + "'";
            if (univers == "Europe")
            {
                industries += " and fac.MXUSLC is null ";
                industries += " AND s.class_name = 'FGA_EU'";
            }
            else
            {
                industries += " and fac.MXEU is null and fac.MXUSLC is not null";
                industries += " AND s.class_name = 'FGA_US'";
            }

            //
            industries += " SELECT distinct";
            industries += " SECTOR, ";
            industries += " SuperSecteurId, ";
            industries += " INDUSTRY, ";
            industries += " SecteurId,";
            industries += " SUM(MXEU) as MXEU,";
            industries += " SUM(MXEUM) as MXEUM,";
            industries += " SUM(MXEM) as MXEM,";
            industries += " SUM(MXUSLC) as MXUSLC,";
            industries += " SUIVI";
            industries += " into #industries";
            industries += " FROM #industriestmp ";
            industries += " GROUP BY SECTOR, SuperSecteurId, INDUSTRY, SecteurId, SUIVI";

            industries += " DROP TABLE #industriestmp";
            return industries;
        }

        /// <summary>
        /// Create the final table for all sectors in univers
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public String GetAllSectorsInUnivers(String univers)
        {
            String secteurs = "";

            secteurs += " Select ";
            secteurs += " distinct s.label as Label_Secteur,";
            secteurs += " NULL AS '##',";
            secteurs += " NULL as Label_Industry,";
            secteurs += " NULL as TICKER,";
            secteurs += " NULL as Company_Name,";
            secteurs += " NULL AS Dev,";
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
            secteurs += " NULL as ID_FGA,";
            secteurs += " NULL as Pays";
            secteurs += " from ACT_RECO_SECTOR as rs";
            secteurs += " right join #sectors as s";
            secteurs += " on rs.id_secteur = s.code AND rs.date = (SELECT MAX(date) from ACT_RECO_SECTOR as rv where rv.id_secteur = s.code)";
            if (univers == "Europe")
                secteurs += " AND (rs.reco_MXUSLC IS NULL OR rs.reco_MXUSLC = '')";
            else
                secteurs += " AND rs.reco_MXUSLC IS NOT NULL AND rs.reco_MXUSLC <> '' AND (rs.recommandation IS NULL OR rs.recommandation = '')";

            return secteurs;
        }

        /// <summary>
        /// Create the final table of all industries in univers
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public String GetAllIndustriesInUnivers(String univers)
        {
            String industries = "";

            industries += " Select";
            industries += " distinct s.SECTOR  as Label_Secteur, ";
            industries += " s.SUIVI AS '##',";
            industries += " s.INDUSTRY as Label_Industry,";
            industries += " NULL as TICKER,";
            industries += " NULL as Company_Name,";
            industries += " NULL AS Dev,";
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
            industries += " Cast(s.SecteurId as VARCHAR(32)) as ID_FGA,";
            industries += " NULL as Pays";
            industries += " from ACT_RECO_SECTOR as rs";
            industries += " right join #industries as s";
            industries += " on rs.id_secteur = s.SecteurId AND rs.date = (SELECT MAX(date) from ACT_RECO_SECTOR as rv where rv.id_secteur = s.SecteurId)";
            if (univers == "Europe")
                industries += " AND (rs.reco_MXUSLC IS NULL OR rs.reco_MXUSLC = '')";
            else
                industries += " AND rs.reco_MXUSLC IS NOT NULL AND rs.reco_MXUSLC <> ''";

            return industries;
        }

        /// <summary>
        /// Create final table with All values in univers with a reco
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public String GetAllValuesWithRecoInUnivers(String univers)
        {
            String values = "";
            values += " Select";
            values += " distinct s.SECTOR as Label_Secteur, ";
            values += " fgafac.SUIVI as '##',";
            values += " s.INDUSTRY as Label_Industry,";
            values += " s.TICKER as TICKER,";
            values += " s.COMPANY_NAME as Company_Name,";
            values += " s.CURRENCY as Dev,";
            values += " CONVERT (INT, s.Q) as Q,";
            values += " CONVERT(DATE, rs.date ,103) as Date,";
            if (univers == "Europe")
            {
                values += " rs.reco_SXXE as RecoMXEM,";
                values += " s.MXEM as PoidsMXEM,";
                values += " rs.reco_SXXP as RecoMXEU,";
                values += " s.MXEU as PoidsMXEU,";
                values += " rs.reco_SXXA as RecoMXEUM,";
                values += " s.MXEUM as PoidsMXEUM,";
            }
            else
            {
                values += " rs.reco_MXUSLC as RecoMXUSLC,";
                values += " s.MXUSLC as PoidsMXUSLC,";
            }
            values += " s.liquidity as Liquidity,";
            values += " s.exclu as Exclu,";
            values += " s.ISIN as ISIN,";
            values += " Cast(s.SuperSecteurId as VARCHAR(32)) as ID_ICB,";
            values += " Cast(s.SecteurId as VARCHAR(32)) as ID_FGA,";
            values += " s.COUNTRY as Pays ";
            values += " from #values as s";
            values += " inner join DATA_FACTSET fgafac on ";
            values += " s.SecteurId = fgafac.FGA_SECTOR AND fgafac.GICS_SUBINDUSTRY IS NULL AND fgafac.TICKER IS NULL AND fgafac.DATE = (SELECT MAX(DATE) FROM DATA_FACTSET) AND fgafac.SUIVI IS NOT NULL";
            values += " inner join ACT_RECO_VALEUR as rs on";
            values += " rs.ISIN = s.ISIN ";
            values += " AND rs.date = (SELECT MAX(date) from ACT_RECO_VALEUR as rv where rv.ISIN = s.ISIN)";
            if (univers == "Europe")
                values += " AND ((rs.reco_SXXP IS NOT NULL AND rs.reco_SXXP <> '') OR (rs.reco_SXXE IS NOT NULL AND rs.reco_SXXE <> '') OR (rs.reco_SXXA IS NOT NULL AND rs.reco_SXXA <> ''))";
            else
                values += " AND rs.reco_MXUSLC IS NOT NULL AND rs.reco_MXUSLC <> ''";

            return values;
        }

        /// <summary>
        /// Create final table with All values in univers pe industry
        /// </summary>
        /// <param name="univers"></param>
        /// <returns></returns>
        public String GetAllValuesPerIndustry(String univers)
        {
            String values = "";
            values += " Select";
            values += " distinct s.SECTOR as Label_Secteur, ";
            values += " fgafac.SUIVI as '##',";
            values += " s.INDUSTRY as Label_Industry,";
            values += " s.TICKER as TICKER,";
            values += " s.COMPANY_NAME as Company_Name,";
            values += " s.CURRENCY as Dev,";
            values += " CONVERT (INT, s.Q) as Q,";
            values += " CONVERT(DATE, rs.date ,103) as Date,";
            if (univers == "Europe")
            {
                values += " rs.reco_SXXE as RecoMXEM,";
                values += " s.MXEM as PoidsMXEM,";
                values += " rs.reco_SXXP as RecoMXEU,";
                values += " s.MXEU as PoidsMXEU,";
                values += " rs.reco_SXXA as RecoMXEUM,";
                values += " s.MXEUM as PoidsMXEUM,";
            }
            else
            {
                values += " rs.reco_MXUSLC as RecoMXUSLC,";
                values += " s.MXUSLC as PoidsMXUSLC,";
            }
            values += " s.liquidity as Liquidity,";
            values += " s.exclu as Exclu,";
            values += " s.ISIN as ISIN,";
            values += " Cast(s.SuperSecteurId as VARCHAR(32)) as ID_ICB,";
            values += " Cast(s.SecteurId as VARCHAR(32)) as ID_FGA,";
            values += " s.COUNTRY as Pays ";
            values += " from ACT_RECO_VALEUR as rs ";            
            values += " right join #values as s on";
            values += " rs.ISIN = s.ISIN ";
            values += " AND rs.date = (SELECT MAX(date) from ACT_RECO_VALEUR as rv where rv.ISIN = s.ISIN)";
            values += " inner join DATA_FACTSET fgafac on ";
            values += " s.SecteurId = fgafac.FGA_SECTOR AND fgafac.GICS_SUBINDUSTRY IS NULL AND fgafac.TICKER IS NULL AND fgafac.DATE = (SELECT MAX(DATE) FROM DATA_FACTSET) AND fgafac.SUIVI IS NOT NULL";
            
            return values;
        }
        
        #endregion

        public void UpdateRecoValue(int id, String newDate,
            String newRecoMXEU, String newRecoMXEUM, String newRecoMXEM, String newRecoMXUSLC)
        {
            String sql = "UPDATE ACT_RECO_VALEUR ";
            sql += " SET date = '" + newDate.Substring(0, 10) + "', ";
            sql += " reco_SXXP = '" + newRecoMXEU + "', ";
            sql += " reco_SXXE = '" + newRecoMXEM + "', ";
            sql += " reco_SXXA = '" + newRecoMXEUM + "', ";
            sql += " reco_MXUSLC = '" + newRecoMXUSLC + "' ";
            sql += " WHERE id_valeur = " + id;

            connection.RequeteSql(sql);
        }

        public void UpdateRecoSector(String prevDate, String prevType, int id,
            String newDate, String newRecoMXEU, String newRecoMXEUM, String newRecoMXEM, String newRecoMXUSLC)
        {
            String sql = "UPDATE ACT_RECO_SECTOR ";
            sql += " SET date = '" + newDate.Substring(0, 10) + "', ";
            sql += " reco_MXEUM = '" + newRecoMXEUM + "', ";
            sql += " reco_MXEM = '" + newRecoMXEM + "', ";
            sql += " recommandation = '" + newRecoMXEU + "', ";
            sql += " reco_MXUSLC = '" + newRecoMXUSLC + "' ";
            sql += " WHERE date = '" + prevDate.Substring(0, 10) + "' AND type = '" + prevType + "' AND id_secteur = " + id;

            connection.RequeteSql(sql);
        }

        public void DeleteRecoValeur(int id_valeur, int id_comment, int id_comment_change)
        {
            String sql = " DELETE FROM ACT_RECO_VALEUR WHERE id_valeur =" + id_valeur;
            sql += " DELETE FROM ACT_RECO_COMMENT WHERE id =" + id_comment_change;
            sql += " DELETE FROM ACT_RECO_COMMENT WHERE id =" + id_comment;

            connection.RequeteSql(sql);
        }

        public void DeleteRecoSector(String prevDate, String prevType, int id_sector,
            int id_comment, int id_comment_change)
        {
            String sql = " DELETE FROM ACT_RECO_SECTOR WHERE date = '" + prevDate.Substring(0, 10) + "' AND type = '" + prevType + "' AND id_secteur = " + id_sector;
            sql += " DELETE FROM ACT_RECO_COMMENT WHERE id =" + id_comment_change;
            sql += " DELETE FROM ACT_RECO_COMMENT WHERE id =" + id_comment;

            connection.RequeteSql(sql);
        }

        public String GetRecoTextVal(String ticker, String date)
        {
            String isin = connection.GetISINFromTicker(ticker);
            String id_comment = connection.SelectDistinctWheres("ACT_RECO_VALEUR", "id_comment",
                new List<String> { "ISIN", "date" },
                new List<object> { isin, date }).First().ToString();

            String text = connection.SelectDistinctWheres("ACT_RECO_COMMENT", "comment",
                 new List<String> { "id" },
                 new List<object> { id_comment }).First().ToString();

            return text;
        }

        public String GetRecoTextSec(String secteurlable, String date, bool isGICS)
        {
            String class_name = isGICS ? "GICS" : "FGA_ALL";
            String sql = "SELECT code FROM [ref_security].[SECTOR] WHERE label='" +
                secteurlable + "' AND class_name='" + class_name + "'";
            String id_secteur = connection.SqlWithReturn(sql).First().ToString();

            String id_comment = connection.SelectDistinctWheres("ACT_RECO_SECTOR", "id_comment",
                new List<String> { "id_secteur", "date" },
                new List<object> { id_secteur, date }).First().ToString();

            String text = connection.SelectDistinctWheres("ACT_RECO_COMMENT", "comment",
                 new List<String> { "id" },
                 new List<object> { id_comment }).First().ToString();

            return text;
        }

    }
}
