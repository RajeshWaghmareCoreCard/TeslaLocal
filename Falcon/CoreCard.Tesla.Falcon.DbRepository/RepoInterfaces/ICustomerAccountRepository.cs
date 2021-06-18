using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;

namespace CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces
{
    public interface ICustomerAccountRepository
    {
         Task<AccountModel> GetCustomerAccountAsync(string cardNumber);
    }
}