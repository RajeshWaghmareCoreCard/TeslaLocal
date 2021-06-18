using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;

namespace CoreCard.Tesla.Falcon.Adc.Contracts
{
    public interface IAuthDecisionControlProvider
    {
        ConcurrentDictionary<string, ADCResult> AdcResults { get; set; }
        List<(string acdId, AuthDecisionControl authDecisionControl, Func<TransactionModel, CustomerModel, AccountModel, CardModel, AuthDecisionControl, Task<ADCResult>> func)> Adcs { get; set; }
        Task Execute(TransactionModel transaction, CustomerModel customer, AccountModel customerAccount, CardModel card);
        Task RegisterAuthDecisionControlsAsync(string productId, string customerId, string accountId, string cardId);
    }
}