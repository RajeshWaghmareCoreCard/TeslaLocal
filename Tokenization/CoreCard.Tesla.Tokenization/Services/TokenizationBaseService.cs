using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoModels;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using Tesla.TokenizationProvider;

namespace CoreCard.Tesla.Tokenization.Services
{
    public abstract class TokenizationBaseService
    {
        #region Interfaces service providers 
        protected readonly ICacheProvider cacheProvider;
        protected readonly IModulePermissionRepository modulePermissionRepository;

        #endregion

        ModulePermissionModel modulePermissionModel = null;
        protected Aes AesSession { get; set; }
        protected string AesClearValue { get; set; }
        public TokenizationBaseService(ICacheProvider cacheProvider, IModulePermissionRepository modulePermissionRepository)
        {
            this.cacheProvider = cacheProvider;
            this.modulePermissionRepository = modulePermissionRepository;
        }
        protected async Task ReportTraceIdToCache<T>(T value, TokenizationHeaders headers)
        {
            try
            {
                var response = await cacheProvider.IsExist<T>($"{headers.TraceId}{headers.ModuleId}");
                if (response?.Item1 == true)
                {
                    //Report to Kafka 
                }
                else
                {
                    await cacheProvider.SetValueAsync<T>($"{headers.TraceId}{headers.ModuleId}", value);
                }
            }
            catch
            {
                if (modulePermissionModel != null && (modulePermissionModel.NotifyDetokenizationOperation || modulePermissionModel.NotifyTokenizationOperation))
                {
                    //TODO - Notify Message Bus
                }

            }
        }

        protected async Task ValidationPermissions(string moduleId, API api, DataModels.Token token)
        {
            modulePermissionModel = await modulePermissionRepository.GetModulePermissions(moduleId, token.Family);

            if (modulePermissionModel != null)
            {
                if (api == API.CreateToken && modulePermissionModel.IsTokenizationAllowed)
                {
                    return;
                }
                if (api == API.Detokenize && modulePermissionModel.IsDetokenizationAllowed)
                {
                    return;
                }

                throw new TeslaException(TokenizationRsponseCodes.ModulePermissionIssue, TokenizationResponseMessages.ModulePermissionIssue);
            }
        }

        protected void CreateAesSession(KeyDetails keyDetails)
        {
            Aes aesAlg = Aes.Create();
            {
                aesAlg.Key = keyDetails.Key.TryFromBase64String();
                aesAlg.IV = keyDetails.Vector.TryFromBase64String();
                AesSession = aesAlg;
            }
        }
        protected void DecryptAesData(string encryptedValueUnderAesKey)
        {
            try
            {
                AesClearValue = AesSession.Decrypt(encryptedValueUnderAesKey);
            }
            catch (Exception)
            {
                throw new TeslaException(TokenizationRsponseCodes.AesGenerationFailed, TokenizationResponseMessages.AesGenerationFailed);
            }
        }
       
    }
}