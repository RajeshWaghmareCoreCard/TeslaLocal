using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.Adc.Contracts;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;

namespace CoreCard.Tesla.Falcon.Adc
{
    public class CardNumberAdc : ICardNumberAdc
    {
        public CardNumberAdc()
        {

        }
        public async Task<ADCResult> Execute(TransactionModel transaction, CustomerModel customer, AccountModel customerAccount, CardModel card, AuthDecisionControl ADCDetail)
        {
            return await Task.FromResult(new ADCResult() { ResultCode = "00", ResultMessage = "Success" });
        }
    }
}