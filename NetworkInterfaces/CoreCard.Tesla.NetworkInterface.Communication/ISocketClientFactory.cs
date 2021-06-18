using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.NetworkInterface.Communication
{
    public interface ISocketClientFactory
    {
        ISocketClient GetTCPSocketClient(NetworkConnectionSettings networkConnectionSettings, ILoggerFactory loggerFactory);
    }
}
