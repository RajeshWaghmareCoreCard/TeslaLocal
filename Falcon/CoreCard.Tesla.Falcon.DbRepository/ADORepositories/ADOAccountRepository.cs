using DBAdapter;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using System.Data;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public class ADOAccountRepository : BaseCockroachADO, IADOAccountRepository
    {
        public ADOAccountRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public Account Add(Account t)
        {
            throw new NotImplementedException();
        }
        public List<Account> Get(Guid id, string idtype)
        {
            throw new NotImplementedException();
        }
        public Account Get(UInt64 id, string idtype)
        {
            throw new NotImplementedException();
        }
        public Task<Account> AddAsync(Account t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Account Find(Expression<Func<Account, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Account Get(Guid id)
        {
            string sql = "SELECT * FROM Account where accountid ='" + id + "'";

            DataSet ds = _dbCommand.GetDataSet(sql);

            Account acc = new Account();
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                acc = (Account)dataRow;
            }
            return acc;
        }
        public Account Get(UInt64 AccountNumber)
        {
            string sql = "SELECT * FROM Account where accountnumber =" + AccountNumber.ToString();

            DataSet ds = _dbCommand.GetDataSet(sql);

            Account acc = new Account();
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                acc = (Account)dataRow;
            }
            return acc;
        }
        public List<Account> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Account GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<Account> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<Account>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public Account Update(Account t, object key)
        {

            throw new NotImplementedException();
        }

        public Task<Account> UpdateAsync(Account t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccountWithPayment(Account t, IDataBaseCommand dbCommand)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update account set ");
            sb.Append(string.Format("currentbal = {0}, ", t.currentbal));
            sb.Append(string.Format("paymentamount = {0}, ", t.paymentamount));
            sb.Append(string.Format("paymentcount = {0} ", t.paymentcount));
            sb.Append(" where ccregion ='" + t.ccregion + "' and accountid = '" + t.accountid.ToString() + "';");
            //sb.Append(" select * from account where accountid = '" + t.accountid.ToString() + "';");
            dbCommand.ExecuteNonQuery(sb.ToString());
            /*Account o = new Account();
            UInt64 AcnNo = (UInt64)t.accountnumber;
            o = GetAccountByNumber(AcnNo, dbCommand);
            return o;*/

        }

        public Account UpdatePurchase(Account t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update account set ");
            sb.Append(string.Format("currentbal = {0}, ", t.currentbal));
            sb.Append(string.Format("principal = {0}, ", t.principal));
            sb.Append(string.Format("purchaseamount = {0} ", t.purchaseamount));
            sb.Append(" where accountid = '" + t.accountid.ToString() + "';");
            sb.Append(" select * from account where accountid = '" + t.accountid.ToString() + "';");

            DataSet ds = _dbCommand.GetDataSet(sb.ToString());
            Account o = new Account();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    o = (Account)ds.Tables[0].Rows[0];
                }
            }

            return o;
        }
        // Update purchase related information.
        public Account UpdatePurchase(Account t, IDataBaseCommand dbCommand)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update account set ");
            sb.Append(string.Format("currentbal = {0}, ", t.currentbal));
            sb.Append(string.Format("principal = {0}, ", t.principal));
            sb.Append(string.Format("purchaseamount = {0} ", t.purchaseamount));
            sb.Append(" where ccregion = '" + t.ccregion + "' and accountid = '" + t.accountid.ToString() + "';");
            //sb.Append(" select * from account where accountid = '" + t.accountid.ToString() + "';");

            //DataSet ds = dbCommand.GetDataSet(sb.ToString());
            dbCommand.ExecuteNonQuery(sb.ToString());
            Account o = new Account();
            o = GetAccountByID(t.accountid, t.ccregion, dbCommand);
            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        o = (Account)ds.Tables[0].Rows[0];
            //    }
            //}

            return o;
        }

        public Guid Insert(Account t, IDataBaseCommand databaseCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            object uuid = databaseCommand.ExecuteParameterizedScalarCommand("insert into account(accountnumber,customerid,creditlimit,currentbal,principal,purchaseamount,fees,interest,purchasecount,paymentamount,paymentcount,ccregion) values (@accountnumber,@customerid,@creditlimit,@currentbal,@principal,@purchaseamount,@fees,@interest,@purchasecount,@paymentamount,@paymentcount,@ccregion) Returning accountid;", dic);
            return (Guid)uuid;
        }

        public Account GetAccountByID(Guid guid, string ccregion, IDataBaseCommand dataBaseCommand)
        {
            Account acc = new Account();

            acc = dataBaseCommand.ExecuteDatareader<Account>("SELECT accountid, accountnumber,ifnull(customerid,'00000000-0000-0000-0000-000000000000') as customerid,creditlimit,ifnull(currentbal,0)as currentbal ,ifnull(principal,0)as principal,ifnull(purchaseamount,0)as purchaseamount,ifnull(fees,0)as fees,ifnull(interest,0)as interest,ifnull(purchasecount,0)as purchasecount,ifnull(paymentamount,0)as paymentamount,ifnull(paymentcount,0)as paymentcount, ifnull(status,0)as status, ifnull(ccregion,'') as ccregion FROM Account where ccregion='" + ccregion + "' and accountid ='" + guid + "' for update;").FirstOrDefault();

            return acc;
        }

        public Account GetAccountByNumber(UInt64 AccountNumber, string ccregion, IDataBaseCommand dataBaseCommand)
        {
            StringBuilder strQry = new StringBuilder();
            strQry.Append("SELECT accountid, accountnumber,ifnull(customerid,'00000000-0000-0000-0000-000000000000') as customerid");
            strQry.Append(", creditlimit,ifnull(currentbal,0)as currentbal ,ifnull(principal,0)as principal,ifnull(purchaseamount,0)as purchaseamount");
            strQry.Append(", ifnull(fees,0)as fees,ifnull(interest,0)as interest,ifnull(purchasecount,0)as purchasecount");
            strQry.Append(" ,ifnull(paymentamount,0)as paymentamount,ifnull(paymentcount,0)as paymentcount, ifnull(status,0)as status, ifnull(ccregion,'') as ccregion ");
            strQry.Append(" FROM Account where ccregion='" + ccregion + "' and accountnumber =" + AccountNumber.ToString() + " for update;");
            Account acc = new Account();

            acc = dataBaseCommand.ExecuteDatareader<Account>(strQry.ToString()).FirstOrDefault();

            return acc;
        }
    }
}
