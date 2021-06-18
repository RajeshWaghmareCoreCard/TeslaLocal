using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;

namespace CoreCard.Tesla.Tokenization.Repository
{
    public class NCipherSettingsRepository : INCipherSettingsRepository
    {
        private readonly IDatabaseConnectionResolver databaseConnection;

        public NCipherSettingsRepository(IDatabaseConnectionResolver databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }
        public ServiceSettings GetServiceSettings(string serviceId)
        {
            return null;
        }
    }
}