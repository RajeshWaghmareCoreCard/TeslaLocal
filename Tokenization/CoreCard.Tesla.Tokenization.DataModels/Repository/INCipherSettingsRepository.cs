using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface INCipherSettingsRepository
    {
        ServiceSettings GetServiceSettings(string serviceId);
    }
}