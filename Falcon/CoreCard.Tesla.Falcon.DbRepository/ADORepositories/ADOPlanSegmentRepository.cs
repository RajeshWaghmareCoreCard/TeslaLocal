using CoreCard.Tesla.Falcon.DataModels.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public class ADOPlanSegmentRepository : BaseCockroachADO, IADOPlansegmentRepository
    {
        public ADOPlanSegmentRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public PlanSegment Add(PlanSegment t)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            object uuid = _dbCommand.ExecuteParameterizedScalarCommand("insert into plansegment(plantype,creationtime,purchaseamount,principal,fees,interest,purchasecount) values (@plantype,@creationtime,@purchaseamount,@principal,@fees,@interest,@purchasecount)  Returning planid;", dic);
            return Get((Guid)uuid);
        }

        public void Add(PlanSegment t, DBAdapter.IDataBaseCommand dbCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            object uuid = dbCommand.ExecuteParameterizedScalarCommand("insert into plansegment(plantype,creationtime,purchaseamount,principal,fees,interest,purchasecount) values (@plantype,@creationtime,@purchaseamount,@principal,@fees,@interest,@purchasecount)  Returning planid;", dic);
        }


        public Task<PlanSegment> AddAsync(PlanSegment t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(PlanSegment entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public PlanSegment Find(Expression<Func<PlanSegment, bool>> match)
        {
            throw new NotImplementedException();
        }

        public PlanSegment Get(Guid id)
        {
            string sql = "SELECT * FROM plansegment where planid ='" + id + "'";

            DataSet ds = _dbCommand.GetDataSet(sql);

            PlanSegment acc = new PlanSegment();
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                acc = (PlanSegment)dataRow;
            }
            return acc;
        }
        public List<PlanSegment> Get(Guid id, string idType)
        {
            string sql = "SELECT * FROM plansegment where planid ='" + id + "'";
            if (idType.ToLower().Equals("account"))
            {
                sql = "SELECT * FROM plansegment where accountid ='" + id + "'";
            }
            DataSet ds = _dbCommand.GetDataSet(sql);

            List<PlanSegment> acc = new List<PlanSegment>();
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                acc.Add((PlanSegment)dataRow);
            }
            return acc;
        }

        public List<PlanSegment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<PlanSegment> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public PlanSegment GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<PlanSegment> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<PlanSegment> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlanSegment>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public PlanSegment Update(PlanSegment t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<PlanSegment> UpdateAsync(PlanSegment t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlanSegmentWithPayment(List<PlanSegment> planSegments, DBAdapter.IDataBaseCommand dbCommand)
        {
            foreach (PlanSegment p in planSegments)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("update plansegment set ");
                sb.Append(string.Format("fees = {0}, ", p.fees));
                sb.Append(string.Format("interest = {0}, ", p.interest));
                sb.Append(string.Format("principal = {0} ", p.principal));
                sb.Append(" where planid = '" + p.planid.ToString() + "';");
                dbCommand.ExecuteNonQuery(sb.ToString());
            }
        }
    }
}
