using System.Threading.Tasks;

namespace CoreCard.Tesla.Tokenization.DataModels.Interfaces
{
    public interface IChannel
    {
        bool IsConnected { get; }
        void ConnectChannel();
        void DisconnectChannel();

        Task<IServiceResponse> SendAsync(IServiceRequest request);
    }
}