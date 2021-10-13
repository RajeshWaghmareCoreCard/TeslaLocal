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
    public class ADOLogArTxnRepository : BaseCockroachADO, IADOLogArTxnRepository
    {
        public ADOLogArTxnRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public LogArTxn Add(LogArTxn t)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            _dbCommand.ExecuteParameterizedNonQuery("insert into logartxn(businessdate,artype,tranid,status) values (@businessdate,@artype,@tranid,@status) Returning logartxnid;", dic);
            return t;

        }
        public void Insert(LogArTxn t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            dataBaseCommand.ExecuteParameterizedNonQuery("insert into logartxn(businessdate,artype,tranid,status,ccregion) values (@businessdate,@artype,@tranid,@status,@ccregion) Returning logartxnid;", dic);
        }
        public Task<LogArTxn> AddAsync(LogArTxn t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(LogArTxn entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public LogArTxn Find(Expression<Func<LogArTxn, bool>> match)
        {
            throw new NotImplementedException();
        }

        public LogArTxn Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<LogArTxn> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<LogArTxn> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public LogArTxn GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<LogArTxn> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<LogArTxn> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<LogArTxn>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public LogArTxn Update(LogArTxn t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<LogArTxn> UpdateAsync(LogArTxn t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
