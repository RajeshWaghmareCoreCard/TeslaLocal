using System.Collections.Concurrent;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;

namespace CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces
{
    public interface ITransactionRepository
    {
         Task<TransactionModel> InsertTransactionAsync(TransactionModel transactionDetails, AccountModel customerAccount, CustomerModel customer, ConcurrentDictionary<string, ADCResult> ADCResultBag);
    }
}