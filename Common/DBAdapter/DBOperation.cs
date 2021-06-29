using System;
using System.Collections.Generic;
using System.Text;


namespace DBAdapter
{
    public class DBOperation
    {
        /// <summary>
        /// Enum of DB type
        /// </summary>
        //public enum DBType
        //{
        //    MSSQL,
        //    MySQL
        //}

        /// <summary>
        /// Return object of requested DB class
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        //public static IDataBaseCommand GetDBObject(DBType dbType)
        //{
        //    IDataBaseCommand dbCommand = null; 
        //    switch (dbType)
        //    {
        //        case DBType.MSSQL:
        //            dbCommand = new MSSQLOperation();
        //             break;
        //         case DBType.MySQL:
        //             dbCommand = new MySQLOperation();
        //             break;
        //        default:
        //            throw new ArgumentException(string.Format("Database type {0} cannot be found.", Enum.GetName (typeof(DBType), dbType)));
        //    }

        //    return dbCommand;
        //}

        public static IDataBaseCommand GetDBObject(string dbTyp)
        {
            IDataBaseCommand dbCommand = null;
            switch (dbTyp.ToLower())
            {
                case "mssql":
                    dbCommand = new MSSQLOperation();
                    break;
                case "mysql":
                    dbCommand = new MySQLOperation();
                    break;
                case "sqlite":
                    dbCommand = new SQLiteOperation();
                    break;
                case "postgres":
                    dbCommand = new PostgresSQLOperation();
                    break;
                default:
                    //throw new ArgumentException(string.Format("Database type {0} cannot be found.", Enum.GetName(typeof(DBType), dbTyp)));
                    throw new ArgumentException(string.Format("Database type {0} cannot be found.", dbTyp));
            }
            return dbCommand;
        }
        public static string GetDecryptedConnectionString(string EncryptedConn)
        {
            Encrypt.Encrypt encrypt = new Encrypt.Encrypt();
            string str = null;
            try
            {
                str = Convert.ToString(encrypt.QuickDecrypt(EncryptedConn, "MagicKey"));
            }
            catch (Exception)
            {
                return EncryptedConn;
            }
            return str;
        }

        public static string GetEncryptedConnectionString(string DecryptedConn)
        {
            Encrypt.Encrypt encrypt = new Encrypt.Encrypt();
            string str = null;
            try
            {
                str = Convert.ToString(encrypt.QuickEncrypt(DecryptedConn, "MagicKey"));
            }
            catch (Exception)
            {
                return DecryptedConn;
            }
            return str;
        }
    }
}
