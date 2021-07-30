using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using CoreCard.Tesla.Falcon.NpgRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public class PaymentNpgBAL : IPaymentNpgBAL
    {
        private readonly IPurchaseUnit _purchaseUnit;
        private readonly ILogger<PaymentNpgBAL> _logger;
        private readonly IConfiguration _configuration;
        private string conn = "";

        public PaymentNpgBAL(IPurchaseUnit purchaseUnit, ILogger<PaymentNpgBAL> logger, IConfiguration configuration)
        {
            _purchaseUnit = purchaseUnit;
            _logger = logger;
            _configuration = configuration;
            conn = configuration.GetConnectionString("CockroachDb");
        }
        public Task<Transaction> DoPayment(PaymentAddDTO paymentAddDTO)
        {
            try
            {
                return _purchaseUnit.MakePayment(paymentAddDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred");
                throw;
            }

        }

        public BaseResponseDTO CheckDBConnection()
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

                throw;
            }
            //baseResponseDTO.BaseEntityInstance = "Result{'Message':'API Responded Successfully'}";
            return baseResponseDTO;
        }

        public async Task<string> CheckDBTransaction()
        {
            // BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            string message = "";
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


                message = "Result{'Message':'API Responded Successfully for transaction open and close'}";
            }
            catch (Exception ex)
            {
                message = "Result{'Message':'Error Occurred for transaction open and close'} " + ex.StackTrace;
            }
            //baseResponseDTO.BaseEntityInstance = "Result{'Message':'API Responded Successfully'}";
            return message;
        }
    }
}
