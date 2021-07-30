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
    public class ADOTranInAcctRepository : BaseRepository, IADOTranInAcctRepository
    {
        public ADOTranInAcctRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {
        }

        public void Insert(Trans_in_Acct t, NpgsqlConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    using (var cmd = new NpgsqlCommand("insert into trans_in_acct(accountid,tranid) values (@accountid,@tranid)",connection))
                    {
                      //  cmd.Connection = connection;

                       // cmd.CommandText = ;
                        cmd.Parameters.AddWithValue("accountid", t.accountid);
                        cmd.Parameters.AddWithValue("tranid", t.tranid);
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
