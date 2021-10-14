using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOPlansegmentRepository:IADOCockroachDBRepository<PlanSegment>
    {
        void Add(PlanSegment t, DBAdapter.IDataBaseCommand dbCommand);
        void UpdatePlanSegmentWithPayment(List<PlanSegment> planSegments, DBAdapter.IDataBaseCommand dbCommand);

        List<PlanSegment> Get(Guid AccountId, string ccregion, DBAdapter.IDataBaseCommand dbCommand);
    }
}
