using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Utilities;
using Polly;
using Polly.Extensions.Http;

namespace Tesla.TokenizationProvider
{
    public class TokenizationUtilityChannel : ITokenizationUtilityChannel
    {
        private readonly string privateKey;
        private readonly double rotateKeyTimer;
        private readonly string moduleKeyId;
        private readonly string tokenizationURL;
        private readonly ICacheProvider cacheProvider;
        private readonly string moduleName;

        //Todo- locking mechanism 
        private Aes aesSession;
        private string sessionId;
        private System.Timers.Timer aesRecycleTimer;

        public TokenizationUtilityChannel(string privateKey, double rotateKeyTimer, string moduleKeyId, string tokenizationURL, ICacheProvider cacheProvider, string moduleName)
        {
            this.privateKey = privateKey;
            this.rotateKeyTimer = rotateKeyTimer;
            this.moduleKeyId = moduleKeyId;
            this.tokenizationURL = tokenizationURL;
            this.cacheProvider = cacheProvider;
            this.moduleName = moduleName;
            if (rotateKeyTimer >= 0)
                CreateAesRecycleTimer(rotateKeyTimer);
            else
            {
                //TODO - logging
            }
            GetAesSession().Wait();
        }

        #region Aes Registration methods

        async Task GetAesSession()
        {
            var createSessionResponse = await CreateSession();
            sessionId = createSessionResponse.SessionId;
            DecryptKeyDetails(createSessionResponse.KeyDetails);
            aesSession = CreateAesKey(createSessionResponse.KeyDetails);
            await cacheProvider.SetValueWithExpirationAsync<Aes>(moduleKeyId, aesSession, DateTime.Now.AddMinutes(rotateKeyTimer));
        }

        private void CreateAesRecycleTimer(double rotateKeyTimer)
        {
            aesRecycleTimer = new System.Timers.Timer(rotateKeyTimer.ConvertMinutesToMilliseconds());
            // Hook up the Elapsed event for the timer. 
            aesRecycleTimer.Elapsed += OnTimedEvent;
            aesRecycleTimer.AutoReset = true;
            aesRecycleTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            GetAesSession().Wait();
        }

        private Aes CreateAesKey(KeyDetails keyDetails)
        {
            Aes aesAlg = Aes.Create();
            {
                aesAlg.Key = keyDetails.Key.TryFromBase64String();
                aesAlg.IV = keyDetails.Vector.TryFromBase64String();
            }
            return aesAlg;
        }


        private void DecryptKeyDetails(KeyDetails keyDetails)
        {
            var rsa = privateKey.CreateRSAFromPrivateKey();
            keyDetails.Key = Encoding.Default.GetString(rsa.Decrypt(keyDetails.Key.TryFromBase64String(), RSAEncryptionPadding.Pkcs1));
            keyDetails.Vector = Encoding.Default.GetString(rsa.Decrypt(keyDetails.Vector.TryFromBase64String(), RSAEncryptionPadding.Pkcs1));
        }


        #endregion
        private async Task<CreateSessionResponse> CreateSession()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(tokenizationURL);
            CreateSessionRequest request = new CreateSessionRequest();
            httpClient.DefaultRequestHeaders.Add("TraceId", "dadsadad");
            httpClient.DefaultRequestHeaders.Add("ModuleId", moduleName);
            request.ModuleKeyId = moduleKeyId;
            request.TTLInMinutes = rotateKeyTimer;
            return await HttpClientExtensions.PostJsonAsync<CreateSessionResponse, CreateSessionRequest>(httpClient, request, "v1/CreateSession");
        }

        public async Task<TokenizationResponse> CreateToken(HttpClient httpClient, TokenizationRequest request)
        {
            request.SessionId = sessionId;
            request.Token.Value = aesSession.Encrypt(request.Token.Value);
            var response = await HttpClientExtensions.PostJsonAsync<TokenizationResponse, TokenizationRequest>(httpClient, request, "v1/CreateToken");
            response.TokenData = aesSession.Decrypt(response.TokenData);
            return response;
        }

        public async Task<DetokenizationResponse> Detokenize(HttpClient httpClient, DetokenizationRequest request)
        {
            request.SessionId = sessionId;
            request.Token.Value = aesSession.Encrypt(request.Token.Value);
            var response = await HttpClientExtensions.PostJsonAsync<DetokenizationResponse, DetokenizationRequest>(httpClient, request, "v1/Detokenize");
            response.Data = aesSession.Decrypt(response.Data);
            return response;
        }

    }
}
