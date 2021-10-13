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
    public class ADOTranInAcctRepository : BaseCockroachADO, IADOTranInAcctRepository
    {
        public ADOTranInAcctRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public Trans_in_Acct Add(Trans_in_Acct t)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            _dbCommand.ExecuteParameterizedNonQuery("insert into trans_in_acct(accountid,tranid,ccregion) values (@accountid,@tranid,@ccregion)", dic);
            return t;
        }

        public Task<Trans_in_Acct> AddAsync(Trans_in_Acct t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Trans_in_Acct entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Trans_in_Acct Find(Expression<Func<Trans_in_Acct, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Trans_in_Acct Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Trans_in_Acct> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Trans_in_Acct> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Trans_in_Acct GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<Trans_in_Acct> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<Trans_in_Acct> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<Trans_in_Acct>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public Trans_in_Acct Update(Trans_in_Acct t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<Trans_in_Acct> UpdateAsync(Trans_in_Acct t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(Trans_in_Acct t)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            _dbCommand.ExecuteParameterizedNonQuery("insert into trans_in_acct(accountid,tranid) values (@accountid,@tranid)", dic);
        }

        public void Insert(Trans_in_Acct t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            dataBaseCommand.ExecuteParameterizedNonQuery("insert into trans_in_acct(accountid,tranid,ccregion) values (@accountid,@tranid,@ccregion)", dic);
        }
    }
}
