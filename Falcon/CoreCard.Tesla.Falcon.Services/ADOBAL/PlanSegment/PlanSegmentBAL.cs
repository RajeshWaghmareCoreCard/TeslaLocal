using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
//using CockroachDb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCard.Tesla.Utilities;
using System.Data.SqlClient;
using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Utilities;

namespace CoreCard.Tesla.Falcon.Services
{
    public class PlanSegmentBAL : /*BaseBAL<PlanSegment>,*/ IPlanSegmentBAL
    {
       // private readonly IPlanSegmentRepository _planSegmentRepository;
        private readonly TimeLogger _timeLogger;
        private readonly IADOPlansegmentRepository _iADOPlansegmentRepository;
        public PlanSegmentBAL(TimeLogger timeLogger, IADOPlansegmentRepository iADOPlansegmentRepository)// : base(planSegmentRepository)
        {
            _timeLogger = timeLogger;
            _iADOPlansegmentRepository = iADOPlansegmentRepository;
        }

        //public async Task<List<PlanSegment>> GetPlanSegmentByAccountId(Guid AccountID)
        //{
        //    string query = "Select  * FROM PlanSegment WHERE accountid='" + AccountID.ToString() + "' Order by creationtime";
        //    //SqlParameter parameterS = new SqlParameter("@AccountNumber", AccountNumber);
        //    List<PlanSegment> plansegments = await base.GetEntityListAsync(query);
        //    return plansegments;
        //}

        public PlanSegment Insert(PlanSegment t)
        {
           return _iADOPlansegmentRepository.Add(t);
        }

        public void Insert(PlanSegment t,DBAdapter.IDataBaseCommand dbcommand)
        {
            _iADOPlansegmentRepository.Add(t, dbcommand);
        }

        public List<PlanSegment> GetPlanSegmentsByAccountID_ADO(Guid AccountID, string ccregion, DBAdapter.IDataBaseCommand dbCommand)
        {
            return _iADOPlansegmentRepository.Get(AccountID, ccregion, dbCommand);
        }
        public void UpdatePlanSegmentWithPayment(List<PlanSegment> planSegments, DBAdapter.IDataBaseCommand dbCommand)
        {
            _iADOPlansegmentRepository.UpdatePlanSegmentWithPayment(planSegments, dbCommand);
        }
    }
}
