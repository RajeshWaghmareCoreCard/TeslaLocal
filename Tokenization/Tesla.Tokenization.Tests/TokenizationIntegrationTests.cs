using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.CacheProvider;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Utilities;
using Tesla.TokenizationProvider;
using Xunit;

namespace Tesla.Tokenization.Tests
{
    public class TokenizationIntegrationTests
    {
        [Fact]
        public void CreateSessionTest()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:6001");
            CreateSessionRequest request = new CreateSessionRequest();
            httpClient.DefaultRequestHeaders.Add("TraceId", "jhgsjf-shabjdab-adjadbak");
            httpClient.DefaultRequestHeaders.Add("ModuleId", "Falcon");
            request.ModuleKeyId = "Falcon01";
            request.TTLInMinutes = 300;
            var response = HttpClientExtensions.PostJsonAsync<CreateSessionResponse, CreateSessionRequest>(httpClient, request, "v1/CreateSession").Result;

            var rsa = RSAExtensions.CreateRSAFromPrivateKey(File.ReadAllText("Falcon_RSA_private_key.pem"));
            var key = Encoding.Default.GetString(rsa.Decrypt(response.KeyDetails.Key.TryFromBase64String(), RSAEncryptionPadding.Pkcs1));
            var vector = Encoding.Default.GetString(rsa.Decrypt(response.KeyDetails.Vector.TryFromBase64String(), RSAEncryptionPadding.Pkcs1));
        }
        [Fact]
        public void ExistingCardNumberTest()
        {
            var keyRotationKey = 100;

            ITokenizationUtilityChannel provider = new TokenizationUtilityChannel(File.ReadAllText("Falcon_RSA_private_key.pem"), keyRotationKey, "Falcon01", "http://localhost:6001", new MemoryCacheProvider(new NewtonsoftSerializer()), "Falcon01");

            TokenizationRequest request = new TokenizationRequest();
            request.Token.Value = "4111111111111111";
            request.Token.Family = "CardNumber";
            request.InstitutionId = "Saturn0001";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("ModuleId", "Falcon01");
            httpClient.BaseAddress = new Uri("http://localhost:6001");
            var response = provider.CreateToken(httpClient, request).Result;

            DetokenizationRequest derequest = new DetokenizationRequest();
            derequest.Token.Value = response.TokenData;
            derequest.Token.Family = "CardNumber";
            derequest.InstitutionId = "Saturn0001";
            HttpClient httpClient1 = new HttpClient();
            httpClient1.DefaultRequestHeaders.Add("ModuleId", "Falcon01");
            httpClient1.BaseAddress = new Uri("http://localhost:6001");
            var response1 = provider.Detokenize(httpClient1, derequest).Result;
        }
        [Fact]
        public void NewCardNumberTest()
        {
            var keyRotationKey = 100;

            ITokenizationUtilityChannel provider = new TokenizationUtilityChannel(File.ReadAllText("Falcon_RSA_private_key.pem"), keyRotationKey, "Falcon01", "http://localhost:6001", new MemoryCacheProvider(new NewtonsoftSerializer()), "Falcon01");

            TokenizationRequest request = new TokenizationRequest();
            request.Token.Value = "5111111111114444";
            request.Token.Family = "CardNumber";
            request.InstitutionId = "Saturn0001";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("ModuleId", "Falcon01");
            httpClient.BaseAddress = new Uri("http://localhost:6001");
            var response = provider.CreateToken(httpClient, request).Result;

            DetokenizationRequest derequest = new DetokenizationRequest();
            derequest.Token.Value = response.TokenData;
            derequest.Token.Family = "CardNumber";
            derequest.InstitutionId = "Saturn0001";
            HttpClient httpClient1 = new HttpClient();
            httpClient1.DefaultRequestHeaders.Add("ModuleId", "Falcon01");
            httpClient1.BaseAddress = new Uri("http://localhost:6001");
            var response1 = provider.Detokenize(httpClient1, derequest).Result;
        }

        [Fact]
        public void DetokenizeTest()
        {
            var keyRotationKey = 100;

            ITokenizationUtilityChannel provider = new TokenizationUtilityChannel(File.ReadAllText("Falcon_RSA_private_key.pem"), keyRotationKey, "Falcon01", "http://localhost:6001", new MemoryCacheProvider(new NewtonsoftSerializer()), "Falcon01");

            DetokenizationRequest derequest = new DetokenizationRequest();
            derequest.Token.Value = "511111AA3A12B5DF344991A9F9267AB4F246F31111";
            derequest.Token.Family = "CardNumber";
            derequest.InstitutionId = "Saturn0001";
            HttpClient httpClient1 = new HttpClient();
            httpClient1.DefaultRequestHeaders.Add("ModuleId", "Falcon01");
            httpClient1.BaseAddress = new Uri("http://localhost:6001");
            var response1 = provider.Detokenize(httpClient1, derequest).Result;
        }
        [Fact]
        public void NetworkInterfaceTokenizeTest()
        {
            var keyRotationKey = 100;

            ITokenizationUtilityChannel provider = new TokenizationUtilityChannel(File.ReadAllText("Falcon_RSA_private_key.pem"), keyRotationKey, "Falcon01", "http://localhost:6001", new MemoryCacheProvider(new NewtonsoftSerializer()), "Falcon01");

            TokenizationRequest request = new TokenizationRequest();
            request.Token.Value = "5111111111114444";
            request.Token.Family = "CardNumber";
            request.InstitutionId = "Saturn0001";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("ModuleId", "Falcon01");
            httpClient.BaseAddress = new Uri("http://localhost:6001");
            var response = provider.CreateToken(httpClient, request).Result;
        }
        [Fact]
        public void EmbossingDetokenizeTest()
        {
            var keyRotationKey = 100;

            ITokenizationUtilityChannel provider = new TokenizationUtilityChannel(File.ReadAllText("Falcon_RSA_private_key.pem"), keyRotationKey, "Falcon01", "http://localhost:6001", new MemoryCacheProvider(new NewtonsoftSerializer()), "Falcon01");
            DetokenizationRequest derequest = new DetokenizationRequest();
            derequest.Token.Value = "511111AA3A12B5DF344991A9F9267AB4F246F31111";
            derequest.Token.Family = "CardNumber";
            derequest.InstitutionId = "Saturn0001";
            HttpClient httpClient1 = new HttpClient();
            httpClient1.DefaultRequestHeaders.Add("ModuleId", "Falcon");
            httpClient1.BaseAddress = new Uri("http://localhost:6001");
            var response1 = provider.Detokenize(httpClient1, derequest).Result;
        }
        [Fact]
        public void PANTokenizeTest()
        {
            var keyRotationKey = 100;

            ITokenizationUtilityChannel provider = new TokenizationUtilityChannel(File.ReadAllText("Falcon_RSA_private_key.pem"), keyRotationKey, "Falcon01", "http://localhost:6001", new MemoryCacheProvider(new NewtonsoftSerializer()), "Falcon");

            TokenizationRequest request = new TokenizationRequest();
            request.Token.Value = "1234567890";
            request.Token.Family = "PAN";
            request.InstitutionId = "Saturn0001";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("ModuleId", "Falcon");
            httpClient.BaseAddress = new Uri("http://localhost:6001");
            var response = provider.CreateToken(httpClient, request).Result;


            DetokenizationRequest derequest = new DetokenizationRequest();
            derequest.Token.Value = response.TokenData;
            derequest.Token.Family = "PAN";
            derequest.InstitutionId = "Saturn0001";
            HttpClient httpClient1 = new HttpClient();
            httpClient1.DefaultRequestHeaders.Add("ModuleId", "Falcon");
            httpClient1.BaseAddress = new Uri("http://localhost:6001");
            var response1 = provider.Detokenize(httpClient1, derequest).Result;
        }
    }
}
