using System;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface ITransInAcctBAL //: IBaseBAL<Trans_in_Acct>
    {
        //Task<Trans_in_Acct> AddTansInAcctAsync(Guid tranId, Guid accountid);
        Trans_in_Acct Insert(Trans_in_Acct t);
        void Insert(Trans_in_Acct t, DBAdapter.IDataBaseCommand dataBaseCommand);
    }
}
