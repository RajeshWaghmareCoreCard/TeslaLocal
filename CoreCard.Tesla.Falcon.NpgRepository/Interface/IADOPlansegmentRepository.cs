using CoreCard.Tesla.Falcon.DataModels.Entity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public interface IADOPlansegmentRepository
    {
        void Add(PlanSegment t);
        void UpdatePlanSegmentWithPayment(List<PlanSegment> planSegments, NpgsqlConnection connection);

        List<PlanSegment> Get(Guid AccountId, NpgsqlConnection connection);
    }
}
