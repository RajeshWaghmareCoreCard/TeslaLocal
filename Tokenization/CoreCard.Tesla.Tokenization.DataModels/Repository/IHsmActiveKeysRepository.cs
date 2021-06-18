using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;

namespace CoreCard.Tesla.Tokenization.DataModels.Repository
{
    public interface IHsmActiveKeysRepository
    {
         Task<HsmActiveKeyModel> GetActiveKey(string tokenFamilyId);
    }
}