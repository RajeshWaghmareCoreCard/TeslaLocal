using System.Threading.Tasks;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoModels;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.NCipher;
using CoreCard.Tesla.Tokenization.DataModels.Repository;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.TokenFamilyProviders
{
    public class OtherTokenFamilyProviders : BaseFamilyProvider, IOtherTokenFamilyProviders
    {
        public OtherTokenFamilyProviders(IOtherTokenFamilyRepository otherTokenFamilyRepository, INCipherProvider nCipherProvider, IHsmActiveKeysRepository hsmActiveKeysRepository)
        {
            tokenDetails = new TokenFamilyDetails() { OtherFamilyDetails = new OtherFamilyModel() };
            this.otherTokenFamilyRepository = otherTokenFamilyRepository;
            this.nCipherProvider = nCipherProvider;
            this.hsmActiveKeysRepository = hsmActiveKeysRepository;
        }

        #region Interface service providers
        private readonly INCipherProvider nCipherProvider;
        private readonly IHsmActiveKeysRepository hsmActiveKeysRepository;
        private readonly IOtherTokenFamilyRepository otherTokenFamilyRepository;
        #endregion
        public async Task<TokenFamilyDetails> CreateTokenAsync(TokenizationRequest request, string aesClearValue)
        {
            try
            {
                activeKeyModel = await GetActiveKeyAsync();
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
        protected async Task<HsmActiveKeyModel> GetActiveKeyAsync()
        {
            //Get KeyId And AES keys from ActiveKeys table
            return await Task.FromResult(new HsmActiveKeyModel() { HsmActiveKeyId = 1, KeyDetails = "asdadsada-asdadasd-adsadasd-" });
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
            tokenDetails.OtherFamilyDetails.TokenHash = aesClearValue.GetHash(request.InstitutionId);
            tokenDetails.OtherFamilyDetails.EncryptedData = encryptResponse.EncryptedData;
            tokenDetails.OtherFamilyDetails.HsmActiveKeyId = activeKeyModel.HsmActiveKeyId;
            tokenDetails.OtherFamilyDetails.InstitutionId = request.InstitutionId;
            tokenDetails.OtherFamilyDetails.OtherTokenId = $"{tokenDetails.Family}{HelperExtensions.GetGUIDWithoutDash()}";
            tokenDetails.OtherFamilyDetails.TokenFamilyId = tokenDetails.Family;
            return tokenDetails;
        }
        protected async Task<TokenFamilyDetails> InsertTokenAsync(TokenFamilyDetails tokenDetails)
        {
            await otherTokenFamilyRepository.InsertToken(tokenDetails.OtherFamilyDetails);
            tokenDetails.TokenId = tokenDetails.OtherFamilyDetails.OtherTokenId;
            return tokenDetails;
        }
        protected Task<TokenFamilyDetails> UpdateTokenAsync(TokenFamilyDetails tokenDetails)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TokenFamilyDetails> GetTokenData(string token)
        {
            var otherFamilyModel = await otherTokenFamilyRepository.GetToken(token);
            TokenFamilyDetails tokenDetails = new TokenFamilyDetails();
            tokenDetails.TokenId = otherFamilyModel.OtherTokenId;
            tokenDetails.Family=otherFamilyModel.TokenFamilyId;
            tokenDetails.OtherFamilyDetails = otherFamilyModel;
            return tokenDetails;
        }

        public Task<TokenFamilyDetails> UpdateTokenData(TokenFamilyDetails tokenDetails)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TokenFamilyDetails> DeTokenize(TokenFamilyDetails tokenDetails)
        {
            return await Task.FromResult(new TokenFamilyDetails() { ClearToken = tokenDetails.OtherFamilyDetails.EncryptedData, OtherFamilyDetails = tokenDetails.OtherFamilyDetails });
        }
        public async Task<TokenFamilyDetails> IsDuplicate(string hash)
        {
            var otherFamilyModel = await otherTokenFamilyRepository.GetTokenByHash(hash);
            if (otherFamilyModel != null)
            {
                TokenFamilyDetails tokenDetails = new TokenFamilyDetails();
                tokenDetails.TokenId = otherFamilyModel.OtherTokenId;
                tokenDetails.Family = otherFamilyModel.TokenFamilyId;
                return tokenDetails;
            }
            return null;
        }
    }
}