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
    public class ADOCBLogRepository : BaseCockroachADO, IADOCBLogRepository
    {
        public ADOCBLogRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public CBLog Add(CBLog t)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            _dbCommand.ExecuteParameterizedNonQuery("insert into cblog(accountid,currentbal,tranid,tranamount,posttime,) values (@accountid,@currentbal,@tranid,@tranamount,@posttime) Returning cblogid;", dic);
            //return Get((Guid)uuid);
            return t;
        }
        public void Insert(CBLog t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            dataBaseCommand.ExecuteParameterizedNonQuery("insert into cblog(accountid,currentbal,tranid,tranamount,posttime,ccregion) values (@accountid,@currentbal,@tranid,@tranamount,@posttime,@ccregion) Returning cblogid;", dic);

        }
        public Task<CBLog> AddAsync(CBLog t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(CBLog entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public CBLog Find(Expression<Func<CBLog, bool>> match)
        {
            throw new NotImplementedException();
        }

        public CBLog Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<CBLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CBLog> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public CBLog GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<CBLog> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<CBLog> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<CBLog>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public CBLog Update(CBLog t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<CBLog> UpdateAsync(CBLog t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
