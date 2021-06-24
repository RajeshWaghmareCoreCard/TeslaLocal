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
        Account UpdatePurchase(Account t);
        Account UpdatePurchase(Account t, IDataBaseCommand dbCommand);
        Guid Insert(Account t, IDataBaseCommand databaseCommand);
        Account UpdateAccountWithPayment(Account t, IDataBaseCommand dbCommand);
        Account Get(UInt64 AccountNumber);
    }
}
