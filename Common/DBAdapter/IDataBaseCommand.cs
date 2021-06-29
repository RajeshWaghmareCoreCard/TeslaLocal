using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Collections.Generic; //for iDictionary

namespace DBAdapter
{
    public interface IDataBaseCommand
    {
        string ConnString
        { get; set; }

        bool AutoCloseDBConnection
        { get; set; }

        string TempPath
        { get; set; }

        object BeginTransaction();
        void CommitTransaction(object tran);
        void RollbackTransaction(object tran);


        //string GetConnStringData(string connString, bool windowsAuthentication, out string serverName, out string dbName, out string userId, out string password, out string connTimeOut);
        //string GenerateConnString(bool windowsAuthentication, string serverName, string dbName, string userId, string password, string connTimeOut);
        //Added new optional parameter in GenerateConnString to connect from SQLite DB
        string GenerateConnString(bool windowsAuthentication, string serverName, string port, string dbName, string userId, string password, string connTimeOut, bool withDBName, string report_TempPath = "");
        bool ReadConnStringInfo(string connString, out bool windowsAuthentication, out string serverName, out string port, out string dbName, out string userId, out string password, out string connTimeOut);
        bool OpenDBConnection();
        bool CloseDBConnection();
        DataSet GetDataSet(string sqlQuery);
        DataSet GetDataSet(string sqlQuery, int commandTimeOut);
        DataSet GetParameterizedDataSet(string sqlQuery,
                                        IDictionary<string, object> values,
                                        int commandTimeOut);
        int ExecuteNonQuery(string sqlQuery);
        int ExecuteParameterizedNonQuery(string sqlQuery, IDictionary<string, object> values);
        int ExecuteNonQuery(string sqlQuery, int commandTimeOut);
        bool ExecuteNonQueryWithTransaction(string[] sqlQuery);
        bool ExecuteParameterizedNonQueryWithTransaction(string[] sqlQuery, IDictionary<string, object> values);
        ArrayList GetDatabaseNameList(string connectionString);
        bool CheckDatabaseConnection(string connectionString);
        bool ExecuteDataSet(string cmdText, string srcTable, DataSet dsData);
        bool ExecuteParameterizedDataSet(string cmdText, IDictionary<string, object> values, string srcTable, DataSet dsData);
        DataSet GetParameterizedDataSet(string sqlQuery, IDictionary<string, object> values);
        object ExecuteScalarCommand(string sqlQuery);
        object ExecuteParameterizedScalarCommand(string sqlQuery, IDictionary<string, object> values);
        int ExecuteQuery(string sqlQuery);
        int ExecuteParameterizedQuery(string sqlQuery, IDictionary<string, object> values);
        bool InsertReportImage(string query, string paramName, byte[] picByte);
        string GetMyDBDate(string inputDate);
        string GetCurrDateTime();
        //Add new Function for RowNumberin mysql
        DataSet GetDataSetWithTwoQuery(string dsQuery, string setQuery);

        //Method for license Manager
        string GetLMData();
        bool SetLMData(string lmData);
        string CalculateTimeDiff(string startTime, string endTime);
        String TimeDiffInMinutes(string startTime, string endTime);
        bool UpdateBulkData(DataTable dataTable, String tableName);
        bool UpdateBulkDataForAll(DataSet dataSet, DataTable dtDelete = null);

        public List<T> ExecuteDatareader<T>(string SqlQuery) where T : new();

    }
}
