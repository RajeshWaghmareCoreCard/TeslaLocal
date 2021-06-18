using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Repository;

namespace CoreCard.Tesla.Tokenization.Repository
{
    public class HsmActiveKeysRepository : IHsmActiveKeysRepository
    {
        public async Task<HsmActiveKeyModel> GetActiveKey(string tokenFamilyId)
        {
           return await Task.FromResult(new HsmActiveKeyModel() { HsmActiveKeyId = 1, KeyDetails = "asdadsada-asdadasd-adsadasd-" });
        }
    }
}