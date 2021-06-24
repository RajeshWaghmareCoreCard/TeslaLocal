using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
////using CockroachDb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public class LogArTxnBAL :ILogArTxnBAL
    {
        //private readonly ILogArTxnRepository _logartxnRepository;
        private readonly IADOLogArTxnRepository _aDOLogArTxnRepository;
        public LogArTxnBAL( IADOLogArTxnRepository aDOLogArTxnRepository)// : base(logartxnRepository)
        {
            //_logartxnRepository = logartxnRepository;
            _aDOLogArTxnRepository = aDOLogArTxnRepository;
        }

        //public async Task<LogArTxn> AddLogArTxnAsync(Guid tranId)
        //{
            
        //    LogArTxn logartxn = new LogArTxn();
        //    logartxn.artype = 1;
        //    logartxn.tranid = tranId;
        //    logartxn.businessdate = DateTime.Now;
        //    logartxn.status = "success";
        //    return await _logartxnRepository.AddAsync(logartxn);

        //}

        public LogArTxn Insert(LogArTxn t)
        {
            return _aDOLogArTxnRepository.Add(t);
        }

        public void Insert(LogArTxn t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            _aDOLogArTxnRepository.Insert(t,dataBaseCommand);
        }
    }
}
