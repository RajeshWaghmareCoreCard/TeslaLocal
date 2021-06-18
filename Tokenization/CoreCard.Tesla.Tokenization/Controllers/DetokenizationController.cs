using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.Tokenization.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Consumes("application/json")]
    public class DetokenizationController : ControllerBase
    {
        private readonly ILogger<DetokenizationController> logger;
        private readonly IDetokenizationService detokenizationService;

        public DetokenizationController(ILogger<DetokenizationController> logger, IDetokenizationService detokenizationService)
        {
            this.logger = logger;
            this.detokenizationService = detokenizationService;
        }
        [HttpPost("v1/Detokenize")]
        public async Task<DetokenizationResponse> Detokenize(DetokenizationRequest request)
        {
            TokenizationHeaders headers = new TokenizationHeaders();
            headers.ModuleId = Request.Headers["ModuleId"];
            headers.TraceId = Request.Headers["TraceId"];
            return await detokenizationService.DetokenizeAsync(request, headers);
        }
    }
}