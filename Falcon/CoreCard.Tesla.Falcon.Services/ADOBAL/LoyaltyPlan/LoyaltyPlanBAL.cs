using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
//using CockroachDb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public class LoyaltyPlanBAL :ILoyaltyPlanBAL
    {
        //private readonly ILoyaltyPlanRepository _loyaltyRepository;
        private readonly IADOLoyaltyPlanRepository _aDOLoyaltyPlanRepository;
        public LoyaltyPlanBAL(IADOLoyaltyPlanRepository aDOLoyaltyPlanRepository)// : base(loyaltyRepository)
        {
            //_loyaltyRepository = loyaltyRepository;
            _aDOLoyaltyPlanRepository = aDOLoyaltyPlanRepository;
        }

        //public async Task<LoyaltyPlan> AddLoyaltyPlanAsync(Guid accountId)
        //{
        //    LoyaltyPlan loyaltyPlan = new LoyaltyPlan();
        //    //loyaltyPlan.loyaltyplanid = Guid.NewGuid();
        //    loyaltyPlan.accountid = accountId;
        //    loyaltyPlan.loyaltyplantype = 0;
        //    loyaltyPlan.rewardbal = 0;
        //    return await _loyaltyRepository.AddAsync(loyaltyPlan);
        //}


        //public void UpdatePurchase(LoyaltyPlan t)
        //{
        //    _aDOLoyaltyPlanRepository.UpdatePurchase(t);
        //}

        public void UpdatePurchase(LoyaltyPlan t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            _aDOLoyaltyPlanRepository.UpdatePurchase(t, dataBaseCommand);
        }

        public void Insert(LoyaltyPlan t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            _aDOLoyaltyPlanRepository.Insert(t, dataBaseCommand);
        }
    }
}
