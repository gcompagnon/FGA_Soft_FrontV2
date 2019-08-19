using System;
using System.Collections.Generic;

namespace FrontV2.Action.Doublons.Model
{
    class DoublonsModel
    {
        readonly Connection connection;

        public DoublonsModel()
        {
            connection = new Connection();
        }

        /// <summary>
        /// Execute a query calling the Connection method
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteQuery(String query)
        {
            connection.RequeteSql(query);
        }

        /// <summary>
        /// Get the data with different tickers or isin that should be the same
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Dictionary<String, object>> GetDoublonsData(String query)
        {
            return connection.sqlToListDico(query);
        }
    }
}
