using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface IPlanSegmentBAL //: IBaseBAL<PlanSegment>
    {
        //Task<List<PlanSegment>> GetPlanSegmentByAccountId(Guid AccountID);

        PlanSegment Insert(PlanSegment t);

        void Insert(PlanSegment t, DBAdapter.IDataBaseCommand dbCommand);
        void UpdatePlanSegmentWithPayment(List<PlanSegment> planSegments, DBAdapter.IDataBaseCommand dbCommand);
        List<PlanSegment> GetPlanSegmentsByAccountID_ADO(Guid AccountID, string ccregion, DBAdapter.IDataBaseCommand dbCommand);
    }
}
