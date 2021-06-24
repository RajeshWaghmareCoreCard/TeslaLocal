using CoreCard.Tesla.Falcon.DataModels.Entity;
using DBAdapter;
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
    public class ADOAccountRepository : BaseCockroachADO, IADOAccountRepository
    {
        public ADOAccountRepository(IConfiguration configuration) :base(configuration)
        {
        }
        public Account Add(Account t)
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
            string sql = "SELECT * FROM Account where accountid ='"+id+"'";

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
        public Account UpdateAccountWithPayment(Account t, IDataBaseCommand dbCommand)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update account set ");
            sb.Append(string.Format("currentbal = {0}, ", t.currentbal));
            sb.Append(string.Format("paymentamount = {0}, ", t.paymentamount));
            sb.Append(string.Format("paymentcount = {0} ", t.paymentcount));
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
            if(ds != null && ds.Tables.Count>0)
            {
                if(ds.Tables[0].Rows.Count > 0)
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
            sb.Append(" where accountid = '" + t.accountid.ToString() + "';");
            sb.Append(" select * from account where accountid = '" + t.accountid.ToString() + "';");

            DataSet ds = dbCommand.GetDataSet(sb.ToString());
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

        public Guid Insert(Account t, IDataBaseCommand databaseCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            object uuid = databaseCommand.ExecuteParameterizedScalarCommand("insert into account(accountnumber,customerid,creditlimit,currentbal,principal,purchaseamount,fees,interest,purchasecount,paymentamount,paymentcount) values (@accountnumber,@customerid,@creditlimit,@currentbal,@principal,@purchaseamount,@fees,@interest,@purchasecount,@paymentamount,@paymentcount) Returning accountid;", dic);
            return (Guid)uuid;
        }
    }
}
