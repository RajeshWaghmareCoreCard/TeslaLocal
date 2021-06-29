using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using DBAdapter;
using System;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface IEmbossingBAL
    {
        //Task<Embossing> AddEmbossingAsync(Guid accountId);
        Embossing GetEmbossingByCardNumber(string cardnumber);

        void Insert(Guid accountId, IDataBaseCommand dataBaseCommand);

        Embossing GetEmbossingByCardNumber(string cardnumber, IDataBaseCommand dataBaseCommand);
    }
}