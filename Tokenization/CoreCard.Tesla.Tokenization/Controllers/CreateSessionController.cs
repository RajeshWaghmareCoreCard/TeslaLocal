using System.Net.Mime;
using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.Tokenization.Controllers
{
    [ApiController]
    // [Route("[controller]s")]
    [Consumes("application/json")]
    public class CreateSessionController : ControllerBase
    {
        private readonly ILogger<CreateSessionController> logger;
        private readonly ICreateSessionService createSessionService;

        public CreateSessionController(ILogger<CreateSessionController> logger, ICreateSessionService createSessionService)
        {
            this.logger = logger;
            this.createSessionService = createSessionService;
        }
        [HttpPost("v1/CreateSession")]
        public async Task<ActionResult<CreateSessionResponse>> CreateSession(CreateSessionRequest request)
        {
            TokenizationHeaders headers = new TokenizationHeaders();
            headers.ModuleId = Request.Headers["ModuleId"];
            headers.TraceId = Request.Headers["TraceId"];
            var response = await createSessionService.CreateSession(request, headers);
            return Ok(response);
        }
    }
}