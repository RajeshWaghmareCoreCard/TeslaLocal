using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels.DtoModels;

namespace CoreCard.Tesla.Tokenization.DataModels.Repository
{
    public interface IOtherTokenFamilyRepository
    {
        Task<OtherFamilyModel> GetToken(string token);
        Task<OtherFamilyModel> GetTokenByHash(string cardHash);
        Task InsertToken(OtherFamilyModel card);
    }
}