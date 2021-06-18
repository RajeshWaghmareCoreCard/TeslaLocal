using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;

namespace CoreCard.Tesla.Tokenization.TokenFamilyProviders
{
    public class SSNTokenFamilyProvider : ISSNTokenFamilyProvider
    {
        public Task<TokenFamilyDetails> CreateTokenAsync(TokenizationRequest request, string aesClearValue)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokenFamilyDetails> DeTokenize(TokenFamilyDetails tokenDetails)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokenFamilyDetails> GetTokenData(string token)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokenFamilyDetails> IsDuplicate(string hash)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokenFamilyDetails> UpdateTokenData(TokenFamilyDetails tokenDetails)
        {
            throw new System.NotImplementedException();
        }
    }
}