using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;

namespace CoreCard.Tesla.Tokenization
{
    public class TokenFamilyProvider : ITokenFamilyProvider
    {
        private readonly ITokenFamilyResolver tokenFamilyProxy;

        public TokenFamilyProvider(ITokenFamilyResolver tokenFamilyProxy)
        {
            this.tokenFamilyProxy = tokenFamilyProxy;
        }
        public async Task<TokenFamilyDetails> GetTokenDataAsync(string tokenFamily, string token)
        {
            var response = await tokenFamilyProxy.GetFamilyProvider(tokenFamily).GetTokenData(token);
            return response;
        }
        public async Task<TokenFamilyDetails> CreateTokenAsync(TokenizationRequest request, string aesClearValue)
        {
            var response = await tokenFamilyProxy.GetFamilyProvider(request.Token.Family).CreateTokenAsync(request, aesClearValue);
            return response;
        }

        public async Task<TokenFamilyDetails> DeTokenize(TokenFamilyDetails tokenDetails)
        {
            return await tokenFamilyProxy.GetFamilyProvider(tokenDetails.Family).DeTokenize(tokenDetails);
        }

        public async Task<TokenFamilyDetails> IsTokenDuplicate(TokenizationRequest request, string aesClearValue)
        {
            try
            {
                var cardHash = aesClearValue.HMACSHA512Hash(request.InstitutionId);
                var response = await tokenFamilyProxy.GetFamilyProvider(request.Token.Family).IsDuplicate(cardHash);
                response.Family = request.Token.Family;
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}