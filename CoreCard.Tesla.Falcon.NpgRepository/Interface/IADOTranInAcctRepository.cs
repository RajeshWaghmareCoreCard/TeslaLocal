using CoreCard.Tesla.Falcon.DataModels.Entity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public interface IADOTranInAcctRepository 
    {
        void Insert(Trans_in_Acct t, NpgsqlConnection connection);
        //void Insert(Trans_in_Acct t, DBAdapter.IDataBaseCommand dataBaseCommand);
    }
}
