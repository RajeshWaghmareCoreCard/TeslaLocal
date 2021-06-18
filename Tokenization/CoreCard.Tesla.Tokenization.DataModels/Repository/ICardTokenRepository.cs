using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface ICardTokenRepository
    {
        Task<CardTokenModel> GetCardToken(string token);
        Task<CardTokenModel> GetCardTokenByHash(string cardHash);
        Task InsertCardToken(CardTokenModel card);
        Task<CardTokenModel> UpdateCardToken(CardTokenModel CardDetails);
    }
}