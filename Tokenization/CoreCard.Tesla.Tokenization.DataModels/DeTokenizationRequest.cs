using System.Security.Cryptography;
using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.DataModels
{
    public interface IDetokenizationService
    {
        Task<DetokenizationResponse> DetokenizeAsync(DetokenizationRequest request, TokenizationHeaders headers);
    }
    public class DetokenizationRequest
    {
        public Token Token { get; set; }  = new Token();
        public string InstitutionId { get; set; }
        public string SessionId { get; set; }
    }

    public class DetokenizationResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string Data { get; set; }
    }
    public class DetokenizationRequestExt
    {
        public DetokenizationRequest Inner { get; set; }
        public TokenFamilyDetails TokenDetails { get; set; }
        public ModuleSessionModel ModuleSessionDetails { get; set; }
        public TokenizationHeaders Headers { get; set; }
    }

    public class DetokenizationResponseExt
    {
        public DetokenizationResponse Inner { get; set; }
    }
}