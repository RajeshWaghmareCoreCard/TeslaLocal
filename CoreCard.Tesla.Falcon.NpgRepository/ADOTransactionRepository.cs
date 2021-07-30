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
    public class ADOTransactionRepository : BaseRepository, IADOTransactionRepository
    {
        public ADOTransactionRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {
        }

        public Guid Add(Transaction t, NpgsqlConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    using (var cmd = new NpgsqlCommand("insert into transaction(accountid,trantype,trancode,trantime,amount,cardnumber) values (@accountid,@trantype,@trancode,@trantime,@amount,@cardnumber) Returning tranid;",connection))
                    {
                        //cmd.Connection = connection;

                        //cmd.CommandText = "insert into transaction(accountid,trantype,trancode,trantime,amount,cardnumber) values (@accountid,@trantype,@trancode,@trantime,@amount,@cardnumber) Returning tranid;";
                        cmd.Parameters.AddWithValue("accountid", t.accountid);
                        cmd.Parameters.AddWithValue("trantype", t.trantype);
                        cmd.Parameters.AddWithValue("trancode", t.trancode);
                        cmd.Parameters.AddWithValue("trantime", t.trantime);
                        cmd.Parameters.AddWithValue("amount", t.amount);
                        cmd.Parameters.AddWithValue("cardnumber", t.cardnumber);


                        object guid = cmd.ExecuteScalar(); ;
                        return (Guid)guid;
                        //cmd.Parameters.AddWithValue("Account_Id", accountId);
                    }
                }
                else
                {
                    return Guid.Empty;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
