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
    public class ADOAccountRepository : BaseRepository, IADOAccountRepository
    {
        public ADOAccountRepository(IDatabaseConnectionResolver databaseConnection) : base(databaseConnection)
        {

        }
        public Account Get(ulong AccountNumber)
        {
            try
            {
                using (NpgsqlConnection connection = databaseConnection.GetNpgsqlConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.Transaction = connection.BeginTransaction();
                        StringBuilder strQry = new StringBuilder();
                        strQry.Append("SELECT accountid, accountnumber,ifnull(customerid,'00000000-0000-0000-0000-000000000000') as customerid");
                        strQry.Append(", creditlimit,ifnull(currentbal,0)as currentbal ,ifnull(principal,0)as principal,ifnull(purchaseamount,0)as purchaseamount");
                        strQry.Append(", ifnull(fees,0)as fees,ifnull(interest,0)as interest,ifnull(purchasecount,0)as purchasecount");
                        strQry.Append(" ,ifnull(paymentamount,0)as paymentamount,ifnull(paymentcount,0)as paymentcount, ifnull(status,0)as status ");
                        strQry.Append(" FROM Account where accountnumber =" + AccountNumber.ToString() + " for update;");

                        cmd.CommandText = strQry.ToString();
                        //cmd.Parameters.AddWithValue("Account_Id", accountId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            Account account = new Account();
                            while (reader.Read())
                            {
                                account.accountid = (Guid)reader["accountid"];
                                account.accountnumber = Convert.ToInt64(reader["accountnumber"]);
                                account.customerid = (Guid)reader["customerid"];
                                account.creditlimit = Convert.ToDecimal(reader["creditlimit"]);
                                account.currentbal = Convert.ToDecimal(reader["currentbal"]);
                                account.principal = Convert.ToDecimal(reader["principal"]);
                                account.purchaseamount = Convert.ToDecimal(reader["purchaseamount"]);
                                account.purchasecount = Convert.ToInt64(reader["purchasecount"]);
                                account.fees = Convert.ToDecimal(reader["fees"]);
                                account.interest = Convert.ToDecimal(reader["interest"]);
                                account.paymentamount = Convert.ToDecimal(reader["paymentamount"]);
                                account.paymentcount = Convert.ToInt64(reader["paymentcount"]);
                                account.status = Convert.ToInt64(reader["status"]);
                            }
                            return account;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Account GetAccountByID(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountByNumber(long AccountNumber, NpgsqlConnection connection)
        {
            Account account = new Account();
            int retryCount = 0;

            do
            {
                try
                {
                    StringBuilder strQry = new StringBuilder();
                    strQry.Append("SELECT accountid, accountnumber,ifnull(customerid,'00000000-0000-0000-0000-000000000000') as customerid");
                    strQry.Append(", creditlimit,ifnull(currentbal,0)as currentbal ,ifnull(principal,0)as principal,ifnull(purchaseamount,0)as purchaseamount");
                    strQry.Append(", ifnull(fees,0)as fees,ifnull(interest,0)as interest,ifnull(purchasecount,0)as purchasecount");
                    strQry.Append(" ,ifnull(paymentamount,0)as paymentamount,ifnull(paymentcount,0)as paymentcount, ifnull(status,0)as status ");
                    strQry.Append(" FROM Account where accountnumber =" + AccountNumber.ToString() + " for update;");
                    if (connection.State == ConnectionState.Open)
                    {
                        using (var cmd = new NpgsqlCommand(strQry.ToString(), connection))
                        {
                            // cmd.Connection = connection;

                            //cmd.CommandText = strQry.ToString();
                            //cmd.Parameters.AddWithValue("Account_Id", accountId);
                            using (var reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    account.accountid = (Guid)reader["accountid"];
                                    account.accountnumber = Convert.ToInt64(reader["accountnumber"]);
                                    account.customerid = (Guid)reader["customerid"];
                                    account.creditlimit = Convert.ToDecimal(reader["creditlimit"]);
                                    account.currentbal = Convert.ToDecimal(reader["currentbal"]);
                                    account.principal = Convert.ToDecimal(reader["principal"]);
                                    account.purchaseamount = Convert.ToDecimal(reader["purchaseamount"]);
                                    account.purchasecount = Convert.ToInt64(reader["purchasecount"]);
                                    account.fees = Convert.ToDecimal(reader["fees"]);
                                    account.interest = Convert.ToDecimal(reader["interest"]);
                                    account.paymentamount = Convert.ToDecimal(reader["paymentamount"]);
                                    account.paymentcount = Convert.ToInt64(reader["paymentcount"]);
                                    account.status = Convert.ToInt64(reader["status"]);
                                }
                                return account;
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
                        throw;
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
                            throw;
                        }
                    }
                    else
                    {
                        retryCount = -1;
                        throw;
                    }
                    retryCount++;
                }
            } while (retryCount > 0 && retryCount <= 5);
            return account;
        }

        public Guid Insert(Account t)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccountWithPayment(Account t, NpgsqlConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("update account set ");
                    sb.Append(string.Format("currentbal = {0}, ", t.currentbal));
                    sb.Append(string.Format("paymentamount = {0}, ", t.paymentamount));
                    sb.Append(string.Format("paymentcount = {0} ", t.paymentcount));
                    sb.Append(" where accountid = '" + t.accountid.ToString() + "';");
                    using (var cmd = new NpgsqlCommand(sb.ToString(), connection))
                    {
                        cmd.ExecuteNonQuery();
                        //cmd.Parameters.AddWithValue("Account_Id", accountId);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Account UpdatePurchase(Account t)
        {
            throw new NotImplementedException();
        }
    }
}
