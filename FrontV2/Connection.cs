using FrontV2.Action;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Linq;
using Telerik.Windows.Data;

namespace FrontV2
{
    class Connection
    {
        // En attendant une combobox pour choisir la connection dans l'appli
        private String connection = "FGA_RW";
        //private String connection = "FGA_PREPROD_RW";
        //private String connection = "FGA_PREPROD_RW";

        public String ConnectionString
        {
            get { return connection; }
            set { connection = value; }
        }

        public static SqlConnection coBase = new SqlConnection();

        // On crée une deuxieme variable SQLConnection 
        // pour pouvoir exécuter 2 datareader en meme temps sur un meme base
        public static SqlConnection coBaseBis = new SqlConnection();

        #region Connection Disconnection

        /// <summary>
        /// Connect do database
        /// </summary>
        public void ToConnectBase()
        {
            SqlCommand cmd;

            try
            {
                if (coBase.State != ConnectionState.Open)
                {
                    String conString = ConfigurationManager.ConnectionStrings[this.connection].ConnectionString;
                    coBase.ConnectionString = conString;
                    cmd = new SqlCommand();
                    coBase.Open();
                    cmd.Connection = coBase;
                    // PUT A LOG
                }
            }
            catch
            {
                // PUT A LOG
            }
        }

        /// <summary>
        /// Connect do database bis
        /// </summary>
        public void ToConnectBaseBis()
        {
            try
            {
                if (coBaseBis.State != ConnectionState.Open)
                {
                    String conString = ConfigurationManager.ConnectionStrings[this.connection].ConnectionString;
                    coBaseBis.ConnectionString = conString;
                    coBaseBis.Open();
                    // PUT A LOG
                }
            }
            catch
            {
                // PUT A LOG
            }
        }

        /// <summary>
        /// Disconnect From database
        /// </summary>
        public void ToDiconnect()
        {
            try
            {
                if (coBase.State == ConnectionState.Open)
                {
                    coBase.Close();
                    // PUT A LOG
                }
            }
            catch
            {
                // PUT A LOG
            }
        }

        #endregion

        /// <summary>
        /// Update ligne dans une tab 
        /// <summary>
        public void Update(String tabName, List<String> colNames, List<object> donnees, String colWhere, object donneeWhere)
        {
            String sqlAdd = "";
            String type = "";
            sqlAdd = "UPDATE " + tabName + " SET ";

            for (int i = 0; i < donnees.Count; i++)
            {
                sqlAdd = sqlAdd + colNames[i] + "=";
                type = donnees[i].GetType().ToString();

                if (type.CompareTo("System.String") == 0 || type.CompareTo("System.Data") == 0)
                    if (donnees[i].ToString().CompareTo("") != 0)
                        sqlAdd = sqlAdd + "'";
                    else
                        sqlAdd = sqlAdd + "NULL";
                if (donnees[i] == null)
                    sqlAdd += "NULL";

                if (donnees[i] == System.DBNull.Value)
                    sqlAdd += "NULL";
                else
                {
                    String str = donnees[i].ToString();
                    str = str.Trim();
                    str = str.Replace("'", "''");
                    sqlAdd += str;
                }

                // Important close the ' for dates and strings do not remove
                if (type.CompareTo("System.String") == 0 || type.CompareTo("System.Data") == 0)
                    if (donnees[i].ToString().CompareTo("") != 0)
                        sqlAdd = sqlAdd + "'";

                sqlAdd += ", ";
            }
            String testEnd = sqlAdd.Substring(sqlAdd.Length - 2, 2);
            if (testEnd.CompareTo(", ") == 0)
                sqlAdd = sqlAdd.Remove(sqlAdd.Length - 2, 2) + " ";

            sqlAdd += " WHERE " + colWhere + "=" + "'" + donneeWhere + "'";

            try
            {
                SqlCommand command2 = new SqlCommand(sqlAdd, coBase);
                SqlCommand commandFormat = new SqlCommand("SET DATEFORMAT dmy", coBase);
                SqlDataReader readerFormat = commandFormat.ExecuteReader();
                readerFormat.Close();
                SqlDataReader reader2 = command2.ExecuteReader();
                reader2.Read();
                reader2.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        /// <summary>
        /// Fonction to execute a sql request and return the result as a list of objects
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<object> SqlWithReturn(String sql)
        {
            SqlCommand command = new SqlCommand(sql, coBase);
            SqlDataReader reader = command.ExecuteReader();

            List<object> res = new List<object>();

            if (reader.HasRows)
                while (reader.Read())
                    res.Add(reader.GetValue(0));

            reader.Close();
            return res;
        }

        /// <summary>
        /// Execute A SELECT in database and return list of native object format
        /// </summary>
        public List<object> SelectSimple(String table, String colName)
        {
            SqlCommand command = new SqlCommand("SELECT " + colName + " FROM " + table, coBase);
            SqlDataReader reader = command.ExecuteReader();

            List<object> res = new List<object>();

            if (reader.HasRows)
                while (reader.Read())
                    res.Add(reader.GetValue(0));

            reader.Close();
            // PUT LOG
            return res;
        }

        /// <summary>
        /// Execute A SELECT in database using a WHERE condition and return list of native object format
        /// </summary>
        public List<object> SelectWhere(String table, String colName, String colWhere, object resWhere)
        {
            String sql = "";
            String type = "";
            sql = "SELECT " + colName + " FROM " + table + " WHERE " + colWhere + " = ";
            type = resWhere.GetType().ToString();

            if (type.CompareTo("System.String") == 0 || type.CompareTo("System.Date") == 0)
            {
                String resStr = resWhere.ToString();
                sql = sql + "'";
                sql = sql + resStr.Replace("'", "''");
                sql = sql + "'";
            }
            else
                sql = sql + resWhere;

            SqlCommand command = new SqlCommand(sql, coBase);
            SqlDataReader reader = command.ExecuteReader();
            List<object> res = new List<object>();

            if (reader.HasRows)
                while (reader.Read())
                    res.Add(reader.GetValue(0));

            reader.Close();
            return res;
        }

        /// <summary>
        /// Execute A SELECT in database using a DISTINCT and ORDER BY
        /// </summary>
        public List<object> SelectDistinctSimple(String table, String colName, String order = "ASC")
        {
            String sql = "SELECT DISTINCT " + colName + " FROM " + table + " ORDER BY " + colName;

            if (order.CompareTo("DESC") == 0)
            {
                sql += " DESC";
            }
            SqlCommand cmd = new SqlCommand(sql, coBase);
            SqlDataReader reader = cmd.ExecuteReader();
            List<object> res = new List<object>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0) != null && reader.GetValue(0) != DBNull.Value)
                        res.Add(reader.GetValue(0));
                }
            }
            reader.Close();
            return res;
            // PUT LOG
        }

        /// <summary>
        /// Return a List a templated object
        /// </summary>
        public List<T> sqlToListObject<T>(string sql, Func<T> ctor)
        {
            List<T> list = new List<T>();
            SqlCommand myCommand = new SqlCommand(sql, coBase);
            SqlDataReader reader = myCommand.ExecuteReader();
            object[] row = new object[reader.FieldCount + 1];

            while (reader.Read())
            {
                T obj = ctor();

                //Copie d'une ligne entière de la table
                reader.GetValues(row);

                //Traitement de la ligne
                for (int i = 0; i <= row.GetLength(0) - 2; i++)
                {
                    try
                    {
                        if (object.ReferenceEquals(row[i], DBNull.Value))
                            obj.GetType().GetProperty(reader.GetName(i)).SetValue(obj, null, null);
                        else
                        {
                            System.Reflection.PropertyInfo prop = obj.GetType().GetProperty(reader.GetName(i));
                            object val = Convert.ChangeType(row[i], prop.PropertyType);
                            obj.GetType().GetProperty(reader.GetName(i)).SetValue(obj, val, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK);
                    }
                }

                list.Add(obj);
            }
            reader.Close();

            // Disable log... Too much pain.
            //log.Log(ELog.Information, "sqlToListObject", " ")
            return list;
        }

        /// <summary>
        /// Retourne une liste de dictionnaire dont chaque dictionnaire correspond à une ligne d'une
        /// table et ses colonnes.
        /// </summary>
        /// <param name="sql">Requête SQL contenant le select</param>
        /// <returns>La liste de dictionnaire ayant comme clé la colonne de la table et comme valeur
        /// le champs de la ligne</returns>
        public List<Dictionary<string, object>> sqlToListDico(string sql)
        {
            List<Dictionary<string, object>> listDico = new List<Dictionary<string, object>>();
            SqlCommand myCommand = new SqlCommand(sql, coBase);
            SqlDataReader reader = myCommand.ExecuteReader();
            object[] row = new object[reader.FieldCount + 1];

            while (reader.Read())
            {
                Dictionary<string, object> dico = new Dictionary<string, object>();

                //Copie d'une ligne entière de la table
                reader.GetValues(row);

                //Traitement de la ligne
                int i = 0;
                while (i < row.GetLength(0))
                {
                    if ((row[i] != null) && (!object.ReferenceEquals(row[i], DBNull.Value)))
                        dico.Add(reader.GetName(i), row[i]);
                    i = i + 1;
                }
                listDico.Add(dico);
            }
            reader.Close();

            return listDico;
        }

        public List<KeyValuePair<String, int>> sqlToListKeyValuePair(string sql)
        {
            List<KeyValuePair<String, int>> res = new List<KeyValuePair<string, int>>();

            SqlCommand myCommand = new SqlCommand(sql, coBase);
            SqlDataReader reader = myCommand.ExecuteReader();
            object[] row = new object[reader.FieldCount + 1];

            while (reader.Read())
            {
                //Copie d'une ligne entière de la table
                reader.GetValues(row);

                //Traitement de la ligne
                if ((row[0] != null) && (row[1] != null))
                    res.Add(new KeyValuePair<String, int>(row[0].ToString(), int.Parse(row[1].ToString())));
            }
            reader.Close();

            return res;
        }

        public List<KeyValuePair<String, double>> sqlToListKeyValuePairDouble(string sql)
        {
            List<KeyValuePair<String, double>> res = new List<KeyValuePair<string, double>>();

            SqlCommand myCommand = new SqlCommand(sql, coBase);
            SqlDataReader reader = myCommand.ExecuteReader();
            try
            {
                object[] row = new object[reader.FieldCount + 1];

                while (reader.Read())
                {
                    //Copie d'une ligne entière de la table
                    reader.GetValues(row);

                    //Traitement de la ligne
                    if (row[0] != null)
                    {
                        if (row[1] != null)
                            if (row[1].ToString() != "")
                                res.Add(new KeyValuePair<String, double>(row[0].ToString(), double.Parse(row[1].ToString())));
                            else
                                res.Add(new KeyValuePair<String, double>(row[0].ToString(), 0));
                        else
                            res.Add(new KeyValuePair<String, double>(row[0].ToString(), 0));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                reader.Close();
            }

            return res;
        }

        public List<DateIndustryValue> ProckStockToDateIndustryValue(String name, String idsector, String ptf, String bench)
        {
            if (ptf == null || ptf == "")
                return null;
            List<DateIndustryValue> res = new List<DateIndustryValue>();

            String sql = "EXECUTE " + name + " '' , " + "'" +
                idsector + "', " + "'" +
                ptf + "', " + "'" +
                bench + "'";

            SqlCommand myCommand = new SqlCommand(sql, coBase);
            SqlDataReader reader = myCommand.ExecuteReader();
            try
            {
                object[] row = new object[reader.FieldCount + 1];

                while (reader.Read())
                {
                    //Copie d'une ligne entière de la table
                    reader.GetValues(row);

                    //Traitement de la ligne
                    if (row[0] != null)
                    {
                        if (row[2] != null)
                            if (row[2].ToString() != "")
                                res.Add(new DateIndustryValue() { Date = row[0].ToString(), Industry = row[1].ToString(), Value = double.Parse(row[2].ToString()) });
                            else
                                res.Add(new DateIndustryValue() { Date = row[0].ToString(), Industry = row[1].ToString(), Value = 0 });
                        else
                            res.Add(new DateIndustryValue() { Date = row[0].ToString(), Industry = row[1].ToString(), Value = 0 });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                reader.Close();

            }
            return res;
        }

        /// <summary>
        /// Selectionne distinct une colonne d'une table d'une bdd avec plusieurs where
        /// </summary>
        public List<object> SelectDistinctWheres(String table, String colName, List<String> colWhere,
            List<object> donnees, String colOR = "", List<object> donneesOR = null)
        {
            String sql = "SELECT DISTINCT " + colName + " FROM " + table + " WHERE ";
            String type;

            for (int i = 0; i < colWhere.Count; i++)
            {
                type = donnees[i].GetType().ToString();
                sql += colWhere[i];

                //Tester si la valeur est sous la forme d'un LIKE ou d'un NOT LIKE
                String Like_Form = donnees[i].ToString().Split(new[] { "LIKE" }, StringSplitOptions.None)[0];
                bool is_Like = Like_Form != donnees[i].ToString();

                if ((!is_Like))
                {
                    if (type.CompareTo("System.String") == 0 || type.CompareTo("System.Date") == 0)
                    {
                        sql += " = '" + donnees[i].ToString().Replace("'", "''") + "'";
                    }
                    else
                    {
                        sql += " = " + donnees[i];
                    }
                    // en like : reprendre la valeur sans modification, tel quelle
                }
                else
                {
                    sql += " " + donnees[i];
                }

                if ((i != colWhere.Count - 1))
                {
                    sql += " AND ";
                }
            }

            if ((!string.IsNullOrEmpty(colOR)))
            {
                sql += " AND " + colOR + " IN (";
                for (int i = 0; i < donneesOR.Count; i++)
                {
                    type = donneesOR[i].GetType().ToString();
                    if ((type.CompareTo("System.String") == 0 || type.CompareTo("System.Date") == 0))
                    {
                        sql += "'" + donneesOR[i] + "'";
                    }
                    else
                    {
                        sql += donneesOR[i];
                    }

                    if ((i != donneesOR.Count - 1))
                    {
                        sql += ",";
                    }
                }
                sql += ")";
            }

            sql += " ORDER BY " + colName;
            SqlCommand command = new SqlCommand(sql, coBase);
            SqlDataReader reader = command.ExecuteReader();
            object[] row = null;
            List<object> res = new List<object>();

            while (reader.Read())
            {
                if (row == null)
                {
                    row = new object[reader.FieldCount + 1];
                }
                //Copie d'une ligne entière de la table
                reader.GetValues(row);
                int i = 0;
                //Traitement de la ligne
                while (i < row.GetLength(0))
                {
                    if ((row[i] != null) && (!object.ReferenceEquals(row[i], DBNull.Value)))
                    {
                        res.Add(row[i]);
                    }
                    i = i + 1;
                }
            }
            reader.Close();

            return res;
        }

        /// <summary>
        /// Selectect with a where not null condition
        /// </summary>
        public List<object> SelectWhereNotNull(string table, string colName, string colWhere1)
        {
            string sql = null;
            sql = "SELECT " + colName + " FROM " + table + " WHERE " + colWhere1 + " IS NOT NULL";

            SqlCommand command = new SqlCommand(sql, coBase);
            SqlDataReader reader = command.ExecuteReader();
            object[] row = null;
            List<object> res = new List<object>();

            while (reader.Read())
            {
                if (row == null)
                {
                    row = new object[reader.FieldCount + 1];
                }
                //Copie d'une ligne entière de la table
                reader.GetValues(row);
                int i = 0;
                //Traitement de la ligne
                while (i < row.GetLength(0))
                {
                    if ((row[i] != null) && (!object.ReferenceEquals(row[i], DBNull.Value)))
                    {
                        res.Add(row[i]);
                    }
                    i = i + 1;
                }
            }
            reader.Close();
            return res;
        }

        /// <summary>
        /// Execute a stored procedure with a return type of list of objects
        /// </summary>    
        public List<object> ProcedureStockeeList(string nameProcedure, List<string> paramName, List<object> paramDonnee)
        {
            String sql = String.Empty;
            for (int i = 0; i < paramName.Count; i++)
            {
                sql += " DECLARE " + paramName[i] + " ";
                if (paramDonnee[i].GetType().ToString() == "System.String")
                    sql += " VARCHAR(150) ";
                else if (paramDonnee[i].GetType().ToString() == "System.Int32")
                    sql += "INTEGER ";
                else
                    sql += paramDonnee[i].GetType().ToString();
            }
            for (int i = 0; i < paramName.Count; i++)
            {
                sql = sql + " SET " + paramName[i] + " = ";

                if ((paramDonnee[i].GetType().ToString() == "System.String" ||
                     paramDonnee[i].GetType().ToString() == "System.DateTime") &&
                    paramDonnee[i].ToString().Split(new[] { "'%" }, StringSplitOptions.None)[0] != " ")
                    sql += "'";

                if (paramDonnee[i].GetType().ToString().Contains("%"))
                    sql += paramDonnee[i].ToString();
                else
                    sql += paramDonnee[i].ToString().Replace("'", "''").ToString();

                if ((paramDonnee[i].GetType().ToString() == "System.String" ||
                     paramDonnee[i].GetType().ToString() == "DateTime") &&
                    paramDonnee[i].ToString().Split(new[] { "'%" }, StringSplitOptions.None)[0] != " ")
                    sql += "'";
            }
            sql += " EXECUTE " + nameProcedure + " ";
            for (int i = 0; i < paramName.Count; i++)
            {
                sql += paramName[i].ToString();
                if ((i != paramName.Count - 1))
                    sql += ", ";
            }

            SqlCommand command = new SqlCommand(sql, coBase);
            SqlDataReader reader = command.ExecuteReader();

            List<object> dico = new List<object>();
            object[] row = null;

            while (reader.Read())
            {
                if (row == null)
                {
                    row = new object[reader.FieldCount + 1];
                }
                //Copie d'une ligne entière de la table
                reader.GetValues(row);
                int i = 0;
                //Traitement de la ligne
                while (i < row.GetLength(0))
                {
                    if ((row[i] != null) && (!object.ReferenceEquals(row[i], DBNull.Value)))
                        dico.Add(row[i]);
                    i++;
                }
            }
            reader.Close();

            return dico;
        }

        /// <summary>
        /// Execute une procedure stockée avec return
        /// </summary>   
        public DataTable ProcedureStockeeForDataGrid(string nameProcedure, List<string> paramName, List<object> paramDonnee)
        {
            String sql = String.Empty;
            for (int i = 0; i < paramName.Count; i++)
            {
                sql += " DECLARE " + paramName[i] + " ";
                if (paramDonnee[i].GetType().ToString() == "System.String")
                    sql += " VARCHAR(150) ";
                else if (paramDonnee[i].GetType().ToString() == "System.Int32")
                    sql += "INTEGER ";
                else
                    sql += paramDonnee[i].GetType().ToString();
            }
            for (int i = 0; i < paramName.Count; i++)
            {
                sql = sql + " SET " + paramName[i] + " = ";

                if ((paramDonnee[i].GetType().ToString() == "System.String" ||
                     paramDonnee[i].GetType().ToString() == "System.DateTime") &&
                    paramDonnee[i].ToString().Split(new[] { "'%" }, StringSplitOptions.None)[0] != " ")
                    sql += "'";

                if (paramDonnee[i].GetType().ToString().Contains("%"))
                    sql += paramDonnee[i].ToString();
                else
                    sql += paramDonnee[i].ToString().Replace("'", "''").ToString();

                if ((paramDonnee[i].GetType().ToString() == "System.String" ||
                     paramDonnee[i].GetType().ToString() == "DateTime") &&
                    paramDonnee[i].ToString().Split(new[] { "'%" }, StringSplitOptions.None)[0] != " ")
                    sql += "'";
            }
            sql += " EXECUTE " + nameProcedure + " ";
            for (int i = 0; i < paramName.Count; i++)
            {
                sql += paramName[i].ToString();
                if ((i != paramName.Count - 1))
                    sql += ", ";
            }

            SqlCommand command = new SqlCommand(sql, coBase);
            DataTable tmp = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(tmp);

            return tmp;
        }

        /// <summary>
        /// Execute une procedure stockée avec return
        /// </summary>   
        public Dictionary<object, object> ProcedureStockeeDico(string nameProcedure, List<string> paramName, List<object> paramDonnee)
        {
            string sql = string.Empty;
            if ((paramName.Count > 0))
            {
                for (int i = 0; i < paramName.Count; i++)
                {
                    sql = sql + " DECLARE " + paramName[i] + " ";
                    if (paramDonnee[i].GetType().ToString() == "System.String")
                        sql = sql + " VARCHAR(150) ";
                    else if (paramDonnee[i].GetType().ToString() == "System.Decimal" ||
                             paramDonnee[i].GetType().ToString() == "System.Double")
                        sql = sql + " FLOAT ";
                    else
                        sql = sql + paramDonnee[i].GetType().ToString();
                }
                for (int i = 0; i < paramName.Count; i++)
                {
                    sql = sql + " SET " + paramName[i] + " = ";
                    if (paramDonnee[i].GetType().ToString() == "System.String" ||
                        paramDonnee[i].GetType().ToString() == "System.DateTime")
                        sql = sql + "'";
                    sql = sql + paramDonnee[i].ToString().Replace("'", "''");
                    if ((paramDonnee[i].GetType().ToString() == "System.String" ||
                         paramDonnee[i].GetType().ToString() == "System.DateTime"))
                        sql = sql + "'";
                }
                sql = sql + " EXECUTE " + nameProcedure + " ";
                for (int i = 0; i < paramName.Count; i++)
                {
                    sql = sql + paramName[i];
                    if ((i != paramName.Count - 1))
                        sql = sql + ", ";
                }
            }
            else
            {
                sql = "EXECUTE " + nameProcedure;
            }

            SqlCommand command = new SqlCommand(sql, coBase);
            SqlDataReader reader = command.ExecuteReader();

            Dictionary<object, object> dico = new Dictionary<object, object>();
            object[] row = null;

            while (reader.Read())
            {
                if (row == null)
                {
                    row = new object[reader.FieldCount + 1];
                }
                //Copie d'une ligne entière de la table
                reader.GetValues(row);
                int i = 0;
                //Traitement de la ligne
                while (i < row.GetLength(0))
                {
                    if ((row[i] != null) && (!object.ReferenceEquals(row[i], DBNull.Value)))
                    {
                        dico.Add(row[i], row[i + 1]);
                    }
                    i = i + 2;
                }
            }
            reader.Close();
            return dico;
        }

        /// <summary>
        /// Nothing more to say just execute proc stock
        /// </summary>
        public void ProcedureStockee(string nameProcedure, List<string> paramName, List<object> paramDonnee)
        {
            String sql = String.Empty;
            for (int i = 0; i < paramName.Count; i++)
            {
                sql += " DECLARE " + paramName[i] + " ";
                if (paramDonnee[i].GetType().ToString() == "System.String")
                    sql += " VARCHAR(150) ";
                else if (paramDonnee[i].GetType().ToString() == "System.Int32")
                    sql += "INTEGER ";
                else
                    sql += paramDonnee[i].GetType().ToString();
            }
            for (int i = 0; i < paramName.Count; i++)
            {
                sql = sql + " SET " + paramName[i] + " = ";

                if ((paramDonnee[i].GetType().ToString() == "System.String" ||
                     paramDonnee[i].GetType().ToString() == "System.DateTime") &&
                    paramDonnee[i].ToString().Split(new[] { "'%" }, StringSplitOptions.None)[0] != " ")
                    sql += "'";

                if (paramDonnee[i].GetType().ToString().Contains("%"))
                    sql += paramDonnee[i].ToString();
                else
                    sql += paramDonnee[i].ToString().Replace("'", "''").ToString();

                if ((paramDonnee[i].GetType().ToString() == "System.String" ||
                     paramDonnee[i].GetType().ToString() == "DateTime") &&
                    paramDonnee[i].ToString().Split(new[] { "'%" }, StringSplitOptions.None)[0] != " ")
                    sql += "'";
            }
            sql += " EXECUTE " + nameProcedure + " ";
            for (int i = 0; i < paramName.Count; i++)
            {
                sql += paramName[i].ToString();
                if ((i != paramName.Count - 1))
                    sql += ", ";
            }

            SqlCommand command = new SqlCommand(sql, coBase);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Remplie un Data Grid à l'aide d'un string
        /// </summary> 
        public DataTable LoadDataGridByString(string requeteSQL)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand(requeteSQL, coBase);
            try
            {
                SqlDataAdapter adpt = new SqlDataAdapter(myCommand);
                adpt.Fill(dt);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return dt;
        }

        /// <summary>
        /// Return the maximal date as a string from the table DATA_FACTSET
        /// <summary>
        public String GetMaxDate()
        {
            String sql = "Select DISTINCT MAX(DATE) FROM DATA_FACTSET";
            SqlCommand command = new SqlCommand(sql, coBase);

            SqlDataReader reader = command.ExecuteReader();

            List<object> res = new List<object>();

            if (reader.HasRows)
                while (reader.Read())
                    res.Add(reader.GetValue(0));

            reader.Close();

            return res[0].ToString();
        }

        public RadObservableCollection<String> GetDates()
        {
            RadObservableCollection<String> collection = new RadObservableCollection<String>();
            if (coBase.State == ConnectionState.Open)
            {
                foreach (DateTime d in SelectDistinctSimple("DATA_FACTSET", "DATE", "DESC"))
                {
                    collection.Add(d.ToShortDateString());
                }
            }
            return collection;
        }

        /// <summary>
        /// Don't think you should use it again
        /// </summary>
        /// <returns></returns>
        public List<object> GetSectorsICB()
        {
            return SelectDistinctWheres("ref_security.SECTOR", "label",
                new List<String> { "class_name", "level" },
                new List<object> { "GICS", 0 });
        }

        /// <summary>
        /// Don't think you should use it again
        /// </summary>
        /// <returns></returns>
        public List<object> GetSectorsFGA()
        {
            return SelectDistinctWheres("ref_security.SECTOR", "label",
                new List<String> { "class_name", "level" },
                new List<object> { "FGA_EU", 0 });
        }

        /// <summary>
        /// Don't think you should use it again
        /// </summary>
        /// <returns></returns>
        public List<object> GetEnterprises()
        {
            return SelectWhereNotNull("DATA_FACTSET", "Company_Name", "ISIN");
        }

        /// <summary>
        /// Don't think you should use it again
        /// </summary>
        /// <returns></returns>
        public List<object> GetFGAIndustries(String sector, String FGA_classname)
        {
            List<object> sectors = new List<object>();

            if (sector == null || sector == "")
                return sectors;
            if (FGA_classname == null || FGA_classname == "")
                return sectors;

            if (FGA_classname == "Europe")
                FGA_classname = "FGA_EU";
            else if (FGA_classname == "USA")
                FGA_classname = "FGA_US";

            if ("".CompareTo(sector) != 0)
            {
                int id_gics_sector = int.Parse(SelectDistinctWheres("ref_security.SECTOR", "code",
                    new List<String> { "label", "class_name" },
                    new List<object> { sector, "GICS", 0 })[0].ToString());
                List<object> libelles = ProcedureStockeeList("ACT_SuperSectorvsSectorFGA",
                    new List<String> { "@supersector", "@fga" },
                    new List<object> { id_gics_sector, FGA_classname });

                foreach (String libelle_FGA in libelles)
                    sectors.Add(libelle_FGA);
            }
            sectors.Sort();
            sectors.Insert(0, "");

            return sectors;
        }

        /// <summary>
        /// Don't think you should use it again
        /// </summary>
        /// <returns></returns>
        public List<String> GetCompanies(String date, String superSector, String sector)
        {
            List<String> values = new List<String>();
            values.Add(" ");

            if (sector != null && sector != "")
            {
                int id_fga = int.Parse(SelectDistinctWheres("ref_security.SECTOR", "code",
                    new List<String> { "class_name", "level", "label" },
                    new List<object> { "FGA_EU", 0, sector })[0].ToString());

                foreach (var subIndustry in SelectDistinctWheres("DATA_FACTSET", "GICS_SUBINDUSTRY",
                    new List<String> { "FGA_SECTOR", "DATE" }, new List<object> { id_fga, date }))
                {
                    if (subIndustry != DBNull.Value)
                    {
                        foreach (var value in SelectDistinctWheres("DATA_FACTSET", "COMPANY_NAME",
                            new List<String> { "SECTOR", "DATE" }, new List<object> { subIndustry, date }))
                        {
                            foreach (var ticker in SelectDistinctWheres("DATA_FACTSET", "TICKER",
                                new List<String> { "COMPANY_NAME", "DATE" }, new List<object> { value, date }))
                            {
                                values.Add(value.ToString() + " | " + ticker.ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                int id_sec = int.Parse(SelectDistinctWheres("ref_security.SECTOR", "code",
                    new List<String> { "class_name", "level", "label" },
                    new List<object> { "GICS", 0, superSector })[0].ToString());

                foreach (var subIndustry in SelectDistinctWheres("DATA_FACTSET", "GICS_SUBINDUSTRY",
                    new List<String> { "GICS_SECTOR", "DATE" }, new List<object> { id_sec, date }))
                {
                    if (subIndustry != DBNull.Value)
                    {
                        foreach (var value in SelectDistinctWheres("DATA_FACTSET", "COMPANY_NAME",
                            new List<String> { "SECTOR", "DATE" }, new List<object> { subIndustry, date }))
                        {
                            foreach (var ticker in SelectDistinctWheres("DATA_FACTSET", "TICKER",
                                new List<String> { "COMPANY_NAME", "DATE" }, new List<object> { value, date }))
                            {
                                values.Add(value.ToString() + " | " + ticker.ToString());
                            }
                        }
                    }
                }
            }

            return values;
        }

        public String GetIdFGA(String s)
        {
            return SelectDistinctWheres("ref_security.SECTOR", "code",
                new List<String> { "class_name", "level", "label" },
                new List<object> { "FGA_ALL", 0, s })[0].ToString();
        }

        public String GetIdSectorFromName(String s)
        {
            return SelectDistinctWheres("ref_security.SECTOR", "code",
                new List<String> { "class_name", "level", "label" },
                new List<object> { "GICS", 0, s })[0].ToString();
        }

        /// <summary>
        /// Simple execution of an SqlRequest from a given string as parameter
        /// <summary>
        public void RequeteSql(string sql)
        {
            SqlCommand command = new SqlCommand(sql, coBase);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur(s) d'une requete sql : " + ex.Message);
            }
        }

        /// <summary>
        /// Return true is the connection with the database is still active
        /// <summary>
        public bool IsOpen()
        {
            if (coBase.State == ConnectionState.Open)
                return true;
            else
                return false;
        }

        public String GetISINFromTicker(String ticker)
        {
            return SelectDistinctWheres("DATA_FACTSET", "ISIN",
                new List<String> { "TICKER" }, new List<object> { ticker }).First().ToString();
        }

        public List<object> RequeteSqltoDataTab(String sql)
        {
            SqlCommand command = new SqlCommand(sql, coBase);
            command.CommandTimeout = 600;
            SqlDataReader reader = command.ExecuteReader();
            List<object> res = new List<object>();
            if (reader.HasRows)
                while (reader.Read())
                {
                    Object[] obj = new Object[21];
                    obj[0] = reader.GetValue(0).ToString().Substring(0, 10);
                    obj[1] = reader.GetValue(1);
                    obj[2] = reader.GetValue(2);
                    obj[3] = reader.GetValue(3);
                    obj[4] = reader.GetValue(4);
                    obj[5] = reader.GetValue(5);
                    obj[6] = reader.GetValue(6);
                    obj[7] = reader.GetValue(7);
                    obj[8] = reader.GetValue(8);
                    obj[9] = reader.GetValue(9);
                    obj[10] = reader.GetValue(10);
                    obj[11] = reader.GetValue(11);
                    obj[12] = reader.GetValue(12);
                    obj[13] = reader.GetValue(13);
                    obj[14] = reader.GetValue(14);
                    obj[15] = reader.GetValue(15);
                    obj[16] = reader.GetValue(16);
                    obj[17] = reader.GetValue(17);
                    String tmp1 = reader.GetValue(18).ToString();
                    if (tmp1 == "")
                        obj[18] = "NULL";
                    else
                        obj[18] = tmp1.Substring(0, 10);
                    obj[19] = reader.GetValue(19);
                    String tmp2 = reader.GetValue(20).ToString();
                    if (tmp2 == "")
                        obj[20] = "NULL";
                    else
                        obj[20] = tmp2.Substring(0, 10);
                    res.Add(obj);
                }

            reader.Close();

            return res;
        }

        public List<object> RequeteSqltoDataTab2(String sql)
        {
            SqlCommand command = new SqlCommand(sql, coBase);
            command.CommandTimeout = 1200;
            SqlDataReader reader = command.ExecuteReader();
            List<object> res = new List<object>();
            if (reader.HasRows)
                while (reader.Read())
                {
                    Object[] obj = new Object[2];
                    obj[0] = reader.GetValue(0).ToString().Substring(0, 10);
                    obj[1] = reader.GetValue(1);

                    res.Add(obj);
                }

            reader.Close();

            return res;
        }

        #region SqlRequest
        public List<object> sqlRequesttoDataTab3(String sql)
        {
            SqlCommand command = new SqlCommand(sql, coBase);
            command.CommandTimeout = 1200;
            SqlDataReader reader = command.ExecuteReader();
            List<object> res = new List<object>();
            if (reader.HasRows)
                while (reader.Read())
                {
                    Object[] obj = new Object[3];
                    obj[0] = reader.GetValue(0).ToString().Substring(0, 10);
                    obj[1] = reader.GetValue(1);
                    obj[2] = reader.GetValue(2);

                    res.Add(obj);
                }

            reader.Close();

            return res;
        }
        #endregion


    }
}