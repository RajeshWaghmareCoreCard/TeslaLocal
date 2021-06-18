using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;

namespace CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces
{
    public interface IAdcRepository
    {
        Task<List<AuthDecisionControl>> GetAuthDecisionControlsAsync(string productId, string customerId, string accountId, string cardId);
    }
}