using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public interface ITransactionBAL//:IBaseBAL<Transaction>
    {
       // Task<Transaction> AddTransactionAsync(TransactionAddDTO customerDTO);
        Transaction AddTransactionADO(Transaction transaction);
        Guid AddTransactionADO(Transaction transaction, DBAdapter.IDataBaseCommand dbCommand);
    }
}
