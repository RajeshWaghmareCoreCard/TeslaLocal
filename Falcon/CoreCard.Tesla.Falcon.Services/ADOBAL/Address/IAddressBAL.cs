using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using DBAdapter;
using System;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface IAddressBAL
    {
        void Insert(Address t, IDataBaseCommand dataBaseCommand);
    }
}