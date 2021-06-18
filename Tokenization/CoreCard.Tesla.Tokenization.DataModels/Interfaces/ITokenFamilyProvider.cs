using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface ITokenFamilyProvider
    {
        Task<TokenFamilyDetails> GetTokenDataAsync(string tokenFamily, string token);

        Task<TokenFamilyDetails> CreateTokenAsync(TokenizationRequest request, string aesClearValue);

        Task<TokenFamilyDetails> IsTokenDuplicate(TokenizationRequest request, string aesClearValue);

        Task<TokenFamilyDetails> DeTokenize(TokenFamilyDetails tokenDetails);

    }
}