using System.Collections.Concurrent;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        public Task<TransactionModel> InsertTransactionAsync(TransactionModel transactionDetails, AccountModel customerAccount, CustomerModel customer, ConcurrentDictionary<string, ADCResult> ADCResultBag)
        {
            throw new System.NotImplementedException();
        }
    }
}