using CoreCard.Tesla.Falcon.DataModels.Model;
using CoreCard.Tesla.Falcon.Services;
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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentBAL _paymentBal;
        private readonly TimeLogger _timeLogger;
        public PaymentController(IPaymentBAL paymentBal, TimeLogger timeLogger)
        {
            _paymentBal = paymentBal;
            //_accountBAL = accountBAL;
            _timeLogger = timeLogger;
        }
        [HttpPost("MakePayment")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentAddDTO payment)
        {
            _timeLogger.Start("PaymentController");

            BaseResponseDTO baseResponseDTO = await _paymentBal.AddPaymentADOAsync(payment);

            if (baseResponseDTO.BaseEntityInstance != null)
            {
                baseResponseDTO.ControllerLayerTime = _timeLogger.StopAndLog("PaymentController");
                _timeLogger.Dispose();
                return new CreatedAtActionResult("MakePayment", "Payment", payment, baseResponseDTO);
            }
            else
            {
                return BadRequest("Error occured please try again.");
            }
        }
        [HttpPost("CheckSimpleRequest")]
        public IActionResult CheckRequest()
        {
            return Ok();
            // _timeLogger.Start("PaymentController");

            //BaseResponseDTO baseResponseDTO = await _paymentBal.AddPaymentADOAsync(payment);

            //if (baseResponseDTO.BaseEntityInstance != null)
            //{
            //    baseResponseDTO.ControllerLayerTime = _timeLogger.StopAndLog("PaymentController");
            //    _timeLogger.Dispose();
            //    return new CreatedAtActionResult("MakePayment", "Payment", payment, baseResponseDTO);
            //}
            //else
            //{
            //    return BadRequest("Error occured please try again.");
            //}
        }

        [HttpPost("CheckDBConnection")]
        public IActionResult CheckRCheckDBConnectionequest()
        {
            //return Ok();
            // _timeLogger.Start("PaymentController");

            BaseResponseDTO baseResponseDTO = _paymentBal.CheckDBConnection();

            if (baseResponseDTO.BaseEntityInstance != null)
            {
                //baseResponseDTO.ControllerLayerTime = _timeLogger.StopAndLog("PaymentController");
                //_timeLogger.Dispose();
                return Ok(baseResponseDTO);
            }
            else
            {
                return BadRequest("Error occured please try again.");
            }
        }

        [HttpPost("CheckDBTransaction")]
        public async Task<IActionResult> CheckRCheckDBTransaction()
        {
            //return Ok();
            // _timeLogger.Start("PaymentController");

            string baseResponseDTO = await _paymentBal.CheckDBTransaction();

            if (baseResponseDTO != null)
            {
                //baseResponseDTO.ControllerLayerTime = _timeLogger.StopAndLog("PaymentController");
                //_timeLogger.Dispose();
                return Ok(baseResponseDTO);
            }
            else
            {
                return BadRequest("Error occured please try again.");
            }
        }
    }
}
