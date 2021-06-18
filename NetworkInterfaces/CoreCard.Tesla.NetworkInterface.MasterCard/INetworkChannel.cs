using System.Threading.Tasks;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;

namespace CoreCard.Tesla.NetworkInterface.MasterCard
{
    public interface INetworkChannel
    {
        public MasterCardNetworkSettings MasterCardNetworkSettings { get; }
        public bool IsConnected { get; }
        public void ConnectToNetwork();
        public void DisconnectFromNetwork();
        public Task<IIsoResponse> SendIsoRequestAsync(IIsoRequest isoRequest);
    }
}