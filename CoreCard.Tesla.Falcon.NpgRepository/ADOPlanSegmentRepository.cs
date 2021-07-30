using CoreCard.Tesla.Falcon.DataModels.Entity;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public class ADOPlanSegmentRepository : BaseRepository, IADOPlansegmentRepository
    {
        public ADOPlanSegmentRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {
        }

        public void Add(PlanSegment t)
        {
            throw new NotImplementedException();
        }

        public List<PlanSegment> Get(Guid AccountId, NpgsqlConnection connection)
        {
            int retryCount = 0;
            List<PlanSegment> planSegments = new List<PlanSegment>();

            do
            {
                try
                {
                    StringBuilder strQry = new StringBuilder();
                    strQry.Append("SELECT planid, ifnull(accountid,'00000000-0000-0000-0000-000000000000') as accountid");
                    strQry.Append(", ifnull(plantype,0) as plantype, CreationTime,ifnull(currentbal,0)as currentbal ,ifnull(principal,0)as principal,ifnull(purchaseamount,0)as purchaseamount");
                    strQry.Append(", ifnull(fees,0)as fees,ifnull(interest,0)as interest,ifnull(purchasecount,0)as purchasecount");
                    strQry.Append(" ,ifnull(paymentamount,0)as paymentamount ");
                    strQry.Append(" FROM plansegment where accountid ='" + AccountId.ToString() + "' for update;");
                    if (connection.State == ConnectionState.Open)
                    {
                        using (var cmd = new NpgsqlCommand(strQry.ToString(), connection))
                        {
                            //cmd.Connection = connection;


                            //cmd.CommandText = strQry.ToString();
                            //cmd.Parameters.AddWithValue("Account_Id", accountId);
                            using (var reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    PlanSegment planSegment = new PlanSegment();
                                    planSegment.planid = (Guid)reader["planid"];
                                    planSegment.accountid = (Guid)reader["accountid"];
                                    planSegment.plantype = Convert.ToInt32(reader["plantype"]);
                                    planSegment.creationtime = Convert.ToDateTime(reader["creationtime"]);
                                    planSegment.currentbal = Convert.ToDecimal(reader["currentbal"]);
                                    planSegment.principal = Convert.ToDecimal(reader["principal"]);
                                    planSegment.purchaseamount = Convert.ToDecimal(reader["purchaseamount"]);
                                    planSegment.fees = Convert.ToDecimal(reader["fees"]);
                                    planSegment.interest = Convert.ToDecimal(reader["interest"]);
                                    planSegment.purchasecount = Convert.ToInt32(reader["purchasecount"]);
                                    planSegment.paymentamount = Convert.ToDecimal(reader["paymentamount"]);
                                    planSegments.Add(planSegment);
                                }
                                return planSegments;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (TimeoutException ex)
                {

                    if (retryCount <= 5)
                    {
                        //Thread.Sleep(50);
                        for (int i = 0; i <= 10000; i++)
                        {
                            //waiting for loop;
                        }
                    }
                    else
                        throw ex;
                    retryCount++;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Trim().ToLower() == "exception while reading from stream")
                    {
                        if (retryCount <= 5)
                        {
                            for (int i = 0; i <= 10000; i++)
                            {
                                //waiting for loop;
                            }
                        }
                        else
                        {
                            retryCount = -1;
                            throw ex;
                        }
                    }
                    else
                    {
                        retryCount = -1;
                        throw ex;
                    }
                    retryCount++;
                }
            } while (retryCount > 0 && retryCount <= 5);
            return planSegments;
        }

        public void UpdatePlanSegmentWithPayment(List<PlanSegment> planSegments, NpgsqlConnection connection)
        {
            try
            {

                foreach (PlanSegment p in planSegments)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("update plansegment set ");
                    sb.Append(string.Format("fees = {0}, ", p.fees));
                    sb.Append(string.Format("interest = {0}, ", p.interest));
                    sb.Append(string.Format("principal = {0} ", p.principal));
                    sb.Append(" where planid = '" + p.planid.ToString() + "';");
                    if (connection.State == ConnectionState.Open)
                    {
                        using (var cmd = new NpgsqlCommand(sb.ToString(), connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        //cmd.Parameters.AddWithValue("Account_Id", accountId);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
