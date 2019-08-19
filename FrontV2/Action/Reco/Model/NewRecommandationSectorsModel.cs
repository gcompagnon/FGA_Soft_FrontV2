using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontV2.Action.Reco.Model
{
    class NewRecommandationSectorsModel
    {
        readonly Connection connection;

        public NewRecommandationSectorsModel()
        {
            connection = new Connection();
        }

        public void AddRecommandation(String id, String recoMXEU, String recoMXEUM,
            String recoMXEM, String recoMXUSLC, String place)
        {                      
            connection.ProcedureStockee("ACT_Add_Reco_Secteur",
                new List<String> { "@id", "@type", "@recoMXEU", "@recoMXEUM", "@recoMXEM", "@recoMXUSLC", "@login" },
                new List<object> { id, place, recoMXEU, recoMXEUM, recoMXEM, recoMXUSLC, Utilisateur.login });
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

        public int GetIdFromIsin(String id)
        {
            return int.Parse(connection.SelectWhere("ACT_RECO_SECTOR",
                "id_comment", "id_secteur", id).Last().ToString());
        }

        public int GetIdChangeFromIsin(String id)
        {
            return int.Parse(connection.SelectWhere("ACT_RECO_SECTOR",
                "id_comment_change", "id_secteur", id).Last().ToString());
        }
    }
}
