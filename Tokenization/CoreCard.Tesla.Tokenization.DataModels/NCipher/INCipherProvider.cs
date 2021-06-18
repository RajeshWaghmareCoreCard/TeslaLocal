using System.Threading.Tasks;

namespace CoreCard.Tesla.Tokenization.DataModels.NCipher
{
    public interface INCipherProvider
    {
         Task<EncryptResponse> EncryptAsync(EncryptRequest request);
         Task<DecryptResponse> DecryptAsync(DecryptRequest request);
    }
}