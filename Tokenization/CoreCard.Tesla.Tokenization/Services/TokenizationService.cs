using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.Types;
using Tesla.TokenizationProvider;

namespace CoreCard.Tesla.Tokenization.Services
{
    public class TokenizationService : TokenizationBaseService, ITokenizationService
    {
        #region Interfaces service providers
        readonly IModuleSessionRepository tokenizationRepository;
        readonly ITokenFamilyProvider familyProvider;
        #endregion

        #region Data members
        TokenizationRequestExt request;
        #endregion

        #region Constructor
        public TokenizationService(ICacheProvider cacheProvider, IModuleSessionRepository tokenizationRepository, ITokenFamilyProvider familyProvider, IModulePermissionRepository modulePermissionRepository) : base(cacheProvider, modulePermissionRepository)
        {
            this.tokenizationRepository = tokenizationRepository;
            this.familyProvider = familyProvider;
            request = new TokenizationRequestExt();
        }
        #endregion
        public async Task<TokenizationResponse> TokenizeData(TokenizationRequest inner, TokenizationHeaders headers)
        {
            try
            {
                InitRequest(inner, headers);
                await ValidationPermissions(headers.ModuleId, API.CreateToken, inner.Token);
                await GetModuleSessionDetailsAsync();
                CreateAesSession(request.ModuleSessionDetails.KeyDetails);
                DecryptAesData(inner.Token.Value);
                request.TokenDetails = await TokenDuplicateCheckAsync();
                if (request.TokenDetails != null)
                    return await PrepareTokenizationResponse();
                await CreateTokenAsync();
                return await PrepareTokenizationResponse();
            }
            catch (TeslaException ex)
            {
                return new TokenizationResponse() { ResponseCode = ex.ResponseCode, ResponseMessage = ex.ResponseMessage };
            }
            catch (Exception)
            {
                return new TokenizationResponse() { ResponseCode = TokenizationRsponseCodes.SystemMalfunction, ResponseMessage = TokenizationResponseMessages.SystemMalfunction };
            }
            finally
            {
                AesSession?.Dispose();
            }

        }
        private void InitRequest(TokenizationRequest inner, TokenizationHeaders headers)
        {
            this.request.Inner = inner;
            this.request.Headers = headers;
        }
        private async Task<TokenizationResponse> PrepareTokenizationResponse()
        {
            await ReportTraceIdToCache(request.Inner, request.Headers);
            TokenizationResponse response = new TokenizationResponse();
            response.ResponseCode = TokenizationRsponseCodes.Success;
            response.ResponseMessage = TokenizationResponseMessages.Success;
            response.TokenData = AesSession.Encrypt(request.TokenDetails.TokenId);
            return response;
        }

        private async Task CreateTokenAsync()
        {
            request.TokenDetails = await familyProvider.CreateTokenAsync(request.Inner, AesClearValue);
        }

        async Task<TokenFamilyDetails> TokenDuplicateCheckAsync()
        {
            return await familyProvider.IsTokenDuplicate(request.Inner, AesClearValue);
        }
        private async Task GetModuleSessionDetailsAsync()
        {
            //Get value from cache
            await GetModuleSessionFromCache();
            if (request.ModuleSessionDetails == null)
            {
                //Get from DB
                request.ModuleSessionDetails = await tokenizationRepository.GetModuleSessionAsync(request.Inner.SessionId);
                await cacheProvider.SetValueWithExpirationAsync<ModuleSessionModel>(request.ModuleSessionDetails.SessionId, request.ModuleSessionDetails, request.ModuleSessionDetails.SessionExpiryDate);
            }
        }
        private async Task GetModuleSessionFromCache()
        {
            request.ModuleSessionDetails = await cacheProvider.GetValueAsync<ModuleSessionModel>(request.Inner.SessionId);
        }
    }
}