using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.Adc.Contracts;
using CoreCard.Tesla.Falcon.ADC;
using CoreCard.Tesla.Falcon.ADC.Contracts;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;

namespace CoreCard.Tesla.Falcon.Adc
{
    public class AuthDecisionControlProvider : IAuthDecisionControlProvider
    {
        #region Data Members
        public ConcurrentDictionary<string, ADCResult> AdcResults { get; set; }
        public List<(string acdId, AuthDecisionControl authDecisionControl, Func<TransactionModel, CustomerModel, AccountModel, CardModel, AuthDecisionControl, Task<ADCResult>> func)> Adcs { get; set; }
        #endregion

        #region interface injection
        readonly IAdcRepository adcRepository;
        readonly IAuthDecisionControlResolver authDecisionControlProxy;
        private readonly IAdcUnitOfWork adcUnitOfWork;
        #endregion

        public AuthDecisionControlProvider(IAdcRepository adcRepository, IAuthDecisionControlResolver authDecisionControlProxy, IAdcUnitOfWork adcUnitOfWork)
        {
            Adcs = new List<(string acdId, AuthDecisionControl authDecisionControl, Func<TransactionModel, CustomerModel, AccountModel, CardModel, AuthDecisionControl, Task<ADCResult>> func)>();
            AdcResults = new ConcurrentDictionary<string, ADCResult>();
            this.adcRepository = adcRepository;
            this.authDecisionControlProxy = authDecisionControlProxy;
            this.adcUnitOfWork = adcUnitOfWork;
        }

        public async Task Execute(TransactionModel transaction, CustomerModel customer, AccountModel account, CardModel card)
        {
            await Task.Run(() => Parallel.ForEach(Adcs, async ADCItem =>
               {
                   var result = await ADCItem.func.Invoke(transaction, customer, account, card, ADCItem.authDecisionControl);
                   AdcResults.TryAdd(ADCItem.acdId, result);
               }));
        }

        public async Task RegisterAuthDecisionControlsAsync(string productId, string customerId, string accountId, string cardId)
        {
            var currentAdcs = await adcUnitOfWork.GetAuthDecisionControlsAsync(productId, customerId, accountId, cardId);
            foreach (var item in currentAdcs)
            {
                var adcImplementor = authDecisionControlProxy.GetAuthDecisionControl(item.Key);
                if (adcImplementor != null)
                    Adcs.Add((item.Key, item.Value, adcImplementor.Execute));
            }
        }
    }
}