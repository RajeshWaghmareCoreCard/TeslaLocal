using DBAdapter;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public class BaseCockroachADO : IBaseCockroachADO
    {
        protected IDataBaseCommand _dbCommand;

        protected object _TranObject;
        protected IConfiguration _configuration;
        protected string ConnectionString { get; set; }

        public BaseCockroachADO(IConfiguration configuration)
        {
            _configuration = configuration;
            // _dbCommand = DBAdapter.DBOperation.GetDBObject("postgres");
            ConnectionString = configuration.GetConnectionString("CockroachDb");
        }

        public Tuple<IDataBaseCommand, object> BeginTransaction()
        {
            IDataBaseCommand dataBaseCommand = new DBOperation().GetDBObject("postgres");
            dataBaseCommand.ConnString = ConnectionString;
            dataBaseCommand.AutoCloseDBConnection = false;
            _TranObject = dataBaseCommand.BeginTransaction();
            return Tuple.Create(dataBaseCommand, _TranObject);
        }

        public void CommitTransaction(IDataBaseCommand dbcommand, object tran)
        {
            dbcommand.CommitTransaction(tran);
            dbcommand.CloseDBConnection();
        }

        public void RollbackTransaction(IDataBaseCommand dbcommand, object tran)
        {
            dbcommand.RollbackTransaction(tran);
            dbcommand.CloseDBConnection();
        }

        public async Task<Tuple<IDataBaseCommand, object>> BeginTransactionAsync()
        {
            IDataBaseCommand dataBaseCommand = new DBOperation().GetDBObject("postgres");
            dataBaseCommand.ConnString = ConnectionString;
            dataBaseCommand.AutoCloseDBConnection = false;
            _TranObject = await dataBaseCommand.BeginTransactionAsync();
            return Tuple.Create(dataBaseCommand, _TranObject);
        }

        public async Task CommitTransactionAsync(IDataBaseCommand dbcommand, object tran)
        {
            await dbcommand.CommitTransactionAsync(tran);
            dbcommand.CloseDBConnection();
        }

        public async Task RollbackTransactionAsync(IDataBaseCommand dbcommand, object tran)
        {
            await dbcommand.RollbackTransactionAsync(tran);
            dbcommand.CloseDBConnection();
        }

        public async Task SavePointAsync(IDataBaseCommand dbcommand, object tran, string savepoint)
        {
            await dbcommand.SavePointAsync(tran, savepoint);
            //dbcommand.CloseDBConnection();
        }

        public async Task RollbackTransactionAsync(IDataBaseCommand dbcommand, object tran, string savepoint)
        {
            await dbcommand.RollbackTransactionAsync(tran, savepoint);
            //dbcommand.CloseDBConnection();
        }

        public bool OpenConnection()
        {
            try
            {
                _dbCommand = new DBOperation().GetDBObject("postgres");
                _dbCommand.ConnString = ConnectionString;
                _dbCommand.OpenDBConnection();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool CloseConnection()
        {
            try
            {
                _dbCommand.CloseDBConnection();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
