using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontV2.Action.Reco.Model
{
    class NewRecommandationValeursModel
    {
        readonly Connection connection;

        public NewRecommandationValeursModel()
        {
            connection = new Connection();
        }

        public void AddRecommandation(String isin, String mxem, String mxeum, String mxeu, String mxuslc)
        {                      
            connection.ProcedureStockee("ACT_Add_Reco_Valeur",
                new List<String> { "@isin", "@reco_SXXE", "@reco_SXXA", "@reco_SXXP", "@reco_MXUSLC", "@login" },
                new List<object> { isin, mxem, mxeum, mxeu, mxuslc, Utilisateur.login });
        }

        public void UpdateReco(int id, int id_change, String text)
        {
            if (id > 0)
            {
                connection.Update("ACT_RECO_COMMENT",
                    new List<String> { "comment" },
                    new List<object> { text},
                    "id", id);
            }

            if (id_change > 0)
            {
                connection.Update("ACT_RECO_COMMENT",
                    new List<String> { "comment" },
                    new List<object> { text },
                    "id", id_change);
            }
        }

        public int GetIdFromIsin(String isin)
        {
            return int.Parse(connection.SelectWhere("ACT_RECO_VALEUR",
                "id_comment", "isin", isin).Last().ToString());
        }

        public int GetIdChangeFromIsin(String isin)
        {
            return int.Parse(connection.SelectWhere("ACT_RECO_VALEUR",
                "id_comment_change", "isin", isin).Last().ToString());
        }
    }
}
