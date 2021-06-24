using CoreCard.Tesla.Falcon.DataModels.Model;
using CoreCard.Tesla.Falcon.Services.Purchase;
using CoreCard.Tesla.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseBAL _purchaseBal;
        private readonly TimeLogger _timeLogger;
        public PurchaseController(IPurchaseBAL purchaseBal, TimeLogger timeLogger)
        {
            _purchaseBal = purchaseBal;
            _timeLogger = timeLogger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionAddDTO transactionDTO)
        {
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            _timeLogger.Start("PurcahseController");
            baseResponseDTO = await _purchaseBal.AddTransactionAsync(transactionDTO);

            if (baseResponseDTO.BaseEntityInstance != null)
            {
                baseResponseDTO.ControllerLayerTime = _timeLogger.StopAndLog("PurchaseController");
                _timeLogger.Dispose();
                return new CreatedAtActionResult("Create", "Purchase", transactionDTO, baseResponseDTO);
            }
            else
            {
                return BadRequest("Error occured please try again.");
            }
        }
    }
}
