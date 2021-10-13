using DBAdapter;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Entity;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public class ADOAddressRepository : BaseCockroachADO, IADOAddressRepository
    {
        public ADOAddressRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public Address Add(Address t)
        {
            throw new NotImplementedException();
        }

        public Task<Address> AddAsync(Address t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Address entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Address Find(Expression<Func<Address, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Address Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Address> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Address> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Address GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<Address> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<Address> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<Address>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(Address t, IDataBaseCommand databaseCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            databaseCommand.ExecuteParameterizedNonQuery("insert into address(housenumber, street, city, state, zipcode, customerid, addresstype,ccregion) values (@housenumber, @street, @city, @state, @zipcode, @customerid, @addresstype,@ccregion) ", dic);
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

        public Address Update(Address t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<Address> UpdateAsync(Address t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
