using CoreCard.Tesla.Falcon.DataModels.Common;

namespace CoreCard.Tesla.Falcon.Adc.Contracts
{
    public interface IAuthDecisionControlResolver
    {
        IAuthDecisionControl GetAuthDecisionControl(string acdId);
        //ICardExpirationAdc CardExpirationADC { get; }
    }
}