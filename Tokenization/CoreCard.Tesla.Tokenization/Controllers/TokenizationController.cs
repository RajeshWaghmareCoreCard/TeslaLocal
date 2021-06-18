using System;
using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreCard.Tesla.Tokenization.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    public class TokenizationController : ControllerBase
    {
        readonly ILogger<TokenizationController> _logger;
        private readonly ITokenizationService tokenizationService;
        public TokenizationController(ILogger<TokenizationController> logger, ITokenizationService tokenizationService)
        {
            _logger = logger;
            this.tokenizationService = tokenizationService;
        }
        [HttpPost("v1/CreateToken")]
        public async Task<TokenizationResponse> TokenizationRequest(TokenizationRequest request)
        {
            TokenizationHeaders headers = new TokenizationHeaders();
            headers.ModuleId = Request.Headers["ModuleId"];
            headers.TraceId = Request.Headers["TraceId"];
            return await tokenizationService.TokenizeData(request, headers);
        }
    }
}