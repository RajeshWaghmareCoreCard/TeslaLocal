using System;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.Adc.Contracts;
using CoreCard.Tesla.Falcon.DataModels.Common;

namespace CoreCard.Tesla.Falcon.Adc
{
    public class AuthDecisionControlResolver : IAuthDecisionControlResolver
    {
        public AuthDecisionControlResolver(Lazy<ICardNumberAdc> cardNumberADC, Lazy<ICardExpirationAdc> cardExpirationADC)
        {
            this.cardExpirationADC = cardExpirationADC;
            this.cardNumberADC = cardNumberADC;

        }
        #region Lazy data members
        private readonly Lazy<ICardNumberAdc> cardNumberADC;
        private readonly Lazy<ICardExpirationAdc> cardExpirationADC;
        #endregion

        public IAuthDecisionControl GetAuthDecisionControl(string acdId)
        {
            switch (acdId)
            {
                case MasteAdcs.CardNumberCheckAdc:
                    {
                        return cardNumberADC.Value;
                    }
                case MasteAdcs.CardExpirationDateAdc:
                    {
                        return cardExpirationADC.Value;
                    }
            }
            return null;
            //throw new TeslaException(FalconResponseCodes.InvalidAdc, FalconResponseMessages.InvalidAdc);
        }
    }
}