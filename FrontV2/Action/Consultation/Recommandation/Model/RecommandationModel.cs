using System;
using System.Data;
using System.Text;

namespace FrontV2.Action.Consultation.Recommandation.Model
{
    class RecommandationModel
    {
        private readonly Connection connection;

        public RecommandationModel()
        {
            connection = new Connection();
        }

        /// <summary>
        /// Get all the recommandations. The recommandation depends on the time difference between the request, 
        /// if period < 3 , display all the found differences of quints and rank in all
        /// else
        /// if diff de rank >=5 and gone in or out of top 3 of quintils, display with restrictions
        /// on sectors and supersectors if selected
        /// </summary>
        /// <param name="date"></param>
        /// <param name="univers"></param>
        /// <param name="sector"></param>
        /// <returns></returns>
        public DataTable GetRecommandations(String date, String univers, Sector sector)
        {
             if (date == null || univers == null || sector == null)
                 return new DataTable();
            StringBuilder requeteDeOuf = new StringBuilder();
            requeteDeOuf.Append("select ss.label as Secteur, fga.label as IndustryFGA, fac.COMPANY_NAME as AssetName, rec_c.comment as Recommandation, rec_v.reco_SXXP as MXEU, rec_v.reco_SXXE as MXEM, rec_v.reco_SXXA as MXEUM, rec_v.reco_MXUSLC as MXUSLC");
            requeteDeOuf.Append(" from ref_security.SECTOR ss");
            requeteDeOuf.Append(" inner join ref_security.SECTOR s on s.id_parent=ss.id");
            requeteDeOuf.Append(" inner join ref_security.SECTOR_TRANSCO tr on tr.id_sector1=s.id");
            requeteDeOuf.Append(" inner join ref_security.SECTOR fga on fga.id=tr.id_sector2");
            requeteDeOuf.Append(" inner join DATA_FACTSET fac on fac.sector=s.code ");
            requeteDeOuf.Append(" inner join ACT_RECO_VALEUR rec_v on rec_v.ISIN=fac.ISIN");
            requeteDeOuf.Append(" inner join ACT_RECO_COMMENT rec_c on rec_c.id=rec_v.id_comment");
            requeteDeOuf.Append(" where ss.code=" + sector.Id.ToString() + " and fac.DATE='" + date + "' and s.class_name='GICS' and tr.class_name='FGA_ALL' and rec_c.comment not like ''");
            requeteDeOuf.Append(" and rec_v.id_comment=(select top(1) id_comment from ACT_RECO_VALEUR where ISIN=rec_v.ISIN order by date desc)");
            requeteDeOuf.Append(" AND ss.class_name = 'GICS'");
            if (univers == "ALL")
            {
            }
            else if (univers == "EUROPE")
            {
                requeteDeOuf.Append(" AND fac.MXEU is not null");
            }
            else if (univers == "USA")
            {
                requeteDeOuf.Append(" AND fac.MXUSLC is not null");
            }
            else if (univers == "EMU")
            {
                requeteDeOuf.Append(" AND fac.MXEM is not null");
            }
            else if (univers == "EUROPE EX EMU")
            {
                requeteDeOuf.Append(" AND fac.MXEUM is not null");
            }
            else if (univers == "FRANCE")
            {
                requeteDeOuf.Append(" AND fac.MXFR is not null");
            }
            else if (univers == "FEDERIS ACTIONS")
            {
                requeteDeOuf.Append(" AND fac.[6100001] is not null");
            }
            else if (univers == "FEDERIS FRANCE ACTIONS")
            {
                requeteDeOuf.Append(" AND fac.[6100002] is not null");
            }
            else if (univers == "FEDERIS ISR EURO")
            {
                requeteDeOuf.Append(" AND fac.[6100004] is not null");
            }
            else if (univers == "FEDERIS NORTH AMERICA")
            {
                requeteDeOuf.Append(" AND fac.[6100024] is not null");
            }
            else if (univers == "FEDERIS EUROPE ACTIONS")
            {
                requeteDeOuf.Append(" AND fac.[6100026] is not null");
            }
            else if (univers == "FEDERIS EURO ACTIONS")
            {
                requeteDeOuf.Append(" AND fac.[6100030] is not null");
            }
            else if (univers == "FEDERIS IRC ACTIONS")
            {
                requeteDeOuf.Append(" AND fac.[6100033] is not null");
            }
            else if (univers == "FEDERIS EX EURO")
            {
                requeteDeOuf.Append(" AND fac.[6100062] is not null");
            }
            else if (univers == "FEDERIS CROISSANCE EURO")
            {
                requeteDeOuf.Append(" AND fac.[6100063] is not null");
            }
            else if (univers == "AVENIR EURO")
            {
                requeteDeOuf.Append(" AND fac.AVEURO is not null");
            }
            else if (univers == "FEDERIS VALUE EURO")
            {
                requeteDeOuf.Append(" AND fac.AVEUROPE is not null");
            }
            else
            {
                return new DataTable();
            }
            requeteDeOuf.Append(" order by ss.label, fga.label, fac.COMPANY_NAME");
            
            return connection.LoadDataGridByString(requeteDeOuf.ToString());
        }
    }
}
