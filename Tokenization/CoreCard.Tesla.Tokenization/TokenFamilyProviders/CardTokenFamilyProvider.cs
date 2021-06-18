using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoModels;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.NCipher;
using CoreCard.Tesla.Tokenization.DataModels.Repository;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.TokenFamilyProviders
{
    public class CardTokenFamilyProvider : BaseFamilyProvider, ICardTokenFamilyProvider
    {
        public CardTokenFamilyProvider(ICardTokenRepository cardRepository, INCipherProvider nCipherProvider, IHsmActiveKeysRepository hsmActiveKeysRepository)
        {
            tokenDetails = new TokenFamilyDetails() { OtherFamilyDetails = new OtherFamilyModel() };
            this.cardRepository = cardRepository;
            this.nCipherProvider = nCipherProvider;
            this.hsmActiveKeysRepository = hsmActiveKeysRepository;
        }

        #region Interface service providers
        private readonly INCipherProvider nCipherProvider;
        private readonly IHsmActiveKeysRepository hsmActiveKeysRepository;
        private readonly ICardTokenRepository cardRepository;
        #endregion
        public async Task<TokenFamilyDetails> CreateTokenAsync(TokenizationRequest request, string aesClearValue)
        {
            try
            {
                activeKeyModel = await hsmActiveKeysRepository.GetActiveKey(request.Token.Family);
                var encryptResponse = await EncryptDataAsync(aesClearValue);
                var tokenDetails = BuildTokenDetails(encryptResponse, aesClearValue, request);
                tokenDetails = await InsertTokenAsync(tokenDetails);
                return tokenDetails;
            }
            catch
            {

            }
            return null;
        }

        public async Task<TokenFamilyDetails> GetTokenData(string token)
        {
            var carddetails = await cardRepository.GetCardToken(token);
            TokenFamilyDetails tokenDetails = new TokenFamilyDetails();
            tokenDetails.TokenId = carddetails.CardTokenId;
            tokenDetails.Family = "CardNumber";
            tokenDetails.CardTokenDetails = carddetails;
            return tokenDetails;
        }

        public async Task<TokenFamilyDetails> UpdateTokenData(TokenFamilyDetails tokenDetails)
        {
            CardTokenModel card = new CardTokenModel();
            card.CardTokenId = tokenDetails.CardTokenDetails.CardTokenId;
            //TODO assignement 
            var carddetails = await cardRepository.UpdateCardToken(tokenDetails.CardTokenDetails);
            return tokenDetails;
        }

        protected async Task<TokenFamilyDetails> UpdateTokenAsync(TokenFamilyDetails tokenDetails)
        {
            CardTokenModel card = new CardTokenModel();
            card.CardTokenId = tokenDetails.CardTokenDetails.CardTokenId;
            var carddetails = await cardRepository.UpdateCardToken(tokenDetails.CardTokenDetails);
            return tokenDetails;
        }
        protected async Task<EncryptResponse> EncryptDataAsync(string aesClearValue)
        {
            EncryptRequest encryptRequest = new EncryptRequest();
            encryptRequest.AesKey = activeKeyModel.KeyDetails;
            encryptRequest.Data = aesClearValue;
            encryptRequest.DataLength = aesClearValue.Length;
            return await nCipherProvider.EncryptAsync(encryptRequest);
        }

        protected TokenFamilyDetails BuildTokenDetails(EncryptResponse encryptResponse, string aesClearValue, TokenizationRequest request)
        {
            tokenDetails.CardTokenDetails.CardBin = aesClearValue.GetCardBin();
            tokenDetails.CardTokenDetails.CardLast4 = aesClearValue.GetCardLast4();
            tokenDetails.CardTokenDetails.CardHash = aesClearValue.GetHash(request.InstitutionId);
            tokenDetails.CardTokenDetails.EncryptedCardNumber = encryptResponse.EncryptedData;
            tokenDetails.CardTokenDetails.HsmActiveKeyId = activeKeyModel.HsmActiveKeyId;
            tokenDetails.CardTokenDetails.InstitutionId = request.InstitutionId;
            tokenDetails.CardTokenDetails.NetworkName = "VISA";
            tokenDetails.Family = request.Token.Family;
            tokenDetails.CardTokenDetails.CardTokenId = $"{tokenDetails.CardTokenDetails.CardBin}{HelperExtensions.GetGUIDWithoutDash()}{tokenDetails.CardTokenDetails.CardLast4}";
            return tokenDetails;
        }

        protected async Task<TokenFamilyDetails> InsertTokenAsync(TokenFamilyDetails tokenDetails)
        {
            await cardRepository.InsertCardToken(tokenDetails.CardTokenDetails);
            tokenDetails.TokenId = tokenDetails.CardTokenDetails.CardTokenId;
            return tokenDetails;
        }

        public async Task<TokenFamilyDetails> DeTokenize(TokenFamilyDetails tokenDetails)
        {
            return await Task.FromResult(new TokenFamilyDetails() { ClearToken = tokenDetails.CardTokenDetails.EncryptedCardNumber, CardTokenDetails = tokenDetails.CardTokenDetails });
        }

        public async Task<TokenFamilyDetails> IsDuplicate(string hash)
        {
            var carddetails = await cardRepository.GetCardTokenByHash(hash);
            TokenFamilyDetails tokenDetails = new TokenFamilyDetails();
            tokenDetails.TokenId = carddetails.CardTokenId;
            return tokenDetails;
        }
    }
}