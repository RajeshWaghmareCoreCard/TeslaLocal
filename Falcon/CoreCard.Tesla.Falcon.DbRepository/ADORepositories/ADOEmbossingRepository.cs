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
    public class ADOEmbossingRepository : BaseCockroachADO, IADOEmbossingRepository
    {
        public ADOEmbossingRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Embossing Add(Embossing t)
        {
            throw new NotImplementedException();
        }

        public Task<Embossing> AddAsync(Embossing t, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Embossing entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Embossing Find(Expression<Func<Embossing, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Embossing Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Embossing> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Embossing> GetAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Embossing GetEmbossingByCardNumber(string cardnumber)
        {
            DataSet ds = _dbCommand.GetDataSet("select * from embossing where cardnumber='" + cardnumber.Trim() + "'");
            Embossing o = new Embossing();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    o = (Embossing)ds.Tables[0].Rows[0];
                }
            }
            return o;
        }

        public Embossing GetEntity(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<Embossing> GetEntityAsync(string sql, object[] parameters = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public List<Embossing> GetEntityList(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<Embossing>> GetEntityListAsync(string sql, object[] parameters, CancellationToken token = default)
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

        public Embossing Update(Embossing t, object key)
        {
            throw new NotImplementedException();
        }

        public Task<Embossing> UpdateAsync(Embossing t, object key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Insert(Embossing t, IDataBaseCommand dataBaseCommand)
        {
            IDictionary<string, object> dic = t.ToDictionary();
            dataBaseCommand.ExecuteParameterizedScalarCommand("insert into embossing(accountid,cardtype,cardnumber,binnumber,ccregion) values (@accountid,@cardtype,@cardnumber,@binnumber, @ccregion)  Returning accountid;", dic);
        }

        public Embossing GetEmbossingByCardNumber(string cardnumber, IDataBaseCommand dataBaseCommand)
        {
            Embossing acc = new Embossing();

            acc = dataBaseCommand.ExecuteDatareader<Embossing>("select embossingid, ifnull(accountid,'00000000-0000-0000-0000-000000000000') as accountid,ifnull(cardtype,0)as cardtype,ifnull(cardnumber,'') as cardnumber , ifnull(binnumber,0) as binnumber, ifnull(ccregion,'') as ccregion from embossing where cardnumber = '" + cardnumber.Trim() + "' for update;").FirstOrDefault();

            return acc;
        }
    }
}
