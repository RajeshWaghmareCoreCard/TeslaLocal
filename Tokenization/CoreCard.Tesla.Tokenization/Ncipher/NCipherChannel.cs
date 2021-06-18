using System.Threading.Tasks;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.DtoTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.Types;
using Microsoft.Extensions.Options;

namespace CoreCard.Tesla.Tokenization
{
    public class NCipherChannel : IChannel
    {
        private readonly INCipherSettingsRepository settingsRepository;
        private readonly IOptions<TokenizationConfig> configurationFile;
        ServiceSettings channelSettings;

        public NCipherChannel(INCipherSettingsRepository settingsRepository, IOptions<TokenizationConfig> configurationFile)
        {
            this.settingsRepository = settingsRepository;
            this.configurationFile = configurationFile;
            channelSettings = settingsRepository.GetServiceSettings(configurationFile.Value.ServiceId);
        }
        public bool IsConnected => throw new System.NotImplementedException();

        public void DisconnectChannel()
        {
            throw new System.NotImplementedException();
        }

        public void ConnectChannel()
        {
            throw new System.NotImplementedException();
        }

        public Task<IServiceResponse> SendAsync(IServiceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}