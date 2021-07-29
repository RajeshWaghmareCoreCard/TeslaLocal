using CoreCard.Tesla.Falcon.DataModels.Entity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public interface IADOAccountRepository
    {
        void UpdateAccountWithPayment(Account t, NpgsqlConnection connection);
        Account UpdatePurchase(Account t);
        //Account UpdatePurchase(Account t, IDataBaseCommand dbCommand);
        Guid Insert(Account t);

        Account Get(UInt64 AccountNumber);

        Account GetAccountByID(Guid guid);
        Account GetAccountByNumber(long AccountNumber, NpgsqlConnection npgsqlConnection);
    }
}
