using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public interface IADOTranInAcctRepository : IADOCockroachDBRepository<Trans_in_Acct>
    {
        void Insert(Trans_in_Acct t);
        void Insert(Trans_in_Acct t, DBAdapter.IDataBaseCommand dataBaseCommand);
    }
}
