using CoreCard.Tesla.Falcon.DataModels.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private string conn = "";

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = configuration.GetConnectionString("CockroachDb");
        }
            public IActionResult Index()
        {
            return View();
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
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(conn))
                {
                    con.Open();
                }

                baseResponseDTO.BaseEntityInstance = "Result{'Message':'API Responded Successfully for connection open and close'}";
                
            }
            catch (Exception ex)
            {

                baseResponseDTO.BaseEntityInstance =  "Result{'Message':'Error Occurred for connection open and close'} " + ex.StackTrace;
            }
            return Ok(baseResponseDTO);
        }

        [HttpPost("CheckDBTransaction")]
        public async Task<IActionResult> CheckRCheckDBTransaction()
        {
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(conn))
                {
                    await con.OpenAsync();
                    using (NpgsqlTransaction tran = con.BeginTransaction())
                    {
                        await tran.CommitAsync();
                    }

                }


                baseResponseDTO.BaseEntityInstance =  "Result{'Message':'API Responded Successfully for transaction open and close'}";
            }
            catch (Exception ex)
            {
                baseResponseDTO.BaseEntityInstance = "Result{'Message':'Error Occurred for transaction open and close'} " + ex.StackTrace;
            }
            return Ok(baseResponseDTO);
        }
    }
}
