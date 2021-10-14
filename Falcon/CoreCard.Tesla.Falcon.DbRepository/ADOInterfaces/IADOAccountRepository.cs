using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using DBAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOAccountRepository: IADOCockroachDBRepository<Account>
    {
        void UpdateAccountWithPayment(Account t, IDataBaseCommand dbCommand);
        Account UpdatePurchase(Account t);
        Account UpdatePurchase(Account t, IDataBaseCommand dbCommand);
        Guid Insert(Account t, IDataBaseCommand databaseCommand);

        Account Get(UInt64 AccountNumber);

        Account GetAccountByID(Guid guid, string ccregion, IDataBaseCommand dataBaseCommand);
        Account GetAccountByNumber(UInt64 AccountNumber, string ccregion, IDataBaseCommand dataBaseCommand);
    }
}
