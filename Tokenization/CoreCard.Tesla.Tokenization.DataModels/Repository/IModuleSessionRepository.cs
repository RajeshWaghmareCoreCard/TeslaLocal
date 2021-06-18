using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Types;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface IModuleSessionRepository
    {
        Task InsertModuleSessionAsync(ModuleSessionModel sessionDetails);
        Task<ModuleSessionModel> GetModuleSessionAsync(string sessionId);
    }
}