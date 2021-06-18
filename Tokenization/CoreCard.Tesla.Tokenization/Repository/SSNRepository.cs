using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;

namespace CoreCard.Tesla.Tokenization.Repository
{
    public class SSNRepository : ISSNRepository
    {
        private readonly IDatabaseConnectionResolver databaseConnection;

        public SSNRepository(IDatabaseConnectionResolver databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }
        public Task<TokenFamilyDetails> CreateTokenAsync(TokenizationRequest request, string aesClearValue)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokenFamilyDetails> GetTokenData(string token)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokenFamilyDetails> UpdateTokenData(TokenFamilyDetails tokenDetails)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokenFamilyDetails> DeTokenize(TokenFamilyDetails tokenDetails)
        {
            throw new NotImplementedException();
        }

        public Task<TokenFamilyDetails> IsDuplicate(string hash)
        {
            throw new NotImplementedException();
        }
    }
}