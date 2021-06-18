using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.DtoModels;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface IModulePermissionRepository
    {
        Task<List<ModulePermissionModel>> GetAllModulePermissions(string moduleId);
        Task<ModulePermissionModel> GetModulePermissions(string moduleId, string tokenFamily);
    }
}