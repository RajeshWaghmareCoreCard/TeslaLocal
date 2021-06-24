using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
//using CockroachDb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public class CBLogBAL : ICBLogBAL
    {
        private readonly IADOCBLogRepository _adocblogRepository;
        public CBLogBAL(IADOCBLogRepository adocblogRepository) 
        {
            _adocblogRepository = adocblogRepository;
        }

        //public async Task<CBLog> AddCBLogAsync(Guid tranId, Guid accountid, decimal amount, decimal currentbal)
        //{

        //    CBLog cbLog = new CBLog();
        //    cbLog.tranid = tranId;
        //    cbLog.accountid = accountid;
        //    cbLog.tranamount = amount;
        //    cbLog.currentbal = currentbal;
        //    cbLog.posttime = DateTime.Now;
        //    return await _cblogRepository.AddAsync(cbLog);

        //}

        public CBLog Insert(CBLog t)
        {
            return _adocblogRepository.Add(t);
        }

        public void  Insert(CBLog t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
           _adocblogRepository.Insert(t, dataBaseCommand);
        }
    }
}
