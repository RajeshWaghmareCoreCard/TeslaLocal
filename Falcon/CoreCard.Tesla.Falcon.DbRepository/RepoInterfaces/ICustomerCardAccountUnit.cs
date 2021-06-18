using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Repository;

namespace CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces
{
    public interface ICustomerCardAccountUnit
    {
        CustomerModel Customer { get; }
        AccountModel Account { get; }
        CardModel Card { get; }
        Task GetAsync(string cardToken);
        Task Update(TransactionModel transactionModel);

        void Dispose();
    }

}