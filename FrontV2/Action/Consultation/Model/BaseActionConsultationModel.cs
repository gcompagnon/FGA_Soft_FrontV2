using FrontV2.Action.Consultation.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using Telerik.Windows.Data;

namespace FrontV2.Action.Consultation.Model
{
    class BaseActionConsultationModel
    {
        #region Fields

        private readonly Connection co = new Connection();

        private readonly String ALLSUPERSECTORS = "**Sectors GICS**";
        private readonly String ALLSECTORS = "**Industries FGA**";

        private DataTable _tableTemp;

        #endregion

        #region Constructor

        public BaseActionConsultationModel()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fill a tmp datatable returned so taht you can use it to fill your own
        /// </summary>
        /// <param name="table">Original table to match the columns</param>
        /// <param name="date">Selected Date</param>
        /// <param name="univers">Selected Univer</param>
        /// <param name="superSector">Selected GICS Sector</param>
        /// <param name="sector">Selected IndustryFGA</param>
        /// <param name="tab">Tab for which you want to fill the DataTable</param>
        /// <returns>Return a DataTable corresponding to sql result for your given parameters</returns>
        public DataTable FillTableWithData(DataTable table, String date, String univers,
            Sector superSector, Sector sector, String tab)
        {
            _tableTemp = new DataTable();
            for (int i = 0; i < table.Columns.Count; i++)
                _tableTemp.Columns.Add(table.Columns[i].ToString());

            if (date == null || univers.CompareTo("") == 0)
                return _tableTemp;

            if (superSector == null || superSector.Libelle.CompareTo("") == 0)
            {
                if (sector == null || sector.Libelle.CompareTo("") == 0)
                {
                    FindSuperSectors(date, univers, tab);
                    FindSectors(date, univers, tab);
                    FindAllData(date, univers, tab);
                    return _tableTemp;
                }
            }

            if (superSector.Libelle.CompareTo(ALLSUPERSECTORS) == 0)
            {
                FindSuperSectors(date, univers, tab);
                if (sector != null && sector.Libelle.CompareTo(ALLSECTORS) == 0)
                    FindSectors(date, univers, tab);
            }
            FindGeneralData(date, univers, superSector, sector, tab);
            FindAllValues(date, univers, superSector, sector, tab);

            return _tableTemp;
        }

        /// <summary>
        /// Inner fonction called when SuperSector and Sectors are null
        /// </summary>
        /// <param name="date">Selected Date</param>
        /// <param name="univers">Selected univers</param>
        /// <param name="tab">Tab for which you want to fill the DataTable</param>
        public void FindAllData(String date, String univers, String tab)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT distinct fac.COMPANY_NAME As 'COMPANY_NAME',");
            sql.Append(" ss.label As 'SECTOR',");
            sql.Append(" ss.code As 'SuperSecteurId',");
            sql.Append(" fga.label As 'INDUSTRY',");
            sql.Append(" fga.code As 'SecteurId',");
            sql.Append(" fgafac.SUIVI,");
            sql.Append(" fac.TICKER,");
            sql.Append(" fac.ISIN,");
            if (tab.CompareTo("Gen") == 0)
                sql.Append(BaseActionConsultationViewModel.GeneralAgr.ToString());
            else if (tab.CompareTo("Cro") == 0)
                sql.Append(BaseActionConsultationViewModel.CroissanceAgr.ToString());
            else if (tab.CompareTo("Qua") == 0)
                sql.Append(BaseActionConsultationViewModel.QualityAgr.ToString());
            else if (tab.CompareTo("Val") == 0)
                sql.Append(BaseActionConsultationViewModel.ValorisationAgr.ToString());
            else if (tab.CompareTo("Mom") == 0)
                sql.Append(BaseActionConsultationViewModel.MomentumAgr.ToString());
            else if (tab.CompareTo("Syn") == 0)
                sql.Append(BaseActionConsultationViewModel.SyntheseAgr.ToString());
            sql.Append(" FROM DATA_FACTSET fac");
            sql.Append(" INNER JOIN ref_security.SECTOR s on s.code = fac.SECTOR");
            sql.Append(" INNER JOIN ref_security.SECTOR ss on ss.id = s.id_parent");
            sql.Append(" INNER JOIN ref_security.SECTOR_TRANSCO st on st.id_sector1 = s.id");
            sql.Append(" INNER JOIN ref_security.SECTOR fga on fga.id = st.id_sector2");
            sql.Append(" INNER JOIN DATA_factset fgafac on fgafac.fga_sector = fga.code AND fgafac.DATE=fac.DATE");
            sql.Append(" WHERE fgafac.gics_sector is null AND fac.ISIN is not null AND fac.date='" + date + "' AND fga.class_name='FGA_ALL'");

            if (univers.CompareTo("ALL") == 0)
            { }
            else if (univers.CompareTo("EUROPE") == 0)
                sql.Append(" AND fac.MXEU is not null");
            else if (univers.CompareTo("USA") == 0)
                sql.Append(" AND fac.MXUSLC is not null");
            else if (univers.CompareTo("EMU") == 0)
                sql.Append(" AND fac.MXEM is not null");
            else if (univers.CompareTo("EUROPE EX EMU") == 0)
                sql.Append(" AND fac.MXEUM is not null");
            else if (univers.CompareTo("FRANCE") == 0)
                sql.Append(" AND fac.MXFR is not null");
            else if (univers.CompareTo("FEDERIS ACTIONS") == 0)
                sql.Append(" AND fac.[6100001] is not null");
            else if (univers.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
                sql.Append(" AND fac.[6100002] is not null");
            else if (univers.CompareTo("FEDERIS ISR EURO") == 0)
                sql.Append(" AND fac.[6100004] is not null");
            else if (univers.CompareTo("FEDERIS NORTH AMERICA") == 0)
                sql.Append(" AND fac.[6100024] is not null");
            else if (univers.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
                sql.Append(" AND fac.[6100026] is not null");
            else if (univers.CompareTo("FEDERIS EURO ACTIONS") == 0)
                sql.Append(" AND fac.[6100030] is not null");
            else if (univers.CompareTo("FEDERIS IRC ACTIONS") == 0)
                sql.Append(" AND fac.[6100033] is not null");
            else if (univers.CompareTo("FEDERIS EX EURO") == 0)
                sql.Append(" AND fac.[6100062] is not null");
            else if (univers.CompareTo("FEDERIS CROISSANCE EURO") == 0)
                sql.Append(" AND fac.[6100063] is not null");
            else if (univers.CompareTo("AVENIR EURO") == 0)
                sql.Append(" AND fac.AVEURO is not null");
            else if (univers.CompareTo("FEDERIS VALUE EURO") == 0)
                sql.Append(" AND fac.AVEUROPE is not null");
            else if (univers.CompareTo("FEDERIS ISR AMERIQUE") == 0)
                sql.Append(" AND fac.[6100066] is not null");
            else
                return;
            sql.Append(" ORDER BY fac.COMPANY_NAME");

            sqlToTable(sql.ToString());
        }

        /// <summary>
        /// Inner function called to find all values for database (1st part)
        /// </summary>
        /// <param name="date">Selected Date</param>
        /// <param name="univers">Selected Univers</param>
        /// <param name="superSector">Selected Gics Sector</param>
        /// <param name="sector">Selected Industry FGA</param>
        /// <param name="tab">Tab for which you want to fill the DataTable</param>
        public void FindGeneralData(String date, String univers, Sector superSector, Sector sector, String tab)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ss.label As 'SECTOR', ss.code As 'SuperSecteurId',");
            sql.Append(" fga.label As 'INDUSTRY', fga.code As 'SecteurId',");
            //sql.Append(Me.criteres)  Je n'ai pas trouve l'utilite de cette ligne, critere etait tjr egal a ""
            sql.Append(" fac.SUIVI,");
            sql.Append(" fac.TICKER,");
            if (tab.CompareTo("Gen") == 0)
            {
                sql.Append(" fac.ISIN,");
                sql.Append(BaseActionConsultationViewModel.GeneralAgr.ToString());
            }
            else if (tab.CompareTo("Cro") == 0)
                sql.Append(BaseActionConsultationViewModel.CroissanceAgr.ToString());
            else if (tab.CompareTo("Qua") == 0)
                sql.Append(BaseActionConsultationViewModel.QualityAgr.ToString());
            else if (tab.CompareTo("Val") == 0)
                sql.Append(BaseActionConsultationViewModel.ValorisationAgr.ToString());
            else if (tab.CompareTo("Mom") == 0)
                sql.Append(BaseActionConsultationViewModel.MomentumAgr.ToString());
            else if (tab.CompareTo("Syn") == 0)
                sql.Append(BaseActionConsultationViewModel.SyntheseAgr.ToString());

            sql.Append(" FROM ref_security.SECTOR ss");
            sql.Append(" INNER JOIN ref_security.SECTOR s ON s.id_parent = ss.id");
            sql.Append(" INNER JOIN ref_security.SECTOR_TRANSCO st on st.id_sector1 = s.id");
            sql.Append(" INNER JOIN ref_security.SECTOR fga on fga.id = st.id_sector2");
            sql.Append(" INNER JOIN DATA_FACTSET fac on fac.FGA_SECTOR = fga.code");
            sql.Append(" WHERE ss.code = " + superSector.Id.ToString());
            sql.Append(" AND fac.GICS_SECTOR is null AND fac.DATE='" + date + "'");

            if (sector != null && sector.Id != -1)
            {
                sql.Append(" AND fga.code = " + sector.Id.ToString());
            }
            sql.Append(" AND s.class_name = 'GICS'");
            if (univers.CompareTo("ALL") == 0)
                sql.Append(" AND fga.class_name = 'FGA_ALL' AND fac.MXEU is not null and fac.MXUSLC is not null");
            else if (univers.CompareTo("EUROPE") == 0)
                sql.Append(" AND fga.class_name = 'FGA_EU' AND fac.MXEU is not null and fac.MXUSLC is null");
            else if (univers.CompareTo("USA") == 0)
                sql.Append(" AND fga.class_name = 'FGA_US' AND fac.MXEU is null and fac.MXUSLC is not null");
            else if (univers.CompareTo("EMU") == 0)
                sql.Append(" AND fac.MXEM is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("EUROPE EX EMU") == 0)
                sql.Append(" AND fac.MXEUM is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FRANCE") == 0)
                sql.Append(" AND fac.MXFR is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS ACTIONS") == 0)
                sql.Append(" AND fac.[6100001] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
                sql.Append(" AND fac.[6100002] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS ISR EURO") == 0)
                sql.Append(" AND fac.[6100004] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS NORTH AMERICA") == 0)
                sql.Append(" AND fac.[6100024] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
                sql.Append(" AND fac.[6100026] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS EURO ACTIONS") == 0)
                sql.Append(" AND fac.[6100030] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS IRC ACTIONS") == 0)
                sql.Append(" AND fac.[6100033] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS EX EURO") == 0)
                sql.Append(" AND fac.[6100062] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS CROISSANCE EURO") == 0)
                sql.Append(" AND fac.[6100063] is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("AVENIR EURO") == 0)
                sql.Append(" AND fac.AVEURO is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS VALUE EURO") == 0)
                sql.Append(" AND fac.AVEUROPE is not null AND fga.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS ISR AMERIQUE") == 0)
                sql.Append(" AND fac.[6100066] is not null AND fga.class_name = 'FGA_ALL'");
            else
                return;

            sql.Append(" UNION");
            sql.Append(" SELECT ss.label As 'SECTOR', ss.code As 'SuperSecteurId', null As 'INDUSTRY', null As 'SecteurId',");
            //sql.Append(Me.criteres);
            sql.Append(" fac.SUIVI,");
            sql.Append(" fac.TICKER,");
            if (tab.CompareTo("Gen") == 0)
            {
                sql.Append(" fac.ISIN,");
                sql.Append(BaseActionConsultationViewModel.GeneralAgr.ToString());
            }
            else if (tab.CompareTo("Cro") == 0)
                sql.Append(BaseActionConsultationViewModel.CroissanceAgr.ToString());
            else if (tab.CompareTo("Qua") == 0)
                sql.Append(BaseActionConsultationViewModel.QualityAgr.ToString());
            else if (tab.CompareTo("Val") == 0)
                sql.Append(BaseActionConsultationViewModel.ValorisationAgr.ToString());
            else if (tab.CompareTo("Mom") == 0)
                sql.Append(BaseActionConsultationViewModel.MomentumAgr.ToString());
            else if (tab.CompareTo("Syn") == 0)
                sql.Append(BaseActionConsultationViewModel.SyntheseAgr.ToString());

            sql.Append(" FROM ref_security.SECTOR ss");
            sql.Append(" INNER JOIN DATA_FACTSET fac on fac.GICS_SECTOR = ss.code");
            sql.Append(" WHERE ss.code = " + superSector.Id.ToString());
            sql.Append(" AND fac.GICS_SUBINDUSTRY is null AND fac.DATE='" + date + "'");
            if (univers.CompareTo("ALL") == 0)
                sql.Append(" AND fac.MXEU is not null AND fac.MXUSLC is not null");
            else if (univers.CompareTo("EUROPE") == 0)
                sql.Append(" AND fac.MXEU is not null AND fac.MXUSLC is null");
            else if (univers.CompareTo("USA") == 0)
                sql.Append(" AND fac.MXEU is null AND fac.MXUSLC is not null");
            else if (univers.CompareTo("EMU") == 0)
                sql.Append(" AND fac.MXEM is not null");
            else if (univers.CompareTo("EUROPE EX EMU") == 0)
                sql.Append(" AND fac.MXEUM is not null");
            else if (univers.CompareTo("FRANCE") == 0)
                sql.Append(" AND fac.MXFR is not null");
            else if (univers.CompareTo("FEDERIS ACTIONS") == 0)
                sql.Append(" AND fac.[6100001] is not null");
            else if (univers.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
                sql.Append(" AND fac.[6100002] is not null");
            else if (univers.CompareTo("FEDERIS ISR EURO") == 0)
                sql.Append(" AND fac.[6100004] is not null");
            else if (univers.CompareTo("FEDERIS NORTH AMERICA") == 0)
                sql.Append(" AND fac.[6100024] is not null");
            else if (univers.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
                sql.Append(" AND fac.[6100026] is not null");
            else if (univers.CompareTo("FEDERIS EURO ACTIONS") == 0)
                sql.Append(" AND fac.[6100030] is not null");
            else if (univers.CompareTo("FEDERIS IRC ACTIONS") == 0)
                sql.Append(" AND fac.[6100033] is not null");
            else if (univers.CompareTo("FEDERIS EX EURO") == 0)
                sql.Append(" AND fac.[6100062] is not null");
            else if (univers.CompareTo("FEDERIS CROISSANCE EURO") == 0)
                sql.Append(" AND fac.[6100063] is not null");
            else if (univers.CompareTo("AVENIR EURO") == 0)
                sql.Append(" AND fac.AVEURO is not null");
            else if (univers.CompareTo("FEDERIS VALUE EURO") == 0)
                sql.Append(" AND fac.AVEUROPE is not null");
            else if (univers.CompareTo("FEDERIS ISR AMERIQUE") == 0)
                sql.Append(" AND fac.[6100066] is not null");
            else
                return;

            sqlToTable(sql.ToString());
        }

        /// <summary>
        /// Inner function called to find all values for database (2nd part)
        /// </summary>
        /// <param name="mDate">Selected Date</param>
        /// <param name="univers">Selected Univers</param>
        /// <param name="superSector">Selected Sector GICS</param>
        /// <param name="sector">Selected Industry FGA</param>
        /// <param name="tab">Tab for which you want to fill the DataTable</param>
        public void FindAllValues(string mDate, string univers, Sector superSector, Sector sector, string tab)
        {
            StringBuilder sql = new StringBuilder();

            if (sector == null || sector.Id == -1)
            {
                sql.Append("SELECT distinct fac.COMPANY_NAME As 'COMPANY_NAME',");
                //sql.Append(this.criteres);
                sql.Append(" (select label from ref_security.SECTOR where code = " + superSector.Id.ToString() + ") As 'SECTOR',");
                sql.Append(" " + superSector.Id.ToString() + " As 'SuperSecteurId',");
                sql.Append(" fga.label As 'INDUSTRY',");
                sql.Append(" fga.code As 'SecteurId',");
                sql.Append(" fgafac.SUIVI,");
                sql.Append(" fac.TICKER,");
                sql.Append(" fac.ISIN,");

                if (tab.CompareTo("Gen") == 0)
                    sql.Append(BaseActionConsultationViewModel.GeneralAgr.ToString());
                else if (tab.CompareTo("Cro") == 0)
                    sql.Append(BaseActionConsultationViewModel.CroissanceAgr.ToString());
                else if (tab.CompareTo("Qua") == 0)
                    sql.Append(BaseActionConsultationViewModel.QualityAgr.ToString());
                else if (tab.CompareTo("Val") == 0)
                    sql.Append(BaseActionConsultationViewModel.ValorisationAgr.ToString());
                else if (tab.CompareTo("Mom") == 0)
                    sql.Append(BaseActionConsultationViewModel.MomentumAgr.ToString());
                else if (tab.CompareTo("Syn") == 0)
                    sql.Append(BaseActionConsultationViewModel.SyntheseAgr.ToString());
                sql.Append(" FROM ref_security.SECTOR s");

                if (sector == null || sector.Id == -1)
                {
                    sql.Append(" INNER JOIN ref_security.SECTOR_TRANSCO st on st.id_sector1 = s.id");
                    sql.Append(" INNER JOIN ref_security.SECTOR fga on fga.id = st.id_sector2");
                }

                //sql.Append(" INNER JOIN ref_security.ASSET_TO_SECTOR assTOsec ON assTOsec.id_sector = s.id")
                //sql.Append(" INNER JOIN ref_security.ASSET ass ON ass.Id = assTOsec.id_asset and ass.MaturityDate=assTOsec.date")
                sql.Append(" INNER JOIN DATA_FACTSET fac ON fac.sector = s.code");
                //sql.Append(" INNER JOIN DATA_FACTSET fac ON fac.ISIN = ass.ISIN")
                sql.Append(" INNER JOIN DATA_factset fgafac on fgafac.fga_sector = fga.code AND fgafac.DATE=fac.DATE");
                if (sector == null || sector.Id == -1)
                {
                    sql.Append(" WHERE fgafac.gics_sector is null AND fac.DATE='" + mDate + "' AND s.id_parent = (select id from ref_security.SECTOR where code = " + superSector.Id.ToString() + ")");
                }
                else
                {
                    sql.Append(" WHERE fgafac.gics_sector is null AND fac.DATE='" + mDate + "' AND s.code = " + sector.Id.ToString());
                }
            }
            else
            {
                sql.Append("SELECT distinct fac.COMPANY_NAME As 'COMPANY_NAME',");
                //sql.Append(this.criteres);
                sql.Append(" (select label from ref_security.SECTOR where code = " + superSector.Id.ToString() + ") As 'SECTOR',");
                sql.Append(" " + superSector.Id.ToString() + " As 'SuperSecteurId',");
                sql.Append(" fga.label As 'INDUSTRY',");
                sql.Append(" fga.code As 'SecteurId',");
                sql.Append(" fgafac.SUIVI,");
                sql.Append(" fac.TICKER,");
                sql.Append(" fac.ISIN,");

                if (tab.CompareTo("Gen") == 0)
                    sql.Append(BaseActionConsultationViewModel.GeneralAgr.ToString());
                else if (tab.CompareTo("Cro") == 0)
                    sql.Append(BaseActionConsultationViewModel.CroissanceAgr.ToString());
                else if (tab.CompareTo("Qua") == 0)
                    sql.Append(BaseActionConsultationViewModel.QualityAgr.ToString());
                else if (tab.CompareTo("Val") == 0)
                    sql.Append(BaseActionConsultationViewModel.ValorisationAgr.ToString());
                else if (tab.CompareTo("Mom") == 0)
                    sql.Append(BaseActionConsultationViewModel.MomentumAgr.ToString());
                else if (tab.CompareTo("Syn") == 0)
                    sql.Append(BaseActionConsultationViewModel.SyntheseAgr.ToString());

                sql.Append(" FROM ref_security.SECTOR fga");
                sql.Append(" INNER JOIN ref_security.SECTOR_TRANSCO st on st.id_sector1 = fga.id");
                sql.Append(" INNER JOIN ref_security.SECTOR s on s.id = st.id_sector2");
                //sql.Append(" INNER JOIN ref_security.ASSET_TO_SECTOR assTOsec ON assTOsec.id_sector = s.id")
                sql.Append(" INNER JOIN DATA_FACTSET fac ON fac.sector = s.code");
                sql.Append(" INNER JOIN DATA_factset fgafac on fgafac.fga_sector = fga.code AND fgafac.DATE=fac.DATE");
                sql.Append(" WHERE fgafac.gics_sector is null AND fac.DATE='" + mDate + "' AND fga.code = " + sector.Id.ToString());
            }
            if (univers.CompareTo("ALL") == 0)
            { }
            else if (univers.CompareTo("EUROPE") == 0)
                sql.Append(" AND fac.MXEU is not null");
            else if (univers.CompareTo("USA") == 0)
                sql.Append(" AND fac.MXUSLC is not null");
            else if (univers.CompareTo("EMU") == 0)
                sql.Append(" AND fac.MXEM is not null");
            else if (univers.CompareTo("EUROPE EX EMU") == 0)
                sql.Append(" AND fac.MXEUM is not null");
            else if (univers.CompareTo("FRANCE") == 0)
                sql.Append(" AND fac.MXFR is not null");
            else if (univers.CompareTo("FEDERIS ACTIONS") == 0)
                sql.Append(" AND fac.[6100001] is not null");
            else if (univers.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
                sql.Append(" AND fac.[6100002] is not null");
            else if (univers.CompareTo("FEDERIS ISR EURO") == 0)
                sql.Append(" AND fac.[6100004] is not null");
            else if (univers.CompareTo("FEDERIS NORTH AMERICA") == 0)
                sql.Append(" AND fac.[6100024] is not null");
            else if (univers.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
                sql.Append(" AND fac.[6100026] is not null");
            else if (univers.CompareTo("FEDERIS EURO ACTIONS") == 0)
                sql.Append(" AND fac.[6100030] is not null");
            else if (univers.CompareTo("FEDERIS IRC ACTIONS") == 0)
                sql.Append(" AND fac.[6100033] is not null");
            else if (univers.CompareTo("FEDERIS EX EURO") == 0)
                sql.Append(" AND fac.[6100062] is not null");
            else if (univers.CompareTo("FEDERIS CROISSANCE EURO") == 0)
                sql.Append(" AND fac.[6100063] is not null");
            else if (univers.CompareTo("AVENIR EURO") == 0)
                sql.Append(" AND fac.AVEURO is not null");
            else if (univers.CompareTo("FEDERIS VALUE EURO") == 0)
                sql.Append(" AND fac.AVEUROPE is not null");
            else if (univers.CompareTo("FEDERIS ISR AMERIQUE") == 0)
                sql.Append(" AND fac.[6100066] is not null");
            else
                return;
            sql.Append(" ORDER BY fac.COMPANY_NAME");

            sqlToTable(sql.ToString());
        }

        /// <summary>
        /// Find all super sectors to add to temp DataTable used for organizing daa in DataGrid
        /// </summary>
        /// <param name="date">Selected Date</param>
        /// <param name="univers">Selected Univers</param>
        /// <param name="tab">Tab for which you want to fill the DataTable</param></param>
        public void FindSuperSectors(String date, String univers, String tab)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ss.label As 'SECTOR', ss.code As 'SuperSecteurId',");
            //sql.Append(this.criteres);
            sql.Append(" fac.SUIVI,");
            if (tab == "Gen")
            {
                sql.Append(" fac.ISIN,");
                sql.Append(BaseActionConsultationViewModel.GeneralAgr.ToString());
            }
            else if (tab == "Cro")
                sql.Append(BaseActionConsultationViewModel.CroissanceAgr.ToString());
            else if (tab == "Qua")
                sql.Append(BaseActionConsultationViewModel.QualityAgr.ToString());
            else if (tab == "Val")
                sql.Append(BaseActionConsultationViewModel.ValorisationAgr.ToString());
            else if (tab == "Mom")
                sql.Append(BaseActionConsultationViewModel.MomentumAgr.ToString());
            else if (tab == "Syn")
                sql.Append(BaseActionConsultationViewModel.SyntheseAgr.ToString());
            sql.Append(" FROM ref_security.SECTOR ss");
            sql.Append(" INNER JOIN DATA_FACTSET fac on fac.GICS_SECTOR = ss.code");
            sql.Append(" WHERE [level] = 0");
            sql.Append(" AND ss.class_name = 'GICS' AND fac.GICS_SUBINDUSTRY is null AND fac.DATE='" + date + "'");
            if (univers.CompareTo("ALL") == 0)
                sql.Append(" AND fac.MXEU is not null AND fac.MXUSLC is not null");
            else if (univers.CompareTo("EUROPE") == 0)
                sql.Append(" AND fac.MXEU is not null AND fac.MXUSLC is null");
            else if (univers.CompareTo("USA") == 0)
                sql.Append(" AND fac.MXEU is null AND fac.MXUSLC is not null");
            else if (univers.CompareTo("EMU") == 0)
                sql.Append(" AND fac.MXEM is not null");
            else if (univers.CompareTo("EUROPE EX EMU") == 0)
                sql.Append(" AND fac.MXEUM is not null");
            else if (univers.CompareTo("FRANCE") == 0)
                sql.Append(" AND fac.MXFR is not null");
            else if (univers.CompareTo("FEDERIS ACTIONS") == 0)
                sql.Append(" AND fac.[6100001] is not null");
            else if (univers.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
                sql.Append(" AND fac.[6100002] is not null");
            else if (univers.CompareTo("FEDERIS ISR EURO") == 0)
                sql.Append(" AND fac.[6100004] is not null");
            else if (univers.CompareTo("FEDERIS NORTH AMERICA") == 0)
                sql.Append(" AND fac.[6100024] is not null");
            else if (univers.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
                sql.Append(" AND fac.[6100026] is not null");
            else if (univers.CompareTo("FEDERIS EURO ACTIONS") == 0)
                sql.Append(" AND fac.[6100030] is not null");
            else if (univers.CompareTo("FEDERIS IRC ACTIONS") == 0)
                sql.Append(" AND fac.[6100033] is not null");
            else if (univers.CompareTo("FEDERIS EX EURO") == 0)
                sql.Append(" AND fac.[6100062] is not null");
            else if (univers.CompareTo("FEDERIS CROISSANCE EURO") == 0)
                sql.Append(" AND fac.[6100063] is not null");
            else if (univers.CompareTo("AVENIR EURO") == 0)
                sql.Append(" AND fac.AVEURO is not null");
            else if (univers.CompareTo("FEDERIS VALUE EURO") == 0)
                sql.Append(" AND fac.AVEUROPE is not null");
            else if (univers.CompareTo("FEDERIS ISR AMERIQUE") == 0)
                sql.Append(" AND fac.[6100066] is not null");
            else
                return;

            sqlToTable(sql.ToString());
        }

        /// <summary>
        /// Find all sectors to add to temp DataTable used for organizing daa in DataGrid
        /// </summary>
        /// <param name="date">Selected Date</param>
        /// <param name="univers">Selected Univers</param>
        /// <param name="tab">Tab for which you want to fill the DataTable</param></param>
        public void FindSectors(String date, String univers, String tab)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT DISTINCT ss.label As 'SECTOR', ss.code As 'SuperSecteurId', s.label As 'INDUSTRY', s.code As 'SecteurId',");
            //sql.Append(this.criteres);
            sql.Append(" fac.SUIVI,");
            if (tab.CompareTo("Gen") == 0)
            {
                sql.Append(" fac.ISIN,");
                sql.Append(BaseActionConsultationViewModel.GeneralAgr.ToString());
            }
            else if (tab.CompareTo("Cro") == 0)
                sql.Append(BaseActionConsultationViewModel.CroissanceAgr.ToString());
            else if (tab.CompareTo("Qua") == 0)
                sql.Append(BaseActionConsultationViewModel.QualityAgr.ToString());
            else if (tab.CompareTo("Val") == 0)
                sql.Append(BaseActionConsultationViewModel.ValorisationAgr.ToString());
            else if (tab.CompareTo("Mom") == 0)
                sql.Append(BaseActionConsultationViewModel.MomentumAgr.ToString());
            else if (tab.CompareTo("Syn") == 0)
                sql.Append(BaseActionConsultationViewModel.SyntheseAgr.ToString());
            sql.Append(" FROM ref_security.SECTOR s");
            sql.Append(" INNER JOIN ref_security.SECTOR_TRANSCO st on st.id_sector1 = s.id");
            sql.Append(" INNER JOIN ref_security.SECTOR fils on fils.id = st.id_sector2");
            sql.Append(" LEFT OUTER JOIN ref_security.SECTOR ss ON fils.id_parent = ss.id");
            sql.Append(" INNER JOIN DATA_FACTSET fac on fac.FGA_SECTOR = s.code");
            sql.Append(" WHERE fac.GICS_SECTOR is null AND ss.class_name = 'GICS' AND fac.DATE='" + date + "'");
            if (univers.CompareTo("ALL") == 0)
                sql.Append(" AND fac.MXEU is not null and fac.MXUSLC is not null AND s.class_name = 'FGA_ALL' ");
            else if (univers.CompareTo("EUROPE") == 0)
                sql.Append(" AND fac.MXEU is not null and fac.MXUSLC is null AND s.class_name = 'FGA_EU'");
            else if (univers.CompareTo("USA") == 0)
                sql.Append(" AND fac.MXEU is null and fac.MXUSLC is not null AND s.class_name = 'FGA_US'");
            else if (univers.CompareTo("EMU") == 0)
                sql.Append(" AND fac.MXEM is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("EUROPE EX EMU") == 0)
                sql.Append(" AND fac.MXEUM is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FRANCE") == 0)
                sql.Append(" AND fac.MXFR is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS ACTIONS") == 0)
                sql.Append(" AND fac.[6100001] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS FRANCE ACTIONS") == 0)
                sql.Append(" AND fac.[6100002] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS ISR EURO") == 0)
                sql.Append(" AND fac.[6100004] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS NORTH AMERICA") == 0)
                sql.Append(" AND fac.[6100024] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS EUROPE ACTIONS") == 0)
                sql.Append(" AND fac.[6100026] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS EURO ACTIONS") == 0)
                sql.Append(" AND fac.[6100030] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS IRC ACTIONS") == 0)
                sql.Append(" AND fac.[6100033] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS EX EURO") == 0)
                sql.Append(" AND fac.[6100062] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS CROISSANCE EURO") == 0)
                sql.Append(" AND fac.[6100063] is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("AVENIR EURO") == 0)
                sql.Append(" AND fac.AVEURO is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS VALUE EURO") == 0)
                sql.Append(" AND fac.AVEUROPE is not null AND s.class_name = 'FGA_ALL'");
            else if (univers.CompareTo("FEDERIS ISR AMERIQUE") == 0)
                sql.Append(" AND fac.[6100066] is not null AND s.class_name = 'FGA_ALL'");
            else
                return;

            sqlToTable(sql.ToString());
        }

        /// <summary>
        /// Fill the temp table in the class given an sql request as a string
        /// </summary>
        /// <param name="sql">Request to fill a DataTable</param>
        public void sqlToTable(String sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql, Connection.coBase);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(_tableTemp);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                MessageBox.Show(sql);
            }
        }

        /// <summary>
        /// Get all date from the database in DESC ORDER
        /// </summary>
        /// <returns>All Dates as Collection of Strings</returns>
        public RadObservableCollection<String> getDates()
        {
            RadObservableCollection<String> collection = new RadObservableCollection<String>();
            if (co.IsOpen())
            {
                try
                {
                    foreach (DateTime d in co.SelectDistinctSimple("DATA_FACTSET", "DATE", "DESC"))
                    {
                        collection.Add(d.ToShortDateString());
                    }
                }
                catch
                { }
            }
            return collection;
        }

        /// <summary>
        /// get all univers from the database
        /// </summary>
        /// <returns></returns>
        public RadObservableCollection<String> getUnivers()
        {
            RadObservableCollection<String> collection = new RadObservableCollection<String>();

            collection.Add("");
            collection.Add("ALL");
            collection.Add("USA");
            collection.Add("EUROPE");
            collection.Add("EUROPE EX EMU");
            collection.Add("EMU");
            collection.Add("FRANCE");
            collection.Add("");
            collection.Add("FEDERIS NORTH AMERICA");
            collection.Add("FEDERIS ISR AMERIQUE");
            collection.Add("FEDERIS EUROPE ACTIONS");
            collection.Add("FEDERIS EX EURO");
            collection.Add("FEDERIS EURO ACTIONS");
            collection.Add("AVENIR EURO");
            collection.Add("FEDERIS ISR EURO");
            collection.Add("FEDERIS ACTIONS");
            collection.Add("FEDERIS IRC ACTIONS");
            collection.Add("FEDERIS CROISSANCE EURO");
            collection.Add("FEDERIS VALUE EURO");
            collection.Add("FEDERIS FRANCE ACTIONS");

            return collection;
        }

        /// <summary>
        /// Get all SuperSectors (GICS Sectors) from the database
        /// </summary>
        /// <returns></returns>
        public RadObservableCollection<Sector> getSuperSectors()
        {
            RadObservableCollection<Sector> collection = new RadObservableCollection<Sector>();

            String sql = "SELECT CODE AS Id, LABEL AS Libelle FROM ref_security.SECTOR WHERE";
            sql += " class_name = 'GICS'";
            sql += " and [level] = 0 ORDER BY label";
            collection.Add(new Sector(-1, ""));
            collection.Add(new Sector(-1, ALLSUPERSECTORS));
            foreach (Sector s in co.sqlToListObject(sql, () => new Sector()))
                collection.Add(s);

            return collection;
        }

        /// <summary>
        /// Get all the sectors (Industry FGA) from the database
        /// </summary>
        /// <param name="univers">Must find in a univers</param>
        /// <param name="supersector">And a GICS Sector</param>
        /// <returns></returns>
        public RadObservableCollection<Sector> getSectors(String univers, Sector supersector)
        {
            RadObservableCollection<Sector> collection = new RadObservableCollection<Sector>();
            String ALLSECTORS = "**Industries FGA**";

            if (univers == null || univers.CompareTo("") == 0)
                return collection;

            string sql = null;
            if (supersector != null)
            {
                if (supersector.Id > 0)
                {
                    sql = "SELECT distinct fga.code AS Id, fga.LABEL AS Libelle";
                    sql += " FROM ref_security.SECTOR s";
                    sql += " INNER JOIN ref_security.SECTOR_TRANSCO st on st.id_sector1 = s.id";
                    sql += " INNER JOIN ref_security.SECTOR fga on fga.id = st.id_sector2";
                    sql += " WHERE s.class_name = 'GICS' and s.id_parent = (select id from ref_security.SECTOR where code = " + supersector.Id.ToString() + ")";
                    if (univers == "ALL")
                        sql += " AND fga.class_name = 'FGA_ALL'";
                    else if (univers == "EUROPE")
                        sql += " AND fga.class_name = 'FGA_EU'";
                    else if (univers == "USA")
                        sql += " AND fga.class_name = 'FGA_US'";
                    else
                        sql += " AND fga.class_name = 'FGA_ALL'";
                    sql += " ORDER BY Libelle";
                }
                else
                {
                    sql = "SELECT code AS Id, LABEL AS Libelle FROM ref_security.SECTOR WHERE";
                    if (univers == "ALL")
                        sql += " class_name = 'FGA_ALL'";
                    else if (univers == "EUROPE")
                        sql += " class_name = 'FGA_EU'";
                    else if (univers == "USA")
                        sql += " class_name = 'FGA_US'";
                    else
                        sql += " class_name = 'FGA_ALL'";
                    sql += " AND [level] = 0 ";
                    sql += " ORDER BY Libelle";
                }

                collection.Add(new Sector(-1, ""));
                collection.Add(new Sector(-1, ALLSECTORS));
                foreach (Sector s in co.sqlToListObject(sql, () => new Sector()))
                {
                    collection.Add(s);
                }
            }
            return collection;
        }

        #endregion
    }
}