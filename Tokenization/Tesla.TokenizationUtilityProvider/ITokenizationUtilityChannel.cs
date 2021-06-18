using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels;

namespace Tesla.TokenizationProvider
{
    public interface ITokenizationUtilityChannel
    {
        Task<TokenizationResponse> CreateToken(HttpClient httpClient, TokenizationRequest request);

        Task<DetokenizationResponse> Detokenize(HttpClient httpClient, DetokenizationRequest request);
    }

}
