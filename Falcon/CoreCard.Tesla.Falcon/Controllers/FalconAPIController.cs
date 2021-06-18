using System.Net.Mime;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace CoreCard.Tesla.Falcon.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    public class FalconAPIController : BaseController
    {
        public FalconAPIController(IPurchaseService purchaseService)
        {
            PurchaseService = purchaseService;
        }
        IPurchaseService PurchaseService { get; }

        [HttpPost]
        [Route("v1/Purchase")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PurchaseRequestAsync(PurchaseRequest request)
        {
            var response = await PurchaseService.PurchaseRequestAsync(request);
            return Ok(response);
        }
    }
}