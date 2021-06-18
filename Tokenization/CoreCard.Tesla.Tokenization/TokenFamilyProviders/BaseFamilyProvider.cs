using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;

namespace CoreCard.Tesla.Tokenization.TokenFamilyProviders
{
    public abstract class BaseFamilyProvider
    {
        #region  Data Members
        protected TokenFamilyDetails tokenDetails;
        protected HsmActiveKeyModel activeKeyModel;
        #endregion
    }
}