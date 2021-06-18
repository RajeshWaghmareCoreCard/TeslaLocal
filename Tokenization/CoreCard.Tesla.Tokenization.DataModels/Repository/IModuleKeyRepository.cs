using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface IModuleKeyRepository
    {
        Task<ModuleKey> GetModuleDetailsAsync(string moduleKeyId);
    }
}