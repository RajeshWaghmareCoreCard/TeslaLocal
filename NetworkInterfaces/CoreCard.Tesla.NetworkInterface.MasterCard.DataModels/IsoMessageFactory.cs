using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels
{
    //Implementing the IsoMessageFactory at MasterCard level and not in IsoMessaging because then MasterCard library can create messages only specific to MasterCard.
    public class IsoMessageFactory : IIsoMessageFactory
    {
        private readonly IIsoFieldFactory _fieldFactory;

        public IsoMessageFactory(IIsoFieldFactory fieldFactory)
        {
            _fieldFactory = fieldFactory;
        }
        public IIsoMessage GetIsoMessage(string MTI) => MTI switch
        {
            MessageTypeIdentifiers.NetworkManagementRequest => new NetworkManagementRequest(_fieldFactory),
            MessageTypeIdentifiers.NetworkManagementResponse => new NetworkManagementResponse(_fieldFactory),
            MessageTypeIdentifiers.AuthorizationRequest => new AuthorizationRequest(_fieldFactory),
            MessageTypeIdentifiers.AuthorizationResponse => new AuthorizationResponse(_fieldFactory),
            MessageTypeIdentifiers.FinancialRequest => new FinancialRequest(_fieldFactory),
            MessageTypeIdentifiers.FinancialResponse => new FinancialResponse(_fieldFactory),
            MessageTypeIdentifiers.FinancialTransactionAdvice => new FinancialTransactionAdvice(_fieldFactory),
            MessageTypeIdentifiers.FinancialTransactionAdviceResponse => new FinancialTransactionAdviceResponse(_fieldFactory),
            _ => null,
        };
    }
}
