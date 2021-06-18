using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface IFamilyProvider
    {
        Task<TokenFamilyDetails> CreateTokenAsync(TokenizationRequest request, string aesClearValue);
        Task<TokenFamilyDetails> GetTokenData(string token);
        Task<TokenFamilyDetails> UpdateTokenData(TokenFamilyDetails tokenDetails);
        Task<TokenFamilyDetails> DeTokenize(TokenFamilyDetails tokenDetails);
         Task<TokenFamilyDetails> IsDuplicate(string hash);
    }
}