using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using Npgsql;

namespace CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces
{
    public interface ICardRepository
    {
        Task<CardModel> GetCardAsync(string cardToken);
        Task<CardModel> GetCardAsync(string cardToken, NpgsqlConnection connection);
    }
}