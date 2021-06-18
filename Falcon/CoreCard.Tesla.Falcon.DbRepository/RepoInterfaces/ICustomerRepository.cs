using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerModel> GetCustomer(string customerId);
        Task<CustomerModel> GetCustomer(string customerId, NpgsqlConnection connection);
    }
}