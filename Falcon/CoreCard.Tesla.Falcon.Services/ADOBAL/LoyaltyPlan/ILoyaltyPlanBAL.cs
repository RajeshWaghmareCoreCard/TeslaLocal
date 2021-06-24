using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public  interface ILoyaltyPlanBAL//:IBaseBAL<LoyaltyPlan> 
    {
        //Task<LoyaltyPlan> AddLoyaltyPlanAsync(Guid accountId);

        //void UpdatePurchase(LoyaltyPlan t);

        void UpdatePurchase(LoyaltyPlan t, DBAdapter.IDataBaseCommand dataBaseCommand);
        void Insert(LoyaltyPlan t, DBAdapter.IDataBaseCommand dataBaseCommand);
    }
}