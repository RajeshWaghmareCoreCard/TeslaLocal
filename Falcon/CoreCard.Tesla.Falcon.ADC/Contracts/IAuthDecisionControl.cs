using System.Threading.Tasks;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DataModels.Repository;

namespace CoreCard.Tesla.Falcon.Adc.Contracts
{
    public interface IAuthDecisionControl
    {
        Task<ADCResult> Execute(TransactionModel transaction,
                                       CustomerModel customer,
                                       AccountModel customerAccount,
                                       CardModel card,
                                       AuthDecisionControl ADCDetail);
    }
}