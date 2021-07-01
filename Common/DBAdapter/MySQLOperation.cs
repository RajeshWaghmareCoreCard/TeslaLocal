using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Collections.Generic;

using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace DBAdapter
{
    class MySQLOperation : IDataBaseCommand
    {

        // private const string _LICENSE_DB_NAME = "APIAutomationLicense";           //for license database name
        private const string _LICENSE_TABLE_NAME = "License";        //for license table name
        private const string _LICENSE_COL_NAME = "LicenseInfo";          //for license column name
        private string _connString;
        private bool _autoCloseDBConn;
        //private MySqlConnection _mySqlConn = new MySqlConnection();
        private MySqlConnection _mySqlConn;
        private string _tempPath = "";

        public MySQLOperation()
        {
            this._autoCloseDBConn = true;
        }

        /// <summary>
        /// read-write connection string property
        /// </summary>
        public string TempPath
        {
            get
            {
                return _tempPath;
            }
            set
            {
                _tempPath = value;
            }
        }

        /// <summary>
        /// read-write connection string property
        /// </summary>
        public string ConnString
        {
            get
            {
                return _connString;
            }
            set
            {
                // _connString = value;
                this._connString = DBOperation.GetDecryptedConnectionString(value);
            }
        }

        /// <summary>
        /// Keeps flag of Auto close DB Connection
        /// </summary>
        public bool AutoCloseDBConnection
        {
            get
            {
                return this._autoCloseDBConn;
            }
            set
            {
                this._autoCloseDBConn = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowsAuthentication"></param>
        /// <param name="serverName"></param>
        /// <param name="port"></param>
        /// <param name="dbName"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="connTimeOut"></param>
        /// <param name="withDBName"></param>
        /// <param name="report_TempPath">This is only for SQlite DB connection</param>
        /// <returns></returns>
        public string GenerateConnString(bool windowsAuthentication, string serverName, string port, string dbName, string userId, string password, string connTimeOut, bool withDBName, string report_TempPath = "")
        {
            //string winAuthConnStringFormat = "Network Address=#SERVERNAME#;Initial Catalog=#DATABASENAME#;Persist Security Info=no;User Name=#USERID#;Password=#PASSWORD#;Connection Timeout=#TIMEOUT#;";
            //string connStringFormat = "Network Address=#SERVERNAME#;Initial Catalog=#DATABASENAME#;Persist Security Info=no;User Name=#USERID#;Password=#PASSWORD#;Connection Timeout=#TIMEOUT#;";
            //"Server=localhost;Port=3307;database=APIAutomation;User Name=root;Password=uts@123;Connection Timeout=30;Pooling=true;Max Pool Size = 100; Min Pool Size=5;"

            //P00017---July13/2012'hardeep commented below code
            //string withDBConnStringFormat = "Server=#SERVERNAME#;database=#DATABASENAME#;Port=#PORT#;User Name=#USERID#;Password=#PASSWORD#;Connection Timeout=#TIMEOUT#;Pooling=true;Max Pool Size=100;Min Pool Size=5;";
            //string withoutDBConnStringFormat ="Server=#SERVERNAME#;Port=#PORT#;User Name=#USERID#;Password=#PASSWORD#;Connection Timeout=#TIMEOUT#;Pooling=true;Max Pool Size=100;Min Pool Size=5;";

            //P00017 ---July14/2012'hardeep
            string withDBConnStringFormat = "Server=#SERVERNAME#;Port=#PORT#;database=#DATABASENAME#;User Name=#USERID#;Password=#PASSWORD#;Connection Timeout=#TIMEOUT#;Pooling=true;Max Pool Size=100;Min Pool Size=5;";
            string withoutDBConnStringFormat = "Server=#SERVERNAME#;Port=#PORT#;User Name=#USERID#;Password=#PASSWORD#;Connection Timeout=#TIMEOUT#;Pooling=true;Max Pool Size=100;Min Pool Size=5;";
            //P00017 ---July14/2012'hardeep end

            StringBuilder connString = new StringBuilder();

            try
            {
                if (withDBName)
                {
                    connString.Append(withDBConnStringFormat);
                    //Replace database name
                    connString.Replace("#DATABASENAME#", dbName);
                }
                else
                {
                    connString.Append(withoutDBConnStringFormat);
                }

                //Replace Timeout name
                connString.Replace("#USERID#", userId);
                //Replace Password name
                connString.Replace("#PASSWORD#", password);
                //Replace server name
                connString.Replace("#SERVERNAME#", serverName);
                //Replace Port
                connString.Replace("#PORT#", port);
                //Replace Timeout name
                connString.Replace("#TIMEOUT#", connTimeOut);
            }
            catch (Exception)
            {
                throw;
            }

            // return connString.ToString();
            return DBOperation.GetEncryptedConnectionString(Convert.ToString(connString.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="windowsAuthentication"></param>
        /// <param name="serverName"></param>
        /// <param name="dbName"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="connTimeOut"></param>
        /// <returns></returns>
        public bool ReadConnStringInfo(string connString, out bool windowsAuthentication, out string serverName, out string port, out string dbName, out string userId, out string password, out string connTimeOut)
        {
            bool status = false;
            windowsAuthentication = false;
            serverName = "";
            port = "";
            dbName = "";
            userId = "";
            password = "";
            connTimeOut = "";
            //string connStringFormat = "Server=#SERVERNAME#;Port=#PORT#;database=#DATABASENAME#;User Name=#USERID#;Password=#PASSWORD#;Connection Timeout=#TIMEOUT#;Pooling=true;Max Pool Size=100;Min Pool Size=5;";

            try
            {
                connString = DBOperation.GetDecryptedConnectionString(connString);
                //Split individual info based on ";"
                string[] connStringCollection = connString.ToString().Split(';');
                string[] serverInfo = connStringCollection[0].ToString().Split('=');
                serverName = serverInfo[1].ToString();

                string[] portID = connStringCollection[1].ToString().Split('=');
                port = portID[1].ToString();

                string[] dbInfo = connStringCollection[2].ToString().Split('=');
                dbName = dbInfo[1].ToString();

                string[] UserIdInfo = connStringCollection[3].ToString().Split('=');
                userId = UserIdInfo[1].ToString();

                string[] passwordInfo = connStringCollection[4].ToString().Split('=');
                password = passwordInfo[1].ToString();

                //If Timeout has defined in connection string
                if (connStringCollection.Length == 10)
                {
                    string[] timeOutInfo = connStringCollection[5].ToString().Split('=');
                    connTimeOut = timeOutInfo[1].ToString();
                }

                ////Considering as Windows Authentication if connection string info length is 3 
                //if (connStringCollection.Length == 3)
                //{
                //    windowsAuthentication = true;
                //}

                status = true;
            }
            catch (Exception)
            {
                throw;
            }

            return status;
        }


        /// <summary>
        /// It's create DB connection
        /// </summary>
        /// <returns></returns>
        public bool OpenDBConnection()
        {
            bool status = false;

            try
            {
                this._mySqlConn = new MySqlConnection();
                this._mySqlConn.ConnectionString = this._connString;
                this._mySqlConn.Open();
                status = true;
            }
            catch (Exception)
            {
                throw;
            }

            return status;
        }

        /// <summary>
        /// It's close DB Connection
        /// </summary>
        /// <returns></returns>
        public bool CloseDBConnection()
        {
            bool status = false;
            try
            {
                if (this._mySqlConn == null)
                {
                    status = true;
                }
                else if (this._mySqlConn.State == ConnectionState.Open)
                {
                    this._mySqlConn.Close();
                    status = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this._mySqlConn != null)
                {
                    this._mySqlConn.Dispose();
                    this._mySqlConn = null;
                }
            }

            return status;
        }

        /// <summary>
        /// It returns dataset of input query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sqlQuery)
        {
            DataSet dsRequested = new DataSet();

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, this._mySqlConn);
            sqlQuery = ReplaceSpecialCharacters(sqlQuery);

            try
            {
                dataAdapter.Fill(dsRequested);
            }
            catch (Exception ex)
            {
                //This is temporary solution to handle Fatal error because actual reason in unlnown. It works perfectly next time after error.
                //Modified on 04 Aug 2012
                if (ex.Message.Contains("Fatal error encountered during command execution"))
                {
                    dataAdapter.Fill(dsRequested);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                dataAdapter.Dispose();
                dataAdapter = null;

                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return dsRequested;
        }

        /// <summary>
        /// It returns dataset of input query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet GetParameterizedDataSet(string sqlQuery, IDictionary<string, object> values)
        {
            DataSet dsRequested = new DataSet();

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, this._mySqlConn);
            foreach (KeyValuePair<string, object> item in values)
            {
                dataAdapter.SelectCommand.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            sqlQuery = ReplaceSpecialCharacters(sqlQuery);

            try
            {
                dataAdapter.Fill(dsRequested);
            }
            catch (Exception ex)
            {
                //This is temporary solution to handle Fatal error because actual reason in unlnown. It works perfectly next time after error.
                //Modified on 04 Aug 2012
                if (ex.Message.Contains("Fatal error encountered during command execution"))
                {
                    dataAdapter.Fill(dsRequested);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                dataAdapter.Dispose();
                dataAdapter = null;

                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return dsRequested;
        }

        /// <summary>
        /// It returns dataset of input query and command TimeOut
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sqlQuery, int commandTimeOut)
        {
            DataSet dsRequested = new DataSet();

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, this._mySqlConn);
            dataAdapter.SelectCommand.CommandTimeout = commandTimeOut;
            sqlQuery = ReplaceSpecialCharacters(sqlQuery);

            try
            {
                dataAdapter.Fill(dsRequested);
            }
            catch (Exception ex)
            {
                //This is temporary solution to handle Fatal error because actual reason in unlnown. It works perfectly next time after error.
                //Modified on 04 Aug 2012
                if (ex.Message.Contains("Fatal error encountered during command execution"))
                {
                    dataAdapter.Fill(dsRequested);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                dataAdapter.Dispose();
                dataAdapter = null;

                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return dsRequested;
        }

        /// <summary>
        /// It returns dataset of input query and command TimeOut
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="values"></param>
        /// <param name="commandTimeOut"></param>
        /// <returns></returns>
        public DataSet GetParameterizedDataSet(string sqlQuery,
                                                IDictionary<string, object> values,
                                                int commandTimeOut)
        {
            DataSet dsRequested = new DataSet();

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, this._mySqlConn);
            foreach (KeyValuePair<string, object> item in values)
            {
                dataAdapter.SelectCommand.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            dataAdapter.SelectCommand.CommandTimeout = commandTimeOut;
            sqlQuery = ReplaceSpecialCharacters(sqlQuery);

            try
            {
                dataAdapter.Fill(dsRequested);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Fatal error encountered during command execution"))
                {
                    dataAdapter.Fill(dsRequested);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                dataAdapter.Dispose();
                dataAdapter = null;

                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }
            return dsRequested;
        }

        /// <summary>
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlQuery)
        {
            int rowAffected = 0;
            //MySqlCommand command;

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlQuery = ReplaceSpecialCharacters(sqlQuery);
                MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                rowAffected = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            }
            catch (Exception ex)
            {
                //This is temporary solution to handle Fatal error because actual reason in unlnown. It works perfectly next time after error.
                //Modified on 04 Aug 2012
                if (ex.Message.Contains("Fatal error encountered during command execution"))
                {
                    CloseDBConnection();
                    OpenDBConnection();
                    MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                    rowAffected = command.ExecuteNonQuery();
                    command.Dispose();
                    command = null;
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return rowAffected;
        }

        /// <summary>
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="values">Parameters passed to query</param>
        /// <returns></returns>
        public int ExecuteParameterizedNonQuery(string sqlQuery, IDictionary<string, object> values)
        {
            int rowAffected = 0;
            //MySqlCommand command;

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlQuery = ReplaceSpecialCharacters(sqlQuery);
                MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                foreach (KeyValuePair<string, object> item in values)
                {
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                rowAffected = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            }
            catch (Exception ex)
            {
                //This is temporary solution to handle Fatal error because actual reason in unlnown. It works perfectly next time after error.
                //Modified on 04 Aug 2012
                if (ex.Message.Contains("Fatal error encountered during command execution"))
                {
                    CloseDBConnection();
                    OpenDBConnection();
                    MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                    foreach (KeyValuePair<string, object> item in values)
                    {
                        command.Parameters.Add("@" + item.Key,(MySqlDbType) item.Value);
                    }
                    rowAffected = command.ExecuteNonQuery();
                    command.Dispose();
                    command = null;
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return rowAffected;
        }

        /// <summary>
        /// It execute queries in array with transaction
        /// </summary>
        /// <param name="sqlQuery">Array of queries need to be executed in transaction</param>
        /// <returns>Bool value depanding on result</returns>
        /// 
        public bool ExecuteNonQueryWithTransaction(string[] sqlQuery)
        {
            bool rowAffected = false;

            using (MySqlConnection connection = new MySqlConnection(this._connString))
            {
                connection.Open();

                // Start a local transaction.
                // SqlTransaction sqlTran = connection.BeginTransaction();
                MySqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist the command in the current transaction.
                //SqlCommand command = connection.CreateCommand();
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    for (int i = 0; i < sqlQuery.Length; i++)
                    {
                        // command.CommandText = sqlQuery[i];

                        string sqlString = "";
                        sqlString = ReplaceSpecialCharacters(sqlQuery[i].ToString());
                        command.CommandText = sqlString;
                        command.ExecuteNonQuery();
                    }
                    rowAffected = true;

                    sqlTran.Commit();
                }
                catch (Exception)
                {
                    rowAffected = false;
                    sqlTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            return rowAffected;

        }

        /// <summary>
        /// It execute queries in array with transaction
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ExecuteParameterizedNonQueryWithTransaction(
            string[] sqlQuery, IDictionary<string, object> values)
        {
            bool rowAffected = false;

            using (MySqlConnection connection = new MySqlConnection(this._connString))
            {
                connection.Open();

                // Start a local transaction.
                // SqlTransaction sqlTran = connection.BeginTransaction();
                MySqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist the command in the current transaction.
                //SqlCommand command = connection.CreateCommand();
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    for (int i = 0; i < sqlQuery.Length; i++)
                    {
                        // command.CommandText = sqlQuery[i];

                        string sqlString = "";
                        sqlString = ReplaceSpecialCharacters(sqlQuery[i].ToString());
                        command.CommandText = sqlString;

                        foreach (KeyValuePair<string, object> item in values)
                        {
                            command.Parameters.AddWithValue("@" + item.Key, item.Value);
                        }
                        command.ExecuteNonQuery();
                    }
                    rowAffected = true;

                    sqlTran.Commit();
                }
                catch (Exception)
                {
                    rowAffected = false;
                    sqlTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            return rowAffected;

        }


        /// <summary>
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlQuery, int commandTimeOut)
        {
            int rowAffected = 0;

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlQuery = ReplaceSpecialCharacters(sqlQuery);
                MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                command.CommandTimeout = commandTimeOut;
                rowAffected = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return rowAffected;
        }

        /// <summary>
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public object ExecuteScalarCommand(string sqlQuery)
        {
            object resultData;

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlQuery = ReplaceSpecialCharacters(sqlQuery);
                MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                resultData = command.ExecuteScalar();
                command.Dispose();
                command = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return resultData;
        }

        /// <summary>
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public object ExecuteParameterizedScalarCommand(string sqlQuery,
            IDictionary<string, object> values)
        {
            object resultData;

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlQuery = ReplaceSpecialCharacters(sqlQuery);
                MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                foreach (KeyValuePair<string, object> item in values)
                {
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                resultData = command.ExecuteScalar();
                command.Dispose();
                command = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return resultData;
        }

        /// <summary>
        /// It checks DB connection state
        /// </summary>
        /// <returns></returns>
        private bool IsDBConnectionOpen()
        {
            bool status = false;

            if (this._mySqlConn != null)
            {
                if (this._mySqlConn.State == ConnectionState.Open)
                {
                    status = true;
                }
            }

            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public bool CheckDatabaseConnection(string connectionString)
        {
            bool isConnected = false;
            try
            {
                connectionString = DBOperation.GetDecryptedConnectionString(connectionString);
                using (MySqlConnection mySqlCon = new MySqlConnection(connectionString))
                {
                    mySqlCon.Open();
                    if (mySqlCon.State == ConnectionState.Open)
                    {
                        isConnected = true;
                        mySqlCon.Close();
                        mySqlCon.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isConnected;
        }


        /// <summary>
        /// Returns database list of given Database connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public ArrayList GetDatabaseNameList(string connectionString)
        {
            ArrayList dbList = new ArrayList();
            //MySqlConnection mySqlCon = new MySqlConnection(connectionString);
            string query = "select SCHEMA_NAME from information_schema.SCHEMATA";

            try
            {
                connectionString = DBOperation.GetDecryptedConnectionString(connectionString);
                using (MySqlConnection mySqlCon = new MySqlConnection(connectionString))
                {
                    mySqlCon.Open();
                    //mySqlCom.Connection = mySqlCon;
                    MySqlCommand mySqlCom = new MySqlCommand(query, mySqlCon);
                    MySqlDataReader mySqlDR;
                    mySqlDR = mySqlCom.ExecuteReader();

                    while (mySqlDR.Read())
                    {
                        dbList.Add(mySqlDR.GetString(0));
                    }

                    mySqlDR.Dispose();
                    mySqlDR = null;
                    mySqlCom.Dispose();
                    mySqlCom = null;
                    mySqlCon.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dbList;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="srcTable"></param>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public bool ExecuteDataSet(string cmdText, string srcTable, DataSet dsData)
        {
            bool status = false;
            MySqlCommand mySqlComm = null; ;
            MySqlDataAdapter mySqlDA = null; ;
            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                cmdText = ReplaceSpecialCharacters(cmdText);
                mySqlComm = new MySqlCommand(cmdText, this._mySqlConn);
                mySqlDA = new MySqlDataAdapter(mySqlComm);
                mySqlDA.Fill(dsData, srcTable);
                status = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                mySqlComm.Dispose();
                mySqlDA.Dispose();
                mySqlComm = null;
                mySqlDA = null;

                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="srcTable"></param>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public bool ExecuteParameterizedDataSet(string cmdText, IDictionary<string, object> values, string srcTable, DataSet dsData)
        {
            bool status = false;
            MySqlCommand mySqlComm = null; ;
            MySqlDataAdapter mySqlDA = null; ;
            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                cmdText = ReplaceSpecialCharacters(cmdText);
                mySqlComm = new MySqlCommand(cmdText, this._mySqlConn);
                foreach (KeyValuePair<string, object> item in values)
                {
                    mySqlComm.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                mySqlDA = new MySqlDataAdapter(mySqlComm);
                mySqlDA.Fill(dsData, srcTable);
                status = true;
            }
            catch (Exception )
            {
                throw;
            }
            finally
            {
                mySqlComm.Dispose();
                mySqlDA.Dispose();
                mySqlComm = null;
                mySqlDA = null;

                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return status;
        }

        /// <summary>
        /// This method replaces "\" and "with(nolock)" character from SQL query before "WHERE" clause because MySQL doesn't support
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string ReplaceSpecialCharacters(string query)
        {
            string tmpQuery = "";
            tmpQuery = query;
            try
            {
                if (tmpQuery.Trim() != "")
                {
                    tmpQuery = tmpQuery.Replace(@"\", @"\\").Replace("with(nolock)", "");
                }
            }
            catch
            {
                tmpQuery = query;
            }

            return tmpQuery;
        }
        /// <summary>
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int ExecuteQuery(string sqlQuery)
        {
            int rowAffected = 0;

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlQuery = ReplaceSpecialCharacters(sqlQuery);
                MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                rowAffected = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return rowAffected;
        }

        /// <summary>
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int ExecuteParameterizedQuery(string sqlQuery, IDictionary<string, object> values)
        {
            int rowAffected = 0;

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlQuery = ReplaceSpecialCharacters(sqlQuery);
                MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
                foreach (KeyValuePair<string, object> item in values)
                {
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                rowAffected = command.ExecuteNonQuery();
                command.Dispose();
                command = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return rowAffected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="picByte"></param>
        /// <returns></returns>
        public bool InsertReportImage(string sqlQuery, string paramName, byte[] picByte)
        {
            bool status = false;
            //string curDate = DateTime.Now.ToString();
            //string query = "insert into ReportImage(imageid,ReportId,CreatedDate,ImageHTMLFile,ImageFile) values('" + imageId + "','" + reportId + "','" + curDate + "','" + imageHTMLFile + "', @pic)";

            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlQuery = ReplaceSpecialCharacters(sqlQuery);
                SaveImage(sqlQuery, paramName, picByte);
                status = true;
            }
            catch (Exception ex)
            {
                //This is temporary solution to handle Fatal error because actual reason in unlnown. It works perfectly next time after error.
                //Modified on 04 Aug 2012
                if (ex.Message.Contains("Fatal error encountered during command execution"))
                {
                    SaveImage(sqlQuery, paramName, picByte);
                }
                else
                {
                    throw;
                }

            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return status;
        }

        /// <summary>
        /// Save image
        /// </summary>
        private void SaveImage(string sqlQuery, string paramName, byte[] picByte)
        {
            MySqlParameter picparameter = new MySqlParameter();
            picparameter.MySqlDbType = MySqlDbType.LongBlob;
            picparameter.ParameterName = paramName; //"@pic";
            picparameter.Value = picByte;
            sqlQuery = ReplaceSpecialCharacters(sqlQuery);
            MySqlCommand command = new MySqlCommand(sqlQuery, this._mySqlConn);
            command.CommandTimeout = 120;
            command.Parameters.Add(picparameter);
            command.ExecuteNonQuery();
            command.Dispose();
            command = null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int KillAllMySQLConnection()
        {
            string query = "SHOW FULL PROCESSLIST";
            //
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(this._connString))
                {
                    // 2. open connection to database using connectionString         
                    mySqlConnection.Open();
                    // 3. Create a command to hold values         
                    MySqlCommand objCmd = new MySqlCommand(query, mySqlConnection);
                    // 4. Add parameters for sqlCommand         
                    MySqlDataReader myReader = objCmd.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            // kill processes with elapsed time > 200 seconds and in Sleep                  
                            if (myReader.GetInt32(5) > 200 & myReader.GetString(4) == "Sleep")
                            {
                                KillMySqlProcess("KILL " + myReader.GetInt32(0));
                            }
                        }
                    }

                    if (myReader != null)
                        myReader.Close();

                    if (mySqlConnection != null)
                        mySqlConnection.Close();
                }
            }
            catch (Exception)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myQuery"></param>
        /// <returns></returns>
        public int KillMySqlProcess(string myQuery)
        {
            //1. Create a query     
            string query = myQuery;
            //     
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(this._connString))
                {
                    // 2. open connection to database using connectionString         
                    mySqlConnection.Open();
                    // 3. Create a command to hold values         
                    MySqlCommand objCmd = new MySqlCommand(query, mySqlConnection);
                    objCmd.ExecuteNonQuery();
                    mySqlConnection.Close();
                    mySqlConnection.Dispose();
                }
            }
            catch (Exception)
            {
                return -1;
            }
            return 0;
        }


        /// <summary>
        /// This accepts date is in format (mm/dd/yyyy) and it retunrs yyyy-mm-dd
        /// </summary>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public string GetMyDBDate(string inputDate)
        {

            string resultDate = "";
            string[] tmpDate;

            try
            {
                tmpDate = inputDate.Split('/');
                if (tmpDate.Length == 3)
                {
                    //Result should be yyyy-mm-dd format
                    resultDate = tmpDate[2] + "-" + tmpDate[0] + "-" + tmpDate[1];
                }

            }
            catch (Exception)
            {
                throw;
            }

            return resultDate;

        }
        /// <summary>
        /// This fuction return current date-time dependes on DB
        /// </summary>
        /// <returns></returns>
        public string GetCurrDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }


        public DataSet GetDataSetWithTwoQuery(string dsQuery, string setQuery)
        {



            DataSet dsRequested = new DataSet();


            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {

                setQuery = ReplaceSpecialCharacters(setQuery);
                MySqlCommand command = new MySqlCommand(setQuery, this._mySqlConn);
                command.ExecuteScalar();

                // command.Parameters.Add("@row", MySqlDbType.Int32).Value = 0;



                dsQuery = ReplaceSpecialCharacters(dsQuery);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(dsQuery, this._mySqlConn);
                //dataAdapter.SelectCommand = command;

                dataAdapter.Fill(dsRequested);
                dataAdapter.Dispose();
                dataAdapter = null;
                command.Dispose();
                command = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return dsRequested;
        }

        /// <summary>
        /// This method use to retrieve LM data from table
        /// </summary>
        /// <returns>returns LM string</returns>
        public string GetLMData()
        {
            string lmData = "";

            MySqlCommand command = null; ;

            try
            {
                //Open DB connection
                OpenDBConnection();

                string query = "Select " + _LICENSE_COL_NAME + " from " + _LICENSE_TABLE_NAME;
                command = new MySqlCommand(query, this._mySqlConn);

                lmData = command.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
                command = null;
                CloseDBConnection();
            }

            return lmData;
        }

        /// <summary>
        /// This method use to set LM data in table
        /// </summary>
        /// <returns>returns true if successful</returns>
        public bool SetLMData(string lmData)
        {
            bool status = true;
            MySqlCommand command = null; ;

            try
            {
                //Open DB connection
                OpenDBConnection();

                string query = "Update " + _LICENSE_TABLE_NAME + " SET " + _LICENSE_COL_NAME + " = '" + lmData + "'";
                int recordUpdated = 0;
                command = new MySqlCommand(query, this._mySqlConn);

                recordUpdated = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                status = false;
                throw;
            }
            finally
            {
                command.Dispose();
                command = null;
                CloseDBConnection();
            }

            return status;
        }


        /// <summary>
        /// Get Date Time Diff in MySql
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string CalculateTimeDiff(string startTime, string endTime)
        {
            DateTime startDT;
            DateTime endDT;
            string result = "";




            string[] tmpDate;
            string[] tmpDateTime;

            try
            {
                tmpDateTime = startTime.Split(' ');
                tmpDate = tmpDateTime[0].Split('-');
                if (tmpDate.Length == 3)
                {
                    //Result should be yyyy-mm-dd format
                    startTime = tmpDate[1] + "/" + tmpDate[2] + "/" + tmpDate[0] + " " + tmpDateTime[1];
                }
                tmpDateTime = endTime.Split(' ');
                tmpDate = tmpDateTime[0].Split('-');
                if (tmpDate.Length == 3)
                {
                    //Result should be yyyy-mm-dd format
                    endTime = tmpDate[1] + "/" + tmpDate[2] + "/" + tmpDate[0] + " " + tmpDateTime[1];
                }

                string format = "G";//"MM/dd/yyyy hh:mm:ss";

                startDT = DateTime.ParseExact(startTime, format, System.Globalization.CultureInfo.InvariantCulture);
                endDT = DateTime.ParseExact(endTime, format, System.Globalization.CultureInfo.InvariantCulture);


                System.TimeSpan timeSpan = endDT.Subtract(startDT);
                result = timeSpan.ToString();

            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Get datetime diff in Minutes
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string TimeDiffInMinutes(string startTime, string endTime)
        {
            DateTime startDT;
            DateTime endDT;
            string result = "";




            string[] tmpDate;
            string[] tmpDateTime;

            try
            {
                tmpDateTime = startTime.Split(' ');
                tmpDate = tmpDateTime[0].Split('-');
                if (tmpDate.Length == 3)
                {
                    //Result should be yyyy-mm-dd format
                    startTime = tmpDate[1] + "/" + tmpDate[2] + "/" + tmpDate[0] + " " + tmpDateTime[1];
                }
                tmpDateTime = endTime.Split(' ');
                tmpDate = tmpDateTime[0].Split('-');
                if (tmpDate.Length == 3)
                {
                    //Result should be yyyy-mm-dd format
                    endTime = tmpDate[1] + "/" + tmpDate[2] + "/" + tmpDate[0] + " " + tmpDateTime[1];
                }

                string format = "G";//"MM/dd/yyyy hh:mm:ss";

                startDT = DateTime.ParseExact(startTime, format, System.Globalization.CultureInfo.InvariantCulture);
                endDT = DateTime.ParseExact(endTime, format, System.Globalization.CultureInfo.InvariantCulture);


                System.TimeSpan timeSpan = endDT.Subtract(startDT);

                result = timeSpan.Minutes.ToString();
            }
            catch
            {
            }
            return result;
        }


        /// <summary>
        /// Update BulkData (datatable data) direct in database without looping 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool UpdateBulkData(DataTable dataTable, String tableName)
        {
            try
            {
                //If connection is not open then create new connection
                if (IsDBConnectionOpen() == false)
                {
                    OpenDBConnection();
                }

                dataTable.TableName = tableName;

                using (MySqlTransaction tran = this._mySqlConn.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = this._mySqlConn;
                        cmd.Transaction = tran;
                        cmd.CommandText = "SELECT * FROM " + tableName;

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.UpdateBatchSize = 1000;
                            using (MySqlCommandBuilder cb = new MySqlCommandBuilder(da))
                            {
                                da.Update(dataTable);
                                tran.Commit();
                            }
                        }
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseDBConnection();
            }
            return true;


        }

        /// <summary>
        /// Update BulkData (dataset data for all BC/BPW/TC) direct in database in single transaction 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <returns>True - if successful / False - if not</returns>
        /// <author> Sameer A. Joshi (P0024) (07 Feb 2020)</author>
        public bool UpdateBulkDataForAll(DataSet dataSet, DataTable dtDelete = null)
        {
            MySqlTransaction tran = null;

            try
            {
                //If connection is not open then create new connection
                if (IsDBConnectionOpen() == false)
                {
                    OpenDBConnection();
                }
                using (tran = this._mySqlConn.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    long xRowsLoaded = 0;

                    // Delete logic is peding

                    foreach (DataTable dataTable in dataSet.Tables)
                    {
                        string tableName = dataTable.TableName;
                        string query = "SELECT * FROM " + tableName;

                        MySqlDataAdapter da = new MySqlDataAdapter(query, this._mySqlConn);
                        MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
                        da.Fill(dataTable);

                        da.UpdateBatchSize = 1000;
                        da.Update(dataTable);
                        xRowsLoaded = dataTable.Rows.Count;
                    }
                    tran.Commit();
                }

            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                CloseDBConnection();
            }
            return true;
        }

        public object BeginTransaction()
        {
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }
            return this._mySqlConn.BeginTransaction(System.Data.IsolationLevel.Serializable);
        }

        public void CommitTransaction(object tran)
        {
            MySqlTransaction tranObj = (MySqlTransaction)tran;
            tranObj.Commit();
        }

        public void RollbackTransaction(object tran)
        {
            MySqlTransaction tranObj = (MySqlTransaction)tran;
            tranObj.Rollback();
        }

        public List<T> ExecuteDatareader<T>(string SqlQuery) where T : new()
        {
            throw new NotImplementedException();
        }

        public async Task<object> BeginTransactionAsync()
        {
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }
            return await this._mySqlConn.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync(object tran)
        {
            MySqlTransaction tranObj = (MySqlTransaction)tran;
            await tranObj.CommitAsync();
        }

        public async Task RollbackTransactionAsync(object tran)
        {
            MySqlTransaction tranObj = (MySqlTransaction)tran;
            await tranObj.RollbackAsync();
        }

        public async Task<List<T>> ExecuteDatareaderAsync<T>(string SqlQuery) where T : new()
        {
            throw new NotImplementedException();
        }
    }
}
