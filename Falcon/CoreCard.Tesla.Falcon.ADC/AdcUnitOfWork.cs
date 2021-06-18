using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.ADC.Contracts;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;

namespace CoreCard.Tesla.Falcon.ADC
{
    public class AdcUnitOfWork : BaseRepository, IAdcUnitOfWork
    {
        private readonly IProductAdcRepository productAdcRepository;
        private readonly IAccountAdcRepository accountAdcRepository;
        private readonly ICardAdcRepository cardAdcRepository;
        private readonly ICacheProvider cacheProvider;

        public AdcUnitOfWork(IDatabaseConnectionResolver databaseConnection, ICacheProvider cacheProvider, IProductAdcRepository productAdcRepository, IAccountAdcRepository accountAdcRepository, ICardAdcRepository cardAdcRepository) : base(databaseConnection)
        {
            this.productAdcRepository = productAdcRepository;
            this.accountAdcRepository = accountAdcRepository;
            this.cardAdcRepository = cardAdcRepository;
            this.cacheProvider = cacheProvider;
        }
        public async Task<ConcurrentDictionary<string, AuthDecisionControl>> GetAuthDecisionControlsAsync(string productId, string customerId, string accountId, string cardId)
        {
            var productAdcs = await cacheProvider.GetValueAsync<List<ProductAdc>>(productId);
            var accountAdcs = await accountAdcRepository.GetAllAccountAdcs(accountId);
            var cardAdcs = await cardAdcRepository.GetAllCardAdcs(cardId);
            
            ConcurrentDictionary<string, AuthDecisionControl> authDecisionControls = new ConcurrentDictionary<string, AuthDecisionControl>();
            

            foreach (var item in cardAdcs)
            {
                authDecisionControls.TryAdd(item.AdcId, new AuthDecisionControl() { ADCId = item.AdcId, ResponseCode = item.ResponseCode, InternalResponseCode = item.InternalResponseCode, ContinueOnTimeout = item.ContinueOnTimeout });
            }

            foreach (var item in accountAdcs)
            {
                authDecisionControls.GetOrAdd(item.AdcId, new AuthDecisionControl() { ADCId = item.AdcId, ResponseCode = item.ResponseCode, InternalResponseCode = item.InternalResponseCode, ContinueOnTimeout = item.ContinueOnTimeout });
            }

            foreach (var item in productAdcs)
            {
                authDecisionControls.GetOrAdd(item.AdcId, new AuthDecisionControl() { ADCId = item.AdcId, ResponseCode = item.ResponseCode, InternalResponseCode = item.InternalResponseCode, ContinueOnTimeout = item.ContinueOnTimeout });
            }
            return authDecisionControls;
        }
    }
}