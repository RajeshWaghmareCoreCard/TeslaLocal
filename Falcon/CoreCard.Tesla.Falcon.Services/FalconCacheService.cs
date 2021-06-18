using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;

namespace CoreCard.Tesla.Falcon.Services
{
    public class FalconCacheService : IFalconCacheService
    {
        private readonly ICacheProvider cacheProvider;
        private readonly IProductAdcRepository productAdcRepository;

        public FalconCacheService(ICacheProvider cacheProvider, IProductAdcRepository productAdcRepository)
        {
            this.cacheProvider = cacheProvider;
            this.productAdcRepository = productAdcRepository;
        }

        public async Task Init()
        {
            await CacheAllProductAdcs();
        }

        private async Task CacheAllProductAdcs()
        {
            var productAdcs = await productAdcRepository.GetAllProductAdcs();
            var productAdcsGroupby = productAdcs.GroupBy(p => p.ProductId, (productId, productAdcs) => new { ProductId = productId, ProductAdcs = productAdcs.ToList() });
            foreach (var item in productAdcsGroupby)
            {
                await cacheProvider.SetValueAsync<List<ProductAdc>>(item.ProductId, item.ProductAdcs);
            }
        }
    }
}