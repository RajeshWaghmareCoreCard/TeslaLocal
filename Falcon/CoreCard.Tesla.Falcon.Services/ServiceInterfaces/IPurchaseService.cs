using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels;

namespace CoreCard.Tesla.Falcon.ServiceInterfaces
{
    public interface IPurchaseService
    {
        Task<PurchaseResponse> PurchaseRequestAsync(PurchaseRequest request);
    }
}