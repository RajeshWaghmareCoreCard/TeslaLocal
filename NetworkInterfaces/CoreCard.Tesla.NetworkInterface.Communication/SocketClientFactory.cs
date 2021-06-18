using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.NetworkInterface.Communication
{
    public class SocketClientFactory : ISocketClientFactory
    {
        public ISocketClient GetTCPSocketClient(NetworkConnectionSettings networkConnectionSettings, ILoggerFactory loggerFactory)
        {
            return new TcpSocketClient(networkConnectionSettings, loggerFactory.CreateLogger<TcpSocketClient>());
        }

    }
}
