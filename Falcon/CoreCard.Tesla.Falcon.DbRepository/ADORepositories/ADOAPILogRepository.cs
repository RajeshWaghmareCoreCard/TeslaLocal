using CoreCard.Tesla.Falcon.DataModels.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public class ADOAPILogRepository : BaseCockroachADO, IADOAPILogRepository
    {
        public ADOAPILogRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public APILog Add(APILog t)
        {
            throw new NotImplementedException();
        }

        public Task<APILog> AddAsync(APILog t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(APILog entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public APILog Find(Expression<Func<APILog, bool>> match)
        {
            throw new NotImplementedException();
        }

        public APILog Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<APILog> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<APILog> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public APILog GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<APILog> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<APILog> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<APILog>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public APILog Update(APILog t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<APILog> UpdateAsync(APILog t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Insert (APILog t)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            _dbCommand.ExecuteParameterizedNonQuery("insert into apilog(apiname,logtime,response) values (@apiname,@logtime,@response)", dic);
        }
    }
}
