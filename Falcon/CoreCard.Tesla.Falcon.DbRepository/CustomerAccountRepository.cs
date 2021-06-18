using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;

namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class CustomerAccountRepository : ICustomerAccountRepository
    {
        public Task<AccountModel> GetCustomerAccountAsync(string cardNumber)
        {
            throw new System.NotImplementedException();
        }
    }
}