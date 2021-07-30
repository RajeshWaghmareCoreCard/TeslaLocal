using CoreCard.Tesla.Falcon.DataModels.Entity;
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
    public class PaymentNpgController : ControllerBase
    {
        private readonly IPaymentNpgBAL _paymentBal;
        public PaymentNpgController(IPaymentNpgBAL paymentBal/*, TimeLogger timeLogger*/)
        {
            _paymentBal = paymentBal;
            //_accountBAL = accountBAL;
            //_timeLogger = timeLogger;
        }
        [HttpPost("MakeNpgPayment")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentAddDTO payment)
        {
            //_timeLogger.Start("PaymentController");

            //BaseResponseDTO baseResponseDTO = await _paymentBal.AddPaymentADOAsync(payment);
            Transaction baseResponseDTO = await _paymentBal.DoPayment(payment);

            if (baseResponseDTO != null)
            {
                //baseResponseDTO.ControllerLayerTime = _timeLogger.StopAndLog("PaymentController");
                //_timeLogger.Dispose();
                return new CreatedAtActionResult("MakePayment", "Payment", payment, baseResponseDTO);
            }
            else
            {
                return BadRequest("Error occured please try again.");
            }
            //return Ok();
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
