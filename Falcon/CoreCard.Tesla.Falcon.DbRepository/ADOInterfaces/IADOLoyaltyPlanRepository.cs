using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOLoyaltyPlanRepository:IADOCockroachDBRepository<LoyaltyPlan>
    {
        void UpdatePurchase(LoyaltyPlan t);
        void UpdatePurchase(LoyaltyPlan t, DBAdapter.IDataBaseCommand dataBaseCommand);
        void Insert(LoyaltyPlan t, DBAdapter.IDataBaseCommand dataBaseCommand);
    }
}
