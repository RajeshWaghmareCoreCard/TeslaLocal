using System;
using System.Security.Cryptography;
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
    public class DetokenizationService : TokenizationBaseService, IDetokenizationService
    {
        #region DataMembers
        DetokenizationRequestExt request;
        #endregion

        #region Interfaces service providers
        readonly IModuleSessionRepository tokenizationRepository;
        readonly ITokenFamilyProvider tokenFamilyAggregator;
        #endregion

        public DetokenizationService(ITokenFamilyProvider tokenFamilyAggregator, ICacheProvider cacheProvider, IModuleSessionRepository tokenizationRepository, IModulePermissionRepository modulePermissionRepository) : base(cacheProvider, modulePermissionRepository)
        {
            this.tokenFamilyAggregator = tokenFamilyAggregator;
            this.tokenizationRepository = tokenizationRepository;
            request = new DetokenizationRequestExt();
        }

        public async Task<DetokenizationResponse> DetokenizeAsync(DetokenizationRequest inner, TokenizationHeaders headers)
        {
            try
            {
                InitRequest(inner, headers);
                await ValidationPermissions(headers.ModuleId, API.Detokenize, request.Inner.Token);
                await GetModuleSessionDetailsAsync();
                CreateAesSession(request.ModuleSessionDetails.KeyDetails);
                DecryptAesData(inner.Token.Value);
                await GetTokenAsync();
                await DecryptToken();
                return await PrepareDetokenizeResponse();
            }
            catch (TeslaException ex)
            {
                return new DetokenizationResponse() { ResponseCode = ex.ResponseCode, ResponseMessage = ex.ResponseMessage };
            }
            catch (Exception)
            {
                return new DetokenizationResponse()
                {
                    ResponseCode = TokenizationRsponseCodes.SystemMalfunction,
                    ResponseMessage = TokenizationResponseMessages.SystemMalfunction
                };
            }
            finally
            {
                AesSession?.Dispose();
            }
        }

        private async Task DecryptToken()
        {
            request.TokenDetails = await tokenFamilyAggregator.DeTokenize(request.TokenDetails);
        }
        private void InitRequest(DetokenizationRequest inner, TokenizationHeaders headers)
        {
            this.request.Inner = inner;
            this.request.Headers = headers;
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
        private async Task<DetokenizationResponse> PrepareDetokenizeResponse()
        {
            await ReportTraceIdToCache(request.Inner, request.Headers);
            DetokenizationResponse response = new DetokenizationResponse();
            response.Data = AesSession.Encrypt(request.TokenDetails.ClearToken);
            response.ResponseCode = TokenizationRsponseCodes.Success;
            response.ResponseMessage = TokenizationResponseMessages.Success;
            return response;
        }

        private async Task GetTokenAsync()
        {
            request.TokenDetails = await tokenFamilyAggregator.GetTokenDataAsync(request.Inner.Token.Family, AesClearValue);
        }
    }
}