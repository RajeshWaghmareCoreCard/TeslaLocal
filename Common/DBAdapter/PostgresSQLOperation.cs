using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBAdapter
{
    public class PostgresSQLOperation : IDataBaseCommand
    {

        // private const string _LICENSE_DB_NAME = "APIAutomationLicense";           //for license database name
        private const string _LICENSE_TABLE_NAME = "License";        //for license table name
        private const string _LICENSE_COL_NAME = "LicenseInfo";          //for license column name
        private string _connString;
        private bool _autoCloseDBConn;
        private NpgsqlConnection _sqlConn = new NpgsqlConnection();

        private string _tempPath = "";

        public PostgresSQLOperation()
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
                //_connString = value;
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
        /// <param name="report_TempPath">This is only for SQlite DB path</param>
        /// <returns></returns>
        public string GenerateConnString(bool windowsAuthentication, string serverName, string port, string dbName, string userId, string password, string connTimeOut, bool withDBName, string report_TempPath = "")
        {

            var connStringBuilder = new NpgsqlConnectionStringBuilder();
            connStringBuilder.Host = serverName;
            connStringBuilder.Port = Convert.ToInt32(port);
            connStringBuilder.TrustServerCertificate = true;// = SslMode.Require;
            connStringBuilder.Username = userId;
            connStringBuilder.Database = dbName;

            return DBOperation.GetEncryptedConnectionString(Convert.ToString(connStringBuilder.ConnectionString));
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
            dbName = "";
            userId = "";
            password = "";
            connTimeOut = "";
            port = "";
            try
            {
                connString = DBOperation.GetDecryptedConnectionString(connString);
                //Split individual info based on ";"
                string[] connStringCollection = connString.ToString().Split(';');

                string[] serverInfo = connStringCollection[0].ToString().Split('=');
                serverName = serverInfo[1].ToString();

                string[] dbInfo = connStringCollection[1].ToString().Split('=');
                dbName = dbInfo[1].ToString();

                string[] UserIdInfo = connStringCollection[2].ToString().Split('=');
                userId = UserIdInfo[1].ToString();

                string[] passwordInfo = connStringCollection[3].ToString().Split('=');
                password = passwordInfo[1].ToString();

                //If Timeout has defined in connection string
                if (connStringCollection.Length == 6)
                {
                    string[] timeOutInfo = connStringCollection[4].ToString().Split('=');
                    connTimeOut = timeOutInfo[1].ToString();
                }
                else if (connStringCollection.Length == 5)
                {
                    string[] timeOutInfo = connStringCollection[3].ToString().Split('=');
                    connTimeOut = timeOutInfo[1].ToString();
                }

                //Considering as Windows Authentication if connection string info length is 3 
                if (connStringCollection.Length == 5)
                {
                    windowsAuthentication = true;
                }

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
                //this._sqlConn = new NpgsqlConnection(this._connString);
                this._sqlConn.ConnectionString = this._connString;
                this._sqlConn.Open();
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
                if (this._sqlConn.State == ConnectionState.Open)
                {
                    this._sqlConn.Close();
                    //this._sqlConn.Dispose();
                    status = true;
                }
            }
            catch (Exception)
            {
                throw;
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
            int retryCount = 0;
            do
            {
                try
                {
                    //If connection is not open then create new connection
                    if (IsDBConnectionOpen() == false)
                    {
                        OpenDBConnection();
                    }



                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sqlQuery, this._sqlConn);
                    dataAdapter.SelectCommand.CommandTimeout = this._sqlConn.ConnectionTimeout;
                    dataAdapter.Fill(dsRequested);
                    dataAdapter.Dispose();
                    dataAdapter = null;
                    break;
                }
                catch (Exception)
                {
                    if (this._autoCloseDBConn)

                    {
                        System.Threading.Thread.Sleep(3000);
                        retryCount = retryCount + 1;
                        if (retryCount > 5)
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
            } while (retryCount <= 5);

            return dsRequested;





        }

        /// <summary>
        /// It returns dataset for parameterized input query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet GetParameterizedDataSet(string sqlQuery, IDictionary<string, object> values)
        {
            DataSet dsRequested = new DataSet();
            int retryCount = 0;
            do
            {
                try
                {
                    //If connection is not open then create new connection
                    if (IsDBConnectionOpen() == false)
                    {
                        OpenDBConnection();
                    }


                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sqlQuery, this._sqlConn);
                    foreach (KeyValuePair<string, object> item in values)
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@" + item.Key, item.Value);
                    }
                    dataAdapter.Fill(dsRequested);
                    dataAdapter.Dispose();
                    dataAdapter = null;
                    break;
                }
                catch (Exception)
                {
                    if (this._autoCloseDBConn)
                    {
                        System.Threading.Thread.Sleep(3000);
                        retryCount = retryCount + 1;
                        if (retryCount > 5)
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
            } while (retryCount <= 5);
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
            int retryCount = 0;

            do
            {
                try
                {
                    //If connection is not open then create new connection
                    if (IsDBConnectionOpen() == false)
                    {
                        OpenDBConnection();
                    }


                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sqlQuery, this._sqlConn);
                    dataAdapter.SelectCommand.CommandTimeout = commandTimeOut;
                    dataAdapter.Fill(dsRequested);
                    dataAdapter.Dispose();
                    dataAdapter = null;
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        throw;
                }
                finally
                {
                    if (this._autoCloseDBConn)
                    {
                        CloseDBConnection();
                    }
                }
            } while (retryCount <= 5);
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

            try
            {
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sqlQuery, this._sqlConn);
                foreach (KeyValuePair<string, object> item in values)
                {
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                dataAdapter.SelectCommand.CommandTimeout = commandTimeOut;
                dataAdapter.Fill(dsRequested);
                dataAdapter.Dispose();
                dataAdapter = null;
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
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlQuery)
        {
            int rowAffected = 0;
            int retryCount = 0;

            do
            {
                try
                {
                    //If connection is not open then create new connection
                    if (IsDBConnectionOpen() == false)
                    {
                        OpenDBConnection();
                    }



                    NpgsqlCommand command = new NpgsqlCommand(sqlQuery, this._sqlConn);
                    command.CommandTimeout = this._sqlConn.ConnectionTimeout;
                    rowAffected = command.ExecuteNonQuery();
                    command.Dispose();
                    command = null;
                    break;
                }
                catch (Exception ex)
                {
                    if (this._autoCloseDBConn)

                    {
                        System.Threading.Thread.Sleep(3000);
                        retryCount = +1;
                        if (retryCount > 5)
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

            } while (retryCount <= 5);
            return rowAffected;


        }

        /// <summary>
        /// It execute query and returns number of row affected, uses parameterized query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int ExecuteParameterizedNonQuery(string sqlQuery, IDictionary<string, object> values)
        {
            int rowAffected = 0;

            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, this._sqlConn);
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
        /// It execute queries in array with transaction
        /// </summary>
        /// <param name="sqlQuery">Array of queries need to be executed in transaction</param>
        /// <returns>Bool value depanding on result</returns>
        /// 
        public bool ExecuteNonQueryWithTransaction(string[] sqlQuery)
        {
            bool rowAffected = false;
            int retryCount = 0;

            do
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(this._connString))
                {
                    NpgsqlTransaction sqlTran = null;
                    try
                    {
                        connection.Open();

                        // Start a local transaction.
                        sqlTran = connection.BeginTransaction();


                        // Enlist the command in the current transaction.
                        NpgsqlCommand command = connection.CreateCommand();
                        command.CommandTimeout = connection.ConnectionTimeout;
                        command.Transaction = sqlTran;





                        for (int i = 0; i < sqlQuery.Length; i++)
                        {
                            command.CommandText = sqlQuery[i];
                            command.ExecuteNonQuery();
                        }
                        rowAffected = true;

                        sqlTran.Commit();
                        break;
                    }
                    catch (Exception)
                    {
                        rowAffected = false;
                        sqlTran.Rollback();
                        retryCount = retryCount + 1;
                        System.Threading.Thread.Sleep(3000);
                        if (retryCount > 5)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            } while (retryCount <= 5);
            return rowAffected;

        }

        /// <summary>
        /// It execute queries in array with transaction
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public bool ExecuteParameterizedNonQueryWithTransaction(
            string[] sqlQuery, IDictionary<string, object> values)
        {
            bool rowAffected = false;

            using (NpgsqlConnection connection = new NpgsqlConnection(this._connString))
            {
                connection.Open();

                // Start a local transaction.
                NpgsqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist the command in the current transaction.
                NpgsqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    for (int i = 0; i < sqlQuery.Length; i++)
                    {
                        command.CommandText = sqlQuery[i];
                        command.Parameters.Clear();
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
            int retryCount = 0;

            do
            {
                try
                {
                    //If connection is not open then create new connection
                    if (IsDBConnectionOpen() == false)
                    {
                        OpenDBConnection();
                    }



                    NpgsqlCommand command = new NpgsqlCommand(sqlQuery, this._sqlConn);
                    command.CommandTimeout = this._sqlConn.ConnectionTimeout;
                    rowAffected = command.ExecuteNonQuery();
                    command.Dispose();
                    command = null;
                    break;
                }
                catch (Exception)
                {

                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        throw;
                }
                finally
                {
                    if (this._autoCloseDBConn)
                    {
                        CloseDBConnection();
                    }
                }
            } while (retryCount <= 5);
            return rowAffected;

        }

        /// <summary>
        /// It execute query and returns number of row affected
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public object ExecuteScalarCommand(string sqlQuery)
        {
            object resultData = null;
            int retryCount = 0;

            do
            {
                //If connection is not open then create new connection
                if (IsDBConnectionOpen() == false)
                {
                    OpenDBConnection();
                }


                try
                {

                    NpgsqlCommand command = new NpgsqlCommand(sqlQuery, this._sqlConn);
                    command.CommandTimeout = this._sqlConn.ConnectionTimeout;
                    resultData = command.ExecuteScalar();
                    command.Dispose();
                    command = null;
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        throw;
                }
                finally
                {
                    if (this._autoCloseDBConn)
                    {
                        CloseDBConnection();
                    }
                }
            } while (retryCount <= 5);
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
                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, this._sqlConn);
                foreach (KeyValuePair<string, object> item in values)
                {
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                resultData = command.ExecuteScalar();
                command.Dispose();
                command = null;
            }
            catch (Exception ex)
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

            if (this._sqlConn.State == ConnectionState.Open)
            {
                status = true;
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
                NpgsqlConnection sqlCon = new NpgsqlConnection(connectionString);
                sqlCon.Open();
                if (sqlCon.State == ConnectionState.Open)
                {
                    isConnected = true;
                    sqlCon.Close();
                    sqlCon.Dispose();
                    sqlCon = null;
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
            connectionString = DBOperation.GetDecryptedConnectionString(connectionString);
            NpgsqlConnection sqlCon = new NpgsqlConnection(connectionString);
            int retryCount = 0;
            do
            {

                try
                {
                    DataTable dbNames = new DataTable();
                    sqlCon = new NpgsqlConnection(connectionString); // SqlCon Object initialized again as sqlCon set to null in Finally block. which gives error while retrying to connect
                    sqlCon.Open();
                    NpgsqlCommand sqlCom = new NpgsqlCommand();
                    sqlCom.Connection = sqlCon;
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandText = "sp_helpdb";
                    sqlCom.CommandTimeout = sqlCon.ConnectionTimeout;
                    NpgsqlDataReader sqlDR;
                    sqlDR = sqlCom.ExecuteReader();

                    while (sqlDR.Read())
                    {
                        dbList.Add(sqlDR.GetString(0));
                    }

                    sqlDR.Dispose();
                    sqlDR = null;
                    sqlCom.Dispose();
                    sqlCom = null;
                    break;

                }
                catch (NpgsqlException)
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        throw;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        throw;
                }
                finally
                {
                    sqlCon.Close();
                    sqlCon = null;
                }
            } while (retryCount <= 5);
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
            NpgsqlCommand sqlComm = null; ;
            NpgsqlDataAdapter sqlDA = null;
            int retryCount = 0;

            do
            {
                //If connection is not open then create new connection
                if (IsDBConnectionOpen() == false)
                {
                    OpenDBConnection();
                }

                try
                {
                    sqlComm = new NpgsqlCommand(cmdText, this._sqlConn);
                    sqlDA = new NpgsqlDataAdapter(sqlComm);
                    sqlDA.Fill(dsData, srcTable);
                    status = true;
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        throw;
                }
                finally
                {
                    sqlComm.Dispose();
                    sqlDA.Dispose();
                    sqlComm = null;
                    sqlDA = null;

                    if (this._autoCloseDBConn)
                    {
                        CloseDBConnection();
                    }
                }

            } while (retryCount <= 5);

            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="values">Parameters to pass to sql query</param>
        /// <param name="srcTable"></param>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public bool ExecuteParameterizedDataSet(string cmdText, IDictionary<string, object> values, string srcTable, DataSet dsData)
        {
            bool status = false;
            NpgsqlCommand sqlComm = null; ;
            NpgsqlDataAdapter sqlDA = null; ;
            //If connection is not open then create new connection
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                sqlComm = new NpgsqlCommand(cmdText, this._sqlConn);
                foreach (KeyValuePair<string, object> item in values)
                {
                    sqlComm.Parameters.Add("@" + item.Key, (NpgsqlDbType)item.Value);
                }
                sqlDA = new NpgsqlDataAdapter(sqlComm);
                sqlDA.Fill(dsData, srcTable);
                status = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlComm.Dispose();
                sqlDA.Dispose();
                sqlComm = null;
                sqlDA = null;

                if (this._autoCloseDBConn)
                {
                    CloseDBConnection();
                }
            }

            return status;
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
            //if (IsDBConnectionOpen() == false)
            //{
            //    OpenDBConnection();
            //}

            //try
            //{
            //    //sqlQuery = sqlQuery.Replace("[", "").Replace("]","");
            //    sqlQuery = sqlQuery.Replace("[", "`").Replace("]", "`");
            //    MyNpgsqlCommand command = new MyNpgsqlCommand(sqlQuery, this._mySqlConn);
            //    rowAffected = command.ExecuteNonQuery();
            //    command.Dispose();
            //    command = null;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    CloseDBConnection();
            //}
            int retryCount = 0;

            do
            {

                try
                {
                    //If connection is not open then create new connection
                    if (IsDBConnectionOpen() == false)
                    {
                        OpenDBConnection();
                    }


                    //sqlQuery = sqlQuery.Replace("[", "").Replace("]","");
                    //sqlQuery = sqlQuery.Replace("[", "`").Replace("]", "`");
                    NpgsqlCommand command = new NpgsqlCommand(sqlQuery, this._sqlConn);
                    rowAffected = command.ExecuteNonQuery();
                    command.Dispose();
                    command = null;
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        throw;
                }
                finally
                {
                    if (this._autoCloseDBConn)
                    {
                        CloseDBConnection();
                    }
                }

            } while (retryCount <= 5);
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
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }

            try
            {
                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, this._sqlConn);
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
        public bool InsertReportImage(string query, string paramName, byte[] picByte)
        {
            bool status = false;
            //string curDate = DateTime.Now.ToString();
            //string query = "insert into ReportImage(imageid,ReportId,CreatedDate,ImageHTMLFile,ImageFile) values('" + imageId + "','" + reportId + "','" + curDate + "','" + imageHTMLFile + "', @pic)";


            int retryCount = 0;


            do
            {
                try
                {
                    if (IsDBConnectionOpen() == false)
                    {
                        OpenDBConnection();
                    }



                    NpgsqlParameter picparameter = new NpgsqlParameter();
                    picparameter.NpgsqlDbType = NpgsqlDbType.Bytea;
                    picparameter.ParameterName = paramName; // "@pic";
                    picparameter.Value = picByte;

                    NpgsqlCommand command = new NpgsqlCommand(query, this._sqlConn);
                    command.CommandTimeout = this._sqlConn.ConnectionTimeout;
                    command.Parameters.Add(picparameter);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    command = null;
                    status = true;
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        throw;
                }
                finally
                {
                    if (this._autoCloseDBConn)
                    {
                        CloseDBConnection();
                    }
                }
            } while (retryCount <= 5);
            return status;
        }

        /// <summary>
        /// This accepts date is in format (mm/dd/yyyy) and retunrs also in same format
        /// </summary>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public string GetMyDBDate(string inputDate)
        {
            return inputDate;
        }
        /// <summary>
        /// This fuction return current date-time dependes on DB
        /// </summary>
        /// <returns></returns>
        public string GetCurrDateTime()
        {
            return DateTime.Now.ToString();
        }

        public DataSet GetDataSetWithTwoQuery(string dsQuery, string setQuery)
        {

            DataSet dsRequested = new DataSet();
            return dsRequested;
            //DataSet dsRequested = new DataSet();
            //object resultData;
            ////If connection is not open then create new connection
            //if (IsDBConnectionOpen() == false)
            //{
            //    OpenDBConnection();
            //}

            //try
            //{
            //    NpgsqlCommand command = new NpgsqlCommand(setQuery, this._sqlConn);
            //    resultData = command.ExecuteScalar();
            //    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(dsQuery, this._sqlConn);
            //    dataAdapter.Fill(dsRequested);
            //    command.Dispose();
            //    command = null;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    if (this._autoCloseDBConn)
            //    {
            //        CloseDBConnection();
            //    }
            //}

            //return dsRequested;

        }

        /// <summary>
        /// This method use to retrieve LM data from table
        /// </summary>
        /// <returns>returns LM string</returns>
        public string GetLMData()
        {
            string lmData = "";

            NpgsqlCommand sqlComm = null; ;

            int retryCount = 0;

            do
            {
                try
                {
                    //Open DB connection
                    OpenDBConnection();

                    string query = "Select " + _LICENSE_COL_NAME + " from " + _LICENSE_TABLE_NAME;
                    sqlComm = new NpgsqlCommand(query, this._sqlConn);

                    lmData = sqlComm.ExecuteScalar().ToString();
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        lmData = "";
                }
                finally
                {
                    sqlComm.Dispose();
                    sqlComm = null;
                    CloseDBConnection();
                }
            } while (retryCount <= 5);
            return lmData;
        }

        /// <summary>
        /// This method use to set LM data in table
        /// </summary>
        /// <returns>returns true if successful</returns>
        public bool SetLMData(string lmData)
        {
            bool status = true;
            NpgsqlCommand sqlComm = null; ;


            int retryCount = 0;

            do
            {
                try
                {
                    //Open DB connection
                    OpenDBConnection();

                    string query = "Update " + _LICENSE_TABLE_NAME + " SET " + _LICENSE_COL_NAME + "= '" + lmData + "'";
                    int recordUpdated = 0;
                    sqlComm = new NpgsqlCommand(query, this._sqlConn);

                    recordUpdated = sqlComm.ExecuteNonQuery();
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(3000);
                    retryCount = retryCount + 1;
                    if (retryCount > 5)
                        status = false;
                }
                finally
                {
                    sqlComm.Dispose();
                    sqlComm = null;
                    CloseDBConnection();
                }
            } while (retryCount <= 5);
            return status;
        }


        /// <summary>
        /// Get datetime diff in MsSql
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string CalculateTimeDiff(string startTime, string endTime)
        {
            DateTime startDT;
            DateTime endDT;
            string result = "";
            try
            {
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

            try
            {
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

            //try
            //{
            //    dataTable.TableName = tableName;

            //    if (IsDBConnectionOpen() == false)
            //    {
            //        OpenDBConnection();
            //    }
            //    long xRowsLoaded = 0;
            //    SqlBulkCopy xBcp = new SqlBulkCopy(this._sqlConn, SqlBulkCopyOptions.Default, null);
            //    xBcp.DestinationTableName = tableName;
            //    xBcp.BatchSize = dataTable.Rows.Count;
            //    xBcp.BulkCopyTimeout = 0;

            //    xBcp.WriteToServer(dataTable);
            //    xRowsLoaded = dataTable.Rows.Count;
            //    xBcp.Close();
            //    this._sqlConn.Close();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    CloseDBConnection();
            //}

            return true;

        }


        /// <summary>
        /// Update BulkData (dataset data for all BC/BPW/TC) direct in database in single transaction 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <returns>True - if successful / False - if not</returns>
        /// <author> Sameer A. Joshi (P0024) (07 Feb 2020)</author>
        public bool UpdateBulkDataForAll(DataSet ds, DataTable dtDelete = null)
        {
            //NpgsqlTransaction tran = null;
            //try
            //{
            //    //If connection is not open then create new connection
            //    if (IsDBConnectionOpen() == false)
            //    {
            //        OpenDBConnection();
            //    }
            //    using (tran = this._sqlConn.BeginTransaction(System.Data.IsolationLevel.Serializable))
            //    {
            //        //long xRowsLoaded = 0;
            //        NpgsqlCommand command = null;
            //        string sqlQuery = string.Empty;
            //        Guid identifierKey = Guid.NewGuid();

            //        #region If delete query is available 
            //        if (dtDelete != null)
            //        {
            //            foreach (DataRow dr in dtDelete.Rows)
            //            {
            //                sqlQuery = Convert.ToString(dr[0]);
            //                command = new NpgsqlCommand(sqlQuery, this._sqlConn, tran);
            //                command.ExecuteNonQuery();
            //            }
            //            command.Dispose();
            //            command = null;
            //        }
            //        #endregion

            //        #region Insert data in database
            //        SqlBulkCopy xBcp = new SqlBulkCopy(this._sqlConn, SqlBulkCopyOptions.Default, tran);
            //        foreach (DataTable dt in ds.Tables)
            //        {
            //            xBcp.DestinationTableName = dt.TableName;
            //            xBcp.BatchSize = dt.Rows.Count;
            //            xBcp.BulkCopyTimeout = 0;

            //            xBcp.WriteToServer(dt);
            //            //xRowsLoaded = dataTable.Rows.Count;
            //            if (dt.TableName.ToLower() == "suite")
            //            {
            //                int suiteID = 0;
            //                sqlQuery = "select id from suite where id =@@IDENTITY ";
            //                command = new NpgsqlCommand(sqlQuery, this._sqlConn, tran);
            //                suiteID = Convert.ToInt32(command.ExecuteScalar());

            //                DataTable table = ds.Tables["SuiteMainBatch"];
            //                int totalRows = table.Rows.Count;
            //                for (int counter = 0; counter < totalRows; counter++)
            //                {
            //                    table.Rows[counter]["suiteID"] = suiteID;
            //                }
            //            }
            //        }

            //        tran.Commit();
            //        xBcp.Close();
            //        #endregion
            //    }
            //}
            //catch (Exception)
            //{
            //    tran.Rollback();
            //    throw;
            //}
            //finally
            //{
            //    CloseDBConnection();
            //}
            return true;
        }

        public object BeginTransaction()
        {
            if (IsDBConnectionOpen() == false)
            {
                OpenDBConnection();
            }
            return this._sqlConn.BeginTransaction();
        }

        public void CommitTransaction(object tran)
        {
            NpgsqlTransaction tranObj = (NpgsqlTransaction)tran;
            tranObj.Commit();
        }

        public void RollbackTransaction(object tran)
        {
            NpgsqlTransaction tranObj = (NpgsqlTransaction)tran;
            tranObj.Rollback();
        }

        public  List<T> ExecuteDatareader<T>(string SqlQuery) where T :new()
        {
            Type businessEntityType = typeof(T);
            List<T> entitys = new List<T>();
            try
            {
                
                Hashtable hashtable = new Hashtable();
                PropertyInfo[] properties = businessEntityType.GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    hashtable[info.Name.ToUpper()] = info;
                }
                if (IsDBConnectionOpen() == false)
                {
                    OpenDBConnection();
                }
                NpgsqlCommand sqlCom = new NpgsqlCommand();
                sqlCom.Connection = this._sqlConn;
                sqlCom.CommandType = CommandType.Text;
                sqlCom.CommandText = SqlQuery;
                sqlCom.CommandTimeout = this._sqlConn.ConnectionTimeout;
                //NpgsqlDataReader dr;
                //sqlDR = sqlCom.ExecuteReader();
                using (NpgsqlDataReader dr = sqlCom.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        T newObject = new T();
                        for (int index = 0; index < dr.FieldCount; index++)
                        {
                            PropertyInfo info = (PropertyInfo)
                                                hashtable[dr.GetName(index).ToUpper()];
                            if ((info != null) && info.CanWrite)
                            {
                                Type propertyType = info.PropertyType;
                                var targetType = IsNullableType(propertyType) ? Nullable.GetUnderlyingType(propertyType) : propertyType;

                                //Returns an System.Object with the specified System.Type and whose value is
                                //equivalent to the specified object.
                                var propertyVal = dr.GetValue(index);
                                propertyVal= Convert.ChangeType(propertyVal, targetType);
                                info.SetValue(newObject, propertyVal, null);
                            }
                        }
                        entitys.Add(newObject);
                    }
                }
                // dr.Close();
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

            return entitys;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
