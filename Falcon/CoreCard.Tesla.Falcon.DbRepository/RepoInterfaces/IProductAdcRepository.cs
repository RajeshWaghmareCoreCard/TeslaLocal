using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Repository;

namespace CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces
{
    public interface IProductAdcRepository
    {
        Task<List<ProductAdc>> GetAllProductAdcs();
        Task<ProductAdc> GetAllProductAdcs(string accountId);
    }
}