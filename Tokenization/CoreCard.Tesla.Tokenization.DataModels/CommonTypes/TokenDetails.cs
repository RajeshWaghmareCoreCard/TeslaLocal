using CoreCard.Tesla.Tokenization.DataModels.DtoModels;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;

namespace CoreCard.Tesla.Tokenization.DataModels.CommonTypes
{
    public class TokenFamilyDetails
    {
        //public Token RequestToken { get; set; }
        public string Family { get; set; }
        public string TokenId { get; set; }
        public string ClearToken { get; set; }

        #region TokenFamily Data Members
        public CardTokenModel CardTokenDetails { get; set; }
        public OtherFamilyModel OtherFamilyDetails { get; set; }
        #endregion
    }
}