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
    public class ADOLoyaltyPlanRepository : BaseCockroachADO, IADOLoyaltyPlanRepository
    {
        public ADOLoyaltyPlanRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public LoyaltyPlan Add(LoyaltyPlan t)
        {
            throw new NotImplementedException();
        }

        public Task<LoyaltyPlan> AddAsync(LoyaltyPlan t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(LoyaltyPlan entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public LoyaltyPlan Find(Expression<Func<LoyaltyPlan, bool>> match)
        {
            throw new NotImplementedException();
        }

        public LoyaltyPlan Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<LoyaltyPlan> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<LoyaltyPlan> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public LoyaltyPlan GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<LoyaltyPlan> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<LoyaltyPlan> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<LoyaltyPlan>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public LoyaltyPlan Update(LoyaltyPlan t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<LoyaltyPlan> UpdateAsync(LoyaltyPlan t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void UpdatePurchase(LoyaltyPlan t)
        {
            _dbCommand.ExecuteNonQuery("update loyaltyplan set rewardbal = " + t.rewardbal + " where accountid = '" + t.accountid + "'");
        }

        public void UpdatePurchase(LoyaltyPlan t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            dataBaseCommand.ExecuteNonQuery("Select * from loyaltyplan where ccregion='" + t.ccregion + "' and  accountid='" + t.accountid + "' FOR UPDATE; update loyaltyplan set rewardbal = " + t.rewardbal + " where ccregion='" + t.ccregion + "' and accountid = '" + t.accountid + "';");
        }

        public void Insert(LoyaltyPlan t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            dataBaseCommand.ExecuteParameterizedNonQuery("insert into loyaltyplan(accountid,loyaltyplantype,rewardbal,ccregion) values (@accountid,@loyaltyplantype,@rewardbal,@ccregion)", dic);
        }
    }
}
