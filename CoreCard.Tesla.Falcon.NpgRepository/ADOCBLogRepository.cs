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
    public class ADOCBLogRepository : BaseRepository, IADOCBLogRepository
    {
        public ADOCBLogRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }
        public void Insert(CBLog t, NpgsqlConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    using (var cmd = new NpgsqlCommand("insert into cblog(accountid,currentbal,tranid,tranamount,posttime) values (@accountid,@currentbal,@tranid,@tranamount,@posttime) ;",connection))
                    {
                        cmd.Parameters.AddWithValue("accountid", t.accountid);
                        cmd.Parameters.AddWithValue("currentbal", t.currentbal);
                        cmd.Parameters.AddWithValue("tranid", t.tranid);
                        cmd.Parameters.AddWithValue("tranamount", t.tranamount);
                        cmd.Parameters.AddWithValue("posttime", t.posttime);
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
