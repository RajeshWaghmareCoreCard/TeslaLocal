using CoreCard.Tesla.Falcon.DataModels.Entity;
using DBAdapter;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public class ADOTransactionRepository : BaseCockroachADO, IADOTransactionRepository
    {
        public ADOTransactionRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public Transaction Add(Transaction t)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            object uuid = _dbCommand.ExecuteParameterizedScalarCommand("insert into transaction(accountid,trantype,trancode,trantime,amount,cardnumber) values (@accountid,@trantype,@trancode,@trantime,@amount,@cardnumber) Returning tranid;", dic);
            return Get((Guid)uuid);

        }
        public Guid Add(Transaction t, IDataBaseCommand dbCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            object uuid = dbCommand.ExecuteParameterizedScalarCommand("insert into transaction(accountid,trantype,trancode,trantime,amount,cardnumber,ccregion) values (@accountid,@trantype,@trancode,@trantime,@amount,@cardnumber,@ccregion) Returning tranid;", dic);
            return (Guid)uuid;
        }
        public Task<Transaction> AddAsync(Transaction t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Transaction Find(Expression<Func<Transaction, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Transaction Get(Guid id)
        {
            string sql = "SELECT * FROM transaction where tranid ='" + id + "'";

            DataSet ds = _dbCommand.GetDataSet(sql);

            Transaction acc = new Transaction();
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                acc = (Transaction)dataRow;
            }
            return acc;
        }

        public List<Transaction> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Transaction GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<Transaction>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void RejectChanges()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Transaction Update(Transaction t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> UpdateAsync(Transaction t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
