using System.Security.Cryptography;
using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.DataModels
{
    public interface ITokenizationService
    {
        Task<TokenizationResponse> TokenizeData(TokenizationRequest request, TokenizationHeaders headers);
    }

    public class TokenizationRequest
    {
        public Token Token { get; set; } = new Token();
        public string InstitutionId { get; set; }
        public string SessionId { get; set; }
    }

    public class TokenizationRequestExt
    {
        public TokenizationRequest Inner { get; set; }
        public ModuleSessionModel ModuleSessionDetails { get; set; }
        public TokenFamilyDetails TokenDetails { get; set; }
        public TokenizationHeaders Headers { get; set; }
    }

    public class TokenizationResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string TokenData { get; set; }
    }
}