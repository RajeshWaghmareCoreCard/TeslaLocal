using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using CoreCard.Tesla.Falcon.NpgRepository.Interface;
using Microsoft.Extensions.Logging;
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
        public PaymentNpgBAL(IPurchaseUnit purchaseUnit, ILogger<PaymentNpgBAL> logger)
        {
            _purchaseUnit = purchaseUnit;
            _logger = logger;
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
    }
}
