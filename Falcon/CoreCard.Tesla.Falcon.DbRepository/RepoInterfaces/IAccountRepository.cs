using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces
{
    public interface IAccountRepository
    {
        Task<AccountModel> GetAccountAsync(string accountId);
        Task<AccountModel> GetAccountAsync(string accountId, NpgsqlConnection connection);
    }
}