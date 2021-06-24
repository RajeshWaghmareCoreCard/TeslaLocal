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
    public class TransInAcctBAL :ITransInAcctBAL
    {
       // private readonly ITransInAcctRepository _transinacctRepository;
        private readonly IADOTranInAcctRepository _adotransinacctRepository;
        public TransInAcctBAL( IADOTranInAcctRepository aDOTranInAcctRepository) //: base(transinacctRepository)
        {
            //_transinacctRepository = transinacctRepository;
            _adotransinacctRepository = aDOTranInAcctRepository;
        }

        //public async Task<Trans_in_Acct> AddTansInAcctAsync(Guid tranId, Guid accountid)
        //{

        //    Trans_in_Acct traninacct = new Trans_in_Acct();
        //    traninacct.accountid = accountid;
        //    traninacct.tranid = tranId;
            
        //    return await _transinacctRepository.AddAsync(traninacct);

        //}

        public Trans_in_Acct Insert(Trans_in_Acct t )
        {
           return  _adotransinacctRepository.Add(t);
        }

        public void Insert(Trans_in_Acct t, DBAdapter.IDataBaseCommand dataBaseCommand)
        {
            _adotransinacctRepository.Insert(t,dataBaseCommand);
        }
    }
}
