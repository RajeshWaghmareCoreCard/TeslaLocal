using System;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;

namespace CoreCard.Tesla.Tokenization.TokenFamilyProviders
{
    public class TokenFamilyResolver : ITokenFamilyResolver
    {
        private readonly Lazy<ICardTokenFamilyProvider> cardTokenFamilyProvider;
        private readonly Lazy<ISSNTokenFamilyProvider> ssnTokenFamilyProvider;
        private readonly Lazy<IOtherTokenFamilyProviders> otherTokenFamilyProviders;

        public TokenFamilyResolver(Lazy<ICardTokenFamilyProvider> cardTokenFamilyProvider, Lazy<ISSNTokenFamilyProvider> ssnTokenFamilyProvider, Lazy<IOtherTokenFamilyProviders> otherTokenFamilyProviders)
        {
            this.cardTokenFamilyProvider = cardTokenFamilyProvider;
            this.ssnTokenFamilyProvider = ssnTokenFamilyProvider;
            this.otherTokenFamilyProviders = otherTokenFamilyProviders;
        }
        public IFamilyProvider GetFamilyProvider(string tokenFamily)
        {
            switch (tokenFamily.ToLower())
            {
                case "accountnumber":
                    break;
                case "sss":
                    return ssnTokenFamilyProvider.Value;
                case "cardnumber":
                    return cardTokenFamilyProvider.Value;
                default:
                    {
                        return otherTokenFamilyProviders.Value;
                    }
            }
            return null;
        }
    }
}