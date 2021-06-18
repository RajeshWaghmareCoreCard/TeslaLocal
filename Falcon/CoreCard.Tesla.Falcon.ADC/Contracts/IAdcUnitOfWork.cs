using System.Collections.Concurrent;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;

namespace CoreCard.Tesla.Falcon.ADC.Contracts
{
    public interface IAdcUnitOfWork
    {
        Task<ConcurrentDictionary<string, AuthDecisionControl>> GetAuthDecisionControlsAsync(string productId, string customerId, string accountId, string cardId);
    }
}