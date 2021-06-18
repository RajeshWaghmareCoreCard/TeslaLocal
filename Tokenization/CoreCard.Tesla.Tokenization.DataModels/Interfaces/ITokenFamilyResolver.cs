using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface ITokenFamilyResolver
    {
        // ICardTokenFamilyProvider CardTokenFamilyProvider { get; }
        //ISSNTokenFamilyProvider SSNTokenFamilyProvider { get; }
        //IFamilyProvider GetFamilyProvider(TokenFamilyType type);
        IFamilyProvider GetFamilyProvider(string tokenFamily);
    }
}
