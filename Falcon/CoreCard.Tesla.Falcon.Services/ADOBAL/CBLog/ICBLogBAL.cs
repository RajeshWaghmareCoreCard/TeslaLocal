using System;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface ICBLogBAL
    {
        //Task<CBLog> AddCBLogAsync(Guid tranId, Guid accountid, decimal amount, decimal currentbal);
        CBLog Insert(CBLog t);
        void Insert(CBLog t, DBAdapter.IDataBaseCommand dataBaseCommand);
    }
}
