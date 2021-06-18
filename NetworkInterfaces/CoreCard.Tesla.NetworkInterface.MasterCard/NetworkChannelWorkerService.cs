using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.NetworkInterface.MasterCard
{
    public class NetworkChannelWorkerService : BackgroundService
    {
        private readonly ILogger<NetworkChannelWorkerService> _logger;
        private readonly INetworkChannel _networkChannel;

        public NetworkChannelWorkerService(ILogger<NetworkChannelWorkerService> logger, INetworkChannel externalChannel)
        {
            _logger = logger;
            _networkChannel = externalChannel;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service started...");

            _networkChannel.ConnectToNetwork();

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stopped...");

            _networkChannel.DisconnectFromNetwork();

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}