using System;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface ILogArTxnBAL //: IBaseBAL<LogArTxn>
    {
        //Task<LogArTxn> AddLogArTxnAsync(Guid tranId);
        LogArTxn Insert(LogArTxn t);
        void Insert(LogArTxn t, DBAdapter.IDataBaseCommand dataBaseCommand);
    }
}
