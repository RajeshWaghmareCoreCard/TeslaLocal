using CoreCard.Tesla.Falcon.DataModels.Entity;
using DBAdapter;
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
    public class ADOCustomerRepository: BaseCockroachADO,IADOCustomerRepository
    {
        public ADOCustomerRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Customer Add(Customer t)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            object uuid = _dbCommand.ExecuteParameterizedScalarCommand("insert into transaction(accountid,trantype,trancode,trantime,amount,cardnumber) values (@accountid,@trantype,@trancode,@trantime,@amount,@cardnumber) Returning tranid;", dic);
            return Get((Guid)uuid);
        }

        public Task<Customer> AddAsync(Customer t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Customer Find(Expression<Func<Customer, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Customer Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Customer GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public Customer Update(Customer t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> UpdateAsync(Customer t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Guid Insert(Customer t, IDataBaseCommand dbcommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            object uuid = dbcommand.ExecuteParameterizedScalarCommand("insert into customer(ssn,firstname,lastname) values (@ssn,@firstname,@lastname) Returning customerid;", dic);
            return (Guid)uuid;
        }
    }
}
