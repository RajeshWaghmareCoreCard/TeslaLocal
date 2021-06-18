using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.NCipher;

namespace CoreCard.Tesla.Tokenization.DataModels.Types
{
   public abstract class FamilyProvider
    {
        protected abstract Task<HsmActiveKeyModel> GetActiveKeyAsync();
        protected abstract Task<EncryptResponse> EncryptDataAsync(string aesClearValue, HsmActiveKeyModel activeKey);
        protected abstract TokenFamilyDetails BuildTokenDetails(HsmActiveKeyModel activeKey, EncryptResponse encryptResponse, string aesClearValue, string InstitutionId);
        protected abstract Task<TokenFamilyDetails> InsertTokenAsync(TokenFamilyDetails tokenDetails);
        protected abstract Task<TokenFamilyDetails> UpdateTokenAsync(TokenFamilyDetails tokenDetails);
    }
}