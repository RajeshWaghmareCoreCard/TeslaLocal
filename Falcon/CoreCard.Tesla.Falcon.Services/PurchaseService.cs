using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using CoreCard.Tesla.Falcon.DataModels;
using CoreCard.Tesla.Falcon.Adc.Contracts;
using CoreCard.Tesla.Falcon.ServiceInterfaces;
using CoreCard.Tesla.Falcon.DataModels.Repository;
using CoreCard.Tesla.Falcon.Services.ServiceInterfaces;
using Tesla.TokenizationProvider;
using System.Linq;

namespace CoreCard.Tesla.Falcon.Services
{
    public class PurchaseService : FalconBaseService, IPurchaseService
    {
        #region Interface Data Members
        private readonly ICustomerCardAccountUnit customerCardAccountUnit;
        private readonly IAuthDecisionControlProvider authDecisionControlManager;
        #endregion

        #region DataModels
        TransactionModel TransactionModel { get; } = new TransactionModel();
        PurchaseResponse response { get; } = new PurchaseResponse();
        #endregion

        public PurchaseService(ICustomerCardAccountUnit customerCardAccountUnit,
                               IAuthDecisionControlProvider authDecisionControlProvider)
        {
            this.customerCardAccountUnit = customerCardAccountUnit;
            this.authDecisionControlManager = authDecisionControlProvider;
        }
        public async Task<PurchaseResponse> PurchaseRequestAsync(PurchaseRequest request)
        {
            try
            {
                CreateTransaction(request);
                await GetCustomerAccountCard();
                await RegisterAuthDecisionControlsAsync();
                await InsertTransactionAsync();
                await ExecuteAdcsAsync();
                ProcessAdcsExecutionResultAsync();
                await UpdateTransactionAsync();
                PrepareResponse();
            }
            catch (Exception ex)
            {
                var message = ex.InnerException;
            }
            finally
            {
            }
            return await Task.FromResult(new PurchaseResponse());
        }
        private void CreateTransaction(PurchaseRequest request)
        {
            TransactionModel.CardToken = request.RequestDataElements.CardToken;
            TransactionModel.ExpirationDate = request.RequestDataElements.ExpirationDate;
            TransactionModel.MerchantDetails = new Merchant() { };
            TransactionModel.TransactionAmount = request.RequestDataElements.TransactionAmount;
            TransactionModel.TransactionId = request.RequestDataElements.TransactionAmount;

        }
        private async Task GetCustomerAccountCard()
        {
            await customerCardAccountUnit.GetAsync(TransactionModel.CardToken);
        }
        private async Task RegisterAuthDecisionControlsAsync()
        {
            await authDecisionControlManager.RegisterAuthDecisionControlsAsync(customerCardAccountUnit.Card.ProductId, customerCardAccountUnit.Card.CustomerId, customerCardAccountUnit.Card.AccountId, customerCardAccountUnit.Card.CardId);
        }
        private void PrepareResponse()
        {

        }

        private async Task UpdateTransactionAsync()
        {
           await customerCardAccountUnit.Update(TransactionModel);
        }

        private async Task InsertTransactionAsync()
        {
            await Task.Delay(1);
        }

        private void ProcessAdcsExecutionResultAsync()
        {
            var adcsExecutionResult = authDecisionControlManager.AdcResults;
            var failedAdcs = adcsExecutionResult.Where(r => r.Value.ResultCode != "00").Select(a => new { AdcId = a.Key, AdcResult = a.Value }).ToList();
            if (failedAdcs?.Count > 0)
            {
                response.ResponseCode = "96";
                response.ResponseMessage = "Declined";
            }
        }

        private async Task ExecuteAdcsAsync()
        {
            await authDecisionControlManager.Execute(TransactionModel, customerCardAccountUnit.Customer, customerCardAccountUnit.Account, customerCardAccountUnit.Card);
        }
    }
}
