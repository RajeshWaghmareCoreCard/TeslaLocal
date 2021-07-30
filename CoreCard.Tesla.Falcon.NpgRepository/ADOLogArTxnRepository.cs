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
    public class ADOLogArTxnRepository : BaseRepository, IADOLogArTxnRepository
    {
        public ADOLogArTxnRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {
        }

        public void Insert(LogArTxn t, NpgsqlConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    using (var cmd = new NpgsqlCommand("insert into logartxn(businessdate,artype,tranid,status) values (@businessdate,@artype,@tranid,@status);", connection))
                    {
                        cmd.Parameters.AddWithValue("businessdate", t.businessdate);
                        cmd.Parameters.AddWithValue("artype", t.artype);
                        cmd.Parameters.AddWithValue("tranid", t.tranid);
                        cmd.Parameters.AddWithValue("status", t.status);
                        cmd.ExecuteNonQuery(); ;
                        //cmd.Parameters.AddWithValue("Account_Id", accountId);
                    }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
