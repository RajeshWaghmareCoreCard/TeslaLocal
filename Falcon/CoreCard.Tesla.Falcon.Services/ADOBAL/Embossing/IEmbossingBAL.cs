using CoreCard.Tesla.Falcon.DataModels.Entity;
using DBAdapter;
using System;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface IEmbossingBAL//:IBaseBAL<Embossing>
    {
        //Task<Embossing> AddEmbossingAsync(Guid accountId);
        Embossing GetEmbossingByCardNumber(string cardnumber);

        void Insert(Guid accountId, IDataBaseCommand dataBaseCommand);
    }
}