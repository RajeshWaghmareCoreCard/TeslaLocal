using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;

namespace CoreCard.Tesla.Tokenization.Services
{
    public class CreateSessionService : TokenizationBaseService, ICreateSessionService
    {
        #region Interface Data Members
        private readonly IModuleSessionRepository moduleSessionRepository;
        private readonly IModuleKeyRepository moduleKeyRepository;

        #endregion

        #region DataMembers
        CreateSessionRequestExt request;
        CreateSessionResponseExt response;

        #endregion
        
        #region Constructor
        public CreateSessionService(IModuleSessionRepository tokenizationRepository, ICacheProvider cacheProvider, IModuleKeyRepository moduleKeyRepository, IModulePermissionRepository modulePermissionRepository) : base(cacheProvider, modulePermissionRepository)
        {
            this.moduleSessionRepository = tokenizationRepository;
            this.moduleKeyRepository = moduleKeyRepository;
            //TODO- need to work on object creation
            request = new CreateSessionRequestExt();
            response = new CreateSessionResponseExt() { Inner = new CreateSessionResponse() };
        }
        #endregion

        public async Task<CreateSessionResponse> CreateSession(CreateSessionRequest request, TokenizationHeaders headers)
        {
            try
            {
                InitRequest(request, headers);
                await GetModuleDetailsAsync();
                CreateAES();
                await InsertSessionAsync();
                await CacheSessionDetailsAsync();
                EncryptAesKey();
                return await PrepareResponse();
            }
            catch (TeslaException ex)
            {
                return new CreateSessionResponse() { ResponseCode = ex.ResponseCode, ResponseMessage = ex.ResponseMessage };
            }
            catch (Exception)
            {
                return new CreateSessionResponse() { ResponseCode = TokenizationRsponseCodes.SystemMalfunction, ResponseMessage = TokenizationResponseMessages.SystemMalfunction };
            }
        }

        private void EncryptAesKey()
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(request.ModuleKeyDetails.PublicKey);
            var keyByte = rsa.Encrypt(Encoding.Default.GetBytes(request.ModuleSessionDetails.KeyDetails.Key), RSAEncryptionPadding.Pkcs1);
            request.EncyptedAesKey = keyByte.TryToBase64String();
            request.EncryptedVector = rsa.Encrypt(Encoding.Default.GetBytes(request.ModuleSessionDetails.KeyDetails.Vector), RSAEncryptionPadding.Pkcs1).TryToBase64String();
        }

        private void InitRequest(CreateSessionRequest inner, TokenizationHeaders headers)
        {
            this.request.Inner = inner;
            this.request.Headers = headers;
        }

        private async Task GetModuleDetailsAsync()
        {
            request.ModuleKeyDetails = await moduleKeyRepository.GetModuleDetailsAsync(request.Inner.ModuleKeyId);
            if (request.ModuleKeyDetails.ModuleId.ToLower() != request.Headers.ModuleId.ToLower())
            {
                //Notify message bus about this request.
                new TeslaException(TokenizationRsponseCodes.InvalidModule, TokenizationResponseMessages.InvalidModule);
            }
        }

        private async Task CacheSessionDetailsAsync()
        {
            await cacheProvider.SetValueWithExpirationAsync<ModuleSessionModel>(request.ModuleSessionDetails.SessionId, request.ModuleSessionDetails, request.ModuleSessionDetails.SessionExpiryDate);
        }

        private async Task<CreateSessionResponse> PrepareResponse()
        {
            await ReportTraceIdToCache(request.Inner, request.Headers);
            response.Inner.KeyDetails = new KeyDetails() { Key = request.EncyptedAesKey, Vector = request.EncryptedVector };
            response.Inner.SessionId = request.ModuleSessionDetails.SessionId;
            response.Inner.ResponseCode = TokenizationRsponseCodes.Success;
            response.Inner.ResponseMessage = TokenizationResponseMessages.Success;
            return response.Inner;
        }
        private async Task InsertSessionAsync()
        {
            request.ModuleSessionDetails.SessionId = HelperExtensions.GetGUID();
            request.ModuleSessionDetails.ModuleId = request.ModuleKeyDetails.ModuleId;
            request.ModuleSessionDetails.SessionExpiryDate = DateTime.Now.AddMinutes(request.Inner.TTLInMinutes);
            await moduleSessionRepository.InsertModuleSessionAsync(request.ModuleSessionDetails);
        }
        private void CreateAES()
        {
            using (Aes AesSession = Aes.Create())
            {
                request.ModuleSessionDetails.KeyDetails.Vector = AesSession.IV.TryToBase64String();
                request.ModuleSessionDetails.KeyDetails.Key = AesSession.Key.TryToBase64String();
            }
        }
    }
}