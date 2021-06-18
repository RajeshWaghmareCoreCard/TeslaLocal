using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class AdcRepository : BaseRepository, IAdcRepository
    {
        private readonly IProductAdcRepository productAdcRepository;
        private readonly IAccountAdcRepository accountAdcRepository;
        private readonly ICardAdcRepository cardAdcRepository;
        private readonly ICacheProvider cacheProvider;

        public AdcRepository(IDatabaseConnectionResolver databaseConnection, ICacheProvider cacheProvider, IProductAdcRepository productAdcRepository, IAccountAdcRepository accountAdcRepository, ICardAdcRepository cardAdcRepository) : base(databaseConnection)
        {
            this.productAdcRepository = productAdcRepository;
            this.accountAdcRepository = accountAdcRepository;
            this.cardAdcRepository = cardAdcRepository;
            this.cacheProvider = cacheProvider;
        }
        public async Task<List<AuthDecisionControl>> GetAuthDecisionControlsAsync(string productId, string customerId, string accountId, string cardId)
        {
            var productAdcs = await cacheProvider.GetValueAsync<List<ProductAdc>>(productId);
            var accountAdcs = await accountAdcRepository.GetAllAccountAdcs(accountId);
            var cardAdcs = await cardAdcRepository.GetAllCardAdcs(cardId);

            List<AuthDecisionControl> authDecisionControls = new List<AuthDecisionControl>();
            ConcurrentDictionary<string, AuthDecisionControl> authDecisionControls1 = new ConcurrentDictionary<string, AuthDecisionControl>();
            foreach (var productAdc in productAdcs)
            {
                authDecisionControls.Add(new AuthDecisionControl() { ADCId = productAdc.AdcId, InternalResponseCode = productAdc.InternalResponseCode, ProductId = productAdc.ProductId, ResponseCode = productAdc.ResponseCode });
            }

            foreach (var cardAdc in cardAdcs)
            {
                authDecisionControls1.TryAdd(cardAdc.AdcId, new AuthDecisionControl() { });
            }


            return authDecisionControls;
        }
    }
}