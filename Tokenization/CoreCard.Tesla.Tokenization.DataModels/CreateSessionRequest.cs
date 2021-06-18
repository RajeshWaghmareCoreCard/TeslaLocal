using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Types;
using Microsoft.AspNetCore.Mvc;

namespace CoreCard.Tesla.Tokenization.DataModels
{
    public interface ICreateSessionService
    {
        Task<CreateSessionResponse> CreateSession(CreateSessionRequest request, TokenizationHeaders headers);
    }
    public class CreateSessionRequest
    {
        ///Max -999
        ///Default- 999
        public double TTLInMinutes { get; set; } = 999;
        public string ModuleKeyId { get; set; }
    }
    public class CreateSessionResponse
    {
        public KeyDetails KeyDetails { get; set; }
        public string SessionId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class CreateSessionRequestExt
    {
        public CreateSessionRequest Inner { get; set; }
        public ModuleSessionModel ModuleSessionDetails { get; set; } = new ModuleSessionModel();
        public ModuleKey ModuleKeyDetails { get; set; }
        public string EncyptedAesKey { get; set; }
        public string EncryptedVector { get; set; }
       public TokenizationHeaders Headers { get; set; }
    }
    public class CreateSessionResponseExt
    {
        public CreateSessionResponse Inner { get; set; }
    }
}
